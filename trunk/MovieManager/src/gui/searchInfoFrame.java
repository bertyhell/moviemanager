/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui;

import gui.actionlisteners.VideoInformationCollector;
import java.awt.BorderLayout;
import java.awt.Dimension;
import java.awt.GridLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JTextField;
import logic.settings.languages.LanguageManager;

/**
 *
 * @author alexander
 */
public class searchInfoFrame extends JFrame {
	private JTextField txtActor;
    
	public searchInfoFrame(){
	super();
	this.setLayout(new BorderLayout());
	
	//the panel on the upper side of the frame to hold the controls for search input
	JPanel searchTerms = new JPanel();
        GridLayout layout = new GridLayout(0,2);
        searchTerms.setLayout(layout);
	
        searchTerms.add(new JLabel(LanguageManager.getInstance().get("lblActor")));
	txtActor = new JTextField(LanguageManager.getInstance().get("txtActor"),50);
        searchTerms.add(txtActor);
	
	JButton searchButton = new JButton(LanguageManager.getInstance().get("btnSearchLabel"));
	searchButton.addActionListener(new ActionListener() {

	    @Override
	    public void actionPerformed(ActionEvent e) {
		if( !txtActor.getText().isEmpty() ){
		VideoInformationCollector.getActorInfoFromTmdb(txtActor.getText());
		}
	    }
	});
	searchTerms.add(searchButton);
	
	this.add(searchTerms,BorderLayout.PAGE_START);
	
	
	//panel for search results
	JPanel searchResults = new JPanel();
	JLabel lblResults =  new JLabel();
	searchResults.add(lblResults);
	this.add(searchResults,BorderLayout.CENTER);
	
	this.setPreferredSize(new Dimension(800,600));
        this.pack();
        this.setLocationRelativeTo(null);
	
    }
    
    
    
}
