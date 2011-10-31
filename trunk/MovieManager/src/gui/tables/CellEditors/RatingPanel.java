/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui.tables.CellEditors;

import img.ImageFactory;
import java.awt.Dimension;
import java.awt.FlowLayout;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.util.ArrayList;
import javax.swing.ImageIcon;
import javax.swing.JLabel;
import javax.swing.JPanel;
import logic.database.Video;

/**
 *
 * @author alexander
 */
public class RatingPanel extends JPanel {

    public static ImageIcon emptyStar = ImageFactory.getInstance().getScaledImageIcon("EmptyStar", 16, 16);
    public static ImageIcon selectedStar = ImageFactory.getInstance().getScaledImageIcon("SelectedStar", 16, 16);
    public static ImageIcon halfSelectedStar = ImageFactory.getInstance().getScaledImageIcon("HalfSelectedStar", 16, 16);
    protected ArrayList<JLabel> stars = new ArrayList<JLabel>();
    protected Video video;
    protected int starCount = 5;

    public RatingPanel(Video v) {
	this.video = v;
	this.setLayout(new FlowLayout(FlowLayout.LEADING, 0, 0));
	for(int i=0; i<starCount ; i++){
	    stars.add(new JLabel());
	}
	stars.get(0).addMouseListener(new MouseAdapter() {
	    @Override
	    public void mousePressed(MouseEvent e) {
		System.out.println("ja");
	    }
	});
	RefreshStars(video.getRating(),selectedStar,halfSelectedStar,emptyStar);
    }

    protected void RefreshStars(double rating, ImageIcon selectedStar, ImageIcon halfSelectedStar, ImageIcon emptyStar) {
	this.removeAll();
	
	// adapt the rating to the star count
	double normalizedRating = rating / 10 * starCount;
	
	//determine the amount of full colered stars
	int selectedStarCount = (int) Math.floor(normalizedRating);
	
	//determine the remaining rating -> used to determine if a half colered star is needed
	double ratingRest = normalizedRating - selectedStarCount;
	selectedStarCount += (ratingRest > 0.75 ? 1 : 0);
	int halfSelectedStarCount = (ratingRest <= 0.75 && ratingRest >= 0.25 ? 1 : 0);
	int EmptyStarCount = starCount - selectedStarCount - halfSelectedStarCount;

	for (int i = 0; i < selectedStarCount; i++) {
	    JLabel starLabel = new JLabel(selectedStar);
	    stars.set(i,starLabel);
	    this.add(starLabel);
	}
	for (int i = 0; i < halfSelectedStarCount; i++) {
	    JLabel starLabel = new JLabel(halfSelectedStar);
	    stars.set(i,starLabel);
	    this.add(starLabel);
	}

	for (int i = 0; i < EmptyStarCount; i++) {
	    JLabel starLabel = new JLabel(emptyStar);
	    stars.set(i,starLabel);
	    this.add(starLabel);
	}
	
	this.updateUI();
    }

    public int getStarCount() {
	return starCount;
    }

    public void setStarCount(int starCount) {
	this.starCount = starCount;
    }

    public ArrayList<JLabel> getStars() {
	return stars;
    }

    public void setStars(ArrayList<JLabel> stars) {
	this.stars = stars;
    }

    public Video getVideo() {
	return video;
    }

    public void setVideo(Video video) {
	this.video = video;
    }
    
    
}
