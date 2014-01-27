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
public class TextFontname extends AbstractProperty {
    
    public TextFontname(){
        displayname = "Fontname";
        SetupObject<String> so = new SetupObject();
        so.setType(SetupObject.Type.Fontname);
        so.set("Arial");
        o = so;
    }
    
}
