namespace TechnicalAxos_OscarBarrera.Views;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

		_viewModel.LoadCountriesAsync();
	}


}