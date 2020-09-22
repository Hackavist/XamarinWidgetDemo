using System;

namespace WidgetDemo.Models
{
    public class Note : BaseModel
    {
        public string NoteTitle { get; set; }
        public string Description { get; set; }
        public DateTime NoteDateTime { get; set; }
    }
}