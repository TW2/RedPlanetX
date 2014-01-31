/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.scripting;

import java.awt.Graphics2D;
import java.io.File;
import java.util.ArrayList;
import java.util.List;

/**
 *
 * @author Yves
 */
public class Scripting {
    
    private static String actualPath = null;
    private static List<Object> sobjectList = new ArrayList<>();
    private static Graphics2D g2 = null;
    
    public Scripting(){
        
    }
    
    /** <p>Search for all script of this directory.<br />
    * Recherche tous les scripts du répertoire.</p>
     * @param directory */
    public void searchForScript(String directory){
        sobjectList.clear(); //Security
        File dir = new File(directory);
        for(File f : dir.listFiles()){
            if(f.getPath().endsWith(".rb")){
                actualPath = f.getPath();
                RubyScript.runRubyScript(actualPath);
            }
            if(f.getPath().endsWith(".py")){
                actualPath = f.getPath();
                PythonScript.runPythonScript(actualPath);
            }
            if(f.getPath().endsWith(".js")){
                actualPath = f.getPath();
                RhinoScript.runRhinoScript(actualPath);
            }
            actualPath = "";
        }
    }
    
    public static void register(Object oname, Object odisplayname, Object odescription,
            Object oversion, Object oauthor, Object odate, Object ofunction){
        if(actualPath!=null && actualPath.isEmpty()==false){
            if(actualPath.endsWith(".py")){
                PythonScript py = new PythonScript();
                py.setScript(
                        oname.toString(), odisplayname.toString(), odescription.toString(),
                        oversion.toString(), oauthor.toString(), odate.toString(),
                        actualPath, ofunction.toString());
                sobjectList.add(py);
            }else if(actualPath.endsWith(".js")){
                RhinoScript rh = new RhinoScript();
                rh.setScript(
                        oname.toString(), odisplayname.toString(), odescription.toString(),
                        oversion.toString(), oauthor.toString(), odate.toString(),
                        actualPath, ofunction.toString());
                sobjectList.add(rh);
            }else if(actualPath.endsWith(".rb")){
                RubyScript rb = new RubyScript();
                rb.setScript(
                        oname.toString(), odisplayname.toString(), odescription.toString(),
                        oversion.toString(), oauthor.toString(), odate.toString(),
                        actualPath, ofunction.toString());
                sobjectList.add(rb);
            }
        }        
    }
    
    public void runScript(Object o){
        if(o instanceof PythonScript){
            PythonScript py = (PythonScript)o;
            PythonScript.runPythonScript(py.getPath());
        }else if(o instanceof RhinoScript){
            RhinoScript rh = (RhinoScript)o;
            RhinoScript.runRhinoScript(rh.getPath());
        }else if(o instanceof RubyScript){
            RubyScript rb = (RubyScript)o;
            RubyScript.runRubyScript(rb.getPath());
        }
    }
    
    public void runScriptAndDo(Object o){
        if(o instanceof PythonScript){
            PythonScript py = (PythonScript)o;
            PythonScript.runPythonScriptAndDo(py.getPath(), py.getFunction());
        }else if(o instanceof RhinoScript){
            RhinoScript rh = (RhinoScript)o;
            RhinoScript.runRhinoScriptAndDo(rh.getPath(), rh.getFunction());
        }else if(o instanceof RubyScript){
            RubyScript rb = (RubyScript)o;
            RubyScript.runRubyScriptAndDo(rb.getPath(), rb.getFunction());
        }
    }
    
    public String runCodeAndDo(Object o){
        String value = "";
        if(o instanceof PythonScript){
            PythonScript py = (PythonScript)o;
            PythonScript.runPythonCodeAndDo(py.getCode(), py.getFunction());
        }else if(o instanceof RhinoScript){
            RhinoScript rh = (RhinoScript)o;
            RhinoScript.runRhinoCodeAndDo(rh.getCode(), rh.getFunction());
        }else if(o instanceof RubyScript){
            RubyScript rb = (RubyScript)o;
            RubyScript.runRubyCodeAndDo(rb.getCode(), rb.getFunction());
        }
        return value;
    }
    
    public List<Object> getSObjectList(){
        return sobjectList;
    }
    
    public void clearSObjectList(){
        sobjectList.clear();
    }
    
    public void setGraphics(Graphics2D g2){
        Scripting.g2 = g2;
    }
    
    //==========================================================================
    //==========================================================================
    //==========================================================================
    
    public static Graphics2D getGraphics(){
        return g2;
    }
    
}
