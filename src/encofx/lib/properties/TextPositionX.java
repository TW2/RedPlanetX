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
public class TextPositionX extends AbstractProperty {
    
    public TextPositionX(){
        displayname = "X";
        SetupObject<Float> so = new SetupObject();
        so.setType(SetupObject.Type.PositionX);
        so.set(0f);
        o = so;
    }
}
