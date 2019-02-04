using System.Windows;
using System.Windows.Controls;
using ImageBirb.ViewModels;

namespace ImageBirb.Controls
{
    internal class SettingsView : Control
    {
        public static readonly DependencyProperty SettingsViewModelProperty = DependencyProperty.Register(
            "SettingsViewModel", typeof(SettingsViewModel), typeof(SettingsView), new PropertyMetadata(default(SettingsViewModel)));

        public SettingsViewModel SettingsViewModel
        {
            get => (SettingsViewModel) GetValue(SettingsViewModelProperty);
            set => SetValue(SettingsViewModelProperty, value);
        }
    }
}