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
using Moq;
using System.Data.Entity;

namespace DSG.DAO.Test.Expansions
{
    /// <summary>
    /// Testing the DAO methods. For help on mocking the context see here: https://docs.microsoft.com/de-de/ef/ef6/fundamentals/testing/mocking
    /// </summary>
    [TestFixture]
    public class DominionExpansionDaoTest
    {
        private DominionExpansionDao _testee;
        private Mock<CardManagementDbContext> _ctxMock;

         [SetUp]
        public void Setup()
        {
            _testee = new DominionExpansionDao();

            DominionExpansion world = new DominionExpansion { Id = 1, Name = "World" };
            DominionExpansion seaside = new DominionExpansion { Id = 2, Name = "Seaside" };
            IQueryable<DominionExpansion> expansions = new List<DominionExpansion> { world, seaside }.AsQueryable();

            Mock<DbSet<DominionExpansion>> expansionMockSet = new Mock<DbSet<DominionExpansion>>();
            expansionMockSet.As<IQueryable<DominionExpansion>>().Setup(set => set.GetEnumerator()).Returns(expansions.GetEnumerator());

            _ctxMock = new Mock<CardManagementDbContext>();
            _ctxMock.Setup(ctx => ctx.DominionExpansion.Include("ContainedCards")).Returns(expansionMockSet.Object);

            _testee.Ctx = _ctxMock.Object;
        }

        [Test]
        public void GetExpansions_TwoAvailable_TwoReturned()
        {
            //Act
            List<DominionExpansion> result = _testee.GetExpansions();

            //Assert
            result.Should().HaveCount(2);
            result.Select(x => x.Name).Should().Contain("World");
            result.Select(x => x.Name).Should().Contain("Seaside");
        }
    }
}
