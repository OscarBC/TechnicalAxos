namespace TechnicalAxos_OscarBarrera.Helpers;
public static class AppInfoHelper
{
    public static string GetPackageOrBundleId()
    {
#if ANDROID
        return Android.App.Application.Context.PackageName; // Package Name for Android
#elif IOS
            return Foundation.NSBundle.MainBundle.BundleIdentifier; // Bundle ID for iOS
#else
            return "Unknown Platform Bundle";
#endif
    }
}
