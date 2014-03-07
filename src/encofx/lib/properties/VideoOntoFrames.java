/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.properties;

import encofx.lib.settings.SetupObject;
import encofx.lib.xuggle.VideoEmulation;

/**
 *
 * @author Yves
 */
public class VideoOntoFrames extends AbstractProperty {
    
    public VideoOntoFrames(){
        displayname = "Video";
        SetupObject<VideoEmulation> so = new SetupObject();
        so.setType(SetupObject.Type.VideoOntoFrames);
        so.set(new VideoEmulation());
        o = so;
    }
}
