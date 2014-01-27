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
public class TextAnchorPosition extends AbstractProperty {
    
    public TextAnchorPosition(){
        displayname = "Position";
        SetupObject<TextCollection.AnchorPosition> so = new SetupObject();
        so.setType(SetupObject.Type.AnchorPosition);
        so.set(TextCollection.AnchorPosition.CornerLeftBottom);
        o = so;
    }
}
