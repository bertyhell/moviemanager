/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package logic.vlc;

import com.sun.awt.AWTUtilities;
import com.sun.jna.platform.WindowUtils;
import java.awt.Color;
import java.awt.GradientPaint;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.RenderingHints;
import java.awt.Window;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import javax.swing.JButton;

/**
 *
 * @author alexander
 */
public class Overlay extends Window {

    private static final long serialVersionUID = 1L;
    private VlcPlayerFrame parent;

    public Overlay(VlcPlayerFrame owner) {
	super(owner, WindowUtils.getAlphaCompatibleGraphicsConfiguration());

	this.parent = owner;
	AWTUtilities.setWindowOpaque(this, false);
	setLayout(null);

	TranslucentComponent c = new TranslucentComponent();
	c.addMouseListener(new MouseAdapter() {

	    @Override
	    public void mouseEntered(MouseEvent e) {
		parent.getPlayer().setControlPanelVisible(true);
	    }
	});
	c.setBounds(0, owner.getSize().height - 15, owner.getSize().width, 12);
	add(c);
	
	TranslucentComponent c2 = new TranslucentComponent();
	c2.addMouseListener(new MouseAdapter() {

	    @Override
	    public void mouseEntered(MouseEvent e) {
		parent.getPlayer().setControlPanelVisible(false);
	    }
	    
	    @Override
	    public void mousePressed(MouseEvent e) {
		if(e.getButton() == MouseEvent.BUTTON1 && e.getClickCount() == 2){
		    parent.switchFullScreen();
		}
	    }
	});
	c2.setBounds(0,0 , owner.getSize().width, owner.getSize().height - 60);
	add(c2);
    }

    @Override
    public void paint(Graphics g) {
	super.paint(g);
    }
}
