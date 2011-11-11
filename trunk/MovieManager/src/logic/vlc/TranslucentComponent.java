/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package logic.vlc;

import java.awt.Color;
import java.awt.Font;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.RenderingHints;
import java.awt.Window;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import javax.swing.JComponent;

/**
 *
 * @author alexander
 */
public class TranslucentComponent extends JComponent {

    private static final long serialVersionUID = 1L;

    public TranslucentComponent() {
	setOpaque(false);
	
    }

    @Override
    protected void paintComponent(Graphics g) {
	super.paintComponent(g);
	Graphics2D g2 = (Graphics2D) g;

	g2.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);
	g2.setRenderingHint(RenderingHints.KEY_TEXT_ANTIALIASING, RenderingHints.VALUE_TEXT_ANTIALIAS_LCD_HRGB);

	g2.setPaint(new Color(255, 128, 128, 100));

	g2.drawRect(0, 0, getWidth() - 1, getHeight() - 1);
	g2.fillRect(0, 0, getWidth(), getHeight());

	g2.setPaint(new Color(0, 0, 0, 128));
	g2.setFont(new Font("Sansserif", Font.BOLD, 18));
	g2.drawString("Translucent", 16, 26);
    }
}
