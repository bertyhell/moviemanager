/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package logic.vlcRemoteControl;

import java.io.IOException;


/**
 *
 * @author alexander
 */
public class VLCProcess{
    private Process p;

    public VLCProcess(String shellCommand) throws IOException {
        this.p =  Runtime.getRuntime().exec(shellCommand);
    }
    
    public boolean isTerminated(){
        boolean terminated = true;
        try{
            p.exitValue();
        }
        catch(IllegalThreadStateException ex){
            terminated  = false;
        }
        return terminated;
    }
    
    public void terminateProcess(){
	p.destroy();
    }
}
