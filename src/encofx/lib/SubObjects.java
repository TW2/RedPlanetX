/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib;

import java.util.ArrayList;
import java.util.List;

/**
 *
 * @author Yves
 * @param <T>
 */
public class SubObjects <T>{
    
    private final T obj = null;
    private final List<T> objects = new ArrayList<>();
    
    public SubObjects(){
        
    }
    
    public void addObject(T obj){
        objects.add(obj);
    }
    
    public void addAllObjects(List<T> objs){
        for(T o : objs){
            objects.add(o);
        }
    }
    
    public void removeObject(T obj){
        objects.remove(obj);
    }
    
    public void clear(){
        objects.clear();
    }
    
    public List<T> getObjects(){
        return objects;
    }
    
    public boolean contains(T obj){
        return objects.contains(obj);
    }
    
}
