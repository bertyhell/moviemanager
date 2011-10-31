/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package logic.database;

import java.util.ArrayList;
import logic.database.Video.VideoType;
import logic.database.jdbc.DatabaseConnector;

/**
 *
 * @author alexander
 */
public class VideoDatabase {//TODO 025 enkel getoonde informatie ophalen
    private static VideoDatabase instance = new VideoDatabase();
    
    private VideoDatabase(){
	videos = DatabaseConnector.getInstance().getAllVideos();
    }
    
    public static VideoDatabase getInstance(){
	return instance;
    }
    
    private ArrayList<Video> videos;
    
    public ArrayList<Video> getVideos(){
	return videos;
    }
    
    public ArrayList<Video> getVideosOfType(VideoType type){
	ArrayList<Video> movies = new ArrayList<Video>();
	for(Video video : videos){
	    if(video.getVideoType().equals(type)){
		movies.add(video);
	    }
	}
	return movies;
    }
    
    public Video getVideo(int index){
	return videos.get(index);
    }
    public void setVideo(int index, Video video){
	videos.set(index, video);
    }
    
    public void addVideos(ArrayList<Video> videos){// TODO 070 check for doubles
	this.videos.addAll(videos);
    }
    
    public void saveToHDD(){
	DatabaseConnector.getInstance().updateAllVideoTables(videos);
    }
    
}
