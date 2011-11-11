/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package logic.vlc;

import java.awt.Color;
import java.awt.Dimension;
import java.awt.Point;
import java.awt.Toolkit;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;
import java.awt.event.WindowListener;
import javax.swing.JFrame;
import uk.co.caprica.vlcj.player.embedded.EmbeddedMediaPlayer;

/**
 *
 * @author alexander
 */
public class VlcPlayerFrame extends JFrame {

    private VlcPlayer vlcPlayer;
    private EmbeddedMediaPlayer player;
    private Dimension previousSize;
    private Point previousLocation;
    private boolean isFullscreen = false;

    public VlcPlayerFrame() {
	vlcPlayer = new VlcPlayer(this);
	player = vlcPlayer.getMediaPlayer();
	this.setUndecorated(true);
	this.setBackground(Color.BLACK);
	this.add(vlcPlayer);
	this.setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);

	this.addWindowListener(new WindowAdapter() {
	    @Override
	    public void windowClosing(WindowEvent e) {
		vlcPlayer.stop();
		vlcPlayer.dispose();
	    }
	});
	
	this.addMouseListener(new MouseAdapter() {
	    @Override
	    public void mouseClicked(MouseEvent e) {
		if (e.getButton() == MouseEvent.BUTTON1 && e.getClickCount() == 2) {
		    if (!isFullscreen) {
			enterFullScreen();
		    } else {
			exitFullScreen();
		    }
		}
	    }
	});

	this.pack();
	this.setLocationRelativeTo(null);

	
	this.setVisible(true);
	
	player.setOverlay(new Overlay(this));
	player.enableOverlay(true);
    }

    public boolean isFullscreen() {
	return isFullscreen;
    }

    public VlcPlayer getPlayer() {
	return vlcPlayer;
    }

    public void enterFullScreen() {
	if (!isFullscreen) {
	    previousLocation = this.getLocation();
	    previousSize = this.getSize();
	    this.setLocation(0, 0);
	    this.setSize(Toolkit.getDefaultToolkit().getScreenSize());
	    isFullscreen = true;
	    //GraphicsEnvironment.getLocalGraphicsEnvironment().getDefaultScreenDevice().setFullScreenWindow(this);
	}
    }

    public void exitFullScreen() {
	if (isFullscreen) {
	    this.setLocation(previousLocation);
	    this.setSize(previousSize);
	    isFullscreen = false;
	}
    }
    
    public void switchFullScreen(){
	if(isFullscreen){
	    exitFullScreen();
	}
	else{
	    enterFullScreen();
	}
    }
}
