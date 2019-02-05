﻿using ImageBirb.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ImageBirb.Controls
{
    internal class SelectedImageView : Control
    {
        public static readonly DependencyProperty SelectedImageViewModelProperty = DependencyProperty.Register(
            "SelectedImageViewModel", typeof(SelectedImageViewModel), typeof(SelectedImageView), new PropertyMetadata(default(SelectedImageViewModel)));

        public static readonly DependencyProperty TagListViewModelProperty = DependencyProperty.Register(
            "TagListViewModel", typeof(TagListViewModel), typeof(SelectedImageView), new PropertyMetadata(default(TagListViewModel)));

        public SelectedImageViewModel SelectedImageViewModel
        {
            get => (SelectedImageViewModel)GetValue(SelectedImageViewModelProperty);
            set => SetValue(SelectedImageViewModelProperty, value);
        }
        
        public TagListViewModel TagListViewModel
        {
            get => (TagListViewModel) GetValue(TagListViewModelProperty);
            set => SetValue(TagListViewModelProperty, value);
        }
    }
}