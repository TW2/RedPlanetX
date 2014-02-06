/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib;

import encofx.lib.effects.ShapeCollection;
import encofx.lib.effects.TextAreaCollection;
import encofx.lib.effects.TextCollection;
import encofx.lib.effects.VTextCollection;
import encofx.lib.vectordrawing.AbstractShape;
import encofx.lib.vectordrawing.Curve;
import encofx.lib.vectordrawing.Line;
import encofx.lib.vectordrawing.SharedPoint;
import encofx.lib.vectordrawing.VectorDrawing;
import encofx.lib.xuggle.VideoInfo;
import java.awt.Color;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.Image;
import java.awt.geom.Point2D;
import java.awt.image.BufferedImage;
import java.io.IOException;
import java.util.List;
import javax.swing.JPanel;

/**
 *
 * @author Yves
 */
public class VTD2 extends JPanel{
    
    private BufferedImage mainImage = null;
    private List<ObjectCollectionInterface> collection = null;
    private int mainframe = 0;
    private int width = 1280, virtualWidth = 1280;
    private int height = 720, virtualHeight = 720;
    private VideoInfo videoInfo = null;
    private ShapeSelection shapeSelected = ShapeSelection.None;
    private ShapeCollection shapeCollection = null;
    private SharedPoint lastSharedPoint = null;
    
    public VTD2(){        
        addMouseListener(new java.awt.event.MouseAdapter() {
            @Override
            public void mouseClicked(java.awt.event.MouseEvent evt) {
                vtdtMouseClicked(evt);
            }
            @Override
            public void mousePressed(java.awt.event.MouseEvent evt) {
                vtdMousePressed(evt);
            }
            @Override
            public void mouseReleased(java.awt.event.MouseEvent evt) {
                vtdMouseReleased(evt);
            }
        });
        addMouseMotionListener(new java.awt.event.MouseMotionAdapter() {
            @Override
            public void mouseDragged(java.awt.event.MouseEvent evt) {
                vtdMouseDragged(evt);
            }
            @Override
            public void mouseMoved(java.awt.event.MouseEvent evt) {
                vtdMouseMoved(evt);
            }
        });
    }
    
    public enum ShapeSelection{
        None, Line, Curve, Move, Point, ControlPoint;
    }
    
    public void setVideoInfo(VideoInfo vi){
        videoInfo = vi;
    }
    
    // <editor-fold defaultstate="collapsed" desc="-----<EVENTS>-----">
    public void vtdtMouseClicked(java.awt.event.MouseEvent evt){
        double xa = evt.getXOnScreen()-getLocationOnScreen().getX();
        double ya = evt.getYOnScreen()-getLocationOnScreen().getY();
        if(shapeSelected==ShapeSelection.None){
            
        }else if(shapeSelected==ShapeSelection.Line && shapeCollection!=null){
            if(evt.getButton()==1){
                VectorDrawing vd = shapeCollection.getVectorDrawing();
                if(vd.getShapes().isEmpty()){
                    vd.addShape(new SharedPoint(xa, ya));
                }else{
                    vd.addShape(new Line(
                            vd.getLastPoint().getEndPoint().getX(),
                            vd.getLastPoint().getEndPoint().getY(),
                            xa,
                            ya));
                    vd.addShape(new SharedPoint(xa, ya));
                }
            }
        }else if(shapeSelected==ShapeSelection.Curve && shapeCollection!=null){
            if(evt.getButton()==1){
                VectorDrawing vd = shapeCollection.getVectorDrawing();
                if(vd.getShapes().isEmpty()){
                    vd.addShape(new SharedPoint(xa, ya));
                }else{
                    Curve c = new Curve(
                            vd.getLastPoint().getEndPoint().getX(),
                            vd.getLastPoint().getEndPoint().getY(),
                            xa,
                            ya);
                    vd.addShape(c);
                    vd.addShape(c.getControl1());
                    vd.addShape(c.getControl2());
                    vd.addShape(new SharedPoint(xa, ya));
                }
            }
        }
        repaint();
    }
    
    public void vtdMousePressed(java.awt.event.MouseEvent evt){
        double xa = evt.getXOnScreen()-getLocationOnScreen().getX();
        double ya = evt.getYOnScreen()-getLocationOnScreen().getY();
        if(shapeSelected==ShapeSelection.None){
            if(evt.getButton()==1 && collection!=null){
                for(ObjectCollectionInterface obj : collection){
                    if(obj instanceof TextCollection){
                        TextCollection tc = (TextCollection)obj;
                        if(tc.isNearOfAnchor(new Point2D.Double(xa, ya))){
                            tc.setAnchorSelection(true);
                            tc.setAnchor(new Point2D.Double(xa, ya));
                            repaint();
                        }else{
                            tc.setAnchorSelection(false);
                        }
                    }

                    if(obj instanceof VTextCollection){
                        VTextCollection vtc = (VTextCollection)obj;
                        if(vtc.isNearOfAnchor(new Point2D.Double(xa, ya))){
                            vtc.setAnchorSelection(true);
                            vtc.setAnchor(new Point2D.Double(xa, ya));
                            repaint();
                        }else{
                            vtc.setAnchorSelection(false);
                        }
                    }

                    if(obj instanceof TextAreaCollection){
                        TextAreaCollection vtc = (TextAreaCollection)obj;
                        if(vtc.isNearOfAnchor(new Point2D.Double(xa, ya))){
                            vtc.setAnchorSelection(true);
                            vtc.setAnchor(new Point2D.Double(xa, ya));
                            repaint();
                        }else{
                            vtc.setAnchorSelection(false);
                        }
                    }

                    if(obj instanceof ShapeCollection){
                        ShapeCollection vtc = (ShapeCollection)obj;
                        if(vtc.isNearOfAnchor(new Point2D.Double(xa, ya))){
                            vtc.setAnchorSelection(true);
                            vtc.setAnchor(new Point2D.Double(xa, ya));
                            repaint();
                        }else{
                            vtc.setAnchorSelection(false);
                        }
                    }

                }
            }
        }else if(shapeSelected==ShapeSelection.Line && shapeCollection!=null){
            if(evt.getButton()==2){
                VectorDrawing vd = shapeCollection.getVectorDrawing();
                lastSharedPoint = vd.getSharedPointAt(evt.getX(), evt.getY());
            }
        }else if(shapeSelected==ShapeSelection.Curve && shapeCollection!=null){
            if(evt.getButton()==2){
                VectorDrawing vd = shapeCollection.getVectorDrawing();
                lastSharedPoint = vd.getSharedPointAt(evt.getX(), evt.getY());
            }
        }
        
    }
    
    public void vtdMouseReleased(java.awt.event.MouseEvent evt){
        double xa = evt.getXOnScreen()-getLocationOnScreen().getX();
        double ya = evt.getYOnScreen()-getLocationOnScreen().getY();
        if(shapeSelected==ShapeSelection.None){
            if(evt.getButton()==1 && collection!=null){
                for(ObjectCollectionInterface obj : collection){
                    if(obj instanceof TextCollection){
                        TextCollection tc = (TextCollection)obj;
                        if(tc.isNearOfAnchor(new Point2D.Double(xa, ya))){
                            tc.setAnchorSelection(true);
                            tc.setAnchor(new Point2D.Double(xa, ya));
                            repaint();
                        }else{
                            tc.setAnchorSelection(false);
                        }
                    }

                    if(obj instanceof VTextCollection){
                        VTextCollection vtc = (VTextCollection)obj;
                        if(vtc.isNearOfAnchor(new Point2D.Double(xa, ya))){
                            vtc.setAnchorSelection(true);
                            vtc.setAnchor(new Point2D.Double(xa, ya));
                            repaint();
                        }else{
                            vtc.setAnchorSelection(false);
                        }
                    }

                    if(obj instanceof TextAreaCollection){
                        TextAreaCollection vtc = (TextAreaCollection)obj;
                        if(vtc.isNearOfAnchor(new Point2D.Double(xa, ya))){
                            vtc.setAnchorSelection(true);
                            vtc.setAnchor(new Point2D.Double(xa, ya));
                            repaint();
                        }else{
                            vtc.setAnchorSelection(false);
                        }
                    }

                    if(obj instanceof ShapeCollection){
                        ShapeCollection vtc = (ShapeCollection)obj;
                        if(vtc.isNearOfAnchor(new Point2D.Double(xa, ya))){
                            vtc.setAnchorSelection(true);
                            vtc.setAnchor(new Point2D.Double(xa, ya));
                            repaint();
                        }else{
                            vtc.setAnchorSelection(false);
                        }
                    }

                }
            }
        }else if(shapeSelected==ShapeSelection.Line && shapeCollection!=null){
            if(evt.getButton()==2 && lastSharedPoint!=null){
                lastSharedPoint.setStartPoint(new Point2D.Float(evt.getX(), evt.getY()));
            }
        }else if(shapeSelected==ShapeSelection.Curve && shapeCollection!=null){
            if(evt.getButton()==2 && lastSharedPoint!=null){
                lastSharedPoint.setStartPoint(new Point2D.Float(evt.getX(), evt.getY()));
            }
        }
        
    }
    
    public void vtdMouseDragged(java.awt.event.MouseEvent evt){
        double xa = evt.getXOnScreen()-getLocationOnScreen().getX();
        double ya = evt.getYOnScreen()-getLocationOnScreen().getY();
        if(shapeSelected==ShapeSelection.None){
            if(collection!=null){
                for(ObjectCollectionInterface obj : collection){
                    if(obj instanceof TextCollection){
                        TextCollection tc = (TextCollection)obj;
                        if(tc.isNearOfAnchor(new Point2D.Double(xa, ya))){
                            tc.setAnchorSelection(true);
                            tc.setAnchor(new Point2D.Double(xa, ya));
                            repaint();
                        }else{
                            tc.setAnchorSelection(false);
                        }
                    }

                    if(obj instanceof VTextCollection){
                        VTextCollection vtc = (VTextCollection)obj;
                        if(vtc.isNearOfAnchor(new Point2D.Double(xa, ya))){
                            vtc.setAnchorSelection(true);
                            vtc.setAnchor(new Point2D.Double(xa, ya));
                            repaint();
                        }else{
                            vtc.setAnchorSelection(false);
                        }
                    }

                    if(obj instanceof TextAreaCollection){
                        TextAreaCollection vtc = (TextAreaCollection)obj;
                        if(vtc.isNearOfAnchor(new Point2D.Double(xa, ya))){
                            vtc.setAnchorSelection(true);
                            vtc.setAnchor(new Point2D.Double(xa, ya));
                            repaint();
                        }else{
                            vtc.setAnchorSelection(false);
                        }
                    }

                    if(obj instanceof ShapeCollection){
                        ShapeCollection vtc = (ShapeCollection)obj;
                        if(vtc.isNearOfAnchor(new Point2D.Double(xa, ya))){
                            vtc.setAnchorSelection(true);
                            vtc.setAnchor(new Point2D.Double(xa, ya));
                            repaint();
                        }else{
                            vtc.setAnchorSelection(false);
                        }
                    }

                }
            }
        }else if(shapeSelected==ShapeSelection.Line && shapeCollection!=null){
            if(lastSharedPoint!=null){
                Point2D new_param = new Point2D.Float(evt.getX(), evt.getY());
                VectorDrawing vd = shapeCollection.getVectorDrawing();
                List<AbstractShape> shapes = vd.getClosestShapes(lastSharedPoint);
                for(AbstractShape as : shapes){
                    if(as instanceof Line | as instanceof Curve){
                        vd.updatePoint2D(as, lastSharedPoint.getStartPoint(), new_param);
                    }
                }
                lastSharedPoint.setStartPoint(new_param);
            }
        }else if(shapeSelected==ShapeSelection.Curve && shapeCollection!=null){
            if(lastSharedPoint!=null){
                Point2D new_param = new Point2D.Float(evt.getX(), evt.getY());
                VectorDrawing vd = shapeCollection.getVectorDrawing();
                List<AbstractShape> shapes = vd.getClosestShapes(lastSharedPoint);
                for(AbstractShape as : shapes){
                    if(as instanceof Line | as instanceof Curve){
                        vd.updatePoint2D(as, lastSharedPoint.getStartPoint(), new_param);
                    } 
                }
                lastSharedPoint.setStartPoint(new_param);
            }
        }      
    }
    
    public void vtdMouseMoved(java.awt.event.MouseEvent evt){
        
    }
    // </editor-fold>
    
    public void init(){
        setSize(width, height);
    }
    
    public void setImage(BufferedImage mainImage){
        this.mainImage = mainImage;
        repaint();
    }
    
    public void setCollections(List<ObjectCollectionInterface> collection){
        this.collection = collection;
        repaint();
    }
    
    public void setFrame(int mainframe){
        this.mainframe = mainframe;
        repaint();
    }
    
    public void setWidth(int width){
        this.width = width;
    }
    
    public void setHeight(int height){
        this.height = height;
    }
    
    public void setVirtualWidth(int virtualWidth){
        this.virtualWidth = virtualWidth;        
        if(virtualWidth<width){
            setSize(virtualWidth, virtualWidth*height/width);
            System.out.println(virtualWidth+" / "+virtualWidth*height/width);
        }else{
            setSize(width, height);
        }
    }
    
    public void setVirtualHeight(int virtualHeight){
        this.virtualHeight = virtualHeight;
        if(virtualHeight<height){
            setSize(virtualHeight*width/height, virtualHeight);
            System.out.println(virtualHeight*width/height+" / "+virtualHeight);
        }else{
            setSize(width, height);
        }
    }
    
    public void updateDrawing(){
        repaint();
    }
    
    @Override
    public void paint(Graphics g){
        
        //On crée des graphics afin de peindre sur notre composant VTD2.
        Graphics2D g2 = (Graphics2D)g;
        
        //On crée une image avec une taille correspondant à la vidéo d'origine
        //afin de pouvoir dessiner dessus et puis après on pourra réduire cette image
        //en utilisant la taille virtuel de la limite de notre composant hôte.
        //Cette image sera peinte sur le VTD2 à la fin.
        BufferedImage drawingTool = new BufferedImage(width, height, BufferedImage.TYPE_INT_ARGB);
        Graphics2D gDT = drawingTool.createGraphics();
        
        //On commance notre processus de peinture sur l'image drawingTool
        
        gDT.setColor(Color.black);
        gDT.fillRect(0, 0, width, height);
        
        if(mainImage!=null){
            gDT.drawImage(mainImage, null, 0, 0);
        }
        
        if(videoInfo!=null){
            try {
                gDT.drawImage(videoInfo.getImage(mainframe), null, 0, 0);
            } catch (IOException ex) {
            }
        }
        
        if(collection!=null){
            for(ObjectCollectionInterface obj : collection){
                if(obj instanceof TextCollection){
                    TextCollection tc = (TextCollection)obj;
                    BufferedImage b2 = tc.getFX(mainframe, width, height, false);
                    if(b2!=null){
                        gDT.drawImage(b2, null, 0, 0);
                    }  
                }

                if(obj instanceof VTextCollection){
                    VTextCollection vtc = (VTextCollection)obj;
                    BufferedImage b2 = vtc.getFX(mainframe, width, height, false);
                    if(b2!=null){
                        gDT.drawImage(b2, null, 0, 0);
                    }  
                }
                
                if(obj instanceof TextAreaCollection){
                    TextAreaCollection vtc = (TextAreaCollection)obj;
                    BufferedImage b2 = vtc.getFX(mainframe, width, height, false);
                    if(b2!=null){
                        gDT.drawImage(b2, null, 0, 0);
                    }  
                }
                
                if(obj instanceof ShapeCollection){
                    ShapeCollection vtc = (ShapeCollection)obj;
                    BufferedImage b2 = vtc.getFX(mainframe, width, height, false);
                    if(b2!=null){
                        gDT.drawImage(b2, null, 0, 0);
                    }  
                }
                
            }
        }
        
        //Le processus de peinture sur drawingToll est fini alors on met l'image
        //à la bonne taille afin de créer l'aperçu pour le VTD2.
        Image thumbnail = drawingTool.getScaledInstance(virtualWidth, virtualHeight, Image.SCALE_FAST);
        g2.drawImage(thumbnail, null, this);
        
    }
    
    public void draw(Graphics2D g2, long current_frame){
        int cframe;
        try{
            cframe = Integer.parseInt(Long.toString(current_frame));
        }catch(NumberFormatException e){
            cframe = 0;
        }
        
//        g2.setColor(Color.black);
//        g2.fillRect(0, 0, width, height);
//        
//        if(videoInfo!=null){
//            try {
//                g2.drawImage(videoInfo.getImage(cframe), null, 0, 0);
//            } catch (IOException ex) {
//            }
//        }
        
        
        
        if(collection!=null){
            for(ObjectCollectionInterface obj : collection){
                if(obj instanceof TextCollection){
                    TextCollection tc = (TextCollection)obj;
                    BufferedImage b2 = tc.getFX(cframe, width, height, true);
                    if(b2!=null){
                        g2.drawImage(b2, null, 0, 0);
                    }  
                }
                
                if(obj instanceof VTextCollection){
                    VTextCollection vtc = (VTextCollection)obj;
                    BufferedImage b2 = vtc.getFX(cframe, width, height, true);
                    if(b2!=null){
                        g2.drawImage(b2, null, 0, 0);
                    }  
                }
                
                if(obj instanceof TextAreaCollection){
                    TextAreaCollection vtc = (TextAreaCollection)obj;
                    BufferedImage b2 = vtc.getFX(cframe, width, height, true);
                    if(b2!=null){
                        g2.drawImage(b2, null, 0, 0);
                    }  
                }
                
                if(obj instanceof ShapeCollection){
                    ShapeCollection vtc = (ShapeCollection)obj;
                    BufferedImage b2 = vtc.getFX(cframe, width, height, true);
                    if(b2!=null){
                        g2.drawImage(b2, null, 0, 0);
                    }  
                }
                
            }
        }
    }
    
    public void setSelectedShape(ShapeSelection s){
        shapeSelected = s;
    }
    
    public void setShapeCollection(ShapeCollection sc){
        shapeCollection = sc;
    }
}
