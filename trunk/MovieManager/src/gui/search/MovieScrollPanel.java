/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui.search;

import img.ImageFactory;
import java.awt.BorderLayout;
import java.awt.Color;
import java.awt.Dimension;
import java.awt.Point;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.ArrayList;
import javax.swing.JButton;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.JViewport;
import javax.swing.ScrollPaneConstants;

/**
 *
 * @author alexander
 */
public class MovieScrollPanel extends JPanel {
    private JScrollPane scrollPanel;
    private MoviePanel moviePanel;
    
    public MovieScrollPanel(){
	this.setLayout(new BorderLayout());

	JButton previous = new JButton(ImageFactory.getInstance().getImageIcon("previous"));
	previous.addActionListener(new ActionListener() {

	    @Override
	    public void actionPerformed(ActionEvent e) {
		ViewportRepositionLeft();
	    }
	});
	this.add(previous,BorderLayout.LINE_START);
	moviePanel = new MoviePanel();
	scrollPanel = new JScrollPane(moviePanel);
	scrollPanel.setVerticalScrollBarPolicy(ScrollPaneConstants.VERTICAL_SCROLLBAR_NEVER);
	scrollPanel.setHorizontalScrollBarPolicy(ScrollPaneConstants.HORIZONTAL_SCROLLBAR_NEVER);
	this.add(scrollPanel, BorderLayout.CENTER);

	JButton next = new JButton(ImageFactory.getInstance().getImageIcon("next"));
	next.addActionListener(new ActionListener() {

	    @Override
	    public void actionPerformed(ActionEvent e) {
		ViewportRepositionRight();
	    }
	});
	this.add(next, BorderLayout.LINE_END);
    }
    
    public void setMovies(ArrayList<String> urls, Dimension dim){
	this.setPreferredSize(dim);
	this.setMinimumSize(dim);
	moviePanel.setImages(urls, dim);
    }
    
    public void ViewportRepositionRight(){
	JViewport viewport = scrollPanel.getViewport();
	int newPosition = viewport.getViewPosition().x + moviePanel.getImageWidth();
	newPosition = (newPosition < moviePanel.getSize().width - scrollPanel.getWidth() ? newPosition : moviePanel.getSize().width - scrollPanel.getWidth());
	viewport.setViewPosition(new Point( newPosition,0));
    }
    
    public void ViewportRepositionLeft(){
	JViewport viewport = scrollPanel.getViewport();
	int newPosition = viewport.getViewPosition().x - moviePanel.getImageWidth();
	newPosition = (newPosition > 0 ? newPosition : 0);
	viewport.setViewPosition(new Point( newPosition,0));
    }
}
