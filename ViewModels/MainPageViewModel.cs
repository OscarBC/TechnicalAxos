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
        private readonly ICountriesApiService _countriesApiService;
        private readonly IMediaPicker _mediaPickerService;
        private readonly IFileService _fileService;
        private string _bundleID;
        private ImageSource _imageSource;
        private ObservableCollection<CountryInfo> _countries;
        private bool _isLoading;


        public MainPageViewModel()
        {
            _countriesApiService = new CountriesApiService();
            _mediaPickerService = MediaPicker.Default;
            _fileService = new FileService();
            PickImageCommand = new Command(async () => await PickImageAsync());
            InitializeViewModel();
        }
        public MainPageViewModel(ICountriesApiService api, IMediaPicker mediaPickerService, IFileService fileService)
        {
            _countriesApiService = api;
            _mediaPickerService = mediaPickerService;
            _fileService = fileService;
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

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        #endregion

        #region Initialization Methods

        private void InitializeViewModel()
        {
            SetBundleID();
            SetDefaultImage();
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

        #endregion

        #region Public Methods

        public async void LoadCountriesAsync()
        {
            try
            {
                IsLoading = true;
                var countries = await _countriesApiService.GetAllCountriesAsync();
                Countries = new ObservableCollection<CountryInfo>(countries);
                IsLoading = false;
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

#if __IOS__ || __ANDROID__
                if (!await CheckAndRequestPhotoLibraryPermissionAsync()) return;
#endif

                var result = await _mediaPickerService.PickPhotoAsync(new MediaPickerOptions
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
            var stream = await _fileService.OpenReadAsync(result);
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
