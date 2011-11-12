/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package logic.vlc;

import java.awt.Color;
import java.awt.Dimension;
import java.awt.Point;
import java.awt.Toolkit;
import java.awt.event.KeyEvent;
import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;
import javax.swing.AbstractAction;
import javax.swing.JComponent;
import javax.swing.JFrame;
import javax.swing.KeyStroke;
import logic.vlc.actionListeners.MuteListener;
import logic.vlc.actionListeners.PauseListener;
import logic.vlc.actionListeners.StopStartListener;

/**
 *
 * @author alexander
 */
public class VlcPlayerFrame extends JFrame {

    private VlcPlayer vlcPlayerPanel;
    private Dimension previousSize;
    private Point previousLocation;
    private boolean isFullscreen = false;

    public VlcPlayerFrame(boolean enableVlcControls) {
	vlcPlayerPanel = new VlcPlayer(this);
//	this.setUndecorated(true);	
	this.add(vlcPlayerPanel);
	this.setBackground(Color.red);
	this.setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);
	this.addWindowListener(new WindowAdapter() {

	    @Override
	    public void windowClosing(WindowEvent e) {
		vlcPlayerPanel.dispose();
	    }
	});


	this.pack();
	this.setLocationRelativeTo(null);
	if (enableVlcControls) {
//	    this.setVisible(true);
	    enableVlcControls();
	}
    }

    public VlcPlayer getVlcPlayer() {
	return vlcPlayerPanel;
    }

    public final void enableVlcControls() {
	if (!isVisible()) {
	    initializeShortcutKeys();



	    this.setVisible(true);
	    this.requestFocus();
	    vlcPlayerPanel.enableVideoOverlay();
	} else {
	    System.out.println("VlcPlayerFrame: The frame must be visible before enabling the controls");
	}
    }

    private void initializeShortcutKeys() {
	initializeShortcutKey("Pause",KeyStroke.getKeyStroke(KeyEvent.VK_SPACE, 0, true) , new PauseListener(vlcPlayerPanel));
	initializeShortcutKey("Stop", KeyStroke.getKeyStroke(KeyEvent.VK_S, KeyEvent.CTRL_DOWN_MASK, true), new StopStartListener(vlcPlayerPanel));
	initializeShortcutKey("Mute", KeyStroke.getKeyStroke(KeyEvent.VK_M, KeyEvent.CTRL_DOWN_MASK, true), new MuteListener(vlcPlayerPanel));
    }
    
    private void initializeShortcutKey(String ActionName, KeyStroke stroke, AbstractAction action){
	getRootPane().getInputMap(JComponent.WHEN_IN_FOCUSED_WINDOW).put(stroke, ActionName);
	getRootPane().getActionMap().put(ActionName, action);
    }

    //<editor-fold defaultstate="collapsed" desc="Fullscreen mode switch">
    public boolean isFullscreen() {
	return isFullscreen;
    }

    public void enterFullScreen() {
	if (!isFullscreen) {
	    previousLocation = this.getLocation();
	    previousSize = this.getSize();
	    this.setLocation(0, 0);
	    this.setSize(Toolkit.getDefaultToolkit().getScreenSize());
	    isFullscreen = true;
//	    GraphicsEnvironment.getLocalGraphicsEnvironment().getDefaultScreenDevice().setFullScreenWindow(this);
	}
    }

    public void exitFullScreen() {
	if (isFullscreen) {
	    this.setLocation(previousLocation);
	    this.setSize(previousSize);
	    isFullscreen = false;
	}
    }

    public void switchFullScreen() {
	if (isFullscreen) {
	    exitFullScreen();
	} else {
	    enterFullScreen();
	}
    }
    //</editor-fold>
}
