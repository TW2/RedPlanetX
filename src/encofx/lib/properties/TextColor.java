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
public class TextColor extends AbstractProperty {
    
    public TextColor(){
        displayname = "Color";
        SetupObject<Color> so = new SetupObject();
        so.setType(SetupObject.Type.Color);
        so.set(Color.red);
        o = so;
    }
    
}
