using DSG.BusinessComponents.GenerationProfiles;
using DSG.BusinessEntities.GenerationProfiles;
using DSG.DAO.GenerationProfiles;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DSG.BusinessComponentsTest.GenerationProfiles
{
    [TestFixture]
    public class GenerationProfileBcTest
    {
        private GenerationProfileBc _testee;
        private Mock<IGenerationProfileDao> _generationProfileDaoMock;

        [SetUp]
        public void Setup()
        {
            _testee = new GenerationProfileBc();

            _generationProfileDaoMock = new Mock<IGenerationProfileDao>();
            _testee.GenerationProfileDao = _generationProfileDaoMock.Object;
        }

        [Test]
        public void GetGenerationProfiles_DependentDaoMethodInvoked()
        {
            //Arrange
            _generationProfileDaoMock.Setup(x => x.GetGenerationProfiles()).Returns(new List<GenerationProfile> { new GenerationProfile() });

            //Act
            List<GenerationProfile> result = _testee.GetGenerationProfiles();

            //Assert
            result.Should().HaveCount(1);
            _generationProfileDaoMock.Verify(x => x.GetGenerationProfiles(), Times.Once);
        }

        [Test]
        public void InsertGenerationProfile_DependentDaoMethodInvoked()
        {
            //Assert
            GenerationProfile profile = new GenerationProfile();

            //Act
            _testee.InsertGenerationProfile(profile);

            //Assert
            _generationProfileDaoMock.Verify(x => x.InsertGenerationProfile(profile), Times.Once);
        }
    }
}
