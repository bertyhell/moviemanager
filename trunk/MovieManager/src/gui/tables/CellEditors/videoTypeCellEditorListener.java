/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui.tables.CellEditors;

import gui.tables.OverviewTable;
import javax.swing.event.CellEditorListener;
import javax.swing.event.ChangeEvent;
import logic.database.Video;
import logic.database.Video.VideoType;
import logic.database.VideoDatabase;

/**
 *
 * @author alexander
 */
public class videoTypeCellEditorListener implements CellEditorListener {
    private VideoTypeCellEditor editor;
    
    public videoTypeCellEditorListener(VideoTypeCellEditor editor){
	this.editor = editor;
    }
    
    @Override
    public void editingStopped(ChangeEvent e) {
	System.out.println("ja");
	String selectedValue = editor.getCellEditorValue();
	int selectedTableRow = OverviewTable.getInstance().getSelectedRow();
	
	VideoDatabase database = VideoDatabase.getInstance();
	Video video = database.getVideos().get(selectedTableRow);
	database.setVideo(selectedTableRow,Video.ConvertVideo(VideoType.valueOf(selectedValue),video));
    }

    @Override
    public void editingCanceled(ChangeEvent e) {
    }
    
}
