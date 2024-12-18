using System.Globalization;

namespace TechnicalAxos_OscarBarrera;

public partial class App : Application
{
	public App()
	{
		 // Set the culture to en-US, to show commas instead of dots for decimal numbers
        CultureInfo culture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;

		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
}