/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui.tables;

import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.io.IOException;
import java.util.logging.Level;
import java.util.logging.Logger;
import logic.vlcRemoteControl.VLCRemoteControl;

/**
 *
 * @author alexander
 */
public class TableMouseListener extends MouseAdapter {
    private static TableMouseListener instance = new TableMouseListener();
    
    @Override
    public void mouseClicked(MouseEvent e) {
      if(e.getButton() == MouseEvent.BUTTON1 && e.getClickCount() == 2){
	  OverviewTable table = OverviewTable.getInstance();
	  VideoTableModel model = VideoTableModel.getInstance();
	  VLCRemoteControl remotecontrol = new VLCRemoteControl();
	    try {
		remotecontrol.VlcRemote(model.getVideo(table.getSelectedRow()));
	    } catch (IOException ex) {
		Logger.getLogger(TableMouseListener.class.getName()).log(Level.SEVERE, null, ex);
	    }
	  
      }
    }

    public static TableMouseListener getInstance() {
	return instance;
    }
    
    
    
}
