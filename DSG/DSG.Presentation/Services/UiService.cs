using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DSG.Presentation.Services
{
    public class UiService : IUiService
    {
        public void ShowMessageBox(string message)
        {
            MessageBox.Show(message);
        }

        public void ShowErrorMessage(string message, string caption)
        {
            MessageBoxButton buttons = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Error;
            MessageBox.Show(message, caption, buttons, icon);
        }

        public bool AskForConfirmationMessage(string message, string caption)
        {
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Exclamation;
            return MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes;
        }

        public bool ShowWarningMessage(string message, string caption)
        {
            MessageBoxButton buttons = MessageBoxButton.OKCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;
            return MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.OK;
        }
    }
}
