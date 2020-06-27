using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigiCaMemory.ViewModels
{
    public class MainPageViewModel : ViewModelBase, INavigationAware
    {
        private IPageDialogService _pageDialogService;
        private int _metaMemory;
        private int _memory;
        public int Memory
        {
            get { return _memory; }
            set { SetProperty(ref _memory, value); }
        }

        private string _color;
        public string Color
        {
            get { return _color; }
            set { SetProperty(ref _color, value); }
        }

        private int _rotation;
        public int Rotation
        {
            get { return _rotation; }
            set { SetProperty(ref _rotation, value); }
        }

        public MainPageViewModel(IPageDialogService pageDialogService, INavigationService navigationService)
            : base(navigationService)
        {
            _pageDialogService = pageDialogService;

            Title = "Main Page";
            Color = "Black";

            MemoryChangeCommand = new DelegateCommand<object>((args) => ChangeMemory(int.Parse(args.ToString())));

            Reset = new DelegateCommand(async() =>
            {
                var result = await pageDialogService.DisplayAlertAsync("リセット", "メモリを初期値に戻しますか？", "はい", "いいえ");
                if (result)
                {
                    MemoryReset();
                }
            });



        }

        private void MemoryReset()
        {
            Color = "Black";
            _metaMemory = 0;
            Memory = 0;
        }

        private void ChangeMemory(int value)
        {
            var afterMemory = _metaMemory + value;
            if (afterMemory > 10 || afterMemory < -10)
            {
                _pageDialogService.DisplayAlertAsync("アラート", "メモリの最大値を超えています", "OK");
                return;
            }
            _metaMemory = afterMemory;
            if (_metaMemory > 0)
            {
                Color = "Red";
                Rotation = 180;
            }
            else if(_metaMemory < 0)
            {
                Color = "Blue";
                Rotation = 0;
            }
            Memory = Math.Abs(_metaMemory);
        }

        public DelegateCommand Reset { get; private set; }
        public DelegateCommand<object> MemoryChangeCommand { get; private set; }
    }
}
