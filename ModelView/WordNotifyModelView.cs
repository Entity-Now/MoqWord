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
        double _opacity;
        public double Opacity
        {
            get => _opacity;
            set{
                this.RaiseAndSetIfChanged(ref _opacity, value);
            }
        }

        public ReactiveCommand<Unit, Unit> PreviousCommand { get; set; }
        public ReactiveCommand<Unit, Unit> LastCommand { get; set; }
        public ReactiveCommand<Unit, Unit> StopCommand { get; set; }
        public ReactiveCommand<Unit, Unit> CloseCommand { get; set; }

        public WordNotifyModelView(IPlayService _playService)
        {
            playService = _playService;
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
            CloseCommand = ReactiveCommand.Create(() =>
            {
                NotifyIconHelper.Close();
            });
        }
    }
}
