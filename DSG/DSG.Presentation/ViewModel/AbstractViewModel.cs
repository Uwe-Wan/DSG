using DSG.Common;
using DSG.Presentation.Services;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DSG.Presentation.ViewModel
{
    public class AbstractViewModel : Notifier, IViewModel
    {
        private INaviService _naviService;
        private bool _isDataLoaded;

        #region Properties

        public INaviService NaviService
        {
            get
            {
                Check.RequireInjected(_naviService, nameof(NaviService), nameof(AbstractViewModel));
                return _naviService;
            }
            set { _naviService = value; }
        }

        public ICommand NavigateToWelcomeScreenCommand { get; private set; }

        public bool IsDataLoaded
        {
            get { return _isDataLoaded; }
            set
            {
                _isDataLoaded = value;
                OnPropertyChanged(nameof(IsDataLoaded));
            }
        }

        #endregion

        public AbstractViewModel()
        {
            NavigateToWelcomeScreenCommand = new RelayCommand(async cmd => await NavigateToAsync(NavigationDestination.WelcomeScreen));
        }

        #region Methods

#pragma warning disable CS1998 // Async method lacks 'await' operator, but it is overwritten in child classes and used asynchronous there
        public virtual async Task OnPageLoadedAsync(params object[] data)
#pragma warning restore CS1998
        {
        }
        
        public virtual async Task NavigateToAsync(NavigationDestination destination)
        {
            await NaviService.NavigateToAsync(destination);
        }


    #endregion
}
}
