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
public interface ObjectCollectionInterface {
    
    public enum Type{
        Unknown, Text, VText, Parent, Shape;
    }
    
    public Type getType();
    
    public SubObjects getSubObjects();
    
    public void setSubObjects(SubObjects subs);
    
}
