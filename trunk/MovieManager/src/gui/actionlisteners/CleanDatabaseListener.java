/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui.actionlisteners;

import gui.MainWindow;
import gui.tables.VideoTableModel;
import img.ImageFactory;
import java.awt.event.ActionEvent;
import java.util.ArrayList;
import java.util.Properties;
import javax.swing.AbstractAction;
import javax.swing.JOptionPane;
import logic.database.Video;
import logic.database.jdbc.DatabaseConnector;
import logic.settings.languages.LanguageManager;

/**
 *
 * @author alexander
 */
public class CleanDatabaseListener extends AbstractAction {

    private static CleanDatabaseListener instance = new CleanDatabaseListener();
    private Properties prop = new Properties();

    private CleanDatabaseListener() {
	super(LanguageManager.getInstance().get("btnCleanDatabaseLabel"), ImageFactory.getInstance().getImageIcon("clear"));
	putValue(SHORT_DESCRIPTION, LanguageManager.getInstance().get("btnCleanDatabaseShortDescription"));
//	MainWindowEventDispatcher.getInstance().addControlMaskedActionListener(getAcceleratorKey(), this);
    }

    public static CleanDatabaseListener getInstance() {
	return instance;
    }

//    public final KeyStroke getAcceleratorKey() {
//	return KeyStroke.getKeyStroke(KeyEvent.VK_N, ActionEvent.CTRL_MASK);
//    }
    @Override
    public void actionPerformed(ActionEvent e) {
	if (JOptionPane.showConfirmDialog(MainWindow.getInstance(), LanguageManager.getInstance().get("lblConfirmMessage"), LanguageManager.getInstance().get("lblConfirm"), JOptionPane.YES_NO_OPTION, JOptionPane.QUESTION_MESSAGE) == JOptionPane.OK_OPTION) {
	    DatabaseConnector.getInstance().emptyTables();
	    VideoTableModel.getInstance().updateData(new ArrayList<Video>());
	}
    }
}
