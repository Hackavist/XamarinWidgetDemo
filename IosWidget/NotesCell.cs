using Foundation;
using System;
using UIKit;
using WidgetDemo.Models;

namespace IosWidget
{
    public partial class NotesCell : UITableViewCell
    {
        public NotesCell(IntPtr handle) : base(handle)
        {
        }

        public void UpdateCell(Note note)
        {
            TitleLabel.Text = note.NoteTitle;
            DateTimeLabel.Text = note.NoteDateTime.ToShortTimeString();
            DescriptionLabel.Text = note.Description;
        }
    }
}