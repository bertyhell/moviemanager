﻿using System;
using System.Windows.Input;
using Tmc.DataAccess.SqlCe;
using Tmc.WinUI.Application.Properties;

namespace Tmc.WinUI.Application.Commands
{
    class ConvertDatabaseCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
	        DataRetriever.ConvertDatabase();
        }
    }
}
