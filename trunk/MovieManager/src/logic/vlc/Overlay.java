/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package logic.vlc;

import com.sun.awt.AWTUtilities;
import com.sun.jna.platform.WindowUtils;
import java.awt.Dimension;
import java.awt.Graphics;
import java.awt.Window;
import java.awt.event.ComponentEvent;
import java.awt.event.ComponentListener;
import java.awt.event.KeyAdapter;
import java.awt.event.KeyEvent;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import javax.swing.JComponent;
import javax.swing.JFrame;
import javax.swing.KeyStroke;
import javax.swing.plaf.basic.BasicSliderUI.ComponentHandler;
import logic.vlc.actionListeners.PauseListener;

/**
 *
 * @author alexander
 */
public class Overlay extends Window implements ComponentListener {

    private VlcPlayerFrame parent;
    private VlcPlayer vlcPlayer;
    private Overlay panel = this;
    TranslucentComponent controlBarEnabler;
    TranslucentComponent FullScreenEnabler;

    public Overlay(VlcPlayerFrame owner) {
	super(owner, WindowUtils.getAlphaCompatibleGraphicsConfiguration());
	this.parent = owner;
	this.vlcPlayer = owner.getVlcPlayer();

	AWTUtilities.setWindowOpaque(this, false);
	setLayout(null);


	//control bar enabler
	controlBarEnabler = new TranslucentComponent();
	controlBarEnabler.addMouseListener(new MouseAdapter() {

	    @Override
	    public void mouseEntered(MouseEvent e) {
		vlcPlayer.setControlPanelVisible(true);
	    }
	});
	
	add(controlBarEnabler);

	//double click full screen +control bar disable
	FullScreenEnabler = new TranslucentComponent();
	FullScreenEnabler.addMouseListener(new MouseAdapter() {

	    @Override
	    public void mouseEntered(MouseEvent e) {
		vlcPlayer.setControlPanelVisible(false);
	    }

	    @Override
	    public void mousePressed(MouseEvent e) {
		if (e.getButton() == MouseEvent.BUTTON1 && e.getClickCount() == 2) {
		    parent.switchFullScreen();
		}
		else if(e.getButton() == MouseEvent.BUTTON3 && e.getClickCount() == 1){
		    VlcPopupMenu menu = new VlcPopupMenu(parent.getVlcPlayer());
		    menu.show(parent, e.getPoint().x, e.getPoint().y);
		}
	    }
	});
	add(FullScreenEnabler);


	parent.addComponentListener(this);
	

    }
    //<editor-fold defaultstate="collapsed" desc="component listener --> resizing handler">

   
    @Override
    public void paint(Graphics g) {
	super.paint(g);
    }

    @Override
    public void componentResized(ComponentEvent e) {
	System.out.println("resized " + parent.getContentPane().getSize());
	this.setSize(parent.getSize());
	FullScreenEnabler.setBounds(0, 0, parent.getSize().width, parent.getContentPane().getSize().height - 60);
	System.out.println(this.getSize());
	controlBarEnabler.setBounds(0, parent.getContentPane().getSize().height - 12, parent.getContentPane().getSize().width, 12);

    }

    @Override
    public void componentMoved(ComponentEvent e) {

    }

    @Override
    public void componentShown(ComponentEvent e) {
    }

    @Override
    public void componentHidden(ComponentEvent e) {
    }
    //</editor-fold>
}
