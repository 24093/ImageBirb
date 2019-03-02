using ImageBirb.Core.BusinessObjects;
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
        private const string AddFoldersSetting = "AddFolders";
        private const string AddFoldersSettingDefault = "True";
        private const string ImageStorageSetting = "ImageStorage";
        private const string ImageStorageSettingDefault = "LinkToSource";
        private const string IgnoreSimilarImagesSetting = "IgnoreSimilarImages";
        private const string IgnoreSimilarImagesSettingDefault = "True";
        private const string SimilarityThresholdSetting = "SimilarityThreshold";
        private const string SimilarityThresholdSettingDefault = "0.9";

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
                UpdateSetting(AddFoldersSetting, value);
            }
        }

        public bool IgnoreSimilarImages
        {
            get => _ignoreSimilarImages;
            set
            {
                Set(ref _ignoreSimilarImages, value);
                UpdateSetting(IgnoreSimilarImagesSetting, value);
            }
        }

        public double SimilarityThreshold
        {
            get => _similarityThreshold;
            set
            {
                Set(ref _similarityThreshold, value);
                UpdateSetting(SimilarityThresholdSetting, value);
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
                UpdateSetting(ImageStorageSetting, value);
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
            await RunAsync(Workflows.ReadConnectionString(), connectionString => DatabaseFilename = connectionString);
            await RunAsync(Workflows.ReadSetting(AddFoldersSetting, AddFoldersSettingDefault), setting => AddFolders = setting.AsBool());
            await RunAsync(Workflows.ReadSetting(ImageStorageSetting, ImageStorageSettingDefault), setting => SelectedImageStorageType = setting.AsEnum<ImageStorageType>());
            await RunAsync(Workflows.ReadSetting(IgnoreSimilarImagesSetting, IgnoreSimilarImagesSettingDefault), setting => IgnoreSimilarImages = setting.AsBool());
            await RunAsync(Workflows.ReadSetting(SimilarityThresholdSetting, SimilarityThresholdSettingDefault), setting => SimilarityThreshold = setting.AsDouble());
        }

        private void UpdateSetting<T>(string type, T value)
        {
            Workflows.UpdateSetting(type, value.ToString());
        }

        private void UpdateSetting(string type, double value)
        {
            Workflows.UpdateSetting(type, value.ToString(CultureInfo.InvariantCulture));
        }
    }
}