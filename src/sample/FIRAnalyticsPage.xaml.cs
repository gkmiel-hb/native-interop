#if IOS
using Firebase;
#endif

#if ANDROID
using MauiFirebaseMessagingSample.Platforms.Android;
using Firebase.Sample.Platforms.Android.Extensions;
#endif

namespace MauiFirebaseMessagingSample;

public partial class FIRAnalyticsPage : ContentPage
{
    public FIRAnalyticsPage()
    {
        InitializeComponent();
    }

    async void OnAnalyticsClicked (object sender, EventArgs e)
    {
        try
        {
#if IOS
            AppTabbedPage.ConfigureFirebase(this);

            MauiFIRAnalytics.LogEvent("OnAnalyticsClicked", new Foundation.NSDictionary("param1", "value1"));
            var appInstanceId = await MauiFIRAnalytics.GetAppInstanceIdAsync();
            await DisplayAlert($"Logged event to app ID {appInstanceId}", "", "OK");
#endif
#if ANDROID

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("param1", "value1");
            Firebase.FirebaseSdk.LogEvent("OnAnalyticsClicked", parameters.ToBundle());

            string appInstanceId = "";
            var appInstanceTask = Firebase.FirebaseSdk.GetAppInstanceId("fake");
            appInstanceTask.AddOnCompleteListener(new OnCompleteListener(async (task) => 
            {
                if (task.IsSuccessful)
                {
                    if(task.Result != null) {
                        appInstanceId = Convert.ToString(task.Result);
                    }
                    await DisplayAlert($"Logged event to app ID {appInstanceId}", "", "OK");
                }
                else
                {
                    // Handle initialization failure
                    Exception exception = task.Exception;
                    //Log.Error("MainActivity", "Error initializing CastContext: " + exception);
                }
            }));

            //var appInstanceId = Firebase.FirebaseSdk.GetAppInstanceId();


#endif
        }
        catch (Exception ex)
        {
            await DisplayAlert("Unable to log event!", ex.Message, "OK");
        }
    }

}
