using ImageBirb.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ImageBirb.Controls
{
    internal class ThumbnailsListView : ListView
    {
        public static readonly DependencyProperty SelectedImageViewModelProperty = DependencyProperty.Register(
            "SelectedImageViewModel", typeof(SelectedImageViewModel), typeof(ThumbnailsListView), new PropertyMetadata(default(SelectedImageViewModel)));

        public static readonly DependencyProperty ThumbnailListViewModelProperty = DependencyProperty.Register(
            "ThumbnailListViewModel", typeof(ThumbnailListViewModel), typeof(ThumbnailsListView), new PropertyMetadata(default(ThumbnailListViewModel)));

        public SelectedImageViewModel SelectedImageViewModel
        {
            get => (SelectedImageViewModel)GetValue(SelectedImageViewModelProperty);
            set => SetValue(SelectedImageViewModelProperty, value);
        }

        public ThumbnailListViewModel ThumbnailListViewModel
        {
            get => (ThumbnailListViewModel)GetValue(ThumbnailListViewModelProperty);
            set => SetValue(ThumbnailListViewModelProperty, value);
        }
    }
}
