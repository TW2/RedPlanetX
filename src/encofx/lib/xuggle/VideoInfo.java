/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.xuggle;

import com.xuggle.xuggler.ICodec;
import com.xuggle.xuggler.ICodec.ID;
import com.xuggle.xuggler.IContainer;
import com.xuggle.xuggler.IRational;
import com.xuggle.xuggler.IStream;
import encofx.lib.VTD2;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import javax.imageio.ImageIO;

/**
 *
 * @author Yves
 */
public class VideoInfo {
    
    private String VIDEO_PATH = "";
    private String DISPLAY_PATH = "";
    private double fps = 0d; //Ce paramètre FPS est plus facile à comprendre pour l'humain
    private IRational framerate = null; //Ce paramètre FPS est plus facile à comprendre pour l'ordinateur
    private long duration = 0L;
    private long filesize = 0L;
    private int bitrate = 0;
    private long frames = 0L;
    private int width = 0;
    private int height = 0;
    private ID codec = null;
    private String fileExtension = "";
    private Extract extract = null;
//    private Encode encode = null;
    
    private EncodeThread etENCODING = null;
    
    public VideoInfo(){
        extract = new Extract(this);
    }
    
    public void setVideo(String videoPath){
        VIDEO_PATH = videoPath;
        fileExtension = videoPath.substring(videoPath.lastIndexOf("."));
        extractInfo();        
    }
    
    public void setSaveFolder(String infoFolder){
        DISPLAY_PATH = infoFolder;
    }
    
    public void setVirtualTimeDisplay(VTD2 vtd){
//        encode = new Encode(vtd, this);
        etENCODING = new EncodeThread(vtd, this, DISPLAY_PATH, "out"+fileExtension);
    }
    
    public void extractVideo_(){
        if(VIDEO_PATH.isEmpty()==false && DISPLAY_PATH.isEmpty()==false){
            frames = extract.startExtract(VIDEO_PATH, DISPLAY_PATH);
        }        
    }
    
    public void extractVideo(){        
        if(VIDEO_PATH.isEmpty()==false && DISPLAY_PATH.isEmpty()==false){
            extract.setMediaPath(VIDEO_PATH);
            extract.setExtractFolder(DISPLAY_PATH);
            extract.start();
            
            int count = -1;
            
            while(extract.isAlive()){
            /*Wait*/
                if(count!=extract.getCurrentFrame()){
                    count = extract.getCurrentFrame();
                    
                }
            }
            
        }
    }
    
    private void extractInfo(){
        IContainer container = IContainer.make();
        container.open(VIDEO_PATH, IContainer.Type.READ, null);
        duration = container.getDuration();
        filesize = container.getFileSize();
        bitrate = container.getBitRate();
        for(int i=0; i<container.getNumStreams(); i++){
            IStream stream = container.getStream(i);
            if(stream.getStreamCoder().getCodecType()==ICodec.Type.CODEC_TYPE_VIDEO){
                //frames = stream.getNumFrames();///Crée une fausse valeur !
                fps = stream.getFrameRate().getDouble();
                framerate = stream.getFrameRate();
                width = stream.getStreamCoder().getWidth();
                height = stream.getStreamCoder().getHeight();
                codec = stream.getStreamCoder().getCodecID();
            }
        }
        container.close();
    }
    
    public BufferedImage getImage(long frame) throws IOException{
        File folder = new File(DISPLAY_PATH);
        if(folder.exists() && frame <= frames && frame >= 0){
            BufferedImage image = ImageIO.read(new File(folder, frame+".png"));
            return image;
        }
        return null;
    }
    
    public void setFrames(long frames){
        this.frames = frames;
    }
    
    public long getFrames(){
        return frames;
    }
    
    public long getDuration(){
        return duration;
    }
    
    public long getFilesize(){
        return filesize;
    }
    
    public int getBitrate(){
        return bitrate;
    }
    
    public void setFPS(double fps){
        this.fps = fps;
    }
    
    /**
     * Obtient le nombre d'image par secondes
     * @return Nombre d'images en 1 secondes
     */
    public double getFPS(){
        return fps;
    }
    
    /**
     * Obtient le nombre d'image par millisecondes
     * @return Nombre d'images en 1 millisecondes
     */
    public double getFPM(){
        return getFPS()/1000;
    }
    
    /**
     * Obtient le nombre d'image par nanosecondes
     * @return Nombre d'images en 1 nanosecondes
     */
    public double getFPN(){
        return getFPS()/1000000;
    }
    
    /**
     * Obtient le timestamp de la frame en secondes
     * @param frame L'image en cours
     * @return Le timestamp en secondes
     */
    public long getTimestamp_S(long frame){
        return Math.round(frame*getFPS());
    }
    
    /**
     * Obtient le timestamp de la frame en millisecondes
     * @param frame L'image en cours
     * @return Le timestamp en millisecondes
     */
    public long getTimestamp_MS(long frame){
        return Math.round(frame*getFPM());
    }
    
    /**
     * Obtient le timestamp de la frame en nanosecondes
     * @param frame L'image en cours
     * @return Le timestamp en nanosecondes
     */
    public long getTimestamp_NS(long frame){
        return Math.round(frame*getFPN());
    }
    
    public String getVideoPath(){
        return VIDEO_PATH;
    }
    
    public String getSaveFolder(){
        return DISPLAY_PATH;
    }
    
    public int getVideoWidth(){
        return width;
    }
    
    public int getVideoHeight(){
        return height;
    }
    
    public ID getCodec(){
        return codec;
    }
    
    public IRational getFramerate(){
        return framerate;
    }
    
    public void encodeVideo(){
//        if(encode!=null && VIDEO_PATH.isEmpty()==false){
//            File file = new File(VIDEO_PATH);
//            encode.startEncode(DISPLAY_PATH+File.separator+file.getName()+fileExtension);
//        }
        
        etENCODING.start();
        
    }
    
    public class EncodeThread extends Thread implements Runnable {
        
        private Encode encode = null;
        private String SAVE_PATH = null;
        private String outputfile = null;
        
        public EncodeThread(VTD2 vtd, VideoInfo vi, String SAVE_PATH, String outputfile){
            encode = new Encode(vtd, vi);
            this.SAVE_PATH = SAVE_PATH;
            this.outputfile = outputfile;
        }
        
        @Override
        public void run(){
            encode.startEncode(SAVE_PATH+File.separator+outputfile);
            interrupt();
        }
        
    }
    
    public class ViewerThread extends Thread implements Runnable {
        
        private final ProcessViewer pv = new ProcessViewer();
        private long frames = 0L, frame = 0L;
        
        public ViewerThread(){
            
        }
        
        @Override
        public void run(){
            
        }
        
        public void setFrames(long frames){
            this.frames = frames;
        }
        
        public void setCurrentFrame(long frame){
            this.frame = frame;
        }
        
        public void startThread(){
            start();
        }
        
        public void stopThread(){
            interrupt();            
        }
    }
    
    public Extract getExtract(){
        return extract;
    }
    
}
