package gui;

import javax.swing.SwingUtilities;
import javax.swing.UIManager;
import logic.MovieFileReader;

//TODO 050 laadbalk
//TODO 050 items in tabel juist ophalen
//TODO 050 opslaan van genres van films
/**
 *
 * @author alexander
 */
public class Main {

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        SwingUtilities.invokeLater(new Runnable() {

            @Override
            public void run() {
                try {
                    // Set cross-platform Java L&F (also called "Metal")
                    UIManager.setLookAndFeel("com.sun.java.swing.plaf.nimbus.NimbusLookAndFeel");
                } catch (Exception e) {
		    
		}
//		System.out.println(MovieFileReader.cleanTitle("Banlieue.13.Ultimatum.2009.720p.BluRay.AC3.x264-JoN"));
                MainWindow window = MainWindow.getInstance();
            }
        });


    }
}
