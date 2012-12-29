using System;
using System.Windows.Input;
using Tmc.WinUI.Application.Wizards;

namespace Tmc.WinUI.Application.Commands
{
    class AddSerieCommand : ICommand
    {
        //TODO 090 fix the damn event checks from null
        //TODO 050 change folder browser dialog

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void OnExecuteChanged()
        {
            if( CanExecuteChanged!= null)
                CanExecuteChanged(this, new EventArgs());
        }

        public void Execute(object parameter)
        {
            AddEpisodesWizard Wizard = new AddEpisodesWizard();
            Wizard.Show();
        }
    }
}
