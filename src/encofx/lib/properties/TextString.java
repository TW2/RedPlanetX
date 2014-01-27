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
public class TextString extends AbstractProperty {
    
    public TextString(){
        displayname = "String";
        SetupObject<String> so = new SetupObject();
        so.setType(SetupObject.Type.Text);
        so.set("Java is amazing !!");
        o = so;
    }
}
