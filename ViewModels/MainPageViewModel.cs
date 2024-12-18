using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TechnicalAxos_OscarBarrera.Helpers;
using TechnicalAxos_OscarBarrera.Models;
using TechnicalAxos_OscarBarrera.Services;

namespace TechnicalAxos_OscarBarrera.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly CountriesApiService _countriesApiService;
        private string _bundleID;
        private ImageSource _imageSource;
        private ObservableCollection<CountryInfo> _countries;

        public MainPageViewModel()
        {
            _countriesApiService = new CountriesApiService();
            PickImageCommand = new Command(async () => await PickImageAsync());

            InitializeViewModel();
        }

        #region Properties

        public string BundleID
        {
            get => _bundleID;
            set => SetProperty(ref _bundleID, value);
        }

        public ImageSource ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        public ObservableCollection<CountryInfo> Countries
        {
            get => _countries;
            set => SetProperty(ref _countries, value);
        }

        public ICommand PickImageCommand { get; }

        #endregion

        #region Initialization Methods

        private void InitializeViewModel()
        {
            SetBundleID();
            SetDefaultImage();
            LoadCountriesAsync();
        }

        private void SetBundleID()
        {
            BundleID = AppInfoHelper.GetPackageOrBundleId();
        }

        private void SetDefaultImage()
        {
            const string defaultImageUrl = "https://random.dog/af70ad75-24af-4518-bf03-fec4a997004c.jpg";
            ImageSource = ImageSource.FromUri(new Uri(defaultImageUrl));
        }

        private async void LoadCountriesAsync()
        {
            try
            {
                var countries = await _countriesApiService.GetAllCountriesAsync();
                Countries = new ObservableCollection<CountryInfo>(countries);
                Console.WriteLine($"Loaded {countries.Count} countries.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading countries: {ex.Message}");
            }
        }

        #endregion

        #region Command Handlers

        private async Task PickImageAsync()
        {
            try
            {
                if (!await CheckAndRequestPhotoLibraryPermissionAsync()) return;

                var result = await MediaPicker.Default.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Please pick an image"
                });

                if (result != null)
                {
                    await SetImageFromResultAsync(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error picking image: {ex.Message}");
            }
        }

        #endregion

        #region Helper Methods

        private async Task<bool> CheckAndRequestPhotoLibraryPermissionAsync()
        {
            var photosPermission = await Permissions.CheckStatusAsync<Permissions.Photos>();
            if (photosPermission != PermissionStatus.Granted && photosPermission != PermissionStatus.Restricted)
            {
                photosPermission = await Permissions.RequestAsync<Permissions.Photos>();
            }

            return photosPermission == PermissionStatus.Granted;
        }

        private async Task SetImageFromResultAsync(FileResult result)
        {
            var stream = await result.OpenReadAsync();
            ImageSource = ImageSource.FromStream(() => stream);

            Console.WriteLine($"Image path: {result.FullPath}");
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetProperty<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(backingField, value))
            {
                backingField = value;
                OnPropertyChanged(propertyName);
            }
        }

        #endregion
    }
}
