using FluentAssertions;
using NUnit.Framework;

namespace DSG.BusinessEntities.Test
{
    [TestFixture]
    public class CardTest
    {
        private Card _testee;

        [Test]
        public void Equals_EqualCards_True()
        {
            // Arrange
            _testee = new Card { Name = TestDataDefines.Cards.BridgeTroll.Name, DominionExpansionId = TestDataDefines.Cards.BridgeTroll.DominionExpansionId };

            // Act
            bool result = _testee.Equals(TestDataDefines.Cards.BridgeTroll);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void Equals_NamesDiffer_False()
        {
            // Arrange
            _testee = new Card { Name = TestDataDefines.Cards.Relic.Name, DominionExpansionId = TestDataDefines.Cards.BridgeTroll.DominionExpansionId };

            // Act
            bool result = _testee.Equals(TestDataDefines.Cards.BridgeTroll);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void Equals_ExpansionIdsDiffer_False()
        {
            // Arrange
            _testee = new Card { Name = TestDataDefines.Cards.Relic.Name, DominionExpansionId = TestDataDefines.Cards.BridgeTroll.DominionExpansionId };

            // Act
            bool result = _testee.Equals(TestDataDefines.Cards.BridgeTroll);

            // Assert
            result.Should().BeFalse();
        }
    }
}
