/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.properties;

import encofx.lib.PropertyInterface;

/**
 *
 * @author Yves
 */
public class AbstractProperty implements PropertyInterface {
    
    protected String name = "Undefined";
    protected String displayname = "Undefined";
    protected Object o = null;
    
    public AbstractProperty(){
        
    }

    @Override
    public void setName(String name) {
        this.name = name;
    }

    @Override
    public String getName() {
        return name;
    }

    @Override
    public void setDisplayName(String displayname) {
        this.displayname = displayname;
    }

    @Override
    public String getDisplayName() {
        return displayname;
    }

    @Override
    public void setObject(Object o) {
        this.o = o;
    }

    @Override
    public Object getObject() {
        return o;
    }
    
    @Override
    public String toString(){
        return getDisplayName();
    }
    
}
