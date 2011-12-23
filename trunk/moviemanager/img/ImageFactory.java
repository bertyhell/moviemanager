/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package img;

import java.awt.AlphaComposite;
import java.awt.Dimension;
import java.awt.Graphics2D;
import java.awt.Image;
import java.awt.RenderingHints;
import java.awt.image.BufferedImage;
import java.io.IOException;
import java.net.MalformedURLException;
import java.net.URL;
import javax.imageio.ImageIO;
import javax.swing.ImageIcon;

/**
 *
 * @author alexander
 */
public class ImageFactory {

    private static ImageFactory instance = new ImageFactory();

    private ImageFactory() {
    }

    public static ImageFactory getInstance() {
	return instance;
    }

    public ImageIcon getImageIcon(String key) {
	return new ImageIcon(getClass().getResource("/img/" + key + ".png"));
    }

    public Image getImageFromUrl(String URLString) throws MalformedURLException, IOException {
	if (!URLString.isEmpty()) {
	    URL url = new URL(URLString);

	    // Get the image
	    return ImageIO.read(url);
	}
	return null;
    }

    public ImageIcon getScaledImageIcon(String key, int width, int height) {
	return new ImageIcon(getImageIcon(key).getImage().getScaledInstance(width, height, java.awt.Image.SCALE_SMOOTH));
    }

    public Image scaleImage(Image originalImage, int width, int height) {
	BufferedImage resizedImage = new BufferedImage(width, height, BufferedImage.TYPE_INT_ARGB);
	Graphics2D g = resizedImage.createGraphics();
	g.drawImage(originalImage, 0, 0, width, height, null);
	g.dispose();
	g.setComposite(AlphaComposite.Src);

	g.setRenderingHint(RenderingHints.KEY_INTERPOLATION,
		RenderingHints.VALUE_INTERPOLATION_BILINEAR);
	g.setRenderingHint(RenderingHints.KEY_RENDERING,
		RenderingHints.VALUE_RENDER_QUALITY);
	g.setRenderingHint(RenderingHints.KEY_ANTIALIASING,
		RenderingHints.VALUE_ANTIALIAS_ON);

	return resizedImage;
    }

    public Image scaleImageNonStretched(Image originalImage, int width, int height) {
	Dimension newDim = determineScaledDimensions(new Dimension(originalImage.getWidth(null), originalImage.getHeight(null)), new Dimension(width, height));
	return scaleImage(originalImage, newDim.width, newDim.height);
    }

    private Dimension determineScaledDimensions(Dimension original, Dimension desired) {
	double width = original.width, height = original.height;
	//fase 1
	if (original.width > desired.width) {
	    width = desired.width;
	    height = original.height * (width * 1.0 / original.width);
	}
	//fase 2
	if (height > desired.height) {
	    width = desired.width * (desired.height * 1.0 / height);
	    height = desired.height;
	}


	return new Dimension((int) width, (int) height);
    }
}
