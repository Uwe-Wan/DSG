using DSG.Presentation.Services;
using Spring.Context;
using Spring.Context.Support;
using System.Windows;


namespace DSG.WinUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            IApplicationContext context = ContextRegistry.GetContext();
            INaviService naviService = context.GetObject<INaviService>("naviService");
            naviService.NavigateToAsync(NavigationDestination.WelcomeScreen);
        }
    }
}
