/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui.tables.headers.contextmenu;

import gui.tables.headers.TableColumnHeaders;
import gui.tables.VideoTableModel;
import java.awt.event.ItemEvent;
import java.awt.event.ItemListener;
import java.util.Locale;
import javax.swing.JCheckBoxMenuItem;

/**
 *
 * @author alexander
 */
public class HeaderContextMenuItemListener implements ItemListener {
    private static HeaderContextMenuItemListener instance = new HeaderContextMenuItemListener();
    
    private HeaderContextMenuItemListener(){
	
    }
    
    public static HeaderContextMenuItemListener getInstance(){
	return instance;
    }
    
    private TableColumnHeaders headers = TableColumnHeaders.getInstance();

    @Override
    public void itemStateChanged(ItemEvent e) {
	JCheckBoxMenuItem item = (JCheckBoxMenuItem)e.getItem();
	headers.setColumnVisibility(item.getText(),item.getState());
	VideoTableModel.getInstance().UpdateTable();
    }
    
}
