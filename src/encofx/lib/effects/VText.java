/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.effects;

import encofx.lib.FXObject;
import encofx.lib.properties.AbstractProperty;
import java.awt.Color;
import java.awt.geom.Point2D;
import java.util.ArrayList;
import java.util.List;

/**
 *
 * @author Yves
 */
public class VText extends FXObject {
    
    //Objects for the table of properties and settings
    private final List<AbstractProperty> properties = new ArrayList<>();
    
    public VText(){
        properties.add(propFontsize);
        properties.add(propX);
        properties.add(propY);
        properties.add(propColor);
    }
    
    @Override
    public List<AbstractProperty> getProperties(){
        return properties;
    }
    
    @Override
    public String toString(){
        return "At the frame "+frame;
    }
    
    public static Color getActualColor(VText before, VText after, int actualframe){
        
        if(before.getFrame()==actualframe){
            return before.getColor();
        }
        
        if(after.getFrame()==actualframe){
            return after.getColor();
        }
        
        Color bc = before.getColor();
        int bc_R = bc.getRed();
        int bc_G = bc.getGreen();
        int bc_B = bc.getBlue();
        int bc_FRAME = before.getFrame();
        
        Color ac = after.getColor();
        int ac_R = ac.getRed();
        int ac_G = ac.getGreen();
        int ac_B = ac.getBlue();
        int ac_FRAME = after.getFrame();
        
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
    
    public static float getActualSize(VText before, VText after, int actualframe){        
        float bc = before.getSize();
        int bc_FRAME = before.getFrame();
        float ac = after.getSize();
        int ac_FRAME = after.getFrame();
        
        if(before.getFrame()==actualframe){
            return before.getSize();
        }
        
        if(after.getFrame()==actualframe){
            return after.getSize();
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
    
    public static float getActualX(VText before, VText after, Point2D anchor, int actualframe){
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
    
    public static float getActualY(VText before, VText after, Point2D anchor, int actualframe){
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
    
}
