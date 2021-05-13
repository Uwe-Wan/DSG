using FluentAssertions;
using NUnit.Framework;

namespace DSG.BusinessEntities.Test
{
    [TestFixture]
    public class IsSelectedAndWeightedExpansionDtoTest
    {
        private IsSelectedAndWeightedExpansionDto _testee;

        [Test]
        public void IsSelected_Checked_WeightChangedToOne()
        {
            // Arrange
            _testee = new IsSelectedAndWeightedExpansionDto(new DominionExpansion(), 0) { IsSelected = false };

            // Act
            _testee.IsSelected = true;

            // Assert
            _testee.Weight.Should().Be(1);
        }

        [Test]
        public void IsSelected_Unchecked_WeightChangedToZero()
        {
            // Arrange
            _testee = new IsSelectedAndWeightedExpansionDto(new DominionExpansion(), 1);

            // Act
            _testee.IsSelected = false;

            // Assert
            _testee.Weight.Should().Be(0);
        }
    }
}
