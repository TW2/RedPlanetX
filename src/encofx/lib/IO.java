/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib;

import java.io.BufferedReader;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.ArrayList;
import java.util.List;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

/**
 *
 * @author Yves
 */
public class IO {
    
    public IO(){
        
    }
    
    public static List<EventLine> extractASS(String asspath) throws FileNotFoundException, IOException{        
        List<EventLine> content = new ArrayList<>();
        
        // Lecture du fichier
        String newline;
        String charset;
        try (FileReader fr = new FileReader(asspath)) {
            charset = detectCharset(fr);
        }
        try (FileInputStream fis = new FileInputStream(asspath); BufferedReader br = new BufferedReader(
                new InputStreamReader(fis, charset))) {
            
            //On lit le fichier
            while((newline=br.readLine())!=null){
                try{
                    if(newline.startsWith("Dialogue:") | newline.startsWith("Comment")){
                        EventLine evline = new EventLine();
                        evline.create(newline);
                        content.add(evline);
                    }
                }catch(IndexOutOfBoundsException ioobe){
                    //erreurs = ioobe.getMessage();
                }
            }
        }
        
        return content;
    }
    
    /** <p>Try to get a correct charset<br />
    * Essaie d'obtenir le bon encodage des caractères.</p>
    * <table><tr><td colspan="2">Byte Order mark :</td><td></td></tr>
    * <tr><td width="100">Bytes</td><td>Encoding Form</td></tr>
    * <tr><td>00 00 FE FF</td><td>UTF-32, big-endian</td></tr>
    * <tr><td>FF FE 00 00</td><td>UTF-32, little-endian</td></tr>
    * <tr><td>FE FF</td><td>UTF-16, big-endian</td></tr>
    * <tr><td>FF FE</td><td>UTF-16, little-endian</td></tr>
    * <tr><td>EF BB BF</td><td>UTF-8</td></tr></table>
     * @param fr
     * @return  */
    private static String detectCharset(FileReader fr){
        String charset = ""; String newline;
        
        try {
            try (BufferedReader br = new BufferedReader(fr)) {
                while ((newline = br.readLine()) != null) {
                    
                    if(newline.startsWith("[\u0000\u0000") |
                            newline.startsWith("\u00FF\u00FE\u0000\u0000")){
                        charset = "UTF-32LE";
                    }else if(newline.startsWith("\u0000\u0000[") |
                            newline.startsWith("\u0000\u0000\u00FE\u00FF")){
                        charset = "UTF-32BE";
                    }else if(newline.startsWith("[\u0000") |
                            newline.startsWith("\u00FF\u00FE")){
                        charset = "UTF-16LE";
                    }else if(newline.startsWith("\u0000[") |
                            newline.startsWith("\u00FE\u00FF")){
                        charset = "UTF-16BE";
                    }else if(newline.startsWith("\u00EF\u00BB\u00BF")){
                        charset = "UTF-8";
                    }
                    
                    // If a charset was found then close the stream
                    // and the return charset encoding.
                    if (charset.length()!=0){
                        br.close();
                        return charset;
                    }
                }
                
                // If nothing was found then set the encoding to system default.
                if (charset.length()==0){
                    charset = fr.getEncoding();
                }
            }
            
        } catch (IOException ioe) {
            System.out.println(ioe.getMessage());
        }
        
        return charset;
    }
    
    public static class EventLine{
        
        String Type = "Dialogue";
        String Layer = "0";
        String Start = "0:00:00.00";
        String End = "0:00:00.00";
        String Style = "Default";
        String Name = "";
        String MarginL = "0000";
        String MarginR = "0000";
        String MarginV = "0000";
        String Effect = "";
        String Text = "";
        
        public EventLine(){
            
        }
        
        public void create(String rawline){
            if(rawline.startsWith("Comment")){
                Type = "Comment";
            }
            
            String newline = rawline.substring(rawline.indexOf(":")+1);
            String[] table = newline.split(",");
            
            Layer = table[0];
            Start = table[1];
            End = table[2];
            Style = table[3];
            Name = table[4];
            MarginL = table[5];
            MarginR = table[6];
            MarginV = table[7];
            Effect = table[8];
            Text = table[9];
        }
        
        public String getType(){
            return Type + ": ";
        }
        
        public String getLayer(){
            return Layer;
        }
        
        public String getStart(){
            return Start;
        }
        
        public String getEnd(){
            return End;
        }
        
        public String getStyle(){
            return Style;
        }
        
        public String getName(){
            return Name;
        }
        
        public String getMarginL(){
            return MarginL;
        }
        
        public String getMarginR(){
            return MarginR;
        }
        
        public String getMarginV(){
            return MarginV;
        }
        
        public String getEffect(){
            return Effect;
        }
        
        public String getText(){
            return Text;
        }
    }
    
    public static String getStrippedElement(String s){
        String str;
        if(s.contains("{\\")){
            try{
                str = s.replaceAll("\\{[^\\}]+\\}", "");
            }catch(Exception e){
                str = s;
            }
        }else{
            str = s;
        }
        return str;
    }
    
    public static String getFrame(String asstime, double fps){
        double fpm = fps/1000;
        /// FPM <> Millisecondes
        // FRAME = Millisecondes / FPM
        Pattern p = Pattern.compile("(\\d+):(\\d+):(\\d+).(\\d+)");
        Matcher m = p.matcher(asstime);
        m.find();
        
        int hours = Integer.parseInt(m.group(1));
        int mins = Integer.parseInt(m.group(2));
        int secs = Integer.parseInt(m.group(3));
        int mils = Integer.parseInt(m.group(4))*10;
        
        Time t = new Time(hours, mins, secs, mils);
        
        return Math.round(Time.toMillisecondsTime(t) * fpm)+"";
    }
    
    public static String getSyllableFrame(String syltime, double fps){
        double fpm = fps/1000;
        /// FPM <> Millisecondes
        // FRAME = Millisecondes / FPM        
        return Math.round((Integer.parseInt(syltime)*10) * fpm)+"";
    }
    
    public static long getMilliseconds(String asstime){
        Pattern p = Pattern.compile("(\\d+):(\\d+):(\\d+).(\\d+)");
        Matcher m = p.matcher(asstime);
        m.find();
        
        int hours = Integer.parseInt(m.group(1));
        int mins = Integer.parseInt(m.group(2));
        int secs = Integer.parseInt(m.group(3));
        int mils = Integer.parseInt(m.group(4))*10;
        
        Time t = new Time(hours, mins, secs, mils);
        return Time.toMillisecondsTime(t);
    }
    
    public static long getSyllableMillisenconds(String syltime){
        return Integer.parseInt(syltime)*10;
    }
    
    public static String getFrame(long millisenconds, double fps){
        double fpm = fps/1000;
        return Math.round(millisenconds * fpm)+"";
    }
}
