using DSG.WinUI.Behavior;
using NUnit.Framework;
using System.Windows;
using System.Windows.Controls;
using FluentAssertions;
using System;
using System.Threading;

namespace DSG.WinUI.Test.Behavior
{
    [TestFixture]
    public class DropDownButtonContextMenuBehaviorTest
    {
        private DropDownButtonContextMenuBehavior _testee;
        private string _errorMessage;

        [Test]
        public void TestExecutor()
        {
            _testee = new DropDownButtonContextMenuBehavior();

            Thread thread = new Thread(AssociatedObject_Click);
            thread.SetApartmentState(ApartmentState.STA);

            thread.Start();
            thread.Join();

            _errorMessage.Should().BeNull();
        }

        private void AssociatedObject_Click()
        {
            Button parentButton = new Button();
            parentButton.ContextMenu = new ContextMenu { IsOpen = true };
            Button button = new Button();
            parentButton.ContextMenu.Items.Add(button);

            //Act
            _testee.AssociatedObject_Click(button, new RoutedEventArgs());

            //Assert
            try
            {
                ContextMenu contextMenu = (ContextMenu)parentButton.ContextMenu;
                contextMenu.IsOpen.Should().BeFalse();
            }
            catch(Exception ex)
            {
                _errorMessage = ex.Message;
            }
        }
    }
}
