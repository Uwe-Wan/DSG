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

            ObservableCollection<IsSelectedAndWeightedExpansionDto> isSelectedAndWeightedExpansionDtos = new ObservableCollection<IsSelectedAndWeightedExpansionDto>();
            isSelectedAndWeightedExpansionDtos.Add(new IsSelectedAndWeightedExpansionDto(new DominionExpansion { Name = "Welt" }));

            //Act
            _testee = new GenerationProfileViewEntity(generationProfile, _messengerMock.Object, isSelectedAndWeightedExpansionDtos);

            //Assert
            _testee.GenerationProfile.Should().Be(generationProfile);
            _testee.IsSelectedAndWeightedExpansionDto.Should().Equal(isSelectedAndWeightedExpansionDtos);
        }

        [Test]
        public void LoadProfile_MessengerInvoked_ExpansionsOfProfileChecked()
        {
            //Arrange
            GenerationProfile generationProfile = new GenerationProfile();

            DominionExpansion world = new DominionExpansion { Id = 1, Name = "World" };
            DominionExpansion seaside = new DominionExpansion { Id = 2, Name = "Seaside" };
            DominionExpansion intrigue = new DominionExpansion { Id = 3, Name = "Intrigue" };

            generationProfile.SelectedExpansions.Add(new SelectedExpansionToGenerationProfile(world, 2));
            generationProfile.SelectedExpansions.Add(new SelectedExpansionToGenerationProfile(intrigue));

            ObservableCollection<IsSelectedAndWeightedExpansionDto> isSelectedAndWeightedExpansionDtos = new ObservableCollection<IsSelectedAndWeightedExpansionDto>();
            isSelectedAndWeightedExpansionDtos.Add(new IsSelectedAndWeightedExpansionDto(world));
            isSelectedAndWeightedExpansionDtos.Add(new IsSelectedAndWeightedExpansionDto(seaside));
            IsSelectedAndWeightedExpansionDto intrigueUnselected = new IsSelectedAndWeightedExpansionDto(intrigue) { IsSelected = false, Weight = 0 };
            isSelectedAndWeightedExpansionDtos.Add(intrigueUnselected);

            _testee = new GenerationProfileViewEntity(generationProfile, _messengerMock.Object, isSelectedAndWeightedExpansionDtos);

            //Act
            _testee.LoadProfile();

            //Assert
            _messengerMock.Verify(x => x.NotifyEventTriggered(It.Is<MessageDto>(m => m.Name == Message.ProfileLoaded)), Times.Once);

            _testee.IsSelectedAndWeightedExpansionDto.Single(x => x.DominionExpansion.Name == "World").IsSelected.Should().BeTrue();
            _testee.IsSelectedAndWeightedExpansionDto.Single(x => x.DominionExpansion.Name == "Seaside").IsSelected.Should().BeFalse();
            _testee.IsSelectedAndWeightedExpansionDto.Single(x => x.DominionExpansion.Name == "Intrigue").IsSelected.Should().BeTrue();

            _testee.IsSelectedAndWeightedExpansionDto.Single(x => x.DominionExpansion.Name == "World").Weight.Should().Be(2);
            _testee.IsSelectedAndWeightedExpansionDto.Single(x => x.DominionExpansion.Name == "Seaside").Weight.Should().Be(1);
            _testee.IsSelectedAndWeightedExpansionDto.Single(x => x.DominionExpansion.Name == "Intrigue").Weight.Should().Be(1);
        }

        [Test]
        public void DeleteProfile_MessengerInvoked()
        {
            // Arrange
            GenerationProfile profile = new GenerationProfile { Name = "Profile 1" };
            _testee = new GenerationProfileViewEntity(profile, _messengerMock.Object, new ObservableCollection<IsSelectedAndWeightedExpansionDto>());

            // Act
            _testee.DeleteProfile();

            // Assert
            _messengerMock.Verify(x => x.NotifyEventTriggered(It.Is<MessageDto>(dto => dto.Name == Message.ProfileDeleted && dto.Data.Equals(profile))), Times.Once);
        }
    }
}
