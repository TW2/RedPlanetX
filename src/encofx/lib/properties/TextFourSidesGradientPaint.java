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
public class TextFourSidesGradientPaint extends AbstractProperty {
    
    public TextFourSidesGradientPaint(){
        displayname = "4 sides gradient";
        SetupObject<Color[]> so = new SetupObject();
        so.setType(SetupObject.Type.FourSidesGradientPaint);
        so.set(new Color[]{Color.black, Color.white, Color.blue, Color.red});
        o = so;
    }
}
