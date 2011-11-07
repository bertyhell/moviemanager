package gui.actionlisteners;

import java.util.ArrayList;
import logic.database.Video;

/**
 *
 * @author Bert Verhelst <verhelstbert@gmail.com>
 */
public interface IVideoReciever {
    public void returnVideos(ArrayList<Video> videos);
}
