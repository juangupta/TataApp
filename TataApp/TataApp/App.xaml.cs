﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TataApp.Views;
using Xamarin.Forms;

namespace TataApp
{
    public partial class App : Application
    {
        public static NavigationPage Navigator { get; internal set; }
        public App()
        {
            InitializeComponent();

            MainPage = new LoginPage();
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
    }
}
