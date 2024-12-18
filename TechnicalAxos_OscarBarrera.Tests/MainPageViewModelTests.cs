using Moq;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TechnicalAxos_OscarBarrera.Models;
using TechnicalAxos_OscarBarrera.Services;
using TechnicalAxos_OscarBarrera.ViewModels;
using Xunit;

public class MainPageViewModelTests
{
    private readonly Mock<ICountriesApiService> _countriesApiServiceMock;
    private readonly Mock<IMediaPicker> _mediaPickerServiceMock;
    private readonly Mock<IFileService> _mockFileService;
    private readonly MainPageViewModel _viewModel;

    public MainPageViewModelTests()
    {
        // Mock the CountriesApiService
        _countriesApiServiceMock = new Mock<ICountriesApiService>();
        _mediaPickerServiceMock = new Mock<IMediaPicker>();
        _mockFileService = new Mock<IFileService>();

        // Replace actual service with mocked service
        _viewModel = new MainPageViewModel(_countriesApiServiceMock.Object, _mediaPickerServiceMock.Object, _mockFileService.Object);
    }

    [Fact]
    public async Task InitializeViewModel_ShouldSetDefaultImageAndLoadCountries()
    {
        // Arrange
        var mockCountries = JsonConvert.DeserializeObject<List<CountryInfo>>(@"[
            {
                ""name"": {
                    ""common"": ""Country 1""
                }
            },
            {
                ""name"": {
                    ""common"": ""Country 2""
                }
            }
        ]");

        // Mock the GetAllCountriesAsync method to return the mock countries
        _countriesApiServiceMock
            .Setup(service => service.GetAllCountriesAsync())
            .ReturnsAsync(mockCountries!);

        // Act
        await Task.Run(() => _viewModel.LoadCountriesAsync());

        // Assert
        Assert.NotNull(_viewModel.ImageSource);
        Assert.Equal(mockCountries.Count, _viewModel.Countries.Count);
    }

    [Fact]
    public async Task LoadCountriesAsync_ShouldHandleExceptionGracefully()
    {
        // Arrange
        _countriesApiServiceMock
            .Setup(service => service.GetAllCountriesAsync())
            .ThrowsAsync(new Exception("API Error"));

        // Act
        var exception = await Record.ExceptionAsync(() => Task.Run(() => _viewModel.LoadCountriesAsync()));

        // Assert
        Assert.Null(exception); // No exception should be thrown
    }

    [Fact]
    public void PropertyChanged_ShouldBeRaised_WhenPropertyChanges()
    {
        // Arrange
        var propertyChangedRaised = false;
        _viewModel.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(_viewModel.BundleID))
            {
                propertyChangedRaised = true;
            }
        };

        // Act
        _viewModel.BundleID = "New Bundle ID";

        // Assert
        Assert.True(propertyChangedRaised);
    }

    [Fact]
    public async Task PickImageCommand_ShouldSetImageSource()
    {

        var defaultImage = _viewModel.ImageSource;

        var mockFileResult = new Mock<FileResult>("mockFile.path");
        var fileStream = new MemoryStream(new byte[] { 1, 2, 3 });

        // Mock FileService
        _mockFileService
            .Setup(service => service.OpenReadAsync(It.IsAny<FileResult>()))
            .ReturnsAsync(fileStream);

        // Mock MediaPickerService behavior
        _mediaPickerServiceMock
            .Setup(service => service.PickPhotoAsync(It.IsAny<MediaPickerOptions>()))
            .ReturnsAsync(mockFileResult.Object);

        // Act
        await Task.Run(() => _viewModel.PickImageCommand.Execute(null));

        // Assert
        Assert.NotNull(_viewModel.ImageSource);
        Assert.NotEqual(defaultImage, _viewModel.ImageSource);
    }

}
