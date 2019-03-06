using System.Windows;
using System.Windows.Controls;
using Image = ImageBirb.Core.BusinessObjects.Image;

namespace ImageBirb.Controls
{
    internal class SelectedImageView : Control
    {
        public static readonly DependencyProperty SelectedImageProperty = DependencyProperty.Register(
            "SelectedImage", typeof(Image), typeof(SelectedImageView), new PropertyMetadata(default(Image)));

        public Image SelectedImage
        {
            get => (Image) GetValue(SelectedImageProperty);
            set => SetValue(SelectedImageProperty, value);
        }
    }
}
