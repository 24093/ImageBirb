using System.Windows;
using System.Windows.Controls;
using ImageBirb.ViewModels;

namespace ImageBirb.Controls
{
    internal class SelectedImageTagsView : Control
    {
        public static readonly DependencyProperty SelectedImageViewModelProperty = DependencyProperty.Register(
            "SelectedImageViewModel", typeof(SelectedImageViewModel), typeof(SelectedImageTagsView), new PropertyMetadata(default(SelectedImageViewModel)));

        public static readonly DependencyProperty TagListViewModelProperty = DependencyProperty.Register(
            "TagListViewModel", typeof(TagListViewModel), typeof(SelectedImageTagsView), new PropertyMetadata(default(TagListViewModel)));

        public SelectedImageViewModel SelectedImageViewModel
        {
            get => (SelectedImageViewModel)GetValue(SelectedImageViewModelProperty);
            set => SetValue(SelectedImageViewModelProperty, value);
        }

        public TagListViewModel TagListViewModel
        {
            get => (TagListViewModel)GetValue(TagListViewModelProperty);
            set => SetValue(TagListViewModelProperty, value);
        }
    }
}