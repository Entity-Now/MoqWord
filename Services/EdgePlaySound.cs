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
            try
            {
                //Edge_tts.Await = true;
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
                Edge_tts.Invoke(new PlayOption
                {
                    Text = word,
                    Volume = (int)setting.SoundVolume / 100,
                    Rate = (int)setting.SpeechSpeed
                }, new eVoice
                {
                    Name = useVoice.Name,
                    SuggestedCodec = useVoice.SuggestedCodec,
                    Locale = useVoice.Locale,
                    Gender = useVoice.Gender,
                    ShortName = useVoice.ShortName
                }, async (sound) =>
                {
                    if (!cancelToken.IsCancellationRequested)
                    {
                        await Audio.PlayToByteAsync(
                            sound.ToArray(),
                            cancellationToken: cancelToken
                        );
                    }
                });
                return Task.CompletedTask;
            }
            catch (Exception)
            {
                return Task.CompletedTask;
            }
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
