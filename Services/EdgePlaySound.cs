using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Edge_tts_sharp;
using Edge_tts_sharp.Model;
using Edge_tts_sharp.Utils;
using MoqWord.Model.Entity;
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

        public async Task PlayAsync(string word, CancellationToken cancelToken = default)
        {
            try
            {
                //Edge_tts.Await = true;
                var setting = settingRepository.First();
                await PlayAsync(setting.SoundName, word, setting.SoundVolume, setting.SpeechSpeed, cancelToken);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task PlayAsync(string soundName, string word, double volume, double speed, CancellationToken cancelToken = default)
        {
            bool finish = false;
            try
            {
                Voice useVoice = GetVoice().FirstOrDefault(x => x.Name == soundName);
                if (useVoice is null)
                {
                    throw new Exception("未找到Voice音频");
                }
                Edge_tts.Invoke(new PlayOption
                {
                    Text = word,
                    Volume = (int)volume / 100,
                    Rate = (int)speed
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
                        try
                        {
                            await Audio.PlayToByteAsync(
                                sound.ToArray(),
                                cancellationToken: cancelToken
                            );
                        }
                        catch (TaskCanceledException)
                        {
                            
                        }
                        finally
                        {
                            finish = true;
                        }
                    }
                });
            }
            catch (TaskCanceledException)
            {

            }
            while (!finish)
            {
                await Task.Delay(100);
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
