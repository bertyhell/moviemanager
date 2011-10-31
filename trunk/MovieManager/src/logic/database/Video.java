/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package logic.database;

import gui.MainWindow;
import java.util.ArrayList;
import java.util.Date;
import javax.swing.JOptionPane;

/**
 *
 * @author alexander
 */
public class Video {

    public static enum VideoType {

	Video, Movie, Episode
    };
    protected int id;
    protected String idImdb;
    protected String name;
    protected Date release;
    protected double rating;
    protected double ratingImdb;
    protected ArrayList<String> genres;
    protected String path; //path to movie
    protected int lastPlayLocation = 0;
    protected boolean watchedToEnd = false;
    protected ArrayList<Subtitle> subs; //Subtitles of the formats .cdg, .idx, .srt, .sub, .utf, .ass, .ssa, .aqt, .jss, .psb, .rt and smi are supported. 

    public Video(String path) {
	this.path = path;
	this.name = path.substring(path.lastIndexOf("/") + 1, path.lastIndexOf("."));
    }

    public Video(String path, String movieName) {
	this.path = path;
	this.name = movieName;
    }

    public Video(int id, String name, String path, int lastPlayLocation) {
	this.id = id;
	if (name == null) {
	    this.name = path.substring(path.lastIndexOf("/") + 1, path.lastIndexOf("."));
	} else {
	    this.name = name;
	}
	this.path = path;
	this.lastPlayLocation = lastPlayLocation;
    }

    public Video(int id, String idImdb, String name, Date release, double rating, double ratingImdb, String path, int lastPlayLocation, ArrayList<Subtitle> subs) {
	this.id = id;
	this.idImdb = idImdb;
	if (name == null) {
	    this.name = path.substring(path.lastIndexOf("/") + 1, path.lastIndexOf("."));
	} else {
	    this.name = name;
	}
	this.release = release;
	this.rating = rating;
	this.ratingImdb = ratingImdb;
	this.path = path;
	this.subs = subs;
	this.lastPlayLocation = lastPlayLocation;
    }

    public VideoType getVideoType() {
	return VideoType.Video;
    }

    public static Video ConvertVideo(VideoType resultingVideoType, Video video) {
	if (resultingVideoType.equals(VideoType.Movie)) {
	    return new Movie(
		    video.getId(),
		    video.getIdImdb(),
		    video.getName(),
		    video.getRelease(),
		    video.getRating(),
		    video.getRatingImdb(),
		    video.getPath(),
		    video.getLastPlayLocation(),
		    null);
	} else if (resultingVideoType.equals(VideoType.Episode)) {
	    return new Episode(
		    video.getId(),
		    video.getIdImdb(),
		    video.getName(),
		    video.getRelease(),
		    video.getRating(),
		    video.getRatingImdb(),
		    video.getPath(),
		    video.getLastPlayLocation(),
		    null);
	} else {
	    return new Video(
		    video.getId(),
		    video.getIdImdb(),
		    video.getName(),
		    video.getRelease(),
		    video.getRating(),
		    video.getRatingImdb(),
		    video.getPath(),
		    video.getLastPlayLocation(),
		    null);
	}
    }

    //return 
    public ArrayList<String> getGenres() {
	return genres;
    }

    public void addGenre(String genre) {
	genres.add(genre);
    }

    public String getGenresString() {
	if (genres.size() > 0) {
	    String genresString = genres.get(0);
	    for (int i = 1; i < genres.size(); i++) {
		genresString += ", "+genres.get(i);
	    }
	    return genresString;
	} else {
	    return "";
	}
    }

    public int getId() {
	return id;
    }

    public void setId(int id) {
	this.id = id;
    }

    public String getIdImdb() {
	return idImdb;
    }

    public void setIdImdb(String idImdb) {
	this.idImdb = idImdb;
    }

    public String getName() {
	return name;
    }

    public void setName(String name) {
	this.name = name;
    }

    public String getPath() {
	return path;
    }

    public void setPath(String path) {
	this.path = path;
    }

    public double getRating() {
	return rating;
    }

    public void setRating(double rating) {
	this.rating = rating;
    }

    public double getRatingImdb() {
	return ratingImdb;
    }

    public void setRatingImdb(double ratingImdb) {
	this.ratingImdb = ratingImdb;
    }

    public Date getRelease() {
	return release;
    }

    public void setRelease(Date release) {
	this.release = release;
    }

    public ArrayList<Subtitle> getSubs() {
	return subs;
    }

    public void setSubs(ArrayList<Subtitle> subs) {
	this.subs = subs;
    }

    public int getLastPlayLocation() {
	return lastPlayLocation;
    }

    public void setLastPlayLocation(int lastPlayLocation) {
	this.lastPlayLocation = lastPlayLocation;
    }

    @Override
    public String toString() {
	return path;
    }

    public void markAsSeen() {
	watchedToEnd = true;
    }

    public void markAsSeen(int movieLength, int iCurrentTimestamp, boolean bWatchedToEnd) {
	if (bWatchedToEnd) {
	    //mark as seen
	    this.watchedToEnd = true;
	} else if (!bWatchedToEnd && (movieLength - (iCurrentTimestamp) < movieLength * 10 / 100)) {
	    //ask to user
	    Object[] options = {"Yes",
		"No"};
	    int result = JOptionPane.showOptionDialog(
		    MainWindow.getInstance(),
		    "Do you want to mark this video as seen?\n"
		    + "Press \"yes\" to mark this video as seen.\n"
		    + "Press \"no\" to save the current timestamp of the video.",
		    "Choose option",
		    JOptionPane.YES_NO_OPTION,
		    JOptionPane.QUESTION_MESSAGE,
		    null,
		    options,
		    options[1]);
	    if (result == JOptionPane.YES_OPTION) {
		this.watchedToEnd = true;
	    } else {
		this.lastPlayLocation = iCurrentTimestamp;
	    }
	} else {
	    //save current timestamp
	    this.lastPlayLocation = iCurrentTimestamp;
	}
    }

    public String getField(String field) {
	if (field.equals("Name")) {
	    return name;
	} else if (field.equals("VideoType")) {
	    return getVideoType().toString();
	} else if(field.equals("Rating")){
	    return ""+rating;
	} else {
	    return path;
	}//TODO 090 create better mechanism for getting fields out of video into table
    }
}
