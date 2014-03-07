/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.properties;

import encofx.lib.settings.SetupObject;
import java.awt.image.BufferedImage;

/**
 * Pour les images que l'on ajoute et qui seront sur le vidéo en diverses tailles.
 * @author Yves
 */
public class ImageOntoFrames extends AbstractProperty {
    
    public ImageOntoFrames(){
        displayname = "Image";
        SetupObject<BufferedImage> so = new SetupObject();
        so.setType(SetupObject.Type.ImageOntoFrames);
        so.set(new BufferedImage(1280, 720, BufferedImage.TYPE_INT_ARGB));
        o = so;
    }
    
}
