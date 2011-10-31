/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package logic.vlcRemoteControl;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.Socket;
import java.net.UnknownHostException;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 *
 * @author alexander
 */
public class TCPClient {

    public static enum VlcCommand {
        Add,
        Enqueue,
        Play,
        F,
        Is_Playing,
        Get_Time,
        Get_Length,
        Seek,
        Pause,
        FastForward,
        Rewind
    };
    private Socket s;
    private PrintWriter out = null;
    private BufferedReader in = null;

    public TCPClient(String computernaam, int poort) throws UnknownHostException, IOException {
        s = new Socket(computernaam, poort);
        out = new PrintWriter(s.getOutputStream(), true);
        in = new BufferedReader(new InputStreamReader(s.getInputStream()));
    }

    public void sendMessage(VlcCommand command) throws IOException {
        out.println(command.toString().toLowerCase());
    }

    private void waitForResponse() {
        try {
            int i = 0;
            while (!in.ready() && i < 5) {
                try {
                    Thread.sleep(250);
                } catch (InterruptedException ex) {
                    Logger.getLogger(TCPClient.class.getName()).log(Level.SEVERE, null, ex);
                }
                i++;
            }
        } catch (IOException ex) {
            Logger.getLogger(TCPClient.class.getName()).log(Level.SEVERE, null, ex);
        }
    }

    public String getResponseWithWait() throws IOException {
        waitForResponse();
        String answer = "";
        while (in.ready()) {
            answer += in.readLine() + "\n";
        }
        return answer;
    }

    public String getResponseLineWithWait() throws IOException {
        waitForResponse();
        return in.ready() ? in.readLine() : "";
    }
    
    public String getResponseLine() throws IOException {
        return in.ready() ? in.readLine() : "";
    }

    public void closeConnection() {
        try {
            out.close();
            in.close();
            s.close();
        } catch (IOException ex) {
            Logger.getLogger(TCPClient.class.getName()).log(Level.SEVERE, null, ex);
        }
    }
}
