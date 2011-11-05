/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui.actionlisteners;

import gui.search.searchInfoFrame;
import img.ImageFactory;
import java.awt.event.ActionEvent;
import javax.swing.AbstractAction;
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
