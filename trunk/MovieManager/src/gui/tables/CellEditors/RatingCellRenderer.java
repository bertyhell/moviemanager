/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui.tables.CellEditors;

import java.awt.Component;
import javax.swing.JTable;
import javax.swing.table.TableCellRenderer;
import logic.database.Video;
import logic.database.VideoDatabase;

/**
 *
 * @author alexander
 */
public class RatingCellRenderer implements TableCellRenderer{
    private RatingPanel ratingPanel;
    
    public RatingCellRenderer(){
	
//	this.addCellEditorListener(new videoTypeCellEditorListener(this));
    }

    @Override
    public Component getTableCellRendererComponent(JTable table, Object value, boolean isSelected, boolean hasFocus, int row, int column) {
	Video video = VideoDatabase.getInstance().getVideos().get(row);
	ratingPanel = new RatingPanel(video);
	return ratingPanel;
    }
}
