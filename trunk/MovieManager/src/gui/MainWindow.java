/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui;

import gui.Menubar.Menubar;
import gui.actionlisteners.MainWindowEventDispatcher;
import gui.tables.CellEditors.RatingEditorPanel;
import java.awt.BorderLayout;
import java.awt.Dimension;
import java.awt.KeyboardFocusManager;
import javax.swing.ImageIcon;
import javax.swing.JFrame;
import logic.database.VideoDatabase;

/**
 *
 * @author alexander
 */
public class MainWindow extends JFrame {
    private static MainWindow window = new MainWindow();
    
    private MainWindow(){
        super("MovieManager v0.1");
	this.setIconImage(new ImageIcon(getClass().getResource("/img/picto.png")).getImage());
        this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
	
        this.add(Menubar.getInstance(),BorderLayout.PAGE_START); 
	KeyboardFocusManager manager = KeyboardFocusManager.getCurrentKeyboardFocusManager();
	manager.addKeyEventDispatcher(MainWindowEventDispatcher.getInstance());

	//add left treeviewpanel
	TreeViewPanel treepanel = new TreeViewPanel();
	this.add(treepanel, BorderLayout.LINE_START);
	
	//add content panel
	this.add(ContentPanel.getInstance(), BorderLayout.CENTER);
	
	//test
//	this.add(new RatingEditorPanel(VideoDatabase.getInstance().getVideo(0)),BorderLayout.PAGE_END);
	
	this.setPreferredSize(new Dimension(800,600));
        this.pack();
        this.setLocationRelativeTo(null);
        this.setVisible(true);
    }
    
    public static MainWindow getInstance(){
        return window;
    }
}
