using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using SQLite;

namespace MovieManager.APP.Commands
{
    class CreateDatabaseCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void OnExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }

        public void Execute(object parameter)
        {
            SaveFileDialog Ofd = new SaveFileDialog();
            if (Ofd.ShowDialog() == DialogResult.OK)
            {
                MMDatabase.CreateDatabase(Ofd.FileName);
            }
        }
    }
}
