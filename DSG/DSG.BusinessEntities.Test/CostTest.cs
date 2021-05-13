using FluentAssertions;
using NUnit.Framework;

namespace DSG.BusinessEntities.Test
{
    [TestFixture]
    public class CostTest
    {
        private Cost _testee;

        [Test]
        public void Equals_CostsEqual_True()
        {
            // Arrange
            _testee = new Cost(TestDataDefines.Costs.TwoMoneyFourDept.Money.Value, TestDataDefines.Costs.TwoMoneyFourDept.Dept.Value);

            // Act
            bool result = _testee.Equals(TestDataDefines.Costs.TwoMoneyFourDept);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void Equals_MoneyValuesDiffer_False()
        {
            // Arrange
            _testee = new Cost(TestDataDefines.Costs.Four.Money.Value, TestDataDefines.Costs.TwoMoneyFourDept.Dept.Value);

            // Act
            bool result = _testee.Equals(TestDataDefines.Costs.TwoMoneyFourDept);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void Equals_DeptValuesDiffer_False()
        {
            // Arrange
            _testee = new Cost(TestDataDefines.Costs.TwoMoneyFourDept.Money.Value, TestDataDefines.Costs.Four.Dept.Value);

            // Act
            bool result = _testee.Equals(TestDataDefines.Costs.TwoMoneyFourDept);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void Equals_PotionValuesDiffer_False()
        {
            // Arrange
            _testee = new Cost(TestDataDefines.Costs.TwoMoneyFourDept.Money.Value, TestDataDefines.Costs.Four.Dept.Value, true);

            // Act
            bool result = _testee.Equals(TestDataDefines.Costs.TwoMoneyFourDept);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void Clone_ExistingEntity_SameValuesButDifferentReference()
        {
            // Arrange
            Cost initialCost = new Cost(3, 2, false);

            // Act
            Cost clonedCost = initialCost.Clone();

            // Assert
            object.ReferenceEquals(initialCost, clonedCost).Should().BeFalse();
            clonedCost.Money.Value.Should().Be(initialCost.Money.Value);
            clonedCost.Dept.Value.Should().Be(initialCost.Dept.Value);
            clonedCost.Potion.Should().Be(initialCost.Potion);
        }
    }
}
