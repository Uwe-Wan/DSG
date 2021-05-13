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

        public ObservableCollection<IsSelectedAndWeightedExpansionDto> IsSelectedAndWeightedExpansionDto { get; set; }

        private IMessenger Messenger { get; set; }

        /// <summary>
        /// This is just for testing purpose.
        /// </summary>
        public GenerationProfileViewEntity()
        {
        }

        public GenerationProfileViewEntity(GenerationProfile generationProfile, IMessenger messenger, ObservableCollection<IsSelectedAndWeightedExpansionDto> isSelectedAndWeightedExpansionDto)
        {
            GenerationProfile = generationProfile;
            IsSelectedAndWeightedExpansionDto = isSelectedAndWeightedExpansionDto;

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
            foreach (IsSelectedAndWeightedExpansionDto isSelectedAndWeightedExpansionDto in IsSelectedAndWeightedExpansionDto)
            {
                isSelectedAndWeightedExpansionDto.IsSelected = false;
                isSelectedAndWeightedExpansionDto.Weight = 1;
            }

            foreach (SelectedExpansionToGenerationProfile selectedExpansion in GenerationProfile.SelectedExpansions)
            {
                IsSelectedAndWeightedExpansionDto isSelectedAndWeightedExpansionDto = IsSelectedAndWeightedExpansionDto
                    .Single(x => x.DominionExpansion.Id == selectedExpansion.DominionExpansionId);
                isSelectedAndWeightedExpansionDto.IsSelected = true;
                isSelectedAndWeightedExpansionDto.Weight = selectedExpansion.Weight;
            }
        }

        internal void DeleteProfile()
        {
            MessageDto messageDto = new MessageDto(Message.ProfileDeleted, GenerationProfile);

            Messenger.NotifyEventTriggered(messageDto);
        }
    }
}
