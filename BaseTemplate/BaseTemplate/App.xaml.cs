using System.Globalization;
using System.Threading;
using TemplateFoundation.IOCFoundation;
using TemplateFoundation.Navigation.Implementations;
using TemplateFoundation.Navigation.NavigationContainers;
using WidgetDemo.Resources;
using WidgetDemo.Services.FileSystemService;
using WidgetDemo.Services.LocalDatabaseService;
using WidgetDemo.ViewModels;
using Xamarin.Forms;

namespace WidgetDemo
{
    public partial class App : Application
    {
        private const string PageName = "HomePage";

        public App()
        {
            InitializeComponent();

            SetupDependencyInjection();
            CreateDataBaseTables();
            SetDefaultLanguage();
            SetStartPage();
        }

        public App(string pageName)
        {
            InitializeComponent();

            SetupDependencyInjection();
            CreateDataBaseTables();
            SetDefaultLanguage();
            SetStartPage(pageName);
        }

        private void SetupDependencyInjection()
        {
            Ioc.Container.Register<ILocalDatabaseService, LocalDatabaseService>().AsSingleton();
            Ioc.Container.Register<IFileSystemService, FileSystemService>().AsSingleton();
        }

        /// <summary>
        ///     Create your database tables that you need
        /// </summary>
        private void CreateDataBaseTables()
        {
            // Ioc.Container.Resolve<LocalDatabaseService>().CreateDatabaseTables(Send List of tabels);
        }

        /// <summary>
        ///     Set your default language for the entire app
        ///     Just change culture info ar,en,fr,es
        /// </summary>
        private void SetDefaultLanguage()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");

            AppResources.Culture = new CultureInfo("en");
        }

        private void SetStartPage(string pageName = PageName)
        {
            if (pageName != PageName)
            {
                Page page = ViewModelResolver.ResolveViewModel<MainViewModel>(pageName);
                NavigationPageContainer navigationContainer = new NavigationPageContainer(page);
                MainPage = navigationContainer;
            }
            else
            {
                Page page = ViewModelResolver.ResolveViewModel<MainViewModel>();
                NavigationPageContainer navigationContainer = new NavigationPageContainer(page);
                MainPage = navigationContainer;
            }
        }
    }
}