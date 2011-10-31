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
public class Movie extends Video {
    private long runtime;
    private int franchise_id;
    
    
    public Movie(int id, String name, String path, int currentTimestamp) {
        super(id, name, path, currentTimestamp);
    }
    
    public Movie(int id, String idImdb, String name, Date release, double rating, double ratingImdb, String path, int lastPlayLocation, ArrayList<Subtitle> subs){
	super(id, idImdb, name, release, rating, ratingImdb, path, lastPlayLocation, subs);
    }

    @Override
    public VideoType getVideoType() {
	return VideoType.Movie;
    }
    
    public int getFranchise_id() {
	return franchise_id;
    }

    public void setFranchise_id(int franchise_id) {
	this.franchise_id = franchise_id;
    }

    public long getRuntime() {
	return runtime;
    }

    public void setRuntime(long runtime) {
	this.runtime = runtime;
    }
    
    
    
}
