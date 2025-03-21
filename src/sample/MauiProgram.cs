using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

#if IOS 
using Firebase;
#endif

namespace MauiFirebaseMessagingSample;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .RegisterFirebaseServices()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }

    private static MauiAppBuilder RegisterFirebaseServices(this MauiAppBuilder builder)
    {
        builder.ConfigureLifecycleEvents(events => {
#if IOS
            events.AddiOS(iOS => iOS.WillFinishLaunching((app, launchOptions) =>
            {
                try
                {
                    MauiFIRApp.AutoConfigure();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return false;
            }));
#endif
#if ANDROID
                events.AddAndroid(android => android.OnCreate((activity, _) =>
                {
                    //CrossFirebase.Initialize(activity);
                    //FirebaseAnalyticsImplementation.Initialize(activity);

                    Firebase.FirebaseSdk.InitializeSDK(activity);

                }));
#endif
        });

        return builder;
    }
}
