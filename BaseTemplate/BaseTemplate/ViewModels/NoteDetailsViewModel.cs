using TemplateFoundation.ViewModelFoundation;
using WidgetDemo.Models;

namespace WidgetDemo.ViewModels
{
    public class NoteDetailsViewModel : BaseViewModel
    {
        public Note SelectedNote { get; set; }

        public override void Init(object initData)
        {
            SelectedNote = initData as Note;
            base.Init(initData);
        }
    }
}