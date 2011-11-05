/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package logic;

import img.ImageFactory;
import java.awt.Image;
import java.io.IOException;
import java.net.MalformedURLException;
import java.util.ArrayList;

/**
 *
 * @author alexander
 */
public class Actor {

    private int tmdbID;
    private String name;
    private ArrayList<String> imageURLs;
    private ArrayList<String> movieURLs;

    public Actor(int tmdbID, String name) {
	imageURLs = new ArrayList<String>();
	movieURLs = new ArrayList<String>();
	this.tmdbID = tmdbID;
	this.name = name;
    }

    public String getName() {
	return name;
    }

    public void setName(String name) {
	this.name = name;
    }

    public int getTmdbID() {
	return tmdbID;
    }

    public void setTmdbID(int tmdbID) {
	this.tmdbID = tmdbID;
    }

    public void addImage(String URL) {
	imageURLs.add(URL);
    }
    
    public ArrayList<String> getImageStrings(){
	return imageURLs;
    }

    public ArrayList<Image> getImages(){

	ArrayList<Image> images = new ArrayList<Image>();
	for (int i = 0; i < imageURLs.size(); i++) {
	    try {
		images.add(ImageFactory.getInstance().getImageFromUrl(imageURLs.get(i)));

	    } catch (IOException ex) {
	    }
	}
	return images;
    }
    
    public void addMovieImageUrl(String URL) {
	movieURLs.add(URL);
    }
    
    public ArrayList<String> getMovieImageUrlStrings(){
	return movieURLs;
    }
}
