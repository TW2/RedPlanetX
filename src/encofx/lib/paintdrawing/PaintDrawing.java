/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.paintdrawing;

import java.awt.Graphics2D;
import java.awt.image.BufferedImage;

/**
 *
 * @author Yves
 */
public class PaintDrawing {
    
    private PaintTool pt = new PaintTool();
    private BufferedImage paintOnIt = null;
    private Graphics2D gra = null;
    
    public PaintDrawing(){
        paintOnIt = new BufferedImage(1280, 720, BufferedImage.TYPE_INT_ARGB);
        gra = paintOnIt.createGraphics();
    }
    
    public PaintDrawing(BufferedImage paintOnIt){
        this.paintOnIt = paintOnIt;
        gra = paintOnIt.createGraphics();
    }
    
    public void setImage(BufferedImage paintOnIt){
        this.paintOnIt = paintOnIt;
        gra = paintOnIt.createGraphics();
    }
    
    public BufferedImage getImage(){
        return paintOnIt;
    }
    
    public void setPainter(PaintTool pt){
        this.pt = pt;
    }
    
    public PaintTool getPainter(){
        return pt;
    }
    
    public void draw(int x, int y){
        pt.getTrace(gra, x, y);
    }
    
}
