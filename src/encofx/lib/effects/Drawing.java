/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.effects;

import encofx.lib.FXObject;
import encofx.lib.paintdrawing.PaintDrawing;
import encofx.lib.properties.AbstractProperty;
import encofx.lib.settings.SetupObject;
import java.awt.Color;
import java.awt.geom.Point2D;
import java.awt.image.BufferedImage;
import java.util.ArrayList;
import java.util.List;

/**
 *
 * @author Yves
 */
public class Drawing extends FXObject {
    
    //Objects for the table of properties and settings
    private final List<AbstractProperty> properties = new ArrayList<>();
    
    private boolean isSyllable = false;
    private int syllableIndex = -1;
    
    private PaintDrawing paint = null;
    
    public Drawing(){
        properties.add(propX);
        properties.add(propY);
        properties.add(propTrans);
        properties.add(propScaleX);
        properties.add(propScaleY);
        properties.add(propAngle);
        properties.add(propImageOnFrames);
    }
    
    @Override
    public List<AbstractProperty> getProperties(){
        return properties;
    }
    
    @Override
    public String toString(){
        if(isSyllable==true){
            return "Syllable at the frame "+frame;
        }
        return "At the frame "+frame;
    }
    
    public void setSyllable(boolean isSyllable){
        this.isSyllable = isSyllable;
    }
    
    public boolean isSyllable(){
        return isSyllable;
    }
    
    public void setSyllableIndex(int index){
        syllableIndex = index;
    }
    
    public int getSyllableIndex(){
        return syllableIndex;
    }
    
    public void setPaintDrawing(PaintDrawing paint){
        this.paint = paint;
    }
    
    public PaintDrawing getPaintDrawing(){
        return paint;
    }
    
    public void setupImage(){
        setupImage(0, 0);
    }
    
    public void setupImage(int w, int h){
        SetupObject<BufferedImage> so = (SetupObject)propImageOnFrames.getObject();  
        BufferedImage image = so.get();
        if(w>0 && h>0){
            propImageOnFrames.init(w, h);
            so = (SetupObject)propImageOnFrames.getObject();
            image = so.get();
        }
        paint = new PaintDrawing();
        paint.setImage(image);
    }
    
//    public static Color getActualColor(Drawing before, Drawing after, int actualframe){
//        
//        if(before.getFrame()==actualframe){
//            return before.getColor();
//        }
//        
//        if(after.getFrame()==actualframe){
//            return after.getColor();
//        }
//        
//        Color bc = before.getColor();
//        int bc_R = bc.getRed();
//        int bc_G = bc.getGreen();
//        int bc_B = bc.getBlue();
//        int bc_FRAME = before.getFrame();
//        
//        Color ac = after.getColor();
//        int ac_R = ac.getRed();
//        int ac_G = ac.getGreen();
//        int ac_B = ac.getBlue();
//        int ac_FRAME = after.getFrame();
//        
//        int diff_R = ac_R - bc_R;
//        int diff_G = ac_G - bc_G;
//        int diff_B = ac_B - bc_B;
//        
//        actualframe = bc_FRAME==0 ? actualframe : actualframe - bc_FRAME;
//        ac_FRAME = bc_FRAME==0 ? ac_FRAME : ac_FRAME - bc_FRAME;
//        
//        int delta_R = diff_R * actualframe / ac_FRAME;
//        int delta_G = diff_G * actualframe / ac_FRAME;
//        int delta_B = diff_B * actualframe / ac_FRAME;
//        
//        int r = bc_R + delta_R;
//        int g = bc_G + delta_G;
//        int b = bc_B + delta_B;
//                
//        return new Color(r, g ,b);
//    }
    
//    public static float getActualSize(Drawing before, Drawing after, int actualframe){        
//        float bc = before.getSize();
//        int bc_FRAME = before.getFrame();
//        float ac = after.getSize();
//        int ac_FRAME = after.getFrame();
//        
//        if(before.getFrame()==actualframe){
//            return before.getSize();
//        }
//        
//        if(after.getFrame()==actualframe){
//            return after.getSize();
//        }
//        
//        if(bc_FRAME==0){ //Particular case
//            float diff = ac - bc;        
//            float delta = diff * actualframe / ac_FRAME;
//            return bc + delta;
//        }else{
//            float diff = ac - bc;        
//            float delta = diff * (actualframe-bc_FRAME) / (ac_FRAME-bc_FRAME);
//            return bc + delta;
//        }
//    }
    
    public static float getActualX(Drawing before, Drawing after, Point2D anchor, int actualframe){
        float bc = Float.parseFloat(Double.toString(anchor.getX())) + before.getX();
        int bc_FRAME = before.getFrame();
        float ac = Float.parseFloat(Double.toString(anchor.getX())) + after.getX();
        int ac_FRAME = after.getFrame();
        
        if(before.getFrame()==actualframe){
            return bc;
        }
        
        if(after.getFrame()==actualframe){
            return ac;
        }
        
        if(bc_FRAME==0){ //Particular case
            float diff = ac - bc;        
            float delta = diff * actualframe / ac_FRAME;
            return bc + delta;
        }else{
            float diff = ac - bc;        
            float delta = diff * (actualframe-bc_FRAME) / (ac_FRAME-bc_FRAME);
            return bc + delta;
        }
    }
    
    public static float getActualY(Drawing before, Drawing after, Point2D anchor, int actualframe){
        float bc = Float.parseFloat(Double.toString(anchor.getY())) + before.getY();
        int bc_FRAME = before.getFrame();
        float ac = Float.parseFloat(Double.toString(anchor.getY())) + after.getY();
        int ac_FRAME = after.getFrame();
        
        if(before.getFrame()==actualframe){
            return bc;
        }
        
        if(after.getFrame()==actualframe){
            return ac;
        }
        
        if(bc_FRAME==0){ //Particular case
            float diff = ac - bc;        
            float delta = diff * actualframe / ac_FRAME;
            return bc + delta;
        }else{
            float diff = ac - bc;        
            float delta = diff * (actualframe-bc_FRAME) / (ac_FRAME-bc_FRAME);
            return bc + delta;
        }
    }
    
    public static float getActualTransparency(Drawing before, Drawing after, int actualframe){        
        float bc = before.getTransparency();
        int bc_FRAME = before.getFrame();
        float ac = after.getTransparency();
        int ac_FRAME = after.getFrame();
        
        if(before.getFrame()==actualframe){
            return before.getTransparency();
        }
        
        if(after.getFrame()==actualframe){
            return after.getTransparency();
        }
        
        if(bc_FRAME==0){ //Particular case
            float diff = ac - bc;        
            float delta = diff * actualframe / ac_FRAME;
            return bc + delta;
        }else{
            float diff = ac - bc;        
            float delta = diff * (actualframe-bc_FRAME) / (ac_FRAME-bc_FRAME);
            return bc + delta;
        }
    }
    
    public static float getActualScaleX(Drawing before, Drawing after, int actualframe){        
        float bc = before.getScaleX();
        int bc_FRAME = before.getFrame();
        float ac = after.getScaleX();
        int ac_FRAME = after.getFrame();
        
        if(before.getFrame()==actualframe){
            return before.getScaleX();
        }
        
        if(after.getFrame()==actualframe){
            return after.getScaleX();
        }
        
        if(bc_FRAME==0){ //Particular case
            float diff = ac - bc;        
            float delta = diff * actualframe / ac_FRAME;
            return bc + delta;
        }else{
            float diff = ac - bc;        
            float delta = diff * (actualframe-bc_FRAME) / (ac_FRAME-bc_FRAME);
            return bc + delta;
        }
    }
    
    public static float getActualScaleY(Drawing before, Drawing after, int actualframe){        
        float bc = before.getScaleY();
        int bc_FRAME = before.getFrame();
        float ac = after.getScaleY();
        int ac_FRAME = after.getFrame();
        
        if(before.getFrame()==actualframe){
            return before.getScaleY();
        }
        
        if(after.getFrame()==actualframe){
            return after.getScaleY();
        }
        
        if(bc_FRAME==0){ //Particular case
            float diff = ac - bc;        
            float delta = diff * actualframe / ac_FRAME;
            return bc + delta;
        }else{
            float diff = ac - bc;        
            float delta = diff * (actualframe-bc_FRAME) / (ac_FRAME-bc_FRAME);
            return bc + delta;
        }
    }
    
    public static float getActualAngle(Drawing before, Drawing after, int actualframe){        
        float bc = before.getAngle();
        int bc_FRAME = before.getFrame();
        float ac = after.getAngle();
        int ac_FRAME = after.getFrame();
        
        if(before.getFrame()==actualframe){
            return before.getAngle();
        }
        
        if(after.getFrame()==actualframe){
            return after.getAngle();
        }
        
        if(bc_FRAME==0){ //Particular case
            float diff = ac - bc;        
            float delta = diff * actualframe / ac_FRAME;
            return bc + delta;
        }else{
            float diff = ac - bc;        
            float delta = diff * (actualframe-bc_FRAME) / (ac_FRAME-bc_FRAME);
            return bc + delta;
        }
    }
    
    private static Color getColorFromFrame(Color c1, Color c2, int frame1, int frame2, int actualframe){
        Color bc = c1;
        int bc_R = bc.getRed();
        int bc_G = bc.getGreen();
        int bc_B = bc.getBlue();
        int bc_FRAME = frame1;
        
        Color ac = c2;
        int ac_R = ac.getRed();
        int ac_G = ac.getGreen();
        int ac_B = ac.getBlue();
        int ac_FRAME = frame2;
        
        int diff_R = ac_R - bc_R;
        int diff_G = ac_G - bc_G;
        int diff_B = ac_B - bc_B;
        
        actualframe = bc_FRAME==0 ? actualframe : actualframe - bc_FRAME;
        ac_FRAME = bc_FRAME==0 ? ac_FRAME : ac_FRAME - bc_FRAME;
        
        int delta_R = diff_R * actualframe / ac_FRAME;
        int delta_G = diff_G * actualframe / ac_FRAME;
        int delta_B = diff_B * actualframe / ac_FRAME;
        
        int r = bc_R + delta_R;
        int g = bc_G + delta_G;
        int b = bc_B + delta_B;
                
        return new Color(r, g ,b);
    }
    
//    public static Color[] getActualGradientColor(Drawing before, Drawing after, int actualframe){
//        
//        if(before.getFrame()==actualframe){
//            return before.getGradient();
//        }
//        
//        if(after.getFrame()==actualframe){
//            return after.getGradient();
//        }
//        
//        Color bc0 = before.getGradient()[0];
//        Color ac0 = after.getGradient()[0];
//        
//        Color c0 = getColorFromFrame(bc0, ac0, before.getFrame(), after.getFrame(), actualframe);
//        
//        Color bc1 = before.getGradient()[1];
//        Color ac1 = after.getGradient()[1];
//        
//        Color c1 = getColorFromFrame(bc1, ac1, before.getFrame(), after.getFrame(), actualframe);
//        
//        return new Color[]{c0, c1};
//    }
    
//    public static Color[] getActualFourSidesGradientColor(Drawing before, Drawing after, int actualframe){
//        
//        if(before.getFrame()==actualframe){
//            return before.getFourSidesGradient();
//        }
//        
//        if(after.getFrame()==actualframe){
//            return after.getFourSidesGradient();
//        }
//        
//        Color bc0 = before.getFourSidesGradient()[0];
//        Color ac0 = after.getFourSidesGradient()[0];
//        
//        Color c0 = getColorFromFrame(bc0, ac0, before.getFrame(), after.getFrame(), actualframe);
//        
//        Color bc1 = before.getFourSidesGradient()[1];
//        Color ac1 = after.getFourSidesGradient()[1];
//        
//        Color c1 = getColorFromFrame(bc1, ac1, before.getFrame(), after.getFrame(), actualframe);
//        
//        Color bc2 = before.getFourSidesGradient()[2];
//        Color ac2 = after.getFourSidesGradient()[2];
//        
//        Color c2 = getColorFromFrame(bc2, ac2, before.getFrame(), after.getFrame(), actualframe);
//        
//        Color bc3 = before.getFourSidesGradient()[3];
//        Color ac3 = after.getFourSidesGradient()[3];
//        
//        Color c3 = getColorFromFrame(bc3, ac3, before.getFrame(), after.getFrame(), actualframe);
//        
//        return new Color[]{c0, c1, c2, c3};
//    }
    
    public static BufferedImage getActualImage(Drawing before, int actualframe){ 
        return before.getImageOnFrames();
    }
}
