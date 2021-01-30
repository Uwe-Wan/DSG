using DSG.BusinessEntities;
using DSG.BusinessEntities.GenerationProfiles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace DSG.Presentation.ViewEntity
{
    public class GenerationProfileViewEntity
    {
        public GenerationProfile GenerationProfile { get; set; }

        public GenerationProfile SelectedProfile { get; set; }

        public ObservableCollection<IsDominionExpansionSelectedDto> IsDominionExpansionSelectedDtos { get; set; }

        public GenerationProfileViewEntity(GenerationProfile generationProfile, GenerationProfile selectedProfile, ObservableCollection<IsDominionExpansionSelectedDto> isDominionExpansionSelectedDtos)
        {
            GenerationProfile = generationProfile;
            SelectedProfile = selectedProfile;
            IsDominionExpansionSelectedDtos = isDominionExpansionSelectedDtos;

            LoadProfileCommand = new RelayCommand(c => LoadProfile());
        }

        public ICommand LoadProfileCommand { get; set; }

        public event EventHandler ProfileLoaded;

        private void LoadProfile()
        {
            ProfileLoaded(GenerationProfile, null);

            CheckSelectedExpansionsOfProfile();
        }

        private void CheckSelectedExpansionsOfProfile()
        {
            foreach(IsDominionExpansionSelectedDto isDominionExpansionSelectedDto in IsDominionExpansionSelectedDtos)
            {
                isDominionExpansionSelectedDto.IsSelected = false;
            }

            foreach (int selectedId in GenerationProfile.SelectedExpansions.Select(x => x.DominionExpansionId))
            {
                IsDominionExpansionSelectedDtos.Single(x => x.DominionExpansion.Id == selectedId).IsSelected = true;
            }
        }
    }
}
