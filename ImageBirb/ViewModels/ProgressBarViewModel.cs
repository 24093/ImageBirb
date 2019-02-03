using System.Windows;
using GalaSoft.MvvmLight;

namespace ImageBirb.ViewModels
{
    internal class ProgressBarViewModel : ViewModelBase
    {
        private int _value;

        private int _maxValue;

        private Visibility _visibility;

        public int Value
        {
            get => _value;
            set => Set(ref _value, value);
        }

        public int MaxValue
        {
            get => _maxValue;
            set => Set(ref _maxValue, value);
        }
        
        public Visibility Visibility
        {
            get => _visibility;
            set => Set(ref _visibility, value);
        }

        public ProgressBarViewModel()
        {
            Visibility = Visibility.Collapsed;
        }
    }
}