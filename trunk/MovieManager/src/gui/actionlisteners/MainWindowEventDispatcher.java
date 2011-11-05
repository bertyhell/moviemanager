/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui.actionlisteners;

import java.awt.KeyEventDispatcher;
import java.awt.event.KeyEvent;
import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;
import javax.swing.AbstractAction;
import javax.swing.KeyStroke;

/**
 *
 * @author alexander
 */
public class MainWindowEventDispatcher implements KeyEventDispatcher {

    private static MainWindowEventDispatcher instance = new MainWindowEventDispatcher();
    private Map<Integer, AbstractAction> controlMaskedActions = new HashMap<Integer, AbstractAction>();

    private MainWindowEventDispatcher() {
    }

    public static MainWindowEventDispatcher getInstance() {
	return instance;
    }

    @Override
    public boolean dispatchKeyEvent(KeyEvent e) {
	if (e.getID() == KeyEvent.KEY_RELEASED) {
	    if(e.isControlDown()){
		if (controlMaskedActions.containsKey(e.getKeyCode())) {
		    controlMaskedActions.get(e.getKeyCode()).actionPerformed(null);
		}
	    }
	}
	return false;
    }

    public void addControlMaskedActionListener(KeyStroke keys, AbstractAction action) {
	controlMaskedActions.put(keys.getKeyCode(), action);
    }

    public void printShortcutKeys() {
	Iterator iterator = controlMaskedActions.keySet().iterator();

	while (iterator.hasNext()) {
	    Integer key = (Integer)iterator.next();
	    String value = controlMaskedActions.get(key).getClass().getSimpleName();

	    System.out.println(key.toString() + " " + value);
	}
    }
}