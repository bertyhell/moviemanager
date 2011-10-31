/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui.tables.CellEditors;

import img.ImageFactory;
import java.awt.Color;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import javax.swing.ImageIcon;
import javax.swing.JLabel;
import logic.database.Video;

/**
 *
 * @author alexander
 */
public class RatingEditorPanel extends RatingPanel {

    public static ImageIcon mouseOverHalfSelectedStar = ImageFactory.getInstance().getScaledImageIcon("MouseOverHalfStar", 16, 16);
    public static ImageIcon mouseOverSelectedStar = ImageFactory.getInstance().getScaledImageIcon("MouseOverStar", 16, 16);
    private int width = RatingPanel.emptyStar.getIconWidth() * 5;
    private double oldMouseOverRating = -1.0;
    private double MouseOverRating = -1.0;

    public RatingEditorPanel(Video v) {
	super(v);



	//mouselistener
	MouseAdapter adapter = new MouseAdapter() {

	    @Override
	    public void mouseMoved(MouseEvent event) {
		int mousePositionX = event.getX();
		if (mousePositionX > width) {
		    mousePositionX = width;
		}
		oldMouseOverRating = MouseOverRating;
		
		//Determine voted score
		MouseOverRating = mousePositionX * 2.0 / RatingPanel.emptyStar.getIconWidth();
		double hulp = Math.floor(MouseOverRating);
		MouseOverRating = hulp + ((MouseOverRating - hulp < 0.5) ? 0 : 1);
		
		if (oldMouseOverRating != MouseOverRating) {
		    RefreshStars(MouseOverRating, mouseOverSelectedStar, mouseOverHalfSelectedStar, emptyStar);
		}
		updateUI();
	    }

	    @Override
	    public void mouseReleased(MouseEvent e) {
		if (e.getX() <= width + 5) {
		    getVideo().setRating(MouseOverRating);
		}
	    }

	    @Override
	    public void mouseExited(MouseEvent e) {
		oldMouseOverRating = -1;
		MouseOverRating = -1;
		RefreshStars(getVideo().getRating(), selectedStar, halfSelectedStar, emptyStar);
	    }
	};
	addMouseListener(adapter);
	addMouseMotionListener(adapter);

    }
}
