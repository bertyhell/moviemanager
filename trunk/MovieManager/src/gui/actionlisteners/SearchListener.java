/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui.actionlisteners;

import gui.MainWindow;
import gui.search.searchInfoFrame;
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
public class SearchListener extends AbstractAction {
    private static SearchListener instance = new SearchListener();
    
    private SearchListener() {
	super(LanguageManager.getInstance().get("btnSearchLabel"), ImageFactory.getInstance().getImageIcon("search"));  
        putValue(SHORT_DESCRIPTION, LanguageManager.getInstance().get("btnSearchShortDescription"));
//	MainWindowEventDispatcher.getInstance().addActionListener(getAcceleratorKey(), this);
    }

    public static SearchListener getInstance() {
	return instance;
    }  
    
//    public final KeyStroke getAcceleratorKey(){
//	return KeyStroke.getKeyStroke(KeyEvent.VK_N, ActionEvent.CTRL_MASK);
//    }
    
    @Override
    public void actionPerformed(ActionEvent e) {
	searchInfoFrame frame = new searchInfoFrame();
	frame.setVisible(true);
    }
    
}
