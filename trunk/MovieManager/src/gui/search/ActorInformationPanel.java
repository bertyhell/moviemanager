/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui.search;

import gui.ImagePanel;
import java.awt.BorderLayout;
import java.awt.Dimension;
import javax.swing.JPanel;
import logic.Actor;

/**
 *
 * @author alexander
 */
public class ActorInformationPanel extends JPanel {

    private ImagePanel imagePanel = new ImagePanel(new Dimension(200, 300));
    private MovieScrollPanel movieScrollPanel = new MovieScrollPanel();
    private Actor actor;
    
    public ActorInformationPanel(){
	this.setLayout(new BorderLayout());
	this.add(imagePanel, BorderLayout.LINE_START);
	
	this.add(movieScrollPanel,BorderLayout.PAGE_END);
	
    }
    
    public ActorInformationPanel(Actor actor) {
	this();
	this.actor  = actor;
	if(actor != null){
	    refreshInfo();
	}
    }

    public Actor getActor() {
	return actor;
    }

    public void setActor(Actor actor) {
	this.actor = actor;
	refreshInfo();
    }
    
    public void refreshInfo(){
	this.imagePanel.setImg(actor.getImages().get(0));
	this.movieScrollPanel.setMovies(actor.getMovieImageUrlStrings(), new Dimension(200,300));
	this.validate();
	
    }
    
}
