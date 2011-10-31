/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui.actionlisteners;

import img.ImageFactory;
import java.awt.event.ActionEvent;
import java.awt.event.KeyEvent;
import javax.swing.AbstractAction;
import javax.swing.KeyStroke;
import logic.database.Video;
import logic.settings.languages.LanguageManager;
import logic.database.VideoDatabase;
import logic.imdbapi.VideoInformationCollector;

/**
 *
 * @author alexander
 */
public class AnalyseListener extends AbstractAction {
    private static AnalyseListener instance = new AnalyseListener();
    
    private AnalyseListener() {
	super(LanguageManager.getInstance().get("btnAnalyseLabel"), ImageFactory.getInstance().getImageIcon("analyse"));  
        putValue(SHORT_DESCRIPTION, LanguageManager.getInstance().get("btnAnalyseShortDescription"));
	MainWindowEventDispatcher.getInstance().addActionListener(getAcceleratorKey(), this);
    }

    public static AnalyseListener getInstance() {
	return instance;
    }  
    
    public final KeyStroke getAcceleratorKey(){
	return KeyStroke.getKeyStroke(KeyEvent.VK_W, ActionEvent.CTRL_MASK);
    }
    
    @Override
    public void actionPerformed(ActionEvent e) {
	//TODO 100 program analyser
	for(Video video : VideoDatabase.getInstance().getVideos()){
	    VideoInformationCollector.getVideoInfoFromImdb(video.getName(),video);
	}
    }
    
}
