using DSG.BusinessEntities;
using DSG.BusinessEntities.GenerationProfiles;
using DSG.Presentation.Messaging;
using DSG.Presentation.ViewEntity;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSG.Presentation.Test.ViewEntity
{
    [TestFixture]
    public class GenerationProfileViewEntityTest
    {
        private GenerationProfileViewEntity _testee;
        private Mock<IMessenger> _messengerMock;

        [SetUp]
        public void Setup()
        {
            _messengerMock = new Mock<IMessenger>();
        }

        [Test]
        public void Initialize_GenerationProfilesAndExpansionsSet()
        {
            //Arrange
            GenerationProfile generationProfile = new GenerationProfile();

            ObservableCollection<IsDominionExpansionSelectedDto> isDominionExpansionSelectedDtos = new ObservableCollection<IsDominionExpansionSelectedDto>();
            isDominionExpansionSelectedDtos.Add(new IsDominionExpansionSelectedDto(new DominionExpansion { Name = "Welt" }));

            //Act
            _testee = new GenerationProfileViewEntity(generationProfile, _messengerMock.Object, isDominionExpansionSelectedDtos);

            //Assert
            _testee.GenerationProfile.Should().Be(generationProfile);
            _testee.IsDominionExpansionSelectedDtos.Should().Equal(isDominionExpansionSelectedDtos);
        }

        [Test]
        public void LoadProfile_MessengerInvoked_ExpansionsOfProfileChecked()
        {
            //Arrange
            GenerationProfile generationProfile = new GenerationProfile();

            DominionExpansion world = new DominionExpansion { Id = 1, Name = "World" };
            DominionExpansion seaside = new DominionExpansion { Id = 2, Name = "Seaside" };
            DominionExpansion intrigue = new DominionExpansion { Id = 3, Name = "Intrigue" };

            generationProfile.SelectedExpansions.Add(new SelectedExpansionToGenerationProfile(world));
            generationProfile.SelectedExpansions.Add(new SelectedExpansionToGenerationProfile(intrigue));

            ObservableCollection<IsDominionExpansionSelectedDto> isDominionExpansionSelectedDtos = new ObservableCollection<IsDominionExpansionSelectedDto>();
            isDominionExpansionSelectedDtos.Add(new IsDominionExpansionSelectedDto(world));
            isDominionExpansionSelectedDtos.Add(new IsDominionExpansionSelectedDto(seaside));
            IsDominionExpansionSelectedDto intrigueUnselected = new IsDominionExpansionSelectedDto(intrigue) { IsSelected = false };
            isDominionExpansionSelectedDtos.Add(intrigueUnselected);

            _testee = new GenerationProfileViewEntity(generationProfile, _messengerMock.Object, isDominionExpansionSelectedDtos);

            //Act
            _testee.LoadProfile();

            //Assert
            _messengerMock.Verify(x => x.NotifyEventTriggered(It.Is<MessageDto>(m => m.Name == Message.ProfileLoaded)), Times.Once);

            _testee.IsDominionExpansionSelectedDtos.Single(x => x.DominionExpansion.Name == "World").IsSelected.Should().BeTrue();
            _testee.IsDominionExpansionSelectedDtos.Single(x => x.DominionExpansion.Name == "Seaside").IsSelected.Should().BeFalse();
            _testee.IsDominionExpansionSelectedDtos.Single(x => x.DominionExpansion.Name == "Intrigue").IsSelected.Should().BeTrue();
        }
    }
}
