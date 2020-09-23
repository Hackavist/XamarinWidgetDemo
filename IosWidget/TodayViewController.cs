using System;
using NotificationCenter;
using Foundation;
using UIKit;
using System.Collections.Generic;
using WidgetDemo.Models;

using CoreGraphics;

namespace IosWidget
{
    public partial class TodayViewController : UIViewController,
    INCWidgetProviding
    {
        List<Note> _notesList = new List<Note>();        
        protected TodayViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _notesList = new List<Note>
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
                },
                new Note
                {
                    NoteTitle = "Buy Apple dev account", NoteDateTime = DateTime.Now.AddDays(2.0).AddHours(3.0),
                    Description = "Buying one will make your life easier and simpler regarding ios development"
                }
            };
            SetDatasource();
        }

        [Export("widgetActiveDisplayModeDidChange:withMaximumSize:")]
        public void WidgetActiveDisplayModeDidChange(NCWidgetDisplayMode activeDisplayMode, CGSize maxSize)
        {  
            int _notesListCount = _notesList.Count < 5 ? _notesList.Count : 5;
            if (_notesListCount == 0) _notesListCount += 1;
            PreferredContentSize = activeDisplayMode == NCWidgetDisplayMode.Expanded ? new CGSize(maxSize.Width, 60.0f * _notesListCount) : maxSize;
            NotesTableView.ReloadData();
        }
        [Export("widgetPerformUpdateWithCompletionHandler:")]
        public void WidgetPerformUpdate(Action<NCUpdateResult> completionHandler)
        {
            // Perform any setup necessary in order to update the view.
            // If an error is encoutered, use NCUpdateResultFailed
            // If there's no update required, use NCUpdateResultNoData
            // If there's an update, use NCUpdateResultNewData
            completionHandler(NCUpdateResult.NewData);
        }

        /*
         * Unfourtunalty that will not work because accourding to 
         * https://dmtopolog.com/ios-app-extensions-data-sharing/
         * and 
         * https://medium.com/@deep_blue_day/how-to-create-an-ios-app-group-for-xamarin-app-a117c172b4
         * 
         * to access the database i must have an apple developer account account to make an app group and make the database shared between the app and the extension 
         * (sadly i don't have one)
         * there is no other way except having the data coming from an api
         * 
        private Task RefreshDataFromDatabase() => Task.Run(async () =>
        {

            try
            {
                var database = Ioc.Container.Resolve<ILocalDatabaseService>() as LocalDatabaseService;
                if (database != null) _notesList = await database.GetAll<Note>();
                isDataReady = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Errroooooooorrr{ex.Message}");
                isDataReady = false;
                throw ex;
            }
        });
        */
        void SetDatasource()
        {
            NotesTableView.Source = new TableSource(_notesList,base.ExtensionContext);
            NotesTableView.RowHeight = UITableView.AutomaticDimension;
            ExtensionContext.SetWidgetLargestAvailableDisplayMode(NCWidgetDisplayMode.Expanded);
            NotesTableView.ReloadData();
        }
    }
}