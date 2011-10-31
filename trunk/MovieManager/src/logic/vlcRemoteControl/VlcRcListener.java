/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package logic.vlcRemoteControl;

import java.io.IOException;
import java.util.logging.Level;
import java.util.logging.Logger;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
import logic.database.Video;
import logic.vlcRemoteControl.TCPClient.VlcCommand;

/**
 *
 * @author alexander
 */
public class VlcRcListener extends Thread {

    private VlcCommand lastCommand;
    private TCPClient client;
    private VLCProcess process;
    private Video video;
    private boolean bWatchedToEnd = false;
    private int iCurrentTimestamp = 0;
    private int iVideoLength = 0;

    public VlcRcListener(VLCProcess process, TCPClient client, Video video) {
	this.process = process;
	this.client = client;
	this.video = video;
    }

    @Override
    public void run() {
	//get the length of the playing video
	if (!process.isTerminated()) {
	    sendMessage(VlcCommand.Get_Length);
	    String response = GetNextIntegerResponseLine();
	    handleResponse(response);
	}

	//check for current video timestamp
	while (!process.isTerminated()) {
	    String response = getResponseLineWithWait();
	    handleResponse(response);
	    sendMessage(VlcCommand.Get_Time);
	}

	//get responses when process is finished
	String response = getResponseLine();
	while (!response.equals("")) {
	    handleResponse(response);
	    response = getResponseLine();
	}

	video.markAsSeen(iVideoLength, iCurrentTimestamp, bWatchedToEnd);
	client.closeConnection();
    }

    private void sendMessage(VlcCommand command) {
	try {
	    client.sendMessage(command);
	    lastCommand = command;
	} catch (IOException ex) {
	    Logger.getLogger(VlcRcListener.class.getName()).log(Level.SEVERE, null, ex);
	}
    }

    private String GetNextIntegerResponseLine() {
	String response = getResponseLineWithWait();
	while (!matchRegExp("^\\d+$", response)) {
	    response = getResponseLineWithWait();
	}
	return response;
    }
    
    private String getResponseLine() {
	String response;
	try {
	    response = client.getResponseLine();
	} catch (IOException ex) {
	    response = "";
	}
	return response;
    }

    private String getResponseLineWithWait() {
	String response;
	try {
	    response = client.getResponseLineWithWait();
	} catch (IOException ex) {
	    response = "";
	}
	return response;
    }

    private void handleResponse(String response) {
	if (!response.equals("")) {
	    //integervalue
	    if (matchRegExp("^\\d+$", response)) {
		int iHulp = Integer.parseInt(response);
		if (lastCommand.equals(VlcCommand.Get_Length)) {
		    iVideoLength = iHulp;
		} else if (lastCommand.equals(VlcCommand.Get_Time) && iHulp != 0) {
		    iCurrentTimestamp = iHulp;
		}
	    } //if playback is stopped
	    else if (matchRegExp("^.*play state: 4.*$", response)) {
		process.terminateProcess();
		//end of movie reached
		if (iCurrentTimestamp > iVideoLength - 10) {
		    bWatchedToEnd = true;
		}
	    }
	}
    }

    private boolean matchRegExp(String needle, String stack) {
	Pattern pattern = Pattern.compile(needle, Pattern.MULTILINE);
	Matcher matcher = pattern.matcher(stack);
	if (matcher.find()) {
	    return true;
	} else {
	    return false;
	}
    }

    private void printResponse(String response) {
	System.out.println("response: " + response + "\n");
    }
}
