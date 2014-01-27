/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.renderers;

import java.awt.Color;
import java.awt.GradientPaint;
import java.awt.Graphics;
import java.awt.Graphics2D;
import javax.swing.JPanel;

/**
 *
 * @author Yves
 */
public class TwoSidesGradientPanel extends JPanel{
        
    Color c1 = Color.black, c2 = Color.white;
    float x1, y1, x2, y2;

    public TwoSidesGradientPanel(int width, int height){
        setOpaque(true);
        x1 = 0;
        y1 = 0;
        x2 = width;
        y2 = height;
    }

    @Override
    public void paint(Graphics g){
        Graphics2D g2 = (Graphics2D)g;
        g2.setPaint(new GradientPaint(x1, y1, c1, x2, y1, c2));
        g2.fillRect((int)x1, (int)y1, (int)x2, (int)y2);
    }

    public void setColor1(Color c){
        c1 = c;
        repaint();
    }

    public void setColor2(Color c){
        c2 = c;
        repaint();
    }
}