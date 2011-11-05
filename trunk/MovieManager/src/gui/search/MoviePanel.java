/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui.search;

import gui.ImagePanel;
import java.awt.Dimension;
import java.awt.GridLayout;
import java.util.ArrayList;
import javax.swing.JPanel;
import javax.swing.SpringLayout;

/**
 *
 * @author alexander
 */
public class MoviePanel extends JPanel {
    private SpringLayout layout;
    private int imageWidth;

    public MoviePanel() {
	layout = new SpringLayout();
	this.setLayout(layout);
    }

    public MoviePanel(ArrayList<String> urls, Dimension dim) {
	this();
	setImages(urls, dim);
    }

    public int getImageWidth() {
	return imageWidth;
    }
    
    public void setImages(ArrayList<String> urls, Dimension dim) {
	imageWidth = dim.width;
	this.removeAll();
	ImagePanel previous = null;
	ImagePanel imagePanel = null;
	for (String url : urls) {
	    imagePanel = new ImagePanel(url, dim);
	    this.add(imagePanel);
	    if(previous ==  null){
		layout.putConstraint(SpringLayout.WEST, imagePanel, 5, SpringLayout.WEST, this);
	    }else{
		layout.putConstraint(SpringLayout.WEST, imagePanel, 5, SpringLayout.EAST, previous);
	    }
	    previous = imagePanel;
	}
	this.setMinimumSize(new Dimension(urls.size() * (dim.width + 5) + 10,300));
	this.setPreferredSize(new Dimension(urls.size() * (dim.width + 5) + 10,300));
	this.invalidate();
	this.validate();
    }
}
