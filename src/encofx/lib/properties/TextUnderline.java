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
public class TextUnderline extends AbstractProperty {
    
    public TextUnderline(){
        displayname = "Underline";
        SetupObject<Boolean> so = new SetupObject();
        so.setType(SetupObject.Type.Underline);
        so.set(false);
        o = so;
    }
}
