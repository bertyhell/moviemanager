/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui;

import gui.tables.OverviewTable;
import java.awt.GridLayout;
import javax.swing.JPanel;
import javax.swing.JScrollPane;

/**
 *
 * @author alexander
 */
public class ContentPanel extends JPanel {

    private static ContentPanel instance = new ContentPanel();

    private ContentPanel() {
	super();
//	this.setPreferredSize(new Dimension(800, 600));


	//table
	OverviewTable table = OverviewTable.getInstance();
	
	JScrollPane scrollPane = new JScrollPane(table);

	this.setLayout(new GridLayout(1, 1));
	this.add(scrollPane);

    }

    public static ContentPanel getInstance() {
	return instance;
    }
}
