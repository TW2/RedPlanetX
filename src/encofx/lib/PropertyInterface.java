/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib;

/**
 *
 * @author Yves
 */
public interface PropertyInterface {
    
    public void setName(String name);
    
    public String getName();
    
    public void setDisplayName(String displayname);
    
    public String getDisplayName();
    
    public void setObject(Object o);
    
    public Object getObject();
    
}
