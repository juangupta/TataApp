namespace TataApp.ViewModels
{
    using Models;
    using Plugin.Media.Abstractions;
    using Services;
    using System.ComponentModel;
    using Xamarin.Forms;

    public class ProfileViewModel: Employee, INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Attributes
        private DialogService dialogService;
        private ApiService apiService;
        private NavigationService navigationService;
        private bool isRunning;
        private bool isEnabled;
        private ImageSource imageSource;
        private MediaFile file;
        private Employee profile;
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
            profile = mainViewModel.Employee;

            EmployeeId = profile.EmployeeId;
            FirstName = profile.FirstName;
            LastName = profile.LastName;
            EmployeeCode = profile.EmployeeCode;
            DocumentTypeId = profile.DocumentTypeId;
            Document = profile.Document;
            Picture = profile.Picture;
            Email = profile.Email;
            Phone = profile.Phone;
            Address = profile.Address;
            apiService = new ApiService();
            dialogService = new DialogService();
            navigationService = new NavigationService();

            IsEnabled = true;

        } 
        #endregion

    }
}