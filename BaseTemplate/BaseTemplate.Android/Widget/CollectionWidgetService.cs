using Android.App;
using Android.Content;
using Android.Widget;

namespace WidgetDemo.Droid.Widget
{
    [Service(Permission = "android.permission.BIND_REMOTEVIEWS", Exported = false)]
    public class CollectionWidgetService : RemoteViewsService
    {
        public override IRemoteViewsFactory OnGetViewFactory(Intent intent) => new DataProvider(ApplicationContext, intent);
    }
}
