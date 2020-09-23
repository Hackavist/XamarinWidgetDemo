using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using WidgetDemo.Models;

namespace IosWidget
{
    internal class TableSource : UITableViewSource
    {
        private List<Note> _notesList;
        private NSExtensionContext _extensionContext;

        public TableSource(List<Note> notesList, NSExtensionContext extensionContext)
        {
            _notesList = notesList;
            _extensionContext = extensionContext;
        }
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            _extensionContext.OpenUrl(new NSUrl("notesapp://"), null);
            tableView.DeselectRow(indexPath, true);
        }
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            NotesCell cell = (NotesCell)tableView.DequeueReusableCell("cell_id", indexPath);
            cell.UpdateCell(_notesList[indexPath.Row]);
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section) => _notesList.Count;
        public override nint NumberOfSections(UITableView tableView)
        {
            nint numberOfSections = 1;
            if (_notesList.Count == 0)
            {
                numberOfSections = 0;
                UILabel noDataLabel = new UILabel(new CoreGraphics.CGRect(0, 0, tableView.Bounds.Size.Width, tableView.Bounds.Size.Height))
                {
                    Text = "No Events Available Yet",
                    TextColor = UIColor.Black
                };
                tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
                tableView.BackgroundView = noDataLabel;
            }
            return numberOfSections;
        }
    }
}