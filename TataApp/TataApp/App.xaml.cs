﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TataApp.Models;
using TataApp.Services;
using TataApp.ViewModels;
using TataApp.Views;
using Xamarin.Forms;

namespace TataApp
{
    public partial class App : Application
    {
        #region Attributes
        private DataService dataService;
        #endregion

        #region Properties
        public static NavigationPage Navigator { get; internal set; }
        public static MasterPage Master { get; internal set; }
        #endregion

        #region Constructors
        public App()
        {
            InitializeComponent();
            dataService = new DataService();
            var employee = dataService.First<Employee>(false);
            if (employee != null &&
                employee.IsRemembered &&
                employee.TokenExpires > DateTime.Now.ToLocalTime())
            {
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Employee = employee;
                mainViewModel.RegisterDevice();
                MainPage = new MasterPage();
            }
            else
            {
                MainPage = new LoginPage();
            }
            
        }
        #endregion

        #region Methods

        public static Action HideLoginView
        {
            get
            {
                return new Action(() => App.Current.MainPage = new LoginPage());
            }
        }

        public static async Task NavigateToProfile(FacebookResponse profile)
        {
            var apiService = new ApiService();
            var dialogService = new DialogService();
            var navigationService = new NavigationService();
            var dataService = new DataService();

            var checkConnetion = await apiService.CheckConnection();
            if (!checkConnetion.IsSuccess)
            {
                await dialogService.ShowMessage("Error", checkConnetion.Message);
                return;
            }

            var urlAPI = Application.Current.Resources["URLAPI"].ToString();
            var token = await apiService.LoginFacebook(
                urlAPI,
                "/api",
                "/Employees/LoginFacebook",
                profile);
            if (token == null)
            {
                App.Current.MainPage = new LoginPage();
                return;
            }

            var response = await apiService.GetEmployeeByEmailOrCode(
                urlAPI,
                "/api",
                "/Employees/GetGetEmployeeByEmailOrCode",
                token.TokenType,
                token.AccessToken,
                token.UserName);

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", "Problem ocurred retrieving user information, try latter.");
                return;
            }

            var employee = (Employee)response.Result;
            employee.AccessToken = token.AccessToken;
            employee.IsRemembered = true;
            employee.Password = profile.Id;
            employee.TokenExpires = token.Expires;
            employee.TokenType = token.TokenType;
            dataService.DeleteAllAndInsert(employee);

            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Employee = employee;
            mainViewModel.RegisterDevice();
            navigationService.SetMainPage("MasterPage");
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        } 
        #endregion
    }
}
