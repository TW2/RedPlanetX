/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.properties;

import encofx.lib.settings.SetupObject;

/**
 *
 * @author Yves
 */
public class TextScaleY extends AbstractProperty {
    
    public TextScaleY(){
        displayname = "Scale Y";
        SetupObject<Float> so = new SetupObject();
        so.setType(SetupObject.Type.ScaleY);
        so.set(100f);
        o = so;
    }
}
