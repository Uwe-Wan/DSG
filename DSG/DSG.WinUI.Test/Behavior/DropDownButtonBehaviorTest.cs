using DSG.WinUI.Behavior;
using NUnit.Framework;
using System.Windows;
using System.Windows.Controls;
using FluentAssertions;
using System;
using System.Threading;
using System.Windows.Controls.Primitives;

namespace DSG.WinUI.Test.Behavior
{
    [TestFixture]
    public class DropDownButtonBehaviorTest
    {
        private DropDownButtonBehavior _testee;
        private string _errorMessage;

        [Test]
        public void TestExecutor()
        {
            _testee = new DropDownButtonBehavior();

            Thread thread = new Thread(AssociatedObject_Click);
            thread.SetApartmentState(ApartmentState.STA);

            thread.Start();
            thread.Join();

            _errorMessage.Should().BeNull();
        }

        private void AssociatedObject_Click()
        {
            Button button = new Button();
            button.ContextMenu = new ContextMenu();

            //Act
            _testee.AssociatedObject_Click(button, new RoutedEventArgs());

            //Assert
            try
            {
                button.ContextMenu.IsOpen.Should().BeTrue();
                button.ContextMenu.PlacementTarget.Should().Be(button);
                button.ContextMenu.Placement.Should().Be(PlacementMode.Bottom);
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
            }
        }
    }
}
