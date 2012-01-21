using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ExcelInterop;

public class ExcelExportController : INotifyPropertyChanged
{

	private readonly bool _isFuifProductExport;
	private readonly int _fuifId;

	private bool _selectAllNone;

	public ExcelExportController(bool isFuifProductExport, int fuifId)
	{
		_isFuifProductExport = isFuifProductExport;
		_fuifId = fuifId;

		_exportProperties = new ObservableCollection<DatabaseMappingItem>();
		_exportProperties.Add(new DatabaseMappingItem {
			ParoganColumn = "Id",
			DatabaseColumn = "ProdId",
			Selected = true
		});
		_exportProperties.Add(new DatabaseMappingItem {
			ParoganColumn = "Naam",
			DatabaseColumn = "ProdNaam",
			Selected = true
		});
		_exportProperties.Add(new DatabaseMappingItem {
			ParoganColumn = "Huidige aankoopprijs",
			DatabaseColumn = "ProdAankoopprijs",
			Selected = true
		});
		if ((_isFuifProductExport)) {
			_exportProperties.Add(new DatabaseMappingItem {
				ParoganColumn = "Afgesproken aankoopprijs",
				DatabaseColumn = "FPAankoopprijs",
				Selected = true
			});
			_exportProperties.Add(new DatabaseMappingItem {
				ParoganColumn = "Verkoopprijs",
				DatabaseColumn = "FPVerkoopprijs",
				Selected = true
			});
			_exportProperties.Add(new DatabaseMappingItem {
				ParoganColumn = "Categorie",
				DatabaseColumn = "FPCategorie",
				Selected = true
			});
			_exportProperties.Add(new DatabaseMappingItem {
				ParoganColumn = "Ratio",
				DatabaseColumn = "FPRatio",
				Selected = true
			});
			_exportProperties.Add(new DatabaseMappingItem {
				ParoganColumn = "Stock totaal aantal",
				DatabaseColumn = "FPAantal",
				Selected = true
			});
			_exportProperties.Add(new DatabaseMappingItem {
				ParoganColumn = "Stock rest aantal",
				DatabaseColumn = "FPRestAantal",
				Selected = true
			});
		} else {
			_exportProperties.Add(new DatabaseMappingItem {
				ParoganColumn = "Beschikbaar",
				DatabaseColumn = "ProdOnbeschikbaar",
				Selected = true
			});
			//only useful for brewer, for organisation its always true (not true: can change after add to fuif)
		}
		_selectAllNone = false;
		ExportFilePath = "";
	}

	private ObservableCollection<DatabaseMappingItem> _exportProperties;
	public ObservableCollection<DatabaseMappingItem> ExportProperties {
		get { return _exportProperties; }
		set { _exportProperties = value; }
	}

	private string _exportFilePath;
	public string ExportFilePath {
		get { return _exportFilePath; }
		set {
			_exportFilePath = value;
			PropChanged("ExportFilePath");
			PropChanged("IsExportEnabled");
		}
	}

	public void PropChanged(string arg)
	{
		if (PropertyChanged != null) {
			PropertyChanged(this, new PropertyChangedEventArgs(arg));
		}
	}

	public void SelectAllNone()
	{
		foreach (DatabaseMappingItem MappingItem in _exportProperties) {
			MappingItem.Selected = _selectAllNone;
		}
		_selectAllNone = !_selectAllNone;
	}

	public void Export()
	{
	    bool MinOneSelected = false;
	    foreach (var MappingItem in ExportProperties)
	    {
	        if( MappingItem.Selected)
	        {
	            MinOneSelected = true;
	            break;
	        }
	    }
		if (MinOneSelected) {
			//export
				Excel.Data2Excel(ParoganConnector.SelectAllFuifProducts(_fuifId, from item in _exportProperties where item.Selecteditem.DatabaseColumn), from item in _exportPropertieswhere item.Selecteditem.ParoganColumn);
		} else {
			MessageBox.Show("U hebt geen kolommen geselecteerd om te exporteren.");
		}
	}
    public event PropertyChangedEventHandler PropertyChanged;
}
