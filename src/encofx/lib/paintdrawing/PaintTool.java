/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.paintdrawing;

import com.kitfox.svg.SVGException;
import encofx.lib.svg.SvgBrush;
import java.awt.AlphaComposite;
import java.awt.Color;
import java.awt.Composite;
import java.awt.Graphics2D;
import java.awt.Paint;
import java.awt.RadialGradientPaint;
import java.awt.geom.AffineTransform;
import java.awt.geom.Point2D;
import java.net.MalformedURLException;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 *
 * @author Yves
 */
public class PaintTool {
    
    private Color paintColor = Color.red;
    private int thickness = 6;
    private Tool tool = Tool.Pen;
    
    //Les brosses spécifiques :
    SvgBrush honeycomb = null;
    SvgBrush yellowstar = null;
    
    public enum Tool{
        Pen("Pen"),
        Brush("Brush"),
        HoneyCombBrush("Honeycomb"),
        YellowStarBrush("Yellow star");
        
        String name;
        
        Tool(String name){
            this.name = name;
        }
        
        @Override
        public String toString(){
            return name;
        }
    }
    
    public PaintTool(){
        reset();
    }
    
    public PaintTool(Color paintColor, int thickness, Tool tool){
        this.paintColor = paintColor;
        this.thickness = thickness;
        this.tool = tool;
        reset();
    }
    
    private void reset(){
        try {
            honeycomb = new SvgBrush(getClass().getResource("honeycomb.svg").getPath().replaceAll("%20", " "));
            yellowstar = new SvgBrush(getClass().getResource("yellowstar.svg").getPath().replaceAll("%20", " "));
        } catch (MalformedURLException ex) {
            Logger.getLogger(PaintTool.class.getName()).log(Level.SEVERE, null, ex);
        }
    }
    
    public void setColor(Color paintColor){
        this.paintColor = paintColor;
    }
    
    public Color getColor(){
        return paintColor;
    }
    
    public void setThickness(int thickness){
        this.thickness = thickness;
    }
    
    public int getThickness(){
        return thickness;
    }
    
    public void setTool(Tool tool){
        this.tool = tool;
    }
    
    public Tool getTool(){
        return tool;
    }
    
    public void getTrace(Graphics2D g2, int x, int y){
        if(tool == Tool.Pen){
            g2.setColor(paintColor);
            g2.fillOval(x-thickness/2, y-thickness/2, thickness, thickness);
        }else if(tool == Tool.Brush){
            Paint defaultPaint = g2.getPaint();
            Composite defaultComposite = g2.getComposite();

            // Définition de la couleur 2 par du transparent.
            Color c2 = new Color(0, 0, 0, 0);

            g2.setComposite(makeComposite(0.01f));

            // Définition de la peinture à utilisé : un dégradé.
            Point2D p  = new Point2D.Float(x, y);
            float radius = 360;
            float[] dist = {0.0f, 1.0f};
            Color[] colors = {paintColor, c2};
            g2.setPaint(new RadialGradientPaint(p, radius, dist, colors));
            g2.fillOval(x-thickness/2, y-thickness/2, thickness, thickness);
            g2.setComposite(defaultComposite);
            g2.setPaint(defaultPaint);
        }else if(tool == Tool.HoneyCombBrush){
            g2.setColor(paintColor);
            AffineTransform at_Origin = g2.getTransform();
            AffineTransform at_Now = new AffineTransform();
            at_Now.setToTranslation(x, y);
            g2.setTransform(at_Now);
            try {
                honeycomb.paintWithBrush(g2);
            } catch (SVGException ex) {
                Logger.getLogger(PaintTool.class.getName()).log(Level.SEVERE, null, ex);
            }
            g2.setTransform(at_Origin);
        }else if(tool == Tool.YellowStarBrush){
            g2.setColor(paintColor);
            AffineTransform at_Origin = g2.getTransform();
            AffineTransform at_Now = new AffineTransform();
            at_Now.setToTranslation(x, y);
            g2.setTransform(at_Now);
            try {
                yellowstar.paintWithBrush(g2);
            } catch (SVGException ex) {
                Logger.getLogger(PaintTool.class.getName()).log(Level.SEVERE, null, ex);
            }
            g2.setTransform(at_Origin);
        }
    }
    
    /**
     * Gestion de la transparence.
     * @param alpha La valeur de la transparence.
     * @return La composite qui définit la transparence
     */
    protected AlphaComposite makeComposite(float alpha) {
        int type = AlphaComposite.SRC_OVER;
        return(AlphaComposite.getInstance(type, alpha));
    }
    
}
