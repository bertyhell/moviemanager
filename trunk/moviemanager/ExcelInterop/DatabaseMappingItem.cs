using System.ComponentModel;

namespace ExcelInterop
{
	public class DatabaseMappingItem : INotifyPropertyChanged
	{

		private string _databaseColumn;
		public string DatabaseColumn {
			get { return _databaseColumn; }
			set { _databaseColumn = value; }
		}

		private string _paroganColumn;
		public string ParoganColumn {
			get { return _paroganColumn; }
			set { _paroganColumn = value; }
		}

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
