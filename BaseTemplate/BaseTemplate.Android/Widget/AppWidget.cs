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
        public const string TOAST_ACTION = "com.nourelgafy.widgetdemo.TOAST_ACTION";
        public const string EXTRA_ITEM = "com.nourelgafy.widgetdemo.EXTRA_ITEM";

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
                Intent toastIntent = new Intent(context, typeof(AppWidget));
                toastIntent.SetAction(TOAST_ACTION);
                toastIntent.PutExtra(AppWidgetManager.ExtraAppwidgetId, appWidgetIds[i]);
                toastIntent.SetData(Uri.Parse(intent.ToUri(IntentUriType.Scheme)));

                //Pending intent to be used as a template
                PendingIntent toastPendingIntent = PendingIntent.GetBroadcast(context, 0, toastIntent, PendingIntentFlags.UpdateCurrent);
                remoteViews.SetPendingIntentTemplate(Resource.Id.list_view, toastPendingIntent);

                appWidgetManager.UpdateAppWidget(appWidgetIds[i], remoteViews);
            }
            base.OnUpdate(context, appWidgetManager, appWidgetIds);
        }

        public override void OnReceive(Context context, Intent intent)
        {
            AppWidgetManager widgetManager = AppWidgetManager.GetInstance(context);
            if (TOAST_ACTION.Equals(intent.Action))
            {
                var appWidgetid = intent.GetIntExtra(AppWidgetManager.ExtraAppwidgetId, AppWidgetManager.InvalidAppwidgetId);
                int viewindex = intent.GetIntExtra(EXTRA_ITEM, 0);
                Toast.MakeText(context, $"touched index number {viewindex}", ToastLength.Short).Show();
            }
            base.OnReceive(context, intent);
        }
    }
}
