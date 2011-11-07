/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui.Menubar;

import gui.actionlisteners.AddListener;
import gui.actionlisteners.AnalyseListener;
import gui.actionlisteners.CleanDatabaseListener;
import gui.actionlisteners.SaveVideosListener;
import gui.actionlisteners.SearchListener;
import java.awt.BorderLayout;
import java.awt.FlowLayout;
import java.awt.GridLayout;
import javax.swing.JButton;
import javax.swing.JMenuBar;
import javax.swing.JPanel;
import javax.swing.JTabbedPane;
import javax.swing.SwingConstants;
import logic.settings.languages.LanguageManager;

/**
 *
 * @author alexander
 */
public class Menubar extends JPanel {
    private static Menubar instance = new Menubar();
    
    private Menubar(){
	JTabbedPane tabbed = new JTabbedPane();
	
	//start tab
	JPanel PnlStart = new JPanel(new FlowLayout(FlowLayout.LEFT));
	//add videos button
        JButton btnAdd = new JButton(AddListener.getInstance());
	btnAdd.setHorizontalTextPosition(SwingConstants.CENTER);
	btnAdd.setVerticalTextPosition(SwingConstants.BOTTOM); 
	PnlStart.add(btnAdd);
	//Save videos button	
	JButton btnSave = new JButton(SaveVideosListener.getInstance());
	btnSave.setHorizontalTextPosition(SwingConstants.CENTER);
	btnSave.setVerticalTextPosition(SwingConstants.BOTTOM);
	PnlStart.add(btnSave);
	//analyse videos button	
	JButton btnAnalyse = new JButton(AnalyseListener.getInstance());
	btnAnalyse.setHorizontalTextPosition(SwingConstants.CENTER);
	btnAnalyse.setVerticalTextPosition(SwingConstants.BOTTOM);
	PnlStart.add(btnAnalyse);
	//search button	
	JButton btnSearch = new JButton(SearchListener.getInstance());
	btnSearch.setHorizontalTextPosition(SwingConstants.CENTER);
	btnSearch.setVerticalTextPosition(SwingConstants.BOTTOM);
	PnlStart.add(btnSearch);
	//cleanDatabase button	
	JButton btnClearDatabase = new JButton(CleanDatabaseListener.getInstance());
	btnClearDatabase.setHorizontalTextPosition(SwingConstants.CENTER);
	btnClearDatabase.setVerticalTextPosition(SwingConstants.BOTTOM);
	PnlStart.add(btnClearDatabase);
	
	
	
	
	
	tabbed.addTab(LanguageManager.getInstance().get("start"), PnlStart);
	
	//help tab
	JMenuBar barHelp = new JMenuBar();
	JPanel helpPanel = new JPanel();
	
	barHelp.add(helpPanel);
	tabbed.addTab(LanguageManager.getInstance().get("help"), barHelp);
	
	
	setLayout(new GridLayout(1, 1));
	this.add(tabbed,BorderLayout.CENTER);
	
    }
    
    public static Menubar getInstance(){
        return instance;
    }
    
}
