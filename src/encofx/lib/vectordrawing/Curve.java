/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.vectordrawing;

import java.awt.Color;
import java.awt.Graphics2D;
import java.awt.geom.CubicCurve2D;
import java.awt.geom.Point2D;

/**
 *
 * @author Yves
 */
public class Curve extends AbstractShape {
    
    SharedPoint control1, control2;
    
    public Curve(){
        
    }
    
    public Curve(double x1, double y1, double x2, double y2){
        start = new Point2D.Double(x1, y1);
        end = new Point2D.Double(x2, y2);
        
        double xdiff = end.getX() - start.getX();
        double ydiff = end.getY() - start.getY();

        double x1_3 = start.getX() + xdiff/3;
        double x2_3 = start.getX() + xdiff*2/3;
        double y1_3 = start.getY() + ydiff/3;
        double y2_3 = start.getY() + ydiff*2/3;
        
        control1 = new SharedPoint(x1_3, y1_3);
        control1.setPointType(true);
        control2 = new SharedPoint(x2_3, y2_3);
        control2.setPointType(true);
        
        control_1 = new Point2D.Double(x1_3, y1_3);
        control_2 = new Point2D.Double(x2_3, y2_3);
    }
    
    public Curve(Point2D start, Point2D end){
        this.start = start;
        this.end = end;
        
        double xdiff = end.getX() - start.getX();
        double ydiff = end.getY() - start.getY();

        double x1_3 = start.getX() + xdiff/3;
        double x2_3 = start.getX() + xdiff*2/3;
        double y1_3 = start.getY() + ydiff/3;
        double y2_3 = start.getY() + ydiff*2/3;
        
        control1 = new SharedPoint(x1_3, y1_3);
        control1.setPointType(true);
        control2 = new SharedPoint(x2_3, y2_3);
        control2.setPointType(true);
        
        control_1 = new Point2D.Double(x1_3, y1_3);
        control_2 = new Point2D.Double(x2_3, y2_3);
    }

    /**
     *
     * @param g
     * @param c
     */
    @Override
    public void draw(Graphics2D g, Color c) {
        g.setColor(c);
        g.draw(new CubicCurve2D.Double(
                start.getX(), 
                start.getY(), 
                control_1.getX(), 
                control_1.getY(), 
                control_2.getX(), 
                control_2.getY(), 
                end.getX(), 
                end.getY()));
    }
    
    public SharedPoint getControl1(){
        return control1;
    }
    
    public SharedPoint getControl2(){
        return control2;
    }
}
