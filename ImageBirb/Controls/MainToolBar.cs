using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ImageBirb.Controls
{
    internal class MainToolBar : Control
    {
        public static readonly DependencyProperty ToggleTagListCommandProperty = DependencyProperty.Register(
            "ToggleTagListCommand", typeof(ICommand), typeof(MainToolBar), new PropertyMetadata(default(ICommand)));

        public ICommand ToggleTagListCommand
        {
            get => (ICommand)GetValue(ToggleTagListCommandProperty);
            set => SetValue(ToggleTagListCommandProperty, value);
        }

        public static readonly DependencyProperty ToggleSelectedImageTagsCommandProperty = DependencyProperty.Register(
            "ToggleSelectedImageTagsCommand", typeof(ICommand), typeof(MainToolBar), new PropertyMetadata(default(ICommand)));

        public ICommand ToggleSelectedImageTagsCommand
        {
            get => (ICommand)GetValue(ToggleSelectedImageTagsCommandProperty);
            set => SetValue(ToggleSelectedImageTagsCommandProperty, value);
        }

        public static readonly DependencyProperty AddImageFromFileCommandProperty = DependencyProperty.Register(
            "AddImageFromFileCommand", typeof(ICommand), typeof(MainToolBar), new PropertyMetadata(default(ICommand)));

        public ICommand AddImageFromFileCommand
        {
            get => (ICommand)GetValue(AddImageFromFileCommandProperty);
            set => SetValue(AddImageFromFileCommandProperty, value);
        }

        public static readonly DependencyProperty RemoveImageCommandProperty = DependencyProperty.Register(
            "RemoveImageCommand", typeof(ICommand), typeof(MainToolBar), new PropertyMetadata(default(ICommand)));

        public ICommand RemoveImageCommand
        {
            get => (ICommand)GetValue(RemoveImageCommandProperty);
            set => SetValue(RemoveImageCommandProperty, value);
        }

        public static readonly DependencyProperty ToggleSettingsCommandProperty = DependencyProperty.Register(
            "ToggleSettingsCommand", typeof(ICommand), typeof(MainToolBar), new PropertyMetadata(default(ICommand)));

        public ICommand ToggleSettingsCommand
        {
            get => (ICommand)GetValue(ToggleSettingsCommandProperty);
            set => SetValue(ToggleSettingsCommandProperty, value);
        }

        public static readonly DependencyProperty SelectedImageIdProperty = DependencyProperty.Register(
            "SelectedImageId", typeof(string), typeof(MainToolBar), new PropertyMetadata(default(string)));
        
        public string SelectedImageId
        {
            get => (string) GetValue(SelectedImageIdProperty);
            set => SetValue(SelectedImageIdProperty, value);
        }
    }
}