/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.scripting;

import java.io.FileNotFoundException;
import java.io.FileReader;
import javax.script.Invocable;
import javax.script.ScriptEngine;
import javax.script.ScriptEngineManager;
import javax.script.ScriptException;

/**
 *
 * @author Yves
 */
public class PythonScript extends BaseScript {
    
    public PythonScript(){
        
    }
    
    public void setScript(String name, String displayname, String description,
            String version, String author, String date, String path, String function){
        this.name = name;
        this.displayname = displayname;
        this.description = description;
        this.version = version;
        this.author = author;
        this.date = date;
        this.path = path;
        this.function = function;
    }
    
    /** <p>Runs a ruby script from the given file.<br />
    * Execute un script ruby à partir d'un fichier.</p>
     * @param path */
    public static void runPythonScript(String path){
        try{
            ScriptEngine pyEngine;
            pyEngine = new ScriptEngineManager().getEngineByName("python");
            pyEngine.eval(new FileReader(path));
        } catch (FileNotFoundException | ScriptException fnfe){
        }
    }
    
     /** <p>Execute a function (of a ruby script).<br />
    * Execute une fonction (d'un script ruby).</p>
     * @param path
     * @param event */
    public static void runPythonScriptAndDo(String path, String event){
        try {
            ScriptEngine pyEngine;
            pyEngine = new ScriptEngineManager().getEngineByName("python");
            Invocable inv = (Invocable)pyEngine;
            pyEngine.eval(new java.io.FileReader(path));
            inv.invokeFunction(event);
        } catch (FileNotFoundException | ScriptException | NoSuchMethodException fnfe){
        }
    }
    
    /** <p>Execute a function from a ruby code.<br />
    * Execute une fonction d'un code ruby.</p>
     * @param code
     * @param event
     * @return  */
    public static String runPythonCodeAndDo(String code, String event){
        String value = "";
        try {
            ScriptEngine pyEngine;
            pyEngine = new ScriptEngineManager().getEngineByName("python");
            Invocable inv = (Invocable)pyEngine;
            pyEngine.eval(new java.io.StringReader(code));
            Object o = inv.invokeFunction(event);
            value = o.toString();
        } catch( ScriptException | NoSuchMethodException se){            
        }
        return value;
    }
    
    @Override
    public String toString(){
        return displayname;
    }
}
