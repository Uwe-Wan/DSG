using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSG.Presentation.Services
{
    public interface INavigate
    {
        Task OnPageLoadedAsync(NavigationDestination navigationDestination, params object[] data);
    }
}
