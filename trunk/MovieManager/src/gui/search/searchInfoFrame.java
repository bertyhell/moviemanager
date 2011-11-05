/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui.search;

import gui.ImagePanel;
import logic.imdbapi.VideoInformationCollector;
import java.awt.BorderLayout;
import java.awt.Color;
import java.awt.Dimension;
import java.awt.GridLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.ArrayList;
import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JTextField;
import logic.Actor;
import logic.settings.languages.LanguageManager;

/**
 *
 * @author alexander
 */
public class searchInfoFrame extends JFrame implements ActionListener {

    private JTextField txtActor;
    private ActorInformationPanel resultsPanel;

    public searchInfoFrame() {
	super();
	this.setLayout(new BorderLayout());

	//the panel on the upper side of the frame to hold the controls for search input
	JPanel searchTerms = new JPanel();
	GridLayout layout = new GridLayout(0, 2);
	searchTerms.setLayout(layout);

	searchTerms.add(new JLabel(LanguageManager.getInstance().get("lblActor")));
	txtActor = new JTextField(LanguageManager.getInstance().get("txtActor"));
	searchTerms.add(txtActor);

	JButton searchButton = new JButton(LanguageManager.getInstance().get("btnSearchLabel"));
	searchButton.addActionListener(this);
	searchTerms.add(searchButton);

	this.add(searchTerms, BorderLayout.PAGE_START);


	//panel for search results
	resultsPanel = new ActorInformationPanel();
	this.add(resultsPanel, BorderLayout.CENTER);


	this.setPreferredSize(new Dimension(800, 600));
	this.pack();
	this.setLocationRelativeTo(null);

    }

    @Override
    public void actionPerformed(ActionEvent e) {
	ArrayList<Actor> actors = VideoInformationCollector.SearchActorFromTmdb(txtActor.getText());
	if (actors.size() == 0) {
	} else if (actors.size() == 1) {
	    VideoInformationCollector.getActorInfoFromTmdb(actors.get(0));
	    this.resultsPanel.setActor(actors.get(0));
	} else {
	}
    }
}
