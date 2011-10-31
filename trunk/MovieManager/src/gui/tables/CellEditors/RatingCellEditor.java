/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui.tables.CellEditors;

import java.awt.Component;
import javax.swing.AbstractCellEditor;
import javax.swing.JComboBox;
import javax.swing.JTable;
import javax.swing.table.TableCellEditor;
import logic.database.Video;
import logic.database.Video.VideoType;
import logic.database.VideoDatabase;

/**
 *
 * @author alexander
 */
public class RatingCellEditor extends AbstractCellEditor implements TableCellEditor{
    private RatingPanel ratingPanel;
    
    public RatingCellEditor(){
	
//	this.addCellEditorListener(new videoTypeCellEditorListener(this));
    }

    @Override
    public Component getTableCellEditorComponent(JTable table, Object value, boolean isSelected, int row, int column) {
	Video video = VideoDatabase.getInstance().getVideos().get(row);
	ratingPanel = new RatingEditorPanel(video);
	return ratingPanel;
    }

    @Override
    public String getCellEditorValue() {
	return "1";
    }    
    
    
}
