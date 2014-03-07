/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.properties;

import encofx.lib.settings.SetupObject;
import java.awt.image.BufferedImage;

/**
 *
 * @author Yves
 */
public class ImageOnFrames extends AbstractProperty {
    
    protected int frames_duration = 1;
    
    public ImageOnFrames(){
        displayname = "Drawing";
        SetupObject<BufferedImage> so = new SetupObject();
        so.setType(SetupObject.Type.ImageOnFrames);
        so.set(new BufferedImage(1280, 720, BufferedImage.TYPE_INT_ARGB));
        o = so;
    }
    
    public void init(int width, int height){
        SetupObject<BufferedImage> so = new SetupObject();
        so.setType(SetupObject.Type.ImageOnFrames);
        so.set(new BufferedImage(width, height, BufferedImage.TYPE_INT_ARGB));
        o = so;
    }
    
    public void setDuration(int frames_duration){
        this.frames_duration = frames_duration;
    }
    
    public int getDuration(){
        return frames_duration;
    }

}
