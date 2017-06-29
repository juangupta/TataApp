﻿using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TataApp.Models;
using TataApp.Services;
using Xamarin.Forms;

namespace TataApp.ViewModels
{
    public class EmployeeDetailViewModel : Employee, INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Attributes
        ApiService apiService;
        DialogService dialogService;
        NavigationService navigationService;
        DataService dataService;
        bool isRunning;
        bool isEnabled;
        Employee employee;
        string message;
        #endregion

        #region Properties
        public bool IsRunning
        {
            set
            {
                if (isRunning != value)
                {
                    isRunning = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRunning"));
                }
            }
            get
            {
                return isRunning;
            }
        }

        public bool IsEnabled
        {
            set
            {
                if (isEnabled != value)
                {
                    isEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsEnabled"));
                }
            }
            get
            {
                return isEnabled;
            }
        }

        public string Message
        {
            set
            {
                if (message != value)
                {
                    message = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Message"));
                }
            }
            get
            {
                return message;
            }
        }
        #endregion

        public EmployeeDetailViewModel(Employee employee)
        {
            this.employee = employee;

            FirstName = employee.FirstName;
            LastName = employee.LastName;
            EmployeeCode = employee.EmployeeCode;
            DocumentTypeId = employee.DocumentTypeId;
            LoginTypeId = employee.LoginTypeId;
            Document = employee.Document;
            Picture = employee.Picture;
            Email = employee.Email;
            Phone = employee.Phone;
            Address = employee.Address;

            apiService = new ApiService();
            dialogService = new DialogService();
            navigationService = new NavigationService();
            dataService = new DataService();

            IsEnabled = true;

        }

        #region Commands
        public ICommand SendMessageCommand
        {
            get { return new RelayCommand(SendMessage); }
        }

        async void SendMessage()
        {
            if (string.IsNullOrEmpty(Message))
            {
                await dialogService.ShowMessage("Error", "You must enter a message to send.");
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var checkConnetion = await apiService.CheckConnection();
            if (!checkConnetion.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage("Error", checkConnetion.Message);
                return;
            }

        }
        #endregion
    }
}
