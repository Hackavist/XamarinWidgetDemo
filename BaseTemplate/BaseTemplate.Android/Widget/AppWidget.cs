using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Net;
using Android.Widget;

namespace WidgetDemo.Droid.Widget
{
    [BroadcastReceiver(Label = "HellApp Widget")]
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    [MetaData("android.appwidget.provider", Resource = "@xml/widget_info")]
    public class AppWidget : AppWidgetProvider
    {
        public const string NavigationAction = "com.nourelgafy.widgetdemo.NavigationAction";
        public const string ExtraItem = "com.nourelgafy.widgetdemo.EXTRA_ITEM";
        public const string PageNumber = "com.nourelgafy.widgetdemo.PageNumber";

        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            for (int i = 0; i < appWidgetIds.Length; i++)
            {
                //the service intent which will provide the views for the listview
                Intent intent = new Intent(context, typeof(CollectionWidgetService));
                //adding the widget id to the intent extras
                intent.PutExtra(AppWidgetManager.ExtraAppwidgetId, appWidgetIds[i]);
                intent.SetData(Uri.Parse(intent.ToUri(IntentUriType.Scheme)));

                // Gets the remote object of the app widget
                RemoteViews remoteViews = new RemoteViews(context.PackageName, Resource.Layout.widget_layout);
                //provide the listview with the needed adapter through the service
                remoteViews.SetRemoteAdapter(Resource.Id.list_view, intent);
                //Set the empty view in case the collection is empty
                remoteViews.SetEmptyView(Resource.Id.list_view, Resource.Id.empty_view);

                //setting up the pending intent template to be filled by each list item on click
                Intent navigationIntent = new Intent(context, typeof(AppWidget));
                navigationIntent.SetAction(NavigationAction);
                navigationIntent.PutExtra(AppWidgetManager.ExtraAppwidgetId, appWidgetIds[i]);
                navigationIntent.SetData(Uri.Parse(intent.ToUri(IntentUriType.Scheme)));

                //Pending intent to be used as a template
                PendingIntent navigationPendingIntent = PendingIntent.GetBroadcast(context, 0, navigationIntent, PendingIntentFlags.UpdateCurrent);
                remoteViews.SetPendingIntentTemplate(Resource.Id.list_view, navigationPendingIntent);

                appWidgetManager.UpdateAppWidget(appWidgetIds[i], remoteViews);
            }
            base.OnUpdate(context, appWidgetManager, appWidgetIds);
        }

        public override void OnReceive(Context context, Intent intent)
        {
            AppWidgetManager widgetManager = AppWidgetManager.GetInstance(context);
            if (NavigationAction.Equals(intent.Action))
            {
                //var appWidgetid = intent.GetIntExtra(AppWidgetManager.ExtraAppwidgetId, AppWidgetManager.InvalidAppwidgetId);
                //Toast.MakeText(context, $"touched index number {viewindex}", ToastLength.Short)?.Show();
                int noteIndex = intent.GetIntExtra(ExtraItem, 0);
                Intent openAppIntent = new Intent(context, typeof(MainActivity));
                openAppIntent.PutExtra(PageNumber, $"NoteIndex/{noteIndex}");
                openAppIntent.SetFlags(ActivityFlags.NewTask);
                context.StartActivity(openAppIntent);
            }
            base.OnReceive(context, intent);
        }
    }
}
