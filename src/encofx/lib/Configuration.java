/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib;

import encofx.lib.xuggle.VideoInfo;
import java.io.File;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import org.json.simple.JSONObject;
import org.json.simple.parser.JSONParser;
import org.json.simple.parser.ParseException;

/**
 *
 * @author Yves
 */
public class Configuration {
    
    private String CONFIG_FOLDER = "";
    private final List<File> projects = new ArrayList<>();
    
    public Configuration(){
        CONFIG_FOLDER = getApplicationDirectory()+File.separator+"settings";
        init();
    }
    
    public Configuration(String configFolder){
        CONFIG_FOLDER = configFolder;
        init();
    }
    
    private String getApplicationDirectory(){
        if(System.getProperty("os.name").equalsIgnoreCase("Mac OS X")){
            java.io.File file = new java.io.File("");
            return file.getAbsolutePath();
        }
        String path = System.getProperty("user.dir");
        if(path.toLowerCase().contains("jre")){
            File f = new File(getClass().getProtectionDomain()
                    .getCodeSource().getLocation().toString()
                    .substring(6));
            path = f.getParent();
        }
        return path;
    }
    
    private void init(){
        File configFolder = new File(CONFIG_FOLDER);
        for(File f : configFolder.listFiles()){
            if(f.getAbsolutePath().endsWith(".json") && projects.contains(f)==false){
                projects.add(f);
            }
        }
    }
    
    public boolean isInConfigFolder(File file){
        return projects.contains(file);
    }
    
    public String getSaveFolderForProject(File file) throws IOException, ParseException{
        JSONParser parser = new JSONParser();
        Object obj = parser.parse(new FileReader(file));
        JSONObject jsonObject = (JSONObject)obj;
        return (String)jsonObject.get("SAVE_PATH");
    }
    
    public void createJSON(VideoInfo vi) throws IOException{
        JSONObject obj = new JSONObject();
        obj.put("VIDEO_PATH", vi.getVideoPath());
        obj.put("SAVE_PATH", vi.getSaveFolder());
        obj.put("frames", vi.getFrames());
        File projectFile = new File(vi.getVideoPath());
        File jsonFile = new File(CONFIG_FOLDER+File.separator+projectFile.getName()+".json");
        FileWriter fw = new FileWriter(jsonFile);
        obj.writeJSONString(fw);
        fw.flush();
        fw.close();
    } 
    
    public VideoInfo fromJSON(File file) throws IOException, ParseException{
        JSONParser parser = new JSONParser();
        FileReader fr = new FileReader(file);
        Object obj = parser.parse(fr);
        JSONObject jsonObject = (JSONObject)obj;
        VideoInfo vi = new VideoInfo();
        
        vi.setVideo((String)jsonObject.get("VIDEO_PATH"));
        vi.setSaveFolder((String)jsonObject.get("SAVE_PATH"));
        vi.setFrames((Long)jsonObject.get("frames"));
        CONFIG_FOLDER = (String)jsonObject.get("SAVE_PATH");
        init();
        
        fr.close();
        
        return vi;
    }
    
    // Nouvelles méthodes ------------------------------------------------------
    
    public static void createJSON(String video_path, String save_path, String conf_folder, int frames) throws IOException{
        JSONObject obj = new JSONObject();
        obj.put("VIDEO_PATH", video_path);
        obj.put("SAVE_PATH", save_path);
        obj.put("frames", frames);
        File projectFile = new File(video_path);
        File jsonFile = new File(conf_folder+File.separator+projectFile.getName()+".json");
        try (FileWriter fw = new FileWriter(jsonFile)) {
            obj.writeJSONString(fw);
            fw.flush();
        }
    }
    
}
