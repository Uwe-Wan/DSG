using DSG.Presentation.Services;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DSG.Presentation.ViewModel
{
    public class WelcomeScreenViewModel : IViewModel
    {
        private INaviService _naviService;

        public INaviService NaviService
        {
            get { return _naviService; }
            set { _naviService = value; }
        }

        public WelcomeScreenViewModel()
        {
            NavigateToManageSetsScreenCommand = new RelayCommand(p => NavigateTo(NavigationDestination.ManageSets));
        }

        public ICommand NavigateToManageSetsScreenCommand { get; private set; }

        public async Task OnPageLoadedAsync(NavigationDestination navigationDestination, params object[] data)
        {
        }

        private void NavigateTo(NavigationDestination destination)
        {
            NaviService.NavigateToAsync(destination);
        }
    }
}
