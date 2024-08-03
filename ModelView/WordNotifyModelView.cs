using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.ModelView
{
    public class WordNotifyModelView : ReactiveObject
    {
        public IPlayService playService { get; set; }
        public PopupConfigModelView popupConfigModelView { get; set; }

        public ReactiveCommand<Unit, Unit> PreviousCommand { get; set; }
        public ReactiveCommand<Unit, Unit> LastCommand { get; set; }
        public ReactiveCommand<Unit, Unit> StopCommand { get; set; }
        public ReactiveCommand<Unit, Unit> LoopedCommand { get; set; }
        public ReactiveCommand<Unit, Unit> LookCommand { get; set; }
        public ReactiveCommand<Unit, Unit> CloseCommand { get; set; }

        public WordNotifyModelView(IPlayService _playService, PopupConfigModelView _popupConfigModelView)
        {
            playService = _playService;
            popupConfigModelView = _popupConfigModelView;
            PreviousCommand = ReactiveCommand.Create(() =>
            {
                playService.Previous();
            });
            LastCommand = ReactiveCommand.Create(() =>
            {
                playService.Next();
            });
            StopCommand = ReactiveCommand.Create(() =>
            {
                playService?.Stop();
            });
            LoopedCommand = ReactiveCommand.Create(() =>
            {
                playService.IsLoopPlay = true;
                playService?.Looped();
            });
            LookCommand = ReactiveCommand.Create(() =>
            {
                popupConfigModelView.IsLock = !popupConfigModelView.IsLock;
            });
            CloseCommand = ReactiveCommand.Create(() =>
            {
                NotifyIconHelper.Close();
            });
        }
    }
}
