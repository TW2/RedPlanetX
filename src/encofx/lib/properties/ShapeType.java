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
public class ShapeType extends AbstractProperty {
    
    public ShapeType(){
        displayname = "Shape type";
        SetupObject<ObjectShapeType> so = new SetupObject();
        so.setType(SetupObject.Type.ShapeType);
        so.set(ObjectShapeType.Free);
        o = so;
    }
    
    public enum ObjectShapeType{
        Free("Free design"),
        Rectangle("Rectangle"),
        RoundRectangle("Round rectangle"),
        Ellipse("Ellipse");
        
        String name;
        
        ObjectShapeType(String name){
            this.name = name;
        }
        
        public String getName(){
            return name;
        }
        
        @Override
        public String toString(){
            return name;
        }
    }
}
