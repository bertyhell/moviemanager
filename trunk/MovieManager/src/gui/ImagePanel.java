/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui;

import img.ImageFactory;
import java.awt.Color;
import java.awt.Dimension;
import java.awt.Graphics;
import java.awt.Image;
import java.io.IOException;
import java.net.MalformedURLException;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.swing.JPanel;

/**
 *
 * @author alexander
 */
public class ImagePanel extends JPanel {

//image object
    private Image img;
    private Dimension imageDimension;

    public ImagePanel(Dimension imageDimension) {
	this.imageDimension = imageDimension;
	this.setPreferredSize(imageDimension);
	this.setMinimumSize(imageDimension);
    }

    public ImagePanel(Image image, Dimension imageDimension) {
	this(imageDimension);
	img = image;
    }

    public ImagePanel(String url, Dimension imageDimension) {
	this(imageDimension);
	try {
	    img = ImageFactory.getInstance().getImageFromUrl(url);
	    if (img != null) {
		img = ImageFactory.getInstance().scaleImageNonStretched(img, this.imageDimension.width, this.imageDimension.height);
	    }
	} catch (MalformedURLException ex) {
	    Logger.getLogger(ImagePanel.class.getName()).log(Level.SEVERE, null, ex);
	} catch (IOException ex) {
	    Logger.getLogger(ImagePanel.class.getName()).log(Level.SEVERE, null, ex);
	}

    }

    public void setImg(Image img) {
	this.img = ImageFactory.getInstance().scaleImageNonStretched(img, imageDimension.width, imageDimension.height);
	this.repaint();
    }

    @Override
    public void paint(Graphics g) {
	super.paint(g);
	if (img != null) {
	    g.drawImage(img, 0, 0, this);
	}
    }
}
