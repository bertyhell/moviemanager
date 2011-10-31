/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package logic;

import java.io.File;
import java.util.ArrayList;
import java.util.Arrays;
import logic.database.Video;

/**
 *
 * @author alexander
 */
public class MovieFileReader {

    static final String[] videoFileExtensions = {"ASX", "DTS", "GXF", "M2V", "M3U", "M4V", "MPEG1", "MPEG2", "MTS", "MXF", "OGM", "PLS", "BUP", "A52", "AAC", "B4S", "CUE", "DIVX", "DV", "FLV", "M1V", "M2TS", "MKV", "MOV", "MPEG4", "OMA", "SPX", "TS", "VLC", "VOB", "XSPF", "DAT", "BIN", "IFO", "PART", "3G2", "AVI", "MPEG", "MPG", "FLAC", "M4A", "MP1", "OGG", "WAV", "XM", "3GP", "SRT", "WMV", "AC3", "ASF", "MOD", "MP2", "MP3", "MP4", "WMA", "MKA", "M4P"};
    static final String[] delimiters = {"CD-1","CD-2","CD1","CD2","DVD-1","DVD-2","[Divx-ITA]","[XviD-ITA]","AC3","DVDRip","Xvid","http","www.",".com","shared","powered","sponsored","sharelive","filedonkey","saugstube","eselfilme","eseldownloads","emulemovies","spanishare","eselpsychos.de","saughilfe.de","goldesel.6x.to","freedivx.org","elitedivx","deviance","-ftv","ftv","-flt","flt","1080p","720p","1080i","720i","480","x264","ext","ac3","6ch","axxo","pukka","klaxxon","edition","limited","dvdscr","screener","unrated","BRRIP","subs","_NL_","m-hd"};

    public static void getVideos(File dir, ArrayList<Video> videos) { // TODO 050 Add search options -> minimal size, limit extensions, ...
	if (dir.isDirectory()) {
	    try {
		for (String path : dir.list()) {
		    getVideos(new File(dir.getAbsolutePath() + "/" + path), videos);
		}
	    } catch (Exception e) { // TODO 005 nullpointerexception while selecting documents in windows
//		      System.out.println(dir.getAbsolutePath()); 
	    }
	} else {
	    String ext = dir.getAbsolutePath().substring(dir.getAbsolutePath().lastIndexOf(".") + 1).toUpperCase();
	    if (Arrays.asList(videoFileExtensions).contains(ext)) {
		String path = dir.getAbsolutePath().replace("\\", "/");
//		videos.add(new Video(path));
//		System.out.println("adding video: " + cleanTitle(path.substring(path.lastIndexOf("/") + 1, path.lastIndexOf("."))));
		videos.add(new Video(path,cleanTitle(path.substring(path.lastIndexOf("/") + 1, path.lastIndexOf(".")))));
	    }
	}
    }

    /**
     * cleans up title, returns the cleaned up title
     * @param fileName original file name
     * @return cleaned up filename, should be closest to movie title with year
     */
    public static String cleanTitle(String fileName) {
	
	String movieName = fileName.toLowerCase();
	for(String delimiter : delimiters){
	    int firstIndex = movieName.indexOf(delimiter.toLowerCase());
	    if(firstIndex != -1){
		movieName = movieName.substring(0, firstIndex);
	    }
	}
//	System.out.println("\t" + movieName);

	return movieName.replace(".", " ").replace("(", " ").replace(")", " ").replace("_", " ").trim();
    }

    private static void print(ArrayList<Video> videos) {
	for (Video video : videos) {
	    System.out.println(video.getPath());
	}
    }
}
