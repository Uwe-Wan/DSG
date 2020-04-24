﻿using DSG.Presentation.Services;
using DSG.Presentation.ViewModel;
using Spring.Context;
using Spring.Context.Support;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using DSG.WinUI.ManagementScreens;

namespace DSG.WinUI.Services
{
    public class NaviService : INaviService
    {
        private NavigationService _navigationService;
        private IApplicationContext _context;

        private NavigationService NavigationService
        {
            get
            {
                if(_navigationService == null)
                {
                    _navigationService = (Application.Current.MainWindow as MainWindow)?.Frame.NavigationService;
                }

                return _navigationService;
            }
        }

        private IApplicationContext Context
        {
            get
            {
                if(_context == null)
                {
                    _context = ContextRegistry.GetContext();
                }

                return _context;
            }
        }

        public async Task NavigateToAsync(NavigationDestination destination, params object[] data)
        {
            switch (destination)
            {
                case NavigationDestination.ManageSets:
                    {
                        ManageSetsViewModel viewModel = Context.GetObject<ManageSetsViewModel>("manageSetsViewModel");
                        ManageSets view = new ManageSets();
                        view.Title = Properties.Resources.ManageSets;
                        await NavigateToDestination(view, viewModel, NavigationDestination.ManageSets, data);
                        break;
                    }

                case NavigationDestination.WelcomeScreen:
                    {
                        WelcomeScreenViewModel viewModel = Context.GetObject<WelcomeScreenViewModel>("welcomeScreenViewModel");
                        WelcomeScreen view = new WelcomeScreen();
                        await NavigateToDestination(view, viewModel, NavigationDestination.WelcomeScreen, data);
                        break;
                    }

                case NavigationDestination.ManageCards:
                    {
                        ManageCardsViewModel viewModel = Context.GetObject<ManageCardsViewModel>("manageCardsViewModel");
                        ManageCards view = new ManageCards();
                        await NavigateToDestination(view, viewModel, NavigationDestination.ManageCards, data);
                        break;
                    }

                default:
                    throw new ArgumentOutOfRangeException(nameof(destination));
            }
        }

        private async Task NavigateToDestination(FrameworkElement view, IViewModel viewModel, NavigationDestination navigationDestination, params object[] data)
        {
            await viewModel.OnPageLoadedAsync(data);
            view.DataContext = viewModel;
            NavigationService.Navigate(view);
        }
    }
}
