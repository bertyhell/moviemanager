/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package logic;

import gui.MainWindow;
import gui.actionlisteners.IVideoReciever;
import gui.progressbars.CustomProgressBar;
import java.io.File;
import java.util.ArrayList;
import java.util.Arrays;
import javax.swing.SwingWorker;
import logic.database.Video;

/**
 *
 * @author alexander
 */
public class MovieFileReader extends SwingWorker<Void, Void> {

    static final String[] videoFileExtensions = {"ASX", "DTS", "GXF", "M2V", "M3U", "M4V", "MPEG1", "MPEG2", "MTS", "MXF", "OGM", "PLS", "BUP", "A52", "AAC", "B4S", "CUE", "DIVX", "DV", "FLV", "M1V", "M2TS", "MKV", "MOV", "MPEG4", "OMA", "SPX", "TS", "VLC", "VOB", "XSPF", "DAT", "BIN", "IFO", "PART", "3G2", "AVI", "MPEG", "MPG", "FLAC", "M4A", "MP1", "OGG", "WAV", "XM", "3GP", "SRT", "WMV", "AC3", "ASF", "MOD", "MP2", "MP3", "MP4", "WMA", "MKA", "M4P"};
    static final String[] delimiters = {"CD-1", "CD-2", "CD1", "CD2", "DVD-1", "DVD-2", "[Divx-ITA]", "[XviD-ITA]", "AC3", "DVDRip", "Xvid", "http", "www.", ".com", "shared", "powered", "sponsored", "sharelive", "filedonkey", "saugstube", "eselfilme", "eseldownloads", "emulemovies", "spanishare", "eselpsychos.de", "saughilfe.de", "goldesel.6x.to", "freedivx.org", "elitedivx", "deviance", "-ftv", "ftv", "-flt", "flt", "1080p", "720p", "1080i", "720i", "480", "x264", "ext", "ac3", "6ch", "axxo", "pukka", "klaxxon", "edition", "limited", "dvdscr", "screener", "unrated", "BRRIP", "subs", "_NL_", "m-hd"};
    private File[] folders;
    private IVideoReciever reciever;
    private ArrayList<Video> videos;
    private CustomProgressBar progressBar;
    private int totalFiles;
    private int processedFiles;
    
    public MovieFileReader(File[] folders, IVideoReciever reciever) {
	this.folders = folders;
	this.reciever = reciever;
	
	System.out.println("starting search for files in folder");
	progressBar = new CustomProgressBar(MainWindow.getInstance(), "calculating required time... ");
	progressBar.setVisible(true);
	
	this.addPropertyChangeListener(progressBar);

	//calculate number of files to be processed
	processedFiles = 0;
	totalFiles = 0;
	for (File folder : folders) {
	    countFiles(folder);
	}
	
	this.setProgress(0);
	
	
	System.out.println("total number of files is: " + totalFiles);
	progressBar.start("Files processed: ",totalFiles);
    }

    private void countFiles(File dir) { // TODO 050 Add search options -> minimal size, limit extensions, ...
	if (dir.isDirectory()) {
	    try {
		for (String path : dir.list()) {
		    countFiles(new File(dir.getAbsolutePath() + "/" + path));
		}
	    } catch (Exception e) { // TODO 005 nullpointerexception while selecting documents in windows
//		      System.out.println(dir.getAbsolutePath()); 
	    }
	} else {
	    totalFiles++;
	}
    }

    private void getVideos(File dir, ArrayList<Video> videos) { // TODO 050 Add search options -> minimal size, limit extensions, ...

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
		videos.add(new Video(path, cleanTitle(path.substring(path.lastIndexOf("/") + 1, path.lastIndexOf(".")))));
	    }
	    processedFiles++;
	    this.setProgress(processedFiles);
	}
    }

    /**
     * cleans up title, returns the cleaned up title
     * @param fileName original file name
     * @return cleaned up filename, should be closest to movie title with year
     */
    private static String cleanTitle(String fileName) {

	String movieName = fileName.toLowerCase();
	for (String delimiter : delimiters) {
	    int firstIndex = movieName.indexOf(delimiter.toLowerCase());
	    if (firstIndex != -1) {
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

    @Override
    protected Void doInBackground() throws Exception {
//	System.out.println("searching files");
//	
//	videos = new ArrayList<Video>();
//	for (File folder : folders) {
//	    getVideos(folder, videos);
//	    this.setProgress(processedFiles);
//	}
	
	for(int i=0;i<10;i++){
	    Thread.sleep(1000);
	    processedFiles++;
	    this.setProgress(processedFiles);
	    
	}
	
	return null;
    }

    @Override
    protected void done() {
	
	System.out.println("finished searching files");
	reciever.returnVideos(videos);
	progressBar.dispose();
    }
}
