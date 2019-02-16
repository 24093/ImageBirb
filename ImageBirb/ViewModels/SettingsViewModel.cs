using ImageBirb.Controls;
using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Primary;
using System.Collections.ObjectModel;
using System.Globalization;

namespace ImageBirb.ViewModels
{
    /// <summary>
    /// Handles the content of the settings view.
    /// </summary>
    internal class SettingsViewModel : WorkflowViewModel
    {
        private bool _addFolders;

        private bool _ignoreSimilarImages;

        private double _similarityThreshold;

        private ImageStorageType _selectedImageStorageType;
        
        public string DatabaseFilename {get; private set; }

        public bool AddFolders
        {
            get => _addFolders;
            set
            {
                Set(ref _addFolders, value);
                UpdateSetting(SettingType.AddFolders, value);
            }
        }

        public bool IgnoreSimilarImages
        {
            get => _ignoreSimilarImages;
            set
            {
                Set(ref _ignoreSimilarImages, value);
                UpdateSetting(SettingType.IgnoreSimilarImages, value);
            }
        }

        public double SimilarityThreshold
        {
            get => _similarityThreshold;
            set
            {
                Set(ref _similarityThreshold, value);
                UpdateSetting(SettingType.SimilarityThreshold, value);
            }
        }

        public ObservableCollection<ImageStorageType> ImageStorageChoices { get; }

        public ImageStorageType SelectedImageStorageType
        {
            get => _selectedImageStorageType;
            set
            {
                Set(ref _selectedImageStorageType, value);
                UpdateSetting(SettingType.ImageStorage, value);
            }
        }

        public SettingsViewModel (IWorkflowAdapter workflows)
            : base (workflows)
        {
            ImageStorageChoices = new ObservableCollection<ImageStorageType>
            {
                ImageStorageType.CopyToDatabase,
                ImageStorageType.LinkToSource
            };

            Run(Workflows.ReadConnectionString(), r => DatabaseFilename = r.ConnectionString);
            Run(Workflows.ReadSetting(SettingType.AddFolders.ToString()), r => AddFolders = r.Setting.AsBool());
            Run(Workflows.ReadSetting(SettingType.ImageStorage.ToString()), r => SelectedImageStorageType = r.Setting.AsEnum<ImageStorageType>());
            Run(Workflows.ReadSetting(SettingType.IgnoreSimilarImages.ToString()), r => IgnoreSimilarImages = r.Setting.AsBool());
            Run(Workflows.ReadSetting(SettingType.SimilarityThreshold.ToString()), r => SimilarityThreshold = r.Setting.AsDouble());
        }

        private void UpdateSetting<T>(SettingType type, T value)
        {
            Workflows.UpdateSettings(type.ToString(), value.ToString());
        }

        private void UpdateSetting(SettingType type, double value)
        {
            Workflows.UpdateSettings(type.ToString(), value.ToString(CultureInfo.InvariantCulture));
        }
    }
}
