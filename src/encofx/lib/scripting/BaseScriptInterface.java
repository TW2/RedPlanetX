/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.scripting;

/**
 *
 * @author Yves
 */
public interface BaseScriptInterface {
    
    public String getName();
    public void setName(String name);
    
    public String getDisplayName();
    public void setDisplayName(String displayname);
    
    public String getDescription();
    public void setDescription(String description);
    
    public String getVersion();
    public void setVersion(String version);
    
    public String getAuthor();
    public void setAuthor(String author);
    
    public String getDate();
    public void setDate(String date);
    
    public String getPath();
    public void setPath(String path);
    
    public String getFunction();
    public void setFunction(String function);
    
    public String getCode();
    public void setCode(String code);
    
}
