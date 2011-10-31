 /*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package logic.database;

import java.util.ArrayList;
import java.util.Date;

/**
 *
 * @author alexander
 */
public class Episode extends Video {
    private int serieId;
    private int season;
    private int episodeNumber;
    private long runtime;
    
    public Episode(int id, String name, int serieId, int season, int episodeNumber) {
	super(name);
	this.serieId = serieId;
	this.season = season;
	this.episodeNumber = episodeNumber;
    }
    
     public Episode(int id, String name, int serieId, int season, int episodeNumber, String path, int currentTimeStamp) {
	super(id, name, path, currentTimeStamp);
	this.serieId = serieId;
	this.season = season;
	this.episodeNumber = episodeNumber;
    }
     
    public Episode(int id, String idImdb, String name, Date release, double rating, double ratingImdb, String path, int lastPlayLocation, ArrayList<Subtitle> subs){
	super(id, idImdb, name, release, rating, ratingImdb, path, lastPlayLocation, subs);
    }

    @Override
    public VideoType getVideoType() {
	return Video.VideoType.Episode;
    }

     
     
    public int getEpisodeNumber() {
	return episodeNumber;
    }

    public void setEpisodeNumber(int episodeNumber) {
	this.episodeNumber = episodeNumber;
    }

    public long getRuntime() {
	return runtime;
    }

    public void setRuntime(long runtime) {
	this.runtime = runtime;
    }

    public int getSeason() {
	return season;
    }

    public void setSeason(int season) {
	this.season = season;
    }

    public int getSerieId() {
	return serieId;
    }

    public void setSerieId(int serieId) {
	this.serieId = serieId;
    }
     
     
}
