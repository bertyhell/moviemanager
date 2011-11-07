package gui.progressbars;

import java.awt.*;
import java.beans.PropertyChangeEvent;
import java.beans.PropertyChangeListener;
import javax.swing.*;

public class CustomProgressBar extends JDialog implements PropertyChangeListener {

    private JProgressBar current;
    private String label;
    private long starttime;

    //TODO 010 add button to progressbar "details", output what is getting done (only if button has been pressed (efficiency)
    /**
     * this dialog shows the progress of a JSwingWorker object
     * you have to set this dialog as the propertylistener in the swingworker
     * so that the componenet knows where to get the progress
     * @param parent this is a modal window, so it has to know its parent
     * @param label  
     */
    public CustomProgressBar(JFrame parent, String label) {
	super(parent, "Progress", false);
	this.setUndecorated(true);
	this.setPreferredSize(new Dimension(300, 50));
	setDefaultCloseOperation(JDialog.DISPOSE_ON_CLOSE);
	
	current = new JProgressBar(0, 100);
	current.setValue(0);
//	current.setStringPainted(true);
//	current.setString(label);
	
	
	
	
	current.setMaximum(2226);
	
	
	
	
	this.add(current, BorderLayout.CENTER);
	
	this.pack();
	this.setLocationRelativeTo(parent);
    }
    


    /**
     * this methode listenes to the changes of the linked swingworker and updates the value of the progressbar
     * @param evt this is the propertychange event and is used to identify the correct event and get the progressvalue
     */
    @Override
    public void propertyChange(PropertyChangeEvent evt) {
	if ("progress".equals(evt.getPropertyName())) {
	    System.out.println("property has changed");
	    int progress = (Integer) evt.getNewValue();
	    
//	    long elapsedTime = System.nanoTime() - starttime;
//	    long estimatedTime = ((100-progress)*elapsedTime)/progress;

	    current.setValue(progress);
//	    System.out.println(label + "(" + progress + " * " + max + " - " + passedProgress + ")  / 100 +  / + " + maxLabel);
//	    current.setString(label +" "+ (progress * current.getMaximum() /100) + " / " + current.getMaximum() + "    " + "eta: " + estimatedTime);
//	    System.out.println(label + (progress * current.getMaximum() /100) + " / " + current.getMaximum() + "\t" + "eta: " + estimatedTime);
	}
    }

    public void start(String label, int totalFiles) {
	current.setMaximum(totalFiles);
//	starttime = System.nanoTime();
//	this.label = label;
//	current.setString(label + "0 / " + current.getMaximum() + "\t" + "eta: ?");
    }
}
