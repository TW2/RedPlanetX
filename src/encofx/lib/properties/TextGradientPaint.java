/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.properties;

import encofx.lib.settings.SetupObject;
import java.awt.Color;

/**
 *
 * @author Yves
 */
public class TextGradientPaint extends AbstractProperty {
    
    public TextGradientPaint(){
        displayname = "Gradient";
        SetupObject<Color[]> so = new SetupObject();
        so.setType(SetupObject.Type.GradientPaint);
        so.set(new Color[]{Color.black, Color.white});
        o = so;
    }
}
