/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package logic.imdbapi;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.net.URLEncoder;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import logic.Actor;
import logic.database.Video;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;
import org.xml.sax.InputSource;

/**
 *
 * @author alexander
 */
public class VideoInformationCollector {

    public static final SimpleDateFormat formatter = new SimpleDateFormat("d MMM yyyy");

    public static void getVideoInfoFromImdb(String query, Video video) {

	try {
	    //do request
	    URL request = new URL("http://www.imdbapi.com/?r=XML&t=" + URLEncoder.encode(query, "UTF-8").toString());
	    String response = doRequest(request);


	    //parse request
	    DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
	    DocumentBuilder db = dbf.newDocumentBuilder();
	    if (!response.contains("Parse Error")) {
		Document doc = db.parse(new InputSource(new java.io.StringReader(response)));
		doc.getDocumentElement();

		//get all movietags
		NodeList movieList = doc.getElementsByTagName("movie");

		//get informatie from movie nodes
		for (int i = 0; i < movieList.getLength(); i++) {
		    Node movie = movieList.item(i);
		    if (movie.getNodeType() == Node.ELEMENT_NODE) {
			Element movieElement = (Element) movie;
			video.setRelease(parseDate(movieElement.getAttribute("release")));
			try {
			    double rating = Double.parseDouble(movieElement.getAttribute("rating"));
			    video.setRatingImdb(rating);
			} catch (NumberFormatException e) {
			    System.out.println(e.getMessage());
			}
			//TODO 100 get all the information in the node
			//TODO 100 Date doesn't get saved in database
		    }
		}
	    }
	} catch (Exception e) {
	    e.printStackTrace();
	}
    }

    public static ArrayList<Actor> SearchActorFromTmdb(String query) {
	ArrayList<Actor> actors = new ArrayList<Actor>();
	try {

	    //do request
	    URL request = new URL("http://api.themoviedb.org/2.1/Person.search/en/xml/02004323eee9878ce511ca57faf0b29c/" + URLEncoder.encode(query, "UTF-8").toString());
	    String response = doRequest(request);

	    System.out.println(response);

	    //parse request
	    DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
	    DocumentBuilder db = dbf.newDocumentBuilder();
	    Document doc = db.parse(new InputSource(new java.io.StringReader(response)));
	    doc.getDocumentElement();

	    //get resultcount
	    int resultsCount = 0;
	    NodeList resultCountList = doc.getElementsByTagName("opensearch:totalResults");
	    Node resCount = resultCountList.item(0);
	    if (resCount.getNodeType() == Node.ELEMENT_NODE) {
		Element castedResultCount = (Element) resCount;
		resultsCount = Integer.parseInt(castedResultCount.getFirstChild().getNodeValue());
	    }

	    if (resultsCount != 0) {
		//get information off ever actor
		NodeList actorList = doc.getElementsByTagName("person");

		//get informatie from movie nodes
		for (int i = 0; i < actorList.getLength(); i++) {

		    Node actorNode = actorList.item(i);
		    if (actorNode.getNodeType() == Node.ELEMENT_NODE) {
			Element castedActorNode = (Element) actorNode;
			Actor actor = new Actor(
				Integer.parseInt(castedActorNode.getElementsByTagName("id").item(0).getFirstChild().getNodeValue()),
				castedActorNode.getElementsByTagName("name").item(0).getFirstChild().getNodeValue());

			//get all picture URLs
			NodeList imageList = castedActorNode.getElementsByTagName("image");
			for (int j = 0; j < imageList.getLength(); j++) {
			    actor.addImage(((Element) imageList.item(j)).getAttribute("url"));
			}

			actors.add(actor);
		    }
		}
	    }

	} catch (Exception e) {
	    e.printStackTrace();
	} finally {
	    return actors;
	}
    }

    public static void getActorInfoFromTmdb(Actor actor) {
	try {

	    //do request
	    URL request = new URL("http://api.themoviedb.org/2.1/Person.getInfo/en/xml/02004323eee9878ce511ca57faf0b29c/" + actor.getTmdbID());
	    String response = doRequest(request);

	    System.out.println(response);

	    //parse request
	    DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
	    DocumentBuilder db = dbf.newDocumentBuilder();
	    Document doc = db.parse(new InputSource(new java.io.StringReader(response)));
	    doc.getDocumentElement();

	    //get resultcount
	    //get information off ever actor
	    NodeList actorList = doc.getElementsByTagName("person");

	    Node actorNode = actorList.item(0);
	    if (actorNode.getNodeType() == Node.ELEMENT_NODE) {
		Element castedActorNode = (Element) actorNode;

		//get all movie picture URLs
		NodeList imageList = castedActorNode.getElementsByTagName("movie");
		for (int j = 0; j < imageList.getLength(); j++) {
		    actor.addMovieImageUrl(((Element) imageList.item(j)).getAttribute("poster"));
		}
	    }
	} catch (Exception e) {
	    e.printStackTrace();
	}
    }

    private static Date parseDate(String date) {
	try {
	    SimpleDateFormat parser = new SimpleDateFormat("d MMM yyyy");
	    return parser.parse(date);
	} catch (ParseException ex) {
	    try {
		SimpleDateFormat parser2 = new SimpleDateFormat("yyyy");
		return parser2.parse(date);
	    } catch (ParseException ex2) {
		return null;
	    }
	}
    }

    /**
     * 
     * @param url
     * @return
     */
    private static String doRequest(URL url) {
	HttpURLConnection connection = null;
	BufferedReader rd = null;
	StringBuilder sb = null;
	String line = null;

	try {
	    //set up out communications stuff
	    connection = null;

	    //Set up the initial connection
	    connection = (HttpURLConnection) url.openConnection();
	    connection.setRequestMethod("GET");
	    connection.setDoOutput(true);
	    connection.setReadTimeout(10000);

	    connection.connect();

	    //read the result from the server
	    rd = new BufferedReader(new InputStreamReader(connection.getInputStream()));
	    sb = new StringBuilder();

	    while ((line = rd.readLine()) != null) {
		sb.append(line).append('\n');
	    }

	    return sb.toString();

	} catch (Exception e) {

	    System.out.println(e.getMessage());
	    e.printStackTrace();
	    return "{\"Response\":\"Parse Error\"}";
	} finally {
	    //close the connection, set all objects to null
	    connection.disconnect();
	    rd = null;
	    sb = null;
	    connection = null;
	}
    }
}
