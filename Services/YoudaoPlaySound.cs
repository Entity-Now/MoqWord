using AntDesign;
using Edge_tts_sharp;
using Edge_tts_sharp.Utils;
using MoqWord.Repository.Interface;
using MoqWord.Services.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xaml;

namespace MoqWord.Services
{
    public class YoudaoPlaySound : IPlaySound
    {
        public IMessageService messageService { get; set; }
        public ISettingRepository settingRepository { get; set; }
        public YoudaoPlaySound(ISettingRepository _settingRepository, IMessageService _messageService)
        {
            settingRepository = _settingRepository;
            messageService = _messageService;
        }

        public IEnumerable<Voice> GetVoice()
        {
            return new List<Voice>
            {
                new Voice { Name = "2", ShortName = "美国发音" },
                new Voice { Name = "1", ShortName = "英国发音" },
            };
        }

        public async Task PlayAsync(string word, CancellationToken cancelToken = default)
        {
            try
            {
                var setting = settingRepository.GetSingle(x => x.Id >= 0);
                string useVoice = "2";
                if (setting.SoundSource == Sound.Youdao && !string.IsNullOrEmpty(setting.SoundName))
                {
                    useVoice = GetVoice().First(x => x.Name == setting.SoundName).Name;
                }
                await Audio.PlayAudioFromUrlAsync
                (
                    $"https://dict.youdao.com/dictvoice?audio={HttpUtility.UrlEncode(word)}&type={useVoice}",
                     cancellationToken: cancelToken
                );
                
            }
            catch (Exception ex)
            {
                await messageService.Error(ex.Message);
            }
        }

        public async Task PlayAsync(string soundName, string word, double volume, double speed, CancellationToken cancelToken = default)
        {
            try
            {
                await Audio.PlayAudioFromUrlAsync
                (
                    $"https://dict.youdao.com/dictvoice?audio={HttpUtility.UrlEncode(word)}&type={soundName}",
                     cancellationToken: cancelToken
                );

            }
            catch (Exception ex)
            {
                await messageService.Error(ex.Message);
            }
        }
    }
}
