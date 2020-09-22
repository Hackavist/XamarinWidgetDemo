using System;
using System.Threading.Tasks;
using BaseMvvmToolKIt.Commands;
using TemplateFoundation.IOCFoundation;
using TemplateFoundation.ViewModelFoundation;
using WidgetDemo.Models;
using WidgetDemo.Services.LocalDatabaseService;

namespace WidgetDemo.ViewModels
{
    public class AddingViewModel : BaseViewModel
    {
        public string NoteTitle { get; set; }
        public string NoteDescription { get; set; }

        public AsyncCommand SaveCommand { get; set; }
        public AsyncCommand CancelCommand { get; set; }

        public AddingViewModel()
        {
            Title = "Add a new Note";
            SaveCommand = new AsyncCommand(SaveCommandExecute);
            CancelCommand = new AsyncCommand(CancelCommandExecute);
        }

        private async Task CancelCommandExecute()
        {
            await NavigationService.PopPageModel(true);
        }

        private async Task SaveCommandExecute()
        {
            LocalDatabaseService database = Ioc.Container.Resolve<LocalDatabaseService>();
            if (string.IsNullOrWhiteSpace(NoteTitle))
                await NavigationService.DisplayAlert("Invalid Note", "You can't leave the title empty", "Ok");
            await database.Insert(new Note
            { NoteTitle = NoteTitle, Description = NoteDescription, NoteDateTime = DateTime.Now });
            await NavigationService.PopPageModel(true);
        }
    }
}