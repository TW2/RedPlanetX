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
import com.xuggle.xuggler.ICodec.Type;
import com.xuggle.xuggler.IContainer;
import com.xuggle.xuggler.IPixelFormat;
import com.xuggle.xuggler.IRational;
import com.xuggle.xuggler.IStream;
import com.xuggle.xuggler.video.ConverterFactory;
import com.xuggle.xuggler.video.IConverter;
import java.awt.Graphics2D;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import javax.imageio.ImageIO;

/**
 *
 * @author Yves
 */
public class Decode {
    
    public Decode(){
        IMediaReader reader = ToolFactory.makeReader("C:\\Users\\Yves\\Desktop\\001.mp4");
        reader.setBufferedImageTypeToGenerate(BufferedImage.TYPE_3BYTE_BGR);
        reader.addListener(new ImageRipperListener("C:\\Users\\Yves\\Desktop\\DOSSIER\\", reader.getContainer()));
        //reader.addListener(ToolFactory.makeWriter("C:\\Users\\Yves\\Desktop\\001.flv", reader));
        while (reader.readPacket() == null){
            
        }
    }
    
    public class ImageRipperListener extends MediaListenerAdapter {
        
        private int lastFrame = -1;
        private String path = null;
        private long countFrames = 0;
        private double fps = 0d;
        private BufferedImage source = null;
        
        public ImageRipperListener(String path, IContainer cont){
            this.path = path;
            
            System.out.println("Streams : "+cont.getNumStreams());
            
            for(int i=0; i<cont.getNumStreams(); i++){
                IStream stream = cont.getStream(i);
                
                source = new BufferedImage(
                            stream.getStreamCoder().getWidth(),
                            stream.getStreamCoder().getHeight(),
                            BufferedImage.TYPE_INT_ARGB);
                
                    countFrames = stream.getNumFrames();
                    double num = Double.parseDouble(Integer.toString(stream.getFrameRate().getNumerator()));
                    double denom = Double.parseDouble(Integer.toString(stream.getFrameRate().getDenominator()));
                    fps = num / denom;
                    
                    System.out.println("Streams : "+stream.getId());
            }
        }
        
        @Override
        public void onVideoPicture(IVideoPictureEvent event) {
            
            saveImage(event.getImage(), path);
            
//            if(event.getPicture().isComplete()){
//                IConverter converter = ConverterFactory.createConverter(event.getImage(), IPixelFormat.Type.ARGB);
//                saveImage(converter.toImage(event.getPicture()), path);
//            }
        }
        
        private void saveImage(BufferedImage image, String path){
            try{
                lastFrame += 1;
                File file = new File(path, lastFrame+".png");
                ImageIO.write(image, "png", file);
            }catch(IOException e){
                
            }
        }
    
    }

}
