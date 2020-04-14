using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Linq;
using DSG.BusinessEntities;
using DSG.DAO.Expansions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace DSG.DAO.Test.Expansions
{
    [TestFixture]
    public class DominionExpansionDaoTest : AbstractDaoSetup
    {
        private DominionExpansionDao _testee;

        [SetUp]
        public void Setup()
        {
            _testee = new DominionExpansionDao();
            CleanDatabase();
            SetupTestDatabase();
        }

        [Test]
        public void GetExpansions_TwoAvailable_TwoReturned()
        {
            //Act
            List<DominionExpansion> result = _testee.GetExpansions();

            //Assert
            result.Should().HaveCount(2);
            result.Select(x => x.Name).Should().Contain("Welt");
            result.Select(x => x.Name).Should().Contain("Seaside");
        }

        [Test]
        public void GetExpansionByName_ExpansionAvailable_ExpansionReturned()
        {
            //Arrange
            string welt = "Welt";

            //Act
            DominionExpansion result = _testee.GetExpansionByName(welt);

            //Assert
            result.Name.Should().Be(welt);
        }

        [Test]
        public void GetExpansionByName_ExpansionNotAvailable_ReturnNull()
        {
            //Arrange
            string darkAges = "Dark Ages";

            //Act
            DominionExpansion result = _testee.GetExpansionByName(darkAges);

            //Assert
            result.Should().BeNull();
        }

        [Test]
        public void GetExpansionByName_SqlInjection_TableNotDeleted()
        {
            //Arrange
            string delete = "Dark Ages; TRUNCATE TABLE DominionExpansion --";

            //Act
            DominionExpansion result = _testee.GetExpansionByName(delete);

            //Assert
            result.Should().BeNull();

            DataContext ctx = new DataContext(ConfigurationManager.AppSettings["DSGConnectionString"]);
            string sqlCmd = "SELECT * FROM DominionExpansion";
            List<DominionExpansion> expansions = ctx.ExecuteQuery<DominionExpansion>(sqlCmd).ToList();
            expansions.Count.Should().NotBe(0);
        }

        [Test]
        public void InsertExpansion_InsertOne_BeInserted()
        {
            //Arrange
            string intrige = "Intrige";

            //Act
            _testee.InsertExpansion(intrige);

            //Assert
            DataContext ctx = new DataContext(ConfigurationManager.AppSettings["DSGConnectionString"]);
            string sqlCmd = "SELECT * FROM DominionExpansion";
            List<string> expansionNames = ctx.ExecuteQuery<DominionExpansion>(sqlCmd).Select(expansion => expansion.Name).ToList();

            expansionNames.Should().HaveCount(3);
            expansionNames.Should().Contain(intrige);
        }

        [Test]
        public void InsertExpansion_SqlInjection_TableNotDeleted()
        {
            //Arrange
            string intrige = "Intrige'); TRUNCATE TABLE DominionExpansion --";

            //Act
            _testee.InsertExpansion(intrige);

            //Assert
            DataContext ctx = new DataContext(ConfigurationManager.AppSettings["DSGConnectionString"]);
            string sqlCmd = "SELECT * FROM DominionExpansion";
            List<string> expansionNames = ctx.ExecuteQuery<DominionExpansion>(sqlCmd).Select(expansion => expansion.Name).ToList();

            expansionNames.Should().HaveCount(3);
        }
    }
}
