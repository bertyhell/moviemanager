﻿using System.ComponentModel;

namespace ExcelInterop
{
	public class DatabaseMappingItem : INotifyPropertyChanged
	{
	    public string DatabaseColumn { get; set; }

	    public string MMColumn { get; set; }

	    private bool _selected;
		public bool Selected {
			get { return _selected; }
			set {
				_selected = value;
				PropChanged("Selected");
				PropChanged("IsExportEnabled");
			}
		}

		public void PropChanged(string arg)
		{
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(arg));
			}
		}

	    public event PropertyChangedEventHandler PropertyChanged;
	}
}
