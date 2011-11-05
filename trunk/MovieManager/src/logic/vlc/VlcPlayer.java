/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package logic.vlc;

import javax.swing.JPanel;
import uk.co.caprica.vlcj.component.EmbeddedMediaPlayerComponent;

/**
 *
 * @author alexander
 */
public class VlcPlayer extends JPanel {

    private EmbeddedMediaPlayerComponent mediaPlayerComponent;

    public VlcPlayer() {
	// vlcj: create the component
	System.setProperty("jna.library.path", "C:\\Program Files (x86)\\VideoLAN\\VLC");
	mediaPlayerComponent = new EmbeddedMediaPlayerComponent();
	this.add(mediaPlayerComponent);
    }

    public void start(String mrl) {
	// vlcj: play the media
	mediaPlayerComponent.getMediaPlayer().playMedia(mrl);
    }
}
