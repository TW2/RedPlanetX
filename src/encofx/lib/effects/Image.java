/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.effects;

import encofx.lib.FXObject;
import encofx.lib.properties.AbstractProperty;
import java.awt.geom.Point2D;
import java.awt.image.BufferedImage;
import java.util.ArrayList;
import java.util.List;

/**
 *
 * @author Yves
 */
public class Image extends FXObject {
    
    //Objects for the table of properties and settings
    private final List<AbstractProperty> properties = new ArrayList<>();
    
    private boolean isSyllable = false;
    private int syllableIndex = -1;
    
    public Image(){
        properties.add(propX);
        properties.add(propY);
        properties.add(propTrans);
        properties.add(propScaleX);
        properties.add(propScaleY);
        properties.add(propAngle);
        properties.add(propImageOntoFrames);
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
    
    public static float getActualX(Image before, Image after, Point2D anchor, int actualframe){
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
    
    public static float getActualY(Image before, Image after, Point2D anchor, int actualframe){
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
    
    public static float getActualTransparency(Image before, Image after, int actualframe){        
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
    
    public static float getActualScaleX(Image before, Image after, int actualframe){        
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
    
    public static float getActualScaleY(Image before, Image after, int actualframe){        
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
    
    public static float getActualAngle(Image before, Image after, int actualframe){        
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
    
    public static BufferedImage getActualImage(Image before, int actualframe){ 
        return before.getImageOntoFrames();
    }
    
}
