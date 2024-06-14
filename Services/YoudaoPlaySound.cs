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
                new Voice { Name = "美音", ShortName = "2" },
                new Voice { Name = "英音", ShortName = "1" },
            };
        }

        public async void Play(string word)
        {
            try
            {
                var setting = settingRepository.GetSingle(x => x.Id >= 0);
                string useVoice = "2";
                if (setting.SoundSource == Sound.Youdao && !string.IsNullOrEmpty(setting.SoundName))
                {
                    useVoice = GetVoice().First(x => x.Name == setting.SoundName).ShortName;
                }
                HttpClient httpClient = new HttpClient();
                var response = await httpClient.GetStreamAsync($"https://dict.youdao.com/dictvoice?audio={HttpUtility.UrlEncodeUnicode(word)}&type={useVoice}");
                var stream = new MemoryStream();
                response.CopyTo(stream);
                stream.Position = 0;

                Audio.PlayToByte(stream, (float)setting.SoundVolume / 100);
            }
            catch (Exception ex)
            {
                messageService.Error(ex.Message);
            }
        }
    }
}
