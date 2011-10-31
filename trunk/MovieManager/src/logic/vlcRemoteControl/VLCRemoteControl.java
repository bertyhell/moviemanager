/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package logic.vlcRemoteControl;

import java.io.File;
import java.io.IOException;
import logic.database.Video;

/**
 *
 * @author alexander
 */
public class VLCRemoteControl {
    
    private Video video;
    private TCPClient client;
    private VLCProcess vlcProcess;
    private VlcRcListener VlcListener;

    public void VlcRemote(Video video) throws IOException {
        this.video = video;
	File file = new File(video.getPath());
	System.out.println(file.getAbsoluteFile());
        vlcProcess = new VLCProcess("C:\\Program Files (x86)\\VideoLAN\\VLC\\vlc.exe \"" + file.getAbsolutePath() + "\" --rc-quiet --extraintf rc --rc-host=127.0.0.1:4000 :start-time=" + video.getLastPlayLocation()); //  -IRC --> no interface , --rc-quiet --> no command prompt,  
	client = new TCPClient("127.0.0.1", 4000);
        VlcListener = new VlcRcListener(vlcProcess, client, video);
        VlcListener.start();
    }
    
    
}
