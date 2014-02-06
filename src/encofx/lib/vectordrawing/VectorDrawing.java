/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.vectordrawing;

import java.awt.Color;
import java.awt.Graphics2D;
import java.awt.Rectangle;
import java.awt.geom.AffineTransform;
import java.awt.geom.GeneralPath;
import java.awt.geom.Point2D;
import java.util.ArrayList;
import java.util.List;

/**
 *
 * @author Yves
 */
public class VectorDrawing {
    
    List<AbstractShape> shapes = new ArrayList<>();
    
    public VectorDrawing(){
        
    }
    
    public void addShape(AbstractShape as){
        shapes.add(as);
    }
    
    public void removeShape(AbstractShape as){
        shapes.remove(as);
    }
    
    public void clearShapes(){
        shapes.clear();
    }
    
    public List<AbstractShape> getShapes(){
        return shapes;
    }
    
    public Line getLastLine(){
        for(int i=shapes.size()-1; i>=0; i--){
            AbstractShape as = shapes.get(i);
            if(as instanceof Line){
                return (Line)as;
            }
        }
        return null;
    }
    
    public Curve getLastCurve(){
        for(int i=shapes.size()-1; i>=0; i--){
            AbstractShape as = shapes.get(i);
            if(as instanceof Curve){
                return (Curve)as;
            }
        }
        return null;
    }
    
    public SharedPoint getLastPoint(){
        for(int i=shapes.size()-1; i>=0; i--){
            AbstractShape as = shapes.get(i);
            if(as instanceof SharedPoint){
                SharedPoint sp = (SharedPoint)as;
                if(sp.isControlPoint==false){
                    return sp;
                }               
            }
        }
        return null;
    }
    
    public SharedPoint getLastControlPoint(){
        for(int i=shapes.size()-1; i>=0; i--){
            AbstractShape as = shapes.get(i);
            if(as instanceof SharedPoint){
                SharedPoint sp = (SharedPoint)as;
                if(sp.isControlPoint==true){
                    return sp;
                }               
            }
        }
        return null;
    }
    
    public Move getLastMove(){
        for(int i=shapes.size()-1; i>=0; i--){
            AbstractShape as = shapes.get(i);
            if(as instanceof Move){
                return (Move)as;
            }
        }
        return null;
    }
    
    public GeneralPath getGeneralPath(){
        GeneralPath gp = new GeneralPath(GeneralPath.WIND_EVEN_ODD);
        int count = 0;
        for(AbstractShape s : shapes){
            // Add to the path
            if(s instanceof Line){
                Line l = (Line)s;
                if(count==0){
                    gp.moveTo(l.start.getX(), l.start.getY());
                }else{
                    gp.lineTo(l.end.getX(), l.end.getY());
                }
            }else if(s instanceof Curve){
                Curve b = (Curve)s;
                if(count==0){
                    gp.moveTo(b.start.getX(), b.start.getY());
                }else{
                    gp.curveTo(b.control_1.getX(), b.control_1.getY(),
                            b.control_2.getX(), b.control_2.getY(),
                            b.end.getX(), b.end.getY());
                }
            }else if(s instanceof SharedPoint){
                if(count==0){
                    SharedPoint p = (SharedPoint)s;
                    gp.moveTo(p.start.getX(), p.start.getY());
                }
            }else if(s instanceof Move){
                Move m = (Move)s;
                try{
                    gp.lineTo(m.end.getX(), m.end.getY());
                }catch(Exception e){
                    gp.moveTo(m.end.getX(), m.end.getY());
                }                
            }
            count+=1;
        }
        return gp;
    }
    
    public void drawLines(Graphics2D g, float x, float y){
        AffineTransform at = g.getTransform();
        AffineTransform mod = new AffineTransform();
        mod.setToTranslation(x, y);
        g.setTransform(mod);
        for(AbstractShape as : shapes){
            if(as instanceof SharedPoint){
                SharedPoint sp = (SharedPoint)as;
                if(sp.isControlPoint==false){
                    sp.draw(g, Color.blue);
                }else{
                    sp.draw(g, Color.orange);
                }
            }else if(as instanceof Line){
                Line l = (Line)as;
                l.draw(g, Color.red);
            }else if(as instanceof Curve){
                Curve c = (Curve)as;
                c.draw(g, Color.magenta);
            }
        }
        g.setTransform(at);
    }
    
    public void drawGeneralPath(Graphics2D g, Color generalPathColor){
        g.setColor(generalPathColor);
        g.fill(getGeneralPath());
    }
    
    public SharedPoint getSharedPointAt(int x, int y){
        Rectangle rect = new Rectangle(x-10, y-10, 20, 20);
        for(AbstractShape as : shapes){
            if(as instanceof SharedPoint){
                SharedPoint sp = (SharedPoint)as;
                if(rect.contains(sp.getStartPoint())){
                    return sp;
                }
            }
        }
        return null;
    }
    
    public List<AbstractShape> getClosestShapes(SharedPoint sp){
        List<AbstractShape> rshapes = new ArrayList<>();
        for(AbstractShape as : shapes){
            if(sp.isControlPoint==true && as instanceof Curve){
                Curve curve = (Curve)as;
                if(curve.control1.equals(sp) | curve.control2.equals(sp)){
                    rshapes.add(as);
                    rshapes.add(curve.control1);
                    rshapes.add(curve.control2);
                }
            }else if(as.hasPoint2D(sp.getStartPoint())){
                rshapes.add(as);
            }
        }
        return rshapes;
    }
    
    public void updatePoint2D(AbstractShape shape, Point2D oldp, Point2D newp){
        if(shape.getStartPoint().equals(oldp)){
            shape.setStartPoint(newp);
        }else if(shape.getEndPoint().equals(oldp)){
            shape.setEndPoint(newp);
        }else if(shape.getControlPoint_1().equals(oldp)){
            shape.setControlPoint_1(newp);
        }else if(shape.getControlPoint_2().equals(oldp)){
            shape.setControlPoint_2(newp);
        }
    }
    
}
