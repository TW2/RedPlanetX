/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.properties;

import encofx.lib.effects.ParentCollection;
import encofx.lib.settings.SetupObject;

/**
 *
 * @author Yves
 */
public class Child extends AbstractProperty {
    
    public Child(){
        displayname = "Parent";
        SetupObject<ParentCollection> so = new SetupObject();
        so.setType(SetupObject.Type.Child);
        so.set(null);
        o = so;
    }
}
