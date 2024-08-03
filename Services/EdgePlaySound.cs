using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Edge_tts_sharp;
using Edge_tts_sharp.Model;
using Edge_tts_sharp.Utils;
using MoqWord.Repository.Interface;
using MoqWord.Services.Interface;

namespace MoqWord.Services
{
    public class EdgePlaySound : IPlaySound
    {
        public ISettingRepository settingRepository { get; set; }
        public EdgePlaySound(ISettingRepository _settingRepository) 
        {
            settingRepository = _settingRepository;
        }

        public Task PlayAsync(string word, CancellationToken cancelToken = default)
        {
            var setting = settingRepository.First();
            Voice useVoice = null;
            if (setting.SoundSource == Sound.Edge && !string.IsNullOrEmpty(setting.SoundName))
            {
                useVoice = GetVoice().First(x => x.Name == setting.SoundName);
            }
            else
            {
                useVoice = GetVoice().First(x => x.Name.Contains("zh"));
            }
            Edge_tts.Invoke(word, new eVoice
            {
                Name = useVoice.Name,
                SuggestedCodec = useVoice.SuggestedCodec,
                Locale = useVoice.Locale,
                Gender = useVoice.Gender,
                ShortName = useVoice.ShortName
            }, (int)setting.SpeechSpeed, async (sound) =>
            {
                if (!cancelToken.IsCancellationRequested)
                {
                    await Audio.PlayToByteAsync(
                        sound.ToArray(),
                        (int)setting.SpeechSpeed,
                        (float)setting.SoundVolume / 100,
                        cancelToken
                    );
                }
            });
            return Task.CompletedTask;
        }

        public IEnumerable<Voice> GetVoice()
        {
            return Edge_tts.GetVoice().Select(x => new Voice 
            {
                Name = x.Name,
                ShortName = x.ShortName,
                Gender = x.Gender,
                Locale = x.Locale,
                SuggestedCodec = x.SuggestedCodec
            });
        }
    }
}
