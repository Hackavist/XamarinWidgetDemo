using System;
using UserNotifications;
using WidgetDemo.Services.LocalNotificationService;
using Xamarin.Forms;

namespace WidgetDemo.iOS.Services.LocalNotificationService
{
    public class IosNotificationReceiver : UNUserNotificationCenterDelegate
    {
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification,
            Action<UNNotificationPresentationOptions> completionHandler)
        {
            DependencyService.Get<ILocalNotificationService>().ReceiveNotification(notification.Request.Content.Title, notification.Request.Content.Body);

            // alerts are always shown for demonstration but this can be set to "None"
            // to avoid showing alerts if the app is in the foreground
            completionHandler(UNNotificationPresentationOptions.Alert);
        }
    }
}