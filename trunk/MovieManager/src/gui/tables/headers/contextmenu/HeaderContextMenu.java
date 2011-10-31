/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui.tables.headers.contextmenu;

import gui.tables.headers.TableColumnHeaders;
import java.util.ArrayList;
import java.util.HashMap;
import javax.swing.JCheckBoxMenuItem;
import javax.swing.JPopupMenu;

/**
 *
 * @author alexander
 */
public class HeaderContextMenu extends JPopupMenu{
    public static HeaderContextMenu instance = new HeaderContextMenu();    
    
    private TableColumnHeaders headers = TableColumnHeaders.getInstance();
    
    public HeaderContextMenu() {
	HashMap<String,Boolean> columns = headers.getColumns();
	ArrayList<String> columnsInOrder =  headers.getColumnsInOrder();
	for(String col : columnsInOrder){
	    JCheckBoxMenuItem item = new JCheckBoxMenuItem(col);
	    item.addItemListener(HeaderContextMenuItemListener.getInstance());
	    if(columns.get(col)){
		item.setState(true);
	    }
	    this.add(item);
	}
    }

    public static HeaderContextMenu getInstance() {
	return instance;
    }
    
    
    
}
