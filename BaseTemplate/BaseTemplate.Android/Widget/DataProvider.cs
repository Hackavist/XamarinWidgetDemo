using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Appwidget;
using Android.Content;
using Android.OS;
using Android.Widget;
using WidgetDemo.Models;

namespace WidgetDemo.Droid.Widget
{
    public class DataProvider : Java.Lang.Object, RemoteViewsService.IRemoteViewsFactory
    {
        List<Note> _notesList = new List<Note>();
        Context _context;
        int _appWidgetId;
        public DataProvider(Context context, Intent intent)
        {
            _context = context;
            _appWidgetId = intent.GetIntExtra(AppWidgetManager.ExtraAppwidgetId, AppWidgetManager.InvalidAppwidgetId);
        }
        public int Count => _notesList.Count;

        public bool HasStableIds => true;

        public RemoteViews LoadingView => null;

        public int ViewTypeCount => 1;

        public long GetItemId(int position) => _notesList[position].Id;


        public RemoteViews GetViewAt(int position)
        {
            RemoteViews remoteview = new RemoteViews(_context.PackageName, Resource.Layout.widget_item);
            remoteview.SetTextViewText(Resource.Id.note_title, _notesList[position].NoteTitle);
            remoteview.SetTextViewText(Resource.Id.date_Time, _notesList[position].NoteDateTime.ToShortTimeString());
            remoteview.SetTextViewText(Resource.Id.note_description, _notesList[position].Description);

            //adding data to be passed inside the fill intent
            Bundle extras = new Bundle();
            extras.PutInt(AppWidget.EXTRA_ITEM, position);

            //the intent that is going to fill the template created in the appwidgetclass
            Intent fillIntent = new Intent();
            fillIntent.PutExtras(extras);
            remoteview.SetOnClickFillInIntent(Resource.Id.note_main_layout, fillIntent);
            return remoteview;
        }

        public void OnCreate()
        {

        }

        public void OnDataSetChanged()
        {
            _notesList = new List<Note>()
            {
                 new Note
                            {
                                NoteTitle = "Walk The Dog", NoteDateTime = DateTime.Now,
                                Description = "I have to take the dog out for a walk"
                            },
                            new Note
                            {
                                NoteTitle = "App Enhancement Ideas", NoteDateTime = DateTime.Now.AddDays(2.0).AddHours(3.0),
                                Description = "we can add support for dark theme"
                            }
            };
        }

        public void OnDestroy()
        {
            throw new NotImplementedException();
        }
    }
}
