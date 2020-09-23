using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Appwidget;
using Android.Content;
using Android.OS;
using Android.Widget;
using Java.Lang;
using TemplateFoundation.IOCFoundation;
using WidgetDemo.Models;
using WidgetDemo.Services.LocalDatabaseService;

namespace WidgetDemo.Droid.Widget
{
    public class DataProvider : Object, RemoteViewsService.IRemoteViewsFactory
    {
        private readonly Context _context;
        private int _appWidgetId;
        private List<Note> _notesList = new List<Note>();

        public DataProvider(Context context, Intent intent)
        {
            _context = context;
            _appWidgetId = intent.GetIntExtra(AppWidgetManager.ExtraAppwidgetId, AppWidgetManager.InvalidAppwidgetId);
        }

        public int Count => _notesList.Count;

        public bool HasStableIds => true;

        public RemoteViews LoadingView => null;

        public int ViewTypeCount => 1;

        public long GetItemId(int position)
        {
            return _notesList[position].Id;
        }

        public RemoteViews GetViewAt(int position)
        {
            RemoteViews remoteView = new RemoteViews(_context.PackageName, Resource.Layout.widget_item);
            remoteView.SetTextViewText(Resource.Id.note_title, _notesList[position].NoteTitle);
            remoteView.SetTextViewText(Resource.Id.date_Time, _notesList[position].NoteDateTime.ToShortTimeString());
            remoteView.SetTextViewText(Resource.Id.note_description, _notesList[position].Description);

            //adding data to be passed inside the fill intent
            Bundle extras = new Bundle();
            extras.PutInt(AppWidget.ExtraItem, position);

            //the intent that is going to fill the template created in the appwidgetclass
            Intent fillIntent = new Intent();
            fillIntent.PutExtras(extras);
            remoteView.SetOnClickFillInIntent(Resource.Id.note_main_layout, fillIntent);
            return remoteView;
        }

        public void OnCreate()
        {

        }

        public void OnDataSetChanged()
        {
            Task.Run(async () =>
            {
                if (Ioc.Container.Resolve<ILocalDatabaseService>() is LocalDatabaseService database)
                    _notesList = await database.GetAll<Note>();
            });
        }

        public void OnDestroy()
        {
            _notesList.Clear();
            _notesList = null;
        }
    }
}