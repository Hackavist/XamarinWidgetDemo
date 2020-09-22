using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Util;
using WidgetDemo.Constants;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Environment = System.Environment;
using Platform = Xamarin.Essentials.Platform;

namespace WidgetDemo.Droid
{
    [Activity(Label = "BaseTemplate", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

            base.OnCreate(savedInstanceState);
            UserDialogs.Init(this);
            Platform.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);
            DisplayCrashReport();
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            [GeneratedEnum] Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


        #region Error Handling
        private static void TaskSchedulerOnUnobservedTaskException(object sender,
            UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            Exception newExc = new Exception("TaskSchedulerOnUnobservedTaskException",
                unobservedTaskExceptionEventArgs.Exception);
            LogUnhandledException(newExc);
        }

        private static void CurrentDomainOnUnhandledException(object sender,
            UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            Exception newExc = new Exception("CurrentDomainOnUnhandledException",
                unhandledExceptionEventArgs.ExceptionObject as Exception);
            LogUnhandledException(newExc);
        }

        internal static void LogUnhandledException(Exception exception)
        {
            try
            {
                string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string errorFilePath = Path.Combine(libraryPath, AppConstants.ErrorFileName);
                string errorMessage = $"Time: {DateTime.Now}\r\nError: Unhandled Exception\r\n{exception}";
                File.WriteAllText(errorFilePath, errorMessage);
                // Log to Android Device Logging.
                Log.Error("Crash Report", errorMessage);
            }
            catch (Exception ex)
            {
                throw ex;
                // just suppress any error logging exceptions
            }
        }

        /// <summary>
        // If there is an unhandled exception, the exception information is diplayed 
        // on screen the next time the app is started (only in debug configuration)
        /// </summary>
        [Conditional("DEBUG")]
        private void DisplayCrashReport()
        {
            string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string errorFilePath = Path.Combine(libraryPath, AppConstants.ErrorFileName);


            if (!File.Exists(errorFilePath)) return;

            string errorText = File.ReadAllText(errorFilePath);
            if (string.IsNullOrEmpty(errorText)) return;


            new AlertDialog.Builder(this)
                .SetPositiveButton("Clear", (sender, args) => { File.Delete(errorFilePath); })
                .SetNegativeButton("Close", (sender, args) =>
                {
                    // When User pressed Close.
                })
                .SetMessage(errorText)
                .SetTitle("Crash Report")
                .Show();
        }
        #endregion
    }
}