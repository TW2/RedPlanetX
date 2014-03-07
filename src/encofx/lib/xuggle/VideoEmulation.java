/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.xuggle;

import com.xuggle.xuggler.ICodec;
import com.xuggle.xuggler.IContainer;
import com.xuggle.xuggler.IRational;
import com.xuggle.xuggler.IStream;
import encofx.lib.xuggle.Extract;
import encofx.lib.xuggle.VideoInfo;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import javax.imageio.ImageIO;

/**
 *
 * @author Yves
 */
public class VideoEmulation {
    
    private File directory = null, realFile = null;
    private double fps = 0d; //Ce paramètre FPS est plus facile à comprendre pour l'humain
    private IRational framerate = null; //Ce paramètre FPS est plus facile à comprendre pour l'ordinateur
    private long duration = 0L;
    private long filesize = 0L;
    private int bitrate = 0;
    private long frames = 0L;
    private int width = 0;
    private int height = 0;
    private ICodec.ID codec = null;
    private String fileExtension = "";
    private Extract extract = null;
    private VideoInfo vi = null;
    
    public VideoEmulation(){
        vi = new VideoInfo();
        extract = vi.getExtract();
    }
    
    public VideoEmulation(File directory, File realFile){
        this.directory = directory;
        this.realFile = realFile;
        vi = new VideoInfo();
        extract = vi.getExtract();
    }
    
    public void setVideoEmulation(File directory, File realFile){
        this.directory = directory;
        this.realFile = realFile;
    }
    
    public BufferedImage getImageAt(int frame) throws IOException{
        if(directory != null){
            if(directory.exists() && frame <= frames && frame >= 0){
                BufferedImage image = ImageIO.read(new File(directory, frame+".png"));
                return image;
            }
        }
        return new BufferedImage(1280, 720, BufferedImage.TYPE_INT_ARGB);
    }
    
    public VideoInfo getVideoInfo(){
        return vi;
    }
    
    public void setVideoInfo(VideoInfo vi){
        this.vi = vi;
    }
    
    public long getFrames(){
        return frames;
    }
    
    public void setFrames(long frames){
        this.frames = frames;
    }
    
    //==========================================================================
    public void setSaveFolder(String infoFolder){
        directory = new File(infoFolder);
    }
    
    public void setVideo(String videoPath){
        realFile = new File(videoPath);
        fileExtension = videoPath.substring(videoPath.lastIndexOf("."));
        extractInfo();        
    }
    
    public void extractVideo(){        
        if(realFile.exists() == true && directory.exists() == true){
            extract.setMediaPath(realFile.getAbsolutePath());
            extract.setExtractFolder(directory.getAbsolutePath());
            extract.start();
            
            int count = -1;
            
            while(extract.isAlive()){
            /*Wait*/
                if(count!=extract.getCurrentFrame()){
                    count = extract.getCurrentFrame();
                    frames = count;
                }
            }
            
        }
    }
    
    private void extractInfo(){
        IContainer container = IContainer.make();
        container.open(realFile.getAbsolutePath(), IContainer.Type.READ, null);
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
    
}
