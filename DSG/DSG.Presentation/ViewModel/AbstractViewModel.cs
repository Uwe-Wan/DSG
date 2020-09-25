using DSG.Common;
using DSG.Presentation.Services;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DSG.Presentation.ViewModel
{
    public class AbstractViewModel : Notifier, IViewModel
    {
        private INaviService _naviService;

        #region Properties

        public INaviService NaviService
        {
            get
            {
                Check.RequireInjected(_naviService, nameof(NaviService), nameof(ManageCardsViewModel));
                return _naviService;
            }
            set { _naviService = value; }
        }

        public ICommand NavigateToWelcomeScreenCommand { get; private set; }

        #endregion

        public AbstractViewModel()
        {
            NavigateToWelcomeScreenCommand = new RelayCommand(cmd => NavigateTo(NavigationDestination.WelcomeScreen));
        }

        #region Methods

        public virtual async Task OnPageLoadedAsync(params object[] data)
        {
        }

        protected void NavigateTo(NavigationDestination destination)
        {
            NaviService.NavigateToAsync(destination);
        }

        #endregion
    }
}
