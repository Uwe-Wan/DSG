using DSG.WinUI.Behavior;
using FluentAssertions;
using NUnit.Framework;
using System.Windows;

namespace DSG.WinUI.Test.Behavior
{
    [TestFixture]
    public class ProfileDropDownBehaviorTest
    {
        private ProfileDropDownBehavior _testee;

        [Test]
        public void AssociatedObject_Click_NameSet()
        {
            // Arrange
            _testee = new ProfileDropDownBehavior();
            _testee.SelectedProfileName = "Before Incovation";

            // Act
            _testee.AssociatedObject_Click(new object(), new RoutedEventArgs());

            // Assert
            _testee.SelectedProfileName.Should().Be("Insert Name");
        }
    }
}
