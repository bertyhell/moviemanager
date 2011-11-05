/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui.actionlisteners;

import gui.MainWindow;
import gui.tables.VideoTableModel;
import img.ImageFactory;
import java.awt.event.ActionEvent;
import java.awt.event.KeyEvent;
import java.io.File;
import java.util.ArrayList;
import javax.swing.AbstractAction;
import javax.swing.JFileChooser;
import javax.swing.KeyStroke;
import logic.MovieFileReader;
import logic.database.Video;
import logic.database.VideoDatabase;
import logic.database.jdbc.DatabaseConnector;
import logic.settings.languages.LanguageManager;

/**
 *
 * @author alexander
 */
public class AddListener extends AbstractAction {
    private static AddListener instance = new AddListener();
    
    private AddListener() {
	super(LanguageManager.getInstance().get("btnAddLabel"), ImageFactory.getInstance().getImageIcon("add"));  
        putValue(SHORT_DESCRIPTION, LanguageManager.getInstance().get("btnAddShortDescription"));
	MainWindowEventDispatcher.getInstance().addControlMaskedActionListener(getAcceleratorKey(), this);
    }

    public static AddListener getInstance() {
	return instance;
    }  
    
    public final KeyStroke getAcceleratorKey(){
	return KeyStroke.getKeyStroke(KeyEvent.VK_N, ActionEvent.CTRL_MASK);
    }
    
    @Override
    public void actionPerformed(ActionEvent e) {
	JFileChooser fc = new JFileChooser();
	fc.setFileSelectionMode(JFileChooser.FILES_AND_DIRECTORIES);
	fc.setMultiSelectionEnabled(true);
//	fc.setSelectedFile(fc.getCurrentDirectory());
	fc.setSelectedFile(new File("M:/Downloads/Movies"));
	int result = fc.showOpenDialog(MainWindow.getInstance());
	if( result == JFileChooser.APPROVE_OPTION){
	    ArrayList<Video> videos = new ArrayList<Video>();
	    for(File file : fc.getSelectedFiles()){
		 MovieFileReader.getVideos(file, videos);
	    }
	    
	    System.out.println("finished getting videos");
	    
	    DatabaseConnector.getInstance().insertVideosHDD(videos);
	    VideoDatabase.getInstance().addVideos(videos);
	    VideoTableModel.getInstance().updateData(videos);
	}
    }
    
}
