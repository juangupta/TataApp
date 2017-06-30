using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TataApp.Interfaces;
using TataApp.Models;
using TataApp.Services;
using Xamarin.Forms;

namespace TataApp.ViewModels
{
    public class MainViewModel
    {
        #region Attributes
        private NavigationService navigationService;    
        #endregion

        #region Properties
        public ObservableCollection<MenuItemViewModel> Menu { get; set; }
        public LoginViewModel Login { get; set; }
        public TimesViewModel Times { get; set; }
        public NewTimeViewModel NewTime { get; set; }
        public LocationsViewModel Locations { get; set; }
        public EmployeesViewModel Employees { get; set; }
        public EmployeeDetailViewModel EmployeeDetail { get; set; }
        public ProfileViewModel Profile { get; internal set; }
        public Employee Employee { get; set; }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;

            navigationService = new NavigationService();
            Menu = new ObservableCollection<MenuItemViewModel>();
            Login = new LoginViewModel();
            LoadMenu();
        }
        #endregion

        #region Singleton
        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new MainViewModel();
            }

            return instance;
        }
        #endregion

        #region Methods

        public void RegisterDevice()
        {
            var register = DependencyService.Get<IRegisterDevice>();
            register.RegisterDevice();
        }

        private void LoadMenu()
        {
            Menu.Add(new MenuItemViewModel
            {
                Title = "Register Time",
                Icon = "ic_access_time.png",
                PageName = "TimesPage",

            });
            Menu.Add(new MenuItemViewModel
            {
                Title = "Employees",
                Icon = "ic_people.png",
                PageName = "EmployeesPage",

            });
            Menu.Add(new MenuItemViewModel
            {
                Title = "Locations",
                Icon = "ic_location_on.png",
                PageName = "LocationsPage",

            });
            Menu.Add(new MenuItemViewModel
            {
                Title = "My Profile",
                Icon = "ic_account_circle.png",
                PageName = "ProfilePage",

            });
            Menu.Add(new MenuItemViewModel
            {
                Title = "Close Sesion",
                Icon = "ic_exit_to_app.png",
                PageName = "LoginPage",

            });
        }
        #endregion

        #region Commands
        public ICommand NewTimeCommand
        {
            get { return new RelayCommand(GoNewTime); }
        }


        private async void GoNewTime()
        {
            NewTime = new NewTimeViewModel();
            await navigationService.Navigate("NewTimePage");
        }
        #endregion
    }

}
