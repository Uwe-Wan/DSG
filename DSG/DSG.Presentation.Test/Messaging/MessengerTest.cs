using DSG.BusinessEntities.GenerationProfiles;
using DSG.Presentation.Messaging;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace DSG.Presentation.Test.Messaging
{
    [TestFixture]
    public class MessengerTest
    {
        private Messenger _testee;

        [SetUp]
        public void Setup()
        {
            _testee = new Messenger();
        }

        [Test]
        public void Register_MessageRegistered()
        {
            //Arrange
            Action<object> action = c => { };

            //Act
            _testee.Register("Test", action);

            //Assert
            _testee.ActionsByMessageName.Should().HaveCount(1);
            _testee.ActionsByMessageName["Test"].Should().Be(action);
        }

        [Test]
        public void NotifyEventTriggered_TwoActionsInDictionary_CorrectOneInvoked()
        {
            //Arrange
            GenerationProfile profile = new GenerationProfile();
            Action<object> firstAction = c => 
            {
                GenerationProfile genProfile =  (GenerationProfile)c;
                genProfile.PropabilityForShelters = 10;
            };
            Action<object> secondAction = c => 
            {
                GenerationProfile genProfile =  (GenerationProfile)c;
                genProfile.PropabilityForPlatinumAndColony = 10;
            };

            _testee.ActionsByMessageName.Add("First", firstAction);
            _testee.ActionsByMessageName.Add("Second", secondAction);

            _testee.MessagingEvent += _testee.HandleEvent;

            MessageDto messageDto = new MessageDto("Second", profile);

            //Act
            _testee.NotifyEventTriggered(messageDto);

            //Assert
            profile.PropabilityForPlatinumAndColony.Should().Be(10);
            profile.PropabilityForShelters.Should().Be(0);
        }

        [Test]
        public void NotifyEventTriggered_TwoActionsInDictionaryNoMatching_NoExceptionThrown()
        {
            //Arrange
            GenerationProfile profile = new GenerationProfile();
            Action<object> firstAction = c => 
            {
                GenerationProfile genProfile =  (GenerationProfile)c;
                genProfile.PropabilityForShelters = 10;
            };
            Action<object> secondAction = c => 
            {
                GenerationProfile genProfile =  (GenerationProfile)c;
                genProfile.PropabilityForPlatinumAndColony = 10;
            };

            _testee.ActionsByMessageName.Add("First", firstAction);
            _testee.ActionsByMessageName.Add("Second", secondAction);

            _testee.MessagingEvent += _testee.HandleEvent;

            MessageDto messageDto = new MessageDto("NonAvailableMessage", profile);

            //Act
            Action act = () => _testee.NotifyEventTriggered(messageDto);

            //Assert
            act.Should().NotThrow<Exception>();
            profile.PropabilityForPlatinumAndColony.Should().Be(0);
            profile.PropabilityForShelters.Should().Be(0);
        }
    }
}
