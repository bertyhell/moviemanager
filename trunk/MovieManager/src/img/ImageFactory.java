/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package img;

import javax.swing.ImageIcon;

/**
 *
 * @author alexander
 */
public class ImageFactory {

    private static ImageFactory instance = new ImageFactory();

    private ImageFactory() {
    }
    public static ImageFactory getInstance(){
	return instance;
    }

    public ImageIcon getImageIcon(String key) {
	return new ImageIcon(getClass().getResource("/img/" + key + ".png"));
    }
    
    
	
    public ImageIcon getScaledImageIcon(String key, int width, int height) {
	return  new ImageIcon(getImageIcon(key).getImage().getScaledInstance(width, height, java.awt.Image.SCALE_SMOOTH));
    }
}
