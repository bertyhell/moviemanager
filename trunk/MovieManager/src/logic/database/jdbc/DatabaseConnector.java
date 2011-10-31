/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package logic.database.jdbc;

import gui.MainWindow;
import java.io.FileInputStream;
import java.sql.Connection;
import java.sql.Driver;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.SQLException;
import java.sql.Statement;
import java.sql.Time;
import java.util.ArrayList;
import java.util.Properties;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.swing.JOptionPane;
import logic.database.Episode;
import logic.database.Movie;
import logic.database.Video;

/**
 *
 * @author alexander
 */
public class DatabaseConnector {

    private final String DatabasePropertiesFile = "settings/database.properties";
    private static DatabaseConnector sqLiteJDBC = new DatabaseConnector();

    private DatabaseConnector() {
	try {
	    prop.load(new FileInputStream(DatabasePropertiesFile));
	    Driver d = (Driver) Class.forName(prop.getProperty("driver")).newInstance();
	    DriverManager.registerDriver(d);

	} catch (Exception ex) {
	    JOptionPane.showMessageDialog(
		    MainWindow.getInstance(),
		    "Failed to start database",
		    "Database error",
		    JOptionPane.ERROR_MESSAGE);
	    Logger.getLogger(DatabaseConnector.class.getName()).log(Level.SEVERE, null, ex);
	}
    }

    public static DatabaseConnector getInstance() {
	return sqLiteJDBC;
    }
    private Properties prop = new Properties();

    private Connection openConnection() throws SQLException {
	return DriverManager.getConnection(prop.getProperty("url"));
    }

    private ResultSet ExecuteQuery(String query) throws SQLException {
	Statement stmt;
	ResultSet res;
	Connection conn = openConnection();
	stmt = conn.createStatement();
	res = stmt.executeQuery(query);
	stmt.close();
	conn.close();

	return res;
    }

    private void printResultSet(ResultSet res) throws SQLException {
	ResultSetMetaData rsmd = res.getMetaData();
	int numCols = rsmd.getColumnCount();

	//print columnames
	for (int i = 1; i <= numCols; i++) {
	    System.out.printf("%20s", rsmd.getColumnLabel(i));
	}
	System.out.println();

	//print data
	while (res.next()) {
	    for (int i = 1; i <= numCols; i++) {
		if (i > 1) {
		    System.out.print(",");
		}
		System.out.print(res.getString(i));
	    }
	    System.out.println();
	}
    }

    public void insertVideosHDD(ArrayList<Video> videos) {
	try {
	    Connection conn = openConnection();
	    try {
		PreparedStatement st = conn.prepareStatement(prop.getProperty("insert_video_hdd"));
		for (Video video : videos) {
		    st.setString(1, video.getName());//setting name
		    st.setString(2, video.getPath());//setting path
		    st.addBatch();
		}
		st.executeBatch();
	    } finally {
		conn.close();
	    }
	} catch (SQLException ex) {
	    ex.printStackTrace();
	}
    }

    private boolean isMoviePresent(Movie movie) {
	boolean result = false;
	try {
	    Connection conn = openConnection();
	    try {
		PreparedStatement st = conn.prepareStatement(prop.getProperty("is_movie_present"));
		st.setInt(1, movie.getId());//setting name
		ResultSet results = st.executeQuery();
		results.next();
		result = results.getBoolean("ispresent");
	    } finally {
		conn.close();
	    }
	} catch (SQLException ex) {
	    ex.printStackTrace();
	}
	return result;
    }

    private void insertMovie(Movie movie) {
	try {
	    Connection conn = openConnection();
	    try {
		PreparedStatement st = conn.prepareStatement(prop.getProperty("insert_movie"));
		st.setInt(1, movie.getId());//setting name
		st.setTime(2, new Time(movie.getRuntime()));//setting path
		st.setInt(3, movie.getFranchise_id());
		st.addBatch();
		st.executeBatch();
	    } finally {
		conn.close();
	    }
	} catch (SQLException ex) {
	    ex.printStackTrace();
	}
    }

    private void updateMovie(Movie movie) {// TODO 100 update the updates function -> not working
	try {
	    Connection conn = openConnection();
	    try {
		PreparedStatement st = conn.prepareStatement(prop.getProperty("update_movie"));
		st.setInt(3, movie.getId());//setting name
		st.setTime(1, new Time(movie.getRuntime()));//setting path
		st.setInt(2, movie.getFranchise_id());
		st.executeUpdate();
	    } finally {
		conn.close();
	    }
	} catch (SQLException ex) {
	    ex.printStackTrace();
	}
    }

    private void updateVideo(Video video) {
	try {
	    Connection conn = openConnection();
	    try {
		PreparedStatement st = conn.prepareStatement(prop.getProperty("update_video"));
		st.setString(1, video.getIdImdb());//setting name
		st.setString(2, video.getName());
		st.setDate(3, (java.sql.Date) video.getRelease());//setting path
		st.setDouble(4, video.getRating());
		System.out.println(video.getRatingImdb());
		st.setDouble(5, video.getRatingImdb());
		st.setInt(6, 1); //TODO 010 convert genre string to int
		st.setString(7, video.getPath());
		st.setInt(8, video.getLastPlayLocation());
		st.setInt(9, video.getId());
		st.executeUpdate();
	    } finally {
		conn.close();
	    }
	} catch (SQLException ex) {
	    ex.printStackTrace();
	}
    }

    public void updateAllVideoTables(ArrayList<Video> videos) {
	for (Video video : videos) {
	    if (video.getVideoType().equals(Video.VideoType.Video)) {
		updateVideo(video);
	    }
	    else  if(video.getVideoType().equals(Video.VideoType.Movie)){
		Movie movie = (Movie) video;
		if(isMoviePresent(movie)){
		    updateMovie(movie);
		}
		else{
		    insertMovie(movie);
		}
	    }
	    //TODO 095 store episode data
	}
    }

    public ArrayList<Video> getAllVideos() {
	ArrayList<Video> videos = new ArrayList<Video>();
	try {

	    Statement stmt;
	    Connection conn = openConnection();
	    stmt = conn.createStatement();
	    ResultSet rsVideos = stmt.executeQuery(prop.getProperty("select_all_videos_extended"));
	    while (rsVideos.next()) { //TODO 001 replace field names with properties
		if (rsVideos.getInt("movie_id") != 0) {
		    videos.add(new Movie(
			    rsVideos.getInt("id"),
			    rsVideos.getString("id_imdb"),
			    rsVideos.getString("name"),
			    null,
			    rsVideos.getInt("rating"),
			    rsVideos.getInt("rating_imdb"),
			    rsVideos.getString("path"),
			    rsVideos.getInt("last_play_location"),
			    null));
		} else if (rsVideos.getInt("episode_id") != 0) {
		    videos.add(new Episode(
			    rsVideos.getInt("id"),
			    rsVideos.getString("id_imdb"),
			    rsVideos.getString("name"),
			    null,
			    rsVideos.getInt("rating"),
			    rsVideos.getInt("rating_imdb"),
			    rsVideos.getString("path"),
			    rsVideos.getInt("last_play_location"),
			    null));
		} else {
		    videos.add(new Video(
			    rsVideos.getInt("id"),
			    rsVideos.getString("id_imdb"),
			    rsVideos.getString("name"),
			    null,
			    rsVideos.getInt("rating"),
			    rsVideos.getInt("rating_imdb"),
			    rsVideos.getString("path"),
			    rsVideos.getInt("last_play_location"),
			    null));
		}
	    }
	    stmt.close();
	    rsVideos.close();
	    conn.close();
	} catch (SQLException ex) {
	    ex.printStackTrace();
	}
	return videos;
    }
}
