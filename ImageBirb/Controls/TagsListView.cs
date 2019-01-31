using ImageBirb.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ImageBirb.Controls
{
    internal class TagsListView : ListView
    {
        public static readonly DependencyProperty TagListViewModelProperty = DependencyProperty.Register(
            "TagListViewModel", typeof(TagListViewModel), typeof(TagsListView), new PropertyMetadata(default(TagListViewModel)));

        public static readonly DependencyProperty ThumbnailListViewModelProperty = DependencyProperty.Register(
            "ThumbnailListViewModel", typeof(ThumbnailListViewModel), typeof(TagsListView), new PropertyMetadata(default(ThumbnailListViewModel)));

        public TagListViewModel TagListViewModel
        {
            get => (TagListViewModel) GetValue(TagListViewModelProperty);
            set => SetValue(TagListViewModelProperty, value);
        }
        
        public ThumbnailListViewModel ThumbnailListViewModel
        {
            get => (ThumbnailListViewModel) GetValue(ThumbnailListViewModelProperty);
            set => SetValue(ThumbnailListViewModelProperty, value);
        }
    }
}
