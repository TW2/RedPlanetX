/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.properties;

import encofx.lib.effects.TextCollection;
import encofx.lib.settings.SetupObject;

/**
 *
 * @author Yves
 */
public class TextFontstyle extends AbstractProperty {
    
    public TextFontstyle(){
        displayname = "Fontstyle";
        SetupObject<TextCollection.FontStyle> so = new SetupObject();
        so.setType(SetupObject.Type.Fontstyle);
        so.set(TextCollection.FontStyle.Plain);
        o = so;
    }
    
}
