/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package logic.vlc;

import gui.MainWindow;
import java.awt.BorderLayout;
import java.awt.Canvas;
import java.awt.Color;
import java.awt.Dimension;
import java.awt.event.ComponentAdapter;
import java.awt.event.ComponentEvent;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.io.File;
import javax.swing.JLabel;
import javax.swing.JLayeredPane;
import javax.swing.JPanel;
import javax.swing.JSlider;
import javax.swing.event.ChangeEvent;
import javax.swing.event.ChangeListener;
import logic.common.TimeFormatter;
import logic.settings.ProgramSettings;
import uk.co.caprica.vlcj.player.MediaPlayer;
import uk.co.caprica.vlcj.player.MediaPlayerEventListener;
import uk.co.caprica.vlcj.player.MediaPlayerFactory;
import uk.co.caprica.vlcj.player.VideoMetaData;
import uk.co.caprica.vlcj.player.embedded.DefaultFullScreenStrategy;
import uk.co.caprica.vlcj.player.embedded.EmbeddedMediaPlayer;
import uk.co.caprica.vlcj.player.embedded.FullScreenStrategy;

/**
 *
 * @author alexander
 */
public class VlcPlayer extends JLayeredPane {

    VlcPlayerFrame parent;
    private VlcPlayer layeredVlcPane = this; // needed in sizelistener
    private JPanel vlcPlayerPanel;
    private Canvas videoSurface;
    private EmbeddedMediaPlayer mediaPlayer;
    private MediaPlayerFactory mediaPlayerFactory;
    //controlpanel
    private JPanel controlPanel;
    private JLabel currentTimeLabel;
    private JSlider timeChooser;
    private boolean sliderChangedByCode = false;
    //sizes
    private Dimension controlPanelSize;
    private Dimension videoSize;

    public VlcPlayer() {
	this.setBackground(Color.red);
	videoSize = new Dimension(800, 600);
	controlPanelSize = new Dimension(500, 50);
	this.setPreferredSize(videoSize);

	initPlayer();
	initControlPanel();

	vlcPlayerPanel.setBounds(0, 0, videoSize.width, videoSize.height);
	controlPanel.setBounds(videoSize.width / 2 - controlPanelSize.width / 2, videoSize.height - controlPanelSize.height, controlPanelSize.width, controlPanelSize.height);

	this.add(vlcPlayerPanel, 1, 0);
	this.add(controlPanel, 2, 0);
	controlPanel.setVisible(false);
	controlPanel.addMouseListener(new MouseAdapter() {

	    @Override
	    public void mouseClicked(MouseEvent e) {
		if(e.getButton() == MouseEvent.BUTTON3){
		    controlPanel.setVisible(false);
		}
	    }
	    
	});



	//automatically rescale video on resize
	vlcPlayerPanel.addComponentListener(new ComponentAdapter() {

	    @Override
	    public void componentResized(ComponentEvent e) {
		videoSurface.setSize(vlcPlayerPanel.getSize());
	    }
	});
	this.addComponentListener(new ComponentAdapter() {

	    @Override
	    public void componentResized(ComponentEvent e) {
		videoSize = layeredVlcPane.getSize();
		vlcPlayerPanel.setBounds(0, 0, videoSize.width, videoSize.height);
		controlPanel.setBounds(videoSize.width / 2 - controlPanelSize.width / 2, videoSize.height - controlPanelSize.height, controlPanelSize.width, controlPanelSize.height);
	    }
	});

    }

    public VlcPlayer(VlcPlayerFrame parent) {
	this();
	this.parent = parent;
    }

    public EmbeddedMediaPlayer getMediaPlayer() {
	return mediaPlayer;
    }

    private void initPlayer() {
	vlcPlayerPanel = new JPanel();
	System.setProperty("jna.library.path", ProgramSettings.getInstance().get("Vlclib_location"));
	mediaPlayerFactory = new MediaPlayerFactory();
	// Create a full screen strategy
	FullScreenStrategy fullScreenStrategy = new DefaultFullScreenStrategy(MainWindow.getInstance());
	// Create a new media player instance for the run-time platform
	mediaPlayer = mediaPlayerFactory.newMediaPlayer(fullScreenStrategy);

	// Create and set a new component to display the rendered video
	videoSurface = new Canvas();
	videoSurface.setSize(videoSize);
	videoSurface.addMouseListener(new MouseAdapter() {

	    @Override
	    public void mouseClicked(MouseEvent e) {
		if (parent != null) {
		    if (e.getButton() == MouseEvent.BUTTON1 && e.getClickCount() == 2) {
			if (!parent.isFullscreen()) {
			    enterFullScreen();
			} else {
			    exitFullScreen();
			}
		    }
		}
	    }
	});
	mediaPlayer.setVideoSurface(videoSurface);
	vlcPlayerPanel.add(videoSurface);

	addMediaPlayerEventListener();
    }

    private void addMediaPlayerEventListener() {

	mediaPlayer.addMediaPlayerEventListener(new MediaPlayerEventListener() {

	    @Override
	    public void mediaChanged(MediaPlayer mediaPlayer) {
	    }

	    @Override
	    public void opening(MediaPlayer mediaPlayer) {
	    }

	    @Override
	    public void buffering(MediaPlayer mediaPlayer) {
	    }

	    @Override
	    public void playing(MediaPlayer mediaPlayer) {
	    }

	    @Override
	    public void paused(MediaPlayer mediaPlayer) {
	    }

	    @Override
	    public void stopped(MediaPlayer mediaPlayer) {
	    }

	    @Override
	    public void forward(MediaPlayer mediaPlayer) {
	    }

	    @Override
	    public void backward(MediaPlayer mediaPlayer) {
	    }

	    @Override
	    public void finished(MediaPlayer mediaPlayer) {
	    }

	    @Override
	    public void timeChanged(MediaPlayer mediaPlayer, long newTime) {
		currentTimeLabel.setText(TimeFormatter.longToTime(newTime) + "/" + TimeFormatter.longToTime(mediaPlayer.getLength()));
		int sliderNewPosition = (int) (mediaPlayer.getPosition() * 1000);
		if (Math.abs(timeChooser.getValue() - sliderNewPosition) > 1) {
		    sliderChangedByCode = true;
		    timeChooser.setValue(sliderNewPosition);
		}
	    }

	    @Override
	    public void positionChanged(MediaPlayer mediaPlayer, float newPosition) {
	    }

	    @Override
	    public void seekableChanged(MediaPlayer mediaPlayer, int newSeekable) {
	    }

	    @Override
	    public void pausableChanged(MediaPlayer mediaPlayer, int newSeekable) {
	    }

	    @Override
	    public void titleChanged(MediaPlayer mediaPlayer, int newSeekable) {
	    }

	    @Override
	    public void snapshotTaken(MediaPlayer mediaPlayer, String filename) {
	    }

	    @Override
	    public void lengthChanged(MediaPlayer mediaPlayer, long newLength) {
	    }

	    @Override
	    public void error(MediaPlayer mediaPlayer) {
	    }

	    @Override
	    public void metaDataAvailable(MediaPlayer mediaPlayer, VideoMetaData videoMetaData) {
	    }
	});
    }

    private void initControlPanel() {
	controlPanel = new JPanel();
	controlPanel.setLayout(new BorderLayout());
	controlPanel.setPreferredSize(controlPanelSize);

	//time label
	currentTimeLabel = new JLabel();

	currentTimeLabel.setText("0:00:00/" + TimeFormatter.longToTime(mediaPlayer.getLength()));
	controlPanel.add(currentTimeLabel, BorderLayout.SOUTH);


	//slider bar to choose time
	timeChooser = new JSlider(0, 1000, 0);

	timeChooser.addChangeListener(new ChangeListener() {

	    @Override
	    public void stateChanged(ChangeEvent e) {
		if (!sliderChangedByCode) {
		    JSlider source = (JSlider) e.getSource();
		    mediaPlayer.setPosition((float) (source.getValue() * 1.0 / source.getMaximum()));
		} else {
		    sliderChangedByCode = false;
		}
	    }
	});



	controlPanel.add(timeChooser);
    }

    public void start(String Path) {
	File file = new File(Path);
	// vlcj: play the media
	mediaPlayer.playMedia(file.getAbsolutePath());

    }

    public void stop() {
	mediaPlayer.stop();
    }

    public void enterFullScreen() {
	if (parent != null) {
	    parent.exitFullScreen();
	}
    }

    public void exitFullScreen() {
	if (parent != null) {
	    parent.enterFullScreen();
	}
    }

    public void setControlPanelVisible(boolean visible){
	controlPanel.setVisible(visible);
    }
    
    public void dispose() {
	// Cleanly dispose of the media player instance and any associated native resources
	mediaPlayer.release();

	// Cleanly dispose of the media player factory and any associated native resources
	mediaPlayerFactory.release();
    }
}
