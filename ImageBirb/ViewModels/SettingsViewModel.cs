using ImageBirb.Controls;
using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Primary;
using System.Collections.ObjectModel;

namespace ImageBirb.ViewModels
{
    /// <summary>
    /// Handles the content of the settings view.
    /// </summary>
    internal class SettingsViewModel : WorkflowViewModel
    {
        private bool _addFolders;

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
        }

        private void UpdateSetting<T>(SettingType type, T value)
        {
            Workflows.UpdateSettings(type.ToString(), value.ToString());
        }
    }
}
