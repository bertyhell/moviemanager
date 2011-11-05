/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui.tables;

import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import javax.swing.JFrame;
import logic.vlc.VlcPlayer;

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
	    JFrame frame = new JFrame();
            System.setProperty("jna.library.path", "C:\\Program Files\\VideoLAN\\VLC");
	    VlcPlayer player = new VlcPlayer();
	    frame.setContentPane(player);

	    frame.setLocation(100, 100);
	    frame.setSize(1050, 600);
	    frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
	    frame.setVisible(true);
            
            String[] mediaOptions = {};
	    player.start(model.getVideo(table.getSelectedRow()).getPath());
	}
    }

    public static TableMouseListener getInstance() {
	return instance;
    }
}
