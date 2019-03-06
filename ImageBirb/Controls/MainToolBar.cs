using ImageBirb.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ImageBirb.Controls
{
    internal class MainToolBar : Control
    {
        public static readonly DependencyProperty MainViewModelProperty = DependencyProperty.Register(
            "MainViewModel", typeof(MainViewModel), typeof(MainToolBar), new PropertyMetadata(default(MainViewModel)));

        public static readonly DependencyProperty ToggleTagListCommandProperty = DependencyProperty.Register(
            "ToggleTagListCommand", typeof(ICommand), typeof(MainToolBar), new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty ToggleSelectedImageTagsCommandProperty = DependencyProperty.Register(
            "ToggleSelectedImageTagsCommand", typeof(ICommand), typeof(MainToolBar), new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty AddImageFromFileCommandProperty = DependencyProperty.Register(
            "AddImageFromFileCommand", typeof(ICommand), typeof(MainToolBar), new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty RemoveImageCommandProperty = DependencyProperty.Register(
            "RemoveImageCommand", typeof(ICommand), typeof(MainToolBar), new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty ToggleSettingsCommandProperty = DependencyProperty.Register(
            "ToggleSettingsCommand", typeof(ICommand), typeof(MainToolBar), new PropertyMetadata(default(ICommand)));


        public MainViewModel MainViewModel
        {
            get => (MainViewModel) GetValue(MainViewModelProperty);
            set => SetValue(MainViewModelProperty, value);
        }

        public ICommand ToggleTagListCommand
        {
            get => (ICommand)GetValue(ToggleTagListCommandProperty);
            set => SetValue(ToggleTagListCommandProperty, value);
        }

        public ICommand ToggleSelectedImageTagsCommand
        {
            get => (ICommand)GetValue(ToggleSelectedImageTagsCommandProperty);
            set => SetValue(ToggleSelectedImageTagsCommandProperty, value);
        }

        public ICommand AddImageFromFileCommand
        {
            get => (ICommand)GetValue(AddImageFromFileCommandProperty);
            set => SetValue(AddImageFromFileCommandProperty, value);
        }

        public ICommand RemoveImageCommand
        {
            get => (ICommand)GetValue(RemoveImageCommandProperty);
            set => SetValue(RemoveImageCommandProperty, value);
        }

        public ICommand ToggleSettingsCommand
        {
            get => (ICommand) GetValue(ToggleSettingsCommandProperty);
            set => SetValue(ToggleSettingsCommandProperty, value);
        }
    }
}