/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui.tables;

import java.awt.Color;
import java.awt.Toolkit;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.io.File;
import javax.swing.JFrame;
import logic.vlc.VlcPlayer;
import logic.vlc.VlcPlayerFrame;

/**
 *
 * @author alexander
 */
public class TableMouseListener extends MouseAdapter {

    private static TableMouseListener instance = new TableMouseListener();

    @Override
    public void mouseClicked(MouseEvent e) {
	if (e.getButton() == MouseEvent.BUTTON1 && e.getClickCount() == 2) {
	    OverviewTable table = OverviewTable.getInstance();
	    VideoTableModel model = VideoTableModel.getInstance();//TODO 100 get selected video in a good way :)
//	  VLCRemoteControl remotecontrol = new VLCRemoteControl();
//	    try {
//		remotecontrol.VlcRemote(model.getVideo(table.getSelectedRow()));
//	    } catch (IOException ex) {
//		Logger.getLogger(TableMouseListener.class.getName()).log(Level.SEVERE, null, ex);
//	    }
	    
	    VlcPlayerFrame frame = new VlcPlayerFrame();
	    VlcPlayer player = frame.getPlayer();
	    
	    
            String[] mediaOptions = {};
	    player.start(model.getVideo(table.getSelectedRow()).getPath());
	}
    }

    public static TableMouseListener getInstance() {
	return instance;
    }
}
