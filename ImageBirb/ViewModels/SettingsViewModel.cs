using ImageBirb.Common;
using ImageBirb.Controls;
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

        private ImageStorageChoice _selectedImageStorageChoice;

        public string DatabaseFilename {get; private set; }

        public bool AddFolders
        {
            get => _addFolders;
            set
            {
                Set(ref _addFolders, value);
                Workflows.UpdateSettings(SettingType.AddFolders.ToString(), value.ToString());
            }
        }

        public ObservableCollection<ImageStorageChoice> ImageStorageChoices { get; }

        public ImageStorageChoice SelectedImageStorageChoice
        {
            get => _selectedImageStorageChoice;
            set
            {
                Set(ref _selectedImageStorageChoice, value);
                Workflows.UpdateSettings(SettingType.ImageStorage.ToString(), value.ToString());
            }
        }

        public SettingsViewModel (IWorkflowAdapter workflows)
            : base (workflows)
        {
            Run(Workflows.ReadConnectionString(), r => DatabaseFilename = r.ConnectionString);
            Run(Workflows.ReadSetting(SettingType.AddFolders.ToString()), r => AddFolders = r.Setting.AsBool());
            Run(Workflows.ReadSetting(SettingType.ImageStorage.ToString()), r => SelectedImageStorageChoice = r.Setting.AsEnum<ImageStorageChoice>());

            ImageStorageChoices = new ObservableCollection<ImageStorageChoice>
            {
                ImageStorageChoice.CopyToDataFolder,
                ImageStorageChoice.CopyToDatabase,
                ImageStorageChoice.LinkToSource
            };

            SelectedImageStorageChoice = ImageStorageChoice.LinkToSource;
        }
    }
}
