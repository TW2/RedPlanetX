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
public abstract class BaseScript implements BaseScriptInterface {
    
    protected String name = "Unknown name";
    protected String displayname = "Unknown displayname";
    protected String description = "Unknown description";
    protected String version = "Unknown version";
    protected String author = "Unknown author";
    protected String date = "Unknown date";
    protected String path = "Unknown path";
    protected String function = "Unknown function";
    protected String code = "";

    @Override
    public String getName() {
        return name;
    }

    @Override
    public void setName(String name) {
        this.name = name;
    }

    @Override
    public String getDisplayName() {
        return displayname;
    }

    @Override
    public void setDisplayName(String displayname) {
        this.displayname = displayname;
    }

    @Override
    public String getDescription() {
        return description;
    }

    @Override
    public void setDescription(String description) {
        this.description = description;
    }

    @Override
    public String getVersion() {
        return version;
    }

    @Override
    public void setVersion(String version) {
        this.version = version;
    }

    @Override
    public String getAuthor() {
        return author;
    }

    @Override
    public void setAuthor(String author) {
        this.author = author;
    }

    @Override
    public String getDate() {
        return date;
    }

    @Override
    public void setDate(String date) {
        this.date = date;
    }

    @Override
    public String getPath() {
        return path;
    }

    @Override
    public void setPath(String path) {
        this.path = path;
    }

    @Override
    public String getFunction() {
        return function;
    }

    @Override
    public void setFunction(String function) {
        this.function = function;
    }    
    
    @Override
    public String getCode() {
        return code;
    }

    @Override
    public void setCode(String code) {
        this.code = code;
    }
}
