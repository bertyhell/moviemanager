/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui.actionlisteners;

import img.ImageFactory;
import java.awt.event.ActionEvent;
import java.awt.event.KeyEvent;
import javax.swing.AbstractAction;
import javax.swing.KeyStroke;
import logic.database.VideoDatabase;
import logic.database.jdbc.DatabaseConnector;
import logic.settings.languages.LanguageManager;

/**
 *
 * @author alexander
 */
public class SaveVideosListener extends AbstractAction {
    private static SaveVideosListener instance = new SaveVideosListener();
    
    private SaveVideosListener() {
	super(LanguageManager.getInstance().get("btnSaveLabel"), ImageFactory.getInstance().getImageIcon("save"));  
        putValue(SHORT_DESCRIPTION, LanguageManager.getInstance().get("btnSaveShortDescription"));
	MainWindowEventDispatcher.getInstance().addControlMaskedActionListener(getAcceleratorKey(), this);
    }

    public static SaveVideosListener getInstance() {
	return instance;
    }  
    
    public final KeyStroke getAcceleratorKey(){
	return KeyStroke.getKeyStroke(KeyEvent.VK_S, ActionEvent.CTRL_MASK);
    }
    
    @Override
    public void actionPerformed(ActionEvent e) {
	System.out.println("begin saving");
	VideoDatabase.getInstance().saveToHDD();
    }
}
