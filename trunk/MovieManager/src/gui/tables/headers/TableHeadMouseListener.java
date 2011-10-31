/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui.tables.headers;

import gui.tables.OverviewTable;
import gui.tables.headers.contextmenu.HeaderContextMenu;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;

/**
 *
 * @author alexander
 */
public class TableHeadMouseListener extends MouseAdapter {
    private static TableHeadMouseListener instance = new TableHeadMouseListener();
    
    @Override
    public void mouseReleased(MouseEvent e) {
      if(e.getButton() == MouseEvent.BUTTON3){
	  HeaderContextMenu menu = HeaderContextMenu.getInstance();
	  menu.show(OverviewTable.getInstance().getTableHeader(), e.getX() , e.getY());
      }
    }

    public static TableHeadMouseListener getInstance() {
	return instance;
    }
    
    
    
}
