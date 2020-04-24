using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSG.Presentation.Services
{
    public interface IUiService
    {
        void ShowMessageBox(string message);

        void ShowErrorMessage(string message, string caption);

        bool AskForConfirmationMessage(string message, string caption);

        bool ShowWarningMessage(string message, string caption);
    }
}
