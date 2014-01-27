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
public class TextStrikeOut extends AbstractProperty {
    
    public TextStrikeOut(){
        displayname = "Strike out";
        SetupObject<Boolean> so = new SetupObject();
        so.setType(SetupObject.Type.StrikeOut);
        so.set(false);
        o = so;
    }
}
