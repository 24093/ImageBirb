using ImageBirb.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ImageBirb.Controls
{
    internal class ThumbnailsListView : ListBox
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

        public ThumbnailsListView()
        {
            SelectionChanged += OnSelectionChanged;
        }

        private static void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listView = (ListBox) e.OriginalSource;

            if (listView != null && 
                e.AddedItems.Count > 0 && 
                listView.Items.IndexOf(e.AddedItems[0]) >= 0)
            {
                listView.ScrollIntoView(e.AddedItems[0]);
            }
        }
    }
}
