/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib;

import encofx.lib.properties.AbstractProperty;
import java.util.List;

/**
 *
 * @author Yves
 */
public interface FXInterface {
    
    public enum Type{
        Java, JavaFX, Java3D, Script, Unknown;
    }
    
    public void setName(String name);
    
    public String getName();
    
    public void setDisplayName(String displayname);
    
    public String getDisplayName();
    
    public void setType(Type type);
    
    public Type getType();
    
    public List<AbstractProperty> getProperties();
    
    public void setFrame(int frame);
    
    public int getFrame();
    
}
