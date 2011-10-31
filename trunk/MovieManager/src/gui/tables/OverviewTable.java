/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui.tables;

import gui.tables.CellEditors.RatingCellEditor;
import gui.tables.CellEditors.RatingCellRenderer;
import gui.tables.CellEditors.VideoTypeCellEditor;
import gui.tables.headers.TableColumnHeaders;
import gui.tables.headers.TableHeadMouseListener;
import javax.swing.JTable;
import javax.swing.table.JTableHeader;
import javax.swing.table.TableCellEditor;
import javax.swing.table.TableCellRenderer;
import logic.database.VideoDatabase;

/**
 *
 * @author alexander
 */
public class OverviewTable extends JTable {

    private static OverviewTable instance;

    private OverviewTable(VideoTableModel model) {
	super(model);
	this.setAutoCreateRowSorter(true);
	model.updateData(VideoDatabase.getInstance().getVideos());
	
	//table
	JTableHeader header = this.getTableHeader();
	header.addMouseListener(TableHeadMouseListener.getInstance());

	this.addMouseListener(TableMouseListener.getInstance());
	this.setFillsViewportHeight(true);
	
	//TODO 020 FIX: when removing and afterwards adding the column, the render isn't applied
	setCustomCellRenderer(TableColumnHeaders.ColumnsEnum.Rating.toString(), new RatingCellRenderer());
	setCustomCellEditor(TableColumnHeaders.ColumnsEnum.VideoType.toString(), new VideoTypeCellEditor());	
	setCustomCellEditor(TableColumnHeaders.ColumnsEnum.Rating.toString(), new RatingCellEditor());
    }
    
    private void setCustomCellRenderer(String columnName,TableCellRenderer renderer){
	this.getColumnModel().getColumn(getColumnIndex(columnName)).setCellRenderer(renderer);
    }
    
    private void setCustomCellEditor(String columnName,TableCellEditor renderer){
	this.getColumnModel().getColumn(getColumnIndex(columnName)).setCellEditor(renderer);
    }
    
    private int getColumnIndex(String columnName){
	int index = 0;
	while (index<this.getColumnCount() && !this.getColumnName(index).equals(columnName)){
	    index++;
	}
	return index;
    }

    public static OverviewTable getInstance() {
	if (instance == null) {
	    synchronized (OverviewTable.class) {
		if (instance == null) {
		    instance = new OverviewTable(VideoTableModel.getInstance());
		}
	    }
	}
	return instance;
    
    }
}
