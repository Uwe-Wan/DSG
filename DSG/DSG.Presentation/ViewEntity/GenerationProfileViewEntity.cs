using DSG.BusinessEntities;
using DSG.BusinessEntities.GenerationProfiles;
using DSG.Presentation.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace DSG.Presentation.ViewEntity
{
    public class GenerationProfileViewEntity
    {
        public GenerationProfile GenerationProfile { get; set; }

        public ObservableCollection<IsDominionExpansionSelectedDto> IsDominionExpansionSelectedDtos { get; set; }

        private IMessenger Messenger { get; set; }

        /// <summary>
        /// This is just for testing purpose.
        /// </summary>
        public GenerationProfileViewEntity()
        {
        }

        public GenerationProfileViewEntity(GenerationProfile generationProfile, IMessenger messenger, ObservableCollection<IsDominionExpansionSelectedDto> isDominionExpansionSelectedDtos)
        {
            GenerationProfile = generationProfile;
            IsDominionExpansionSelectedDtos = isDominionExpansionSelectedDtos;

            Messenger = messenger;

            LoadProfileCommand = new RelayCommand(c => LoadProfile());
            DeleteProfileCommand = new RelayCommand(c => DeleteProfile());
        }

        public ICommand LoadProfileCommand { get; set; }
        public ICommand DeleteProfileCommand { get; private set; }

        internal void LoadProfile()
        {
            MessageDto messageDto = new MessageDto(Message.ProfileLoaded, GenerationProfile);

            Messenger.NotifyEventTriggered(messageDto);

            CheckSelectedExpansionsOfProfile();
        }

        private void CheckSelectedExpansionsOfProfile()
        {
            foreach (IsDominionExpansionSelectedDto isDominionExpansionSelectedDto in IsDominionExpansionSelectedDtos)
            {
                isDominionExpansionSelectedDto.IsSelected = false;
            }

            foreach (int selectedId in GenerationProfile.SelectedExpansions.Select(x => x.DominionExpansionId))
            {
                IsDominionExpansionSelectedDtos.Single(x => x.DominionExpansion.Id == selectedId).IsSelected = true;
            }
        }

        internal void DeleteProfile()
        {
            MessageDto messageDto = new MessageDto(Message.ProfileDeleted, GenerationProfile);

            Messenger.NotifyEventTriggered(messageDto);
        }
    }
}
