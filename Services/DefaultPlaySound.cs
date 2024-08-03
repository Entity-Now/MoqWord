using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using SqlSugar;
using MoqWord.Services.Interface;
using MoqWord.Repository.Interface;

namespace MoqWord.Services
{
    public class DefaultPlaySound : IPlaySound
    {
        public ISettingRepository settingRepository { get; set; }
        public DefaultPlaySound(ISettingRepository _settingRepository)
        {
            settingRepository = _settingRepository;
        }
        public async Task PlayAsync(string word, CancellationToken cancelToken = default)
        {
            var synth = new SpeechSynthesizer();
            try
            {
                var selectVoice = settingRepository.GetSingle(x => x.Id >= 0);
                string useVoice = null;
                if (selectVoice.SoundSource == Sound.Default && !string.IsNullOrEmpty(selectVoice.SoundName))
                {
                    useVoice = selectVoice.SoundName;
                }
                else
                {
                    useVoice = GetVoice().First().Name;
                }
                synth.Rate = (int)selectVoice.SpeechSpeed / 10;
                synth.Volume = (int)selectVoice.SoundVolume;
                synth.SelectVoice(useVoice);

                var tcs = new TaskCompletionSource<bool>();
                cancelToken.Register(() =>
                {
                    synth?.SpeakAsyncCancelAll();
                    tcs?.TrySetResult(true);
                });

                synth.SpeakCompleted += (s, e) => tcs.TrySetResult(true);
                synth?.SpeakAsync(word);

                await tcs.Task;
            }
            finally
            {
                //synth.Dispose();
            }
        }

        public IEnumerable<Voice> GetVoice()
        {
            using (var synth = new SpeechSynthesizer())
            {
                return synth.GetInstalledVoices().Select(x => new Voice
                {
                    Name = x.VoiceInfo.Name,
                    ShortName = x.VoiceInfo.Name,
                    Locale = x.VoiceInfo.Culture.Name,
                    Gender = x.VoiceInfo.Gender.ToString(),
                    SuggestedCodec = ""
                });
            }
        }
    }
}
