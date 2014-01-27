/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.xuggle;

import com.xuggle.mediatool.IMediaReader;
import com.xuggle.mediatool.MediaListenerAdapter;
import com.xuggle.mediatool.ToolFactory;
import com.xuggle.mediatool.event.IVideoPictureEvent;
import com.xuggle.xuggler.ICodec;
import com.xuggle.xuggler.IStream;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import javax.imageio.ImageIO;

/**
 * Extract all frames from a video
 * (The original capture code can be viewed at :
 * http://www.javacodegeeks.com/2011/02/xuggler-tutorial-frames-capture-video.html )
 * @author TW2 (adaptation)
 */
public class Extract extends Thread implements Runnable {
    
    private String extractFolder = null;
    private String mediaPath = null;
    private int lastFrame = -1;
    private long frames = -1L;
//    private WorkFrame wf = null;
//    private ProcessViewer pv = null;
    private VideoInfo vi = null;
    
    public Extract(VideoInfo vi){
        this.vi = vi;
    }
    
    public Extract(){
        //For test only
    }
    
    public int startExtract(String mediaPath, String extractFolder){
        this.mediaPath = mediaPath;
        this.extractFolder = extractFolder;
        
        IMediaReader reader = ToolFactory.makeReader(mediaPath);
        reader.setBufferedImageTypeToGenerate(BufferedImage.TYPE_3BYTE_BGR);
        reader.addListener(new ImageRipper());

        while (reader.readPacket() == null){
            for(int i=0; i<reader.getContainer().getNumStreams();i++){
                IStream stream = reader.getContainer().getStream(i);
                if(stream.getStreamCoder().getCodecType() == ICodec.Type.CODEC_TYPE_VIDEO){
                    frames = stream.getNumFrames();
//                    if(pv != null){                        
//                        pv.setTotalFrames(frames);
//                        pv.setCurrentFrame(lastFrame); 
//                    } 
                }
            }
        }
        
//        interrupt();
//        pv.setVisible(false);
        return lastFrame;
    }
    
    private class ImageRipper extends MediaListenerAdapter {
        
        public ImageRipper(){
            
        }
        
        @Override
        public void onVideoPicture(IVideoPictureEvent event) {            
            saveImage(event.getImage());
        }
        
        private void saveImage(BufferedImage image){
            try{
                lastFrame += 1;
                File file = new File(extractFolder, lastFrame+".png");
                ImageIO.write(image, "png", file);
            }catch(IOException e){
                
            }
        }
        
    }
    
    public int getFrames(){
        return Integer.parseInt(Long.toString(frames));
    }
    
    public int getCurrentFrame(){
        return lastFrame;
    }
    
    public void setExtractFolder(String extractFolder){
        this.extractFolder = extractFolder;
    }
    
    public void setMediaPath(String mediaPath){
        this.mediaPath = mediaPath;
    }
    
    @Override
    public void run(){
        if(extractFolder != null && mediaPath != null && isInterrupted()==false){
//            pv = new ProcessViewer();
//            pv.setVisible(true);
//            pv.setAlwaysOnTop(true);
            frames = startExtract(mediaPath, extractFolder);
            vi.setFrames(frames);
            interrupt();
        }
    }
    
    public void runaway(){
        start();
    }
}
