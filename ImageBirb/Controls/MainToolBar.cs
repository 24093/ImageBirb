using System.Windows;
using System.Windows.Controls;
using ImageBirb.ViewModels;

namespace ImageBirb.Controls
{
    internal class MainToolBar : Control
    {
        public static readonly DependencyProperty MainViewModelProperty = DependencyProperty.Register(
            "MainViewModel", typeof(MainViewModel), typeof(MainToolBar), new PropertyMetadata(default(MainViewModel)));

        public MainViewModel MainViewModel
        {
            get => (MainViewModel) GetValue(MainViewModelProperty);
            set => SetValue(MainViewModelProperty, value);
        }
    }
}