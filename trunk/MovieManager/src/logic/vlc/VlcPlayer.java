/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package logic.vlc;

import gui.MainWindow;
import java.awt.Canvas;
import javax.swing.JPanel;
import uk.co.caprica.vlcj.binding.LibVlc;
import uk.co.caprica.vlcj.player.MediaPlayerEventAdapter;
import uk.co.caprica.vlcj.player.MediaPlayerFactory;
import uk.co.caprica.vlcj.player.embedded.DefaultFullScreenStrategy;
import uk.co.caprica.vlcj.player.embedded.EmbeddedMediaPlayer;
import uk.co.caprica.vlcj.player.embedded.FullScreenStrategy;

/**
 *
 * @author alexander
 */
public class VlcPlayer extends JPanel {

    private EmbeddedMediaPlayer mediaPlayer;
    private MediaPlayerFactory mediaPlayerFactory;

    public VlcPlayer() {
//        String[] libvlcArgs = {};

        // Create a media player factory
        mediaPlayerFactory = new MediaPlayerFactory();
        // Create a full screen strategy
        FullScreenStrategy fullScreenStrategy = new DefaultFullScreenStrategy(MainWindow.getInstance());
        // Create a new media player instance for the run-time platform
        mediaPlayer = mediaPlayerFactory.newMediaPlayer(fullScreenStrategy);

        // Set standard options as needed to be applied to all subsequently played media items
        String[] standardMediaOptions = {"video-filter=logo", "logo-file=vlcj-logo.png", "logo-opacity=25"};

        mediaPlayer.setStandardMediaOptions(standardMediaOptions);

        // Add a component to be notified of player events
        mediaPlayer.addMediaPlayerEventListener(
                new MediaPlayerEventAdapter() {
                });

        // Create and set a new component to display the rendered video
        Canvas videoSurface = new Canvas();
        this.add(videoSurface);
        mediaPlayer.setVideoSurface(videoSurface);
        // Do some interesting things in the application      
    }
    
    

    public void start(String mrl) {
        // vlcj: play the media
        mediaPlayer.playMedia(mrl);
    }
    
    public void dispose(){
    // Cleanly dispose of the media player instance and any associated native resources
        mediaPlayer.release();

        // Cleanly dispose of the media player factory and any associated native resources
        mediaPlayerFactory.release();
    }
}
