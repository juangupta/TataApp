namespace TataApp.ViewModels
{
    using Models;
    using Plugin.Media.Abstractions;
    using Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using Xamarin.Forms;
    using System;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Plugin.Media;
    using Helpers;

    public class ProfileViewModel: Employee, INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Attributes
        private DialogService dialogService;
        private ApiService apiService;
        private DataService dataService;
        private NavigationService navigationService;
        private bool isRunning;
        private bool isEnabled;
        private ImageSource imageSource;
        private MediaFile file;
        private Employee profile;
        #endregion

        #region Properties

        public ObservableCollection<DocumentType> DocumentTypes
        {
            get;
            set;
        }
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

        public ImageSource ImageSource
        {
            set
            {
                if (imageSource != value)
                {
                    imageSource = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ImageSource"));
                }
            }
            get
            {
                return imageSource;
            }
        }
        #endregion

        #region Constructors
        public ProfileViewModel()
        {
            var mainViewModel = MainViewModel.GetInstance();
            DocumentTypes = new ObservableCollection<DocumentType>();
            apiService = new ApiService();
            dataService = new DataService();
            dialogService = new DialogService();
            navigationService = new NavigationService();
            GetDocumentTypes();
            profile = mainViewModel.Employee;

            EmployeeId = profile.EmployeeId;
            FirstName = profile.FirstName;
            LastName = profile.LastName;
            EmployeeCode = profile.EmployeeCode;
            DocumentTypeId = profile.DocumentTypeId;
            LoginTypeId = profile.LoginTypeId;
            AccessToken = profile.AccessToken;
            TokenType = profile.TokenType;
            TokenExpires = profile.TokenExpires;
            Password = profile.Password;
            IsRemembered = profile.IsRemembered;
            Document = profile.Document;
            Picture = profile.Picture;
            Email = profile.Email;
            Phone = profile.Phone;
            Address = profile.Address;

            IsEnabled = true;

        }
        #endregion

        #region Commands
        public ICommand SaveProfileCommand
        {
            get { return new RelayCommand(SaveProfile); }
        }

        private async void SaveProfile()
        {
            if (string.IsNullOrEmpty(FirstName))
            {
                await dialogService.ShowMessage("Error", "You must enter a first name.");
                return;
            }

            if (string.IsNullOrEmpty(LastName))
            {
                await dialogService.ShowMessage("Error", "You must enter a last name.");
                return;
            }

            if (EmployeeCode == 0)
            {
                await dialogService.ShowMessage("Error", "You must enter an employee code.");
                return;
            }

            if (string.IsNullOrEmpty(Document))
            {
                await dialogService.ShowMessage("Error", "You must enter a document.");
                return;
            }

            if (string.IsNullOrEmpty(Email))
            {
                await dialogService.ShowMessage("Error", "You must enter an email.");
                return;
            }

            if (string.IsNullOrEmpty(Phone))
            {
                await dialogService.ShowMessage("Error", "You must enter a phone.");
                return;
            }

            if (string.IsNullOrEmpty(Address))
            {
                await dialogService.ShowMessage("Error", "You must enter an address.");
                return;
            }


            byte[] imageArray = null;
            if (file != null)
            {
                imageArray = FilesHelper.ReadFully(file.GetStream());
                file.Dispose();
            }

            var newProfile = new Employee
            {
                EmployeeId = EmployeeId,
                FirstName = FirstName,
                LastName = LastName,
                EmployeeCode = EmployeeCode,
                DocumentTypeId = DocumentTypeId,
                LoginTypeId = LoginTypeId,
                Password = Password,
                Document = Document,
                ImageArray = imageArray,
                Email = Email,
                Phone = Phone,
                Address = Address,
            };
            IsRunning = true;
            IsEnabled = false;

           var urlAPI = Application.Current.Resources["URLAPI"].ToString();
           var response = await apiService.Put<Employee>(
                urlAPI,
                "/api",
                "/Employees",
                profile.TokenType,
                profile.AccessToken,
                newProfile);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }
             
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Employee = this;
            dataService.DeleteAllAndInsert(this);
            await navigationService.Back();
        }
        public ICommand TakePictureCommand
        {
            get { return new RelayCommand(TakePicture); }
        }

        private async void TakePicture()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable ||
                !CrossMedia.Current.IsTakePhotoSupported)
            {
                await dialogService.ShowMessage(
                    "No Camera",
                    ":( No camera available.");
                return;
            }

            file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg",
                PhotoSize = PhotoSize.Small,
            });

            IsRunning = true;

            if (file != null)
            {
                ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }

            IsRunning = false;
        }

        public ICommand PickPictureCommand
        {
            get { return new RelayCommand(PickPicture); }
        }

        private async void PickPicture()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await dialogService.ShowMessage("Photos Not Supported", ":( Permission not granted to photos.");
                return;
            }

            file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
            });

            IsRunning = true;
            if (file != null)
            {
                ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    //file.Dispose();
                    return stream;
                });
            }

            IsRunning = false;

        }
        #endregion

        #region Methods
        private async void GetDocumentTypes()
        {
            var checkConnetion = await apiService.CheckConnection();
            if (!checkConnetion.IsSuccess)
            {
                await dialogService.ShowMessage("Error", checkConnetion.Message);
                return;
            }

            var urlAPI = Application.Current.Resources["URLAPI"].ToString();
            var mainViewModel = MainViewModel.GetInstance();
            var employee = mainViewModel.Employee;
            var response = await apiService.GetList<DocumentType>(
                urlAPI,
                "/api",
                "/DocumentTypes",
                profile.TokenType,
                profile.AccessToken);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = false;
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            LoadDocumentTypes((List<DocumentType>)response.Result);
        }

        private void LoadDocumentTypes(List<DocumentType> documentTypes)
        {
            DocumentTypes.Clear();
            foreach (var documentType in documentTypes)
            {
                DocumentTypes.Add(new DocumentType
                {
                    Description = documentType.Description,
                    DocumentTypeId = documentType.DocumentTypeId,
                });
            }

            var mainViewModel = MainViewModel.GetInstance();
            DocumentTypeId = mainViewModel.Employee.DocumentTypeId;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DocumentTypeId"));
        }
        #endregion

    }
}