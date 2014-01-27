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
public class TextGradientType extends AbstractProperty {
    
    public TextGradientType(){
        displayname = "Gradient type";
        SetupObject<TextCollection.GradientType> so = new SetupObject();
        so.setType(SetupObject.Type.GradientType);
        so.set(TextCollection.GradientType.None);
        o = so;
    }
}
