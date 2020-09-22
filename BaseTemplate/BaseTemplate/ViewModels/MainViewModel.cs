using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BaseMvvmToolKIt.Commands;
using SQLite;
using TemplateFoundation.IOCFoundation;
using TemplateFoundation.ViewModelFoundation;
using WidgetDemo.Models;
using WidgetDemo.Services.LocalDatabaseService;

namespace WidgetDemo.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<Note> Notes { get; set; }
        public AsyncCommand NavigateToAddingPage { get; set; }
        public AsyncCommand ItemSelectedCommand { get; set; }
        public Note SelectedNote { get; set; }

        private LocalDatabaseService _database;

        public MainViewModel()
        {
            Title = "MainPage";
            Notes = new ObservableCollection<Note>();
            NavigateToAddingPage = new AsyncCommand(NavigateToAddingPageExecute);
            ItemSelectedCommand = new AsyncCommand(ItemSelectedExecute);
        }

        private async Task ItemSelectedExecute()
        {
            await NavigationService.PushPageModel<NoteDetailsViewModel>(SelectedNote);
        }

        private async Task NavigateToAddingPageExecute()
        {
            await NavigationService.PushPageModelWithNewNavigation<AddingViewModel>(true);
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            Task.Run(async () =>
            {
                _database = Ioc.Container.Resolve<ILocalDatabaseService>() as LocalDatabaseService;
                if (!LocalDatabaseService.DbInitialized) await InitializeDb();
                if (_database != null) Notes = new ObservableCollection<Note>(await _database.GetAll<Note>());
            });
        }

        private async Task Seed()
        {
            await _database.InsertAll(new List<Note>
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
            });
        }

        private async Task InitializeDb()
        {
            if (_database != null)
            {
                var typesList = new List<Type> { typeof(Note) };
                await _database.CreateDatabaseTables(typesList, CreateFlags.AllImplicit | CreateFlags.AutoIncPK);
                var temp = await _database.GetAll<Note>();
                if (temp.Count == 0) await Seed();
            }
        }

        public override void Init(object initData)
        {
            if (initData != null)
            {
                string pageInfo = initData as string;
                if (!string.IsNullOrWhiteSpace(pageInfo))
                {
                    var splitResult = pageInfo.Split('/');
                    int noteNumber = Convert.ToInt32(splitResult[1]);
                    Task.Run(async () =>
                    {
                        await NavigationService.PushPageModel<NoteDetailsViewModel>(Notes[noteNumber]);
                    });

                }
            }
            base.Init(initData);
        }
    }
}