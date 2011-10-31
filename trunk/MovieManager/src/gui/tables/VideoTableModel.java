/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui.tables;

import gui.tables.headers.TableColumnHeaders;
import java.util.ArrayList;
import javax.swing.table.AbstractTableModel;
import logic.database.Video;
import logic.database.jdbc.DatabaseConnector;

/**
 *
 * @author alexander
 */
public class VideoTableModel extends AbstractTableModel {

    private static VideoTableModel instance = new VideoTableModel();
    private TableColumnHeaders headers = TableColumnHeaders.getInstance();
    private ArrayList<Video> videos;

    private VideoTableModel() {
    }

    public static VideoTableModel getInstance() {
	return instance;
    }

    @Override
    public int getRowCount() {
	return videos.size();
    }

    @Override
    public int getColumnCount() {
	return headers.getVisibleColumnsCount();
    }

    @Override
    public String getColumnName(int columnIndex) {
	return headers.getVisibleColumns().get(columnIndex);
    }

    @Override
    public boolean isCellEditable(int rowIndex, int columnIndex) {
	if (getColumnName(columnIndex).equals(TableColumnHeaders.ColumnsEnum.VideoType.toString())
		|| getColumnName(columnIndex).equals(TableColumnHeaders.ColumnsEnum.Rating.toString())) {
	    return true;
	} else {
	    return false;
	}
    }

    @Override
    public Object getValueAt(int rowIndex, int columnIndex) {
	return videos.get(rowIndex).getField(getColumnName(columnIndex));
    }

    @Override
    public void setValueAt(Object aValue, int rowIndex, int columnIndex) {
    }

    public void updateData(ArrayList<Video> videos) {
	this.videos = videos;
	this.fireTableDataChanged();
    }

    public void UpdateTable() {
	this.fireTableStructureChanged();
	this.fireTableDataChanged();
    }

    public Video getVideo(int index) {
	return videos.get(index);
    }
}
