using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Primary;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;

namespace ImageBirb.ViewModels
{
    /// <summary>
    /// Handles the content of the settings view.
    /// </summary>
    internal class SettingsViewModel : WorkflowViewModel
    {
        private string _databaseFilename;

        private bool _addFolders;

        private bool _ignoreSimilarImages;

        private double _similarityThreshold;

        private ImageStorageType _selectedImageStorageType;

        public string DatabaseFilename
        {
            get => _databaseFilename;
            private set => Set(ref _databaseFilename, value);
        }

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

        public ObservableCollection<ImageStorageType> ImageStorageChoices { get; } =
            new ObservableCollection<ImageStorageType>
            {
                ImageStorageType.CopyToDatabase,
                ImageStorageType.LinkToSource
            };

        public ImageStorageType SelectedImageStorageType
        {
            get => _selectedImageStorageType;
            set
            {
                Set(ref _selectedImageStorageType, value);
                UpdateSetting(SettingType.ImageStorage, value);
            }
        }

        public SettingsViewModel(IWorkflowAdapter workflows)
            : base(workflows)
        {
        }

        /// <summary>
        /// Refresh the settings from the database.
        /// Call this whenever the settings view becomes visible.
        /// </summary>
        public async Task ReadSettings()
        {
            await RunAsync(Workflows.ReadConnectionString(), r => DatabaseFilename = r.ConnectionString);
            await RunAsync(Workflows.ReadSetting(SettingType.AddFolders.ToString()), r => AddFolders = r.Setting.AsBool());
            await RunAsync(Workflows.ReadSetting(SettingType.ImageStorage.ToString()), r => SelectedImageStorageType = r.Setting.AsEnum<ImageStorageType>());
            await RunAsync(Workflows.ReadSetting(SettingType.IgnoreSimilarImages.ToString()), r => IgnoreSimilarImages = r.Setting.AsBool());
            await RunAsync(Workflows.ReadSetting(SettingType.SimilarityThreshold.ToString()), r => SimilarityThreshold = r.Setting.AsDouble());
        }

        private void UpdateSetting<T>(SettingType type, T value)
        {
            Workflows.UpdateSetting(type.ToString(), value.ToString());
        }

        private void UpdateSetting(SettingType type, double value)
        {
            Workflows.UpdateSetting(type.ToString(), value.ToString(CultureInfo.InvariantCulture));
        }
    }
}