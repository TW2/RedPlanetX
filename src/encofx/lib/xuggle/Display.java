/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.xuggle;

import com.xuggle.xuggler.ICodec;
import com.xuggle.xuggler.IContainer;
import com.xuggle.xuggler.IPacket;
import com.xuggle.xuggler.IPixelFormat;
import com.xuggle.xuggler.IStream;
import com.xuggle.xuggler.IStreamCoder;
import com.xuggle.xuggler.IVideoPicture;
import com.xuggle.xuggler.IVideoResampler;
import com.xuggle.xuggler.Utils;
import java.awt.Color;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.image.BufferedImage;
import javax.swing.JPanel;

/**
 * @deprecated Refonte en VideoInfo
 * @author Yves
 */
public class Display extends JPanel {
    
    private String path = "";
    private double fps = 0d;
    private long frames = 0L;
    private long milliseconds = 0L;
    private int videoStream = -1;
    
    private IStreamCoder videoCoder, audioCoder;
    private IVideoResampler resampler;
    private IContainer container;
    private IPacket packet;
    private BufferedImage image = null;
    
    public Display(String path){
        setOpaque(true);
        this.path = path;
        
        container = IContainer.make();
        container.open(path, IContainer.Type.READ, null);
        milliseconds = container.getDuration()/1000;
        for(int i=0; i<container.getNumStreams(); i++){
            IStream stream = container.getStream(i);
            if(stream.getStreamCoder().getCodecType()==ICodec.Type.CODEC_TYPE_VIDEO){
                frames = stream.getNumFrames();
                fps = stream.getFrameRate().getDouble();
                image = new BufferedImage(
                        stream.getStreamCoder().getWidth(),
                        stream.getStreamCoder().getHeight(),
                        BufferedImage.TYPE_INT_ARGB);
                videoStream = i;
                videoCoder = stream.getStreamCoder();
                
                Graphics2D g = image.createGraphics();
                g.setColor(Color.red);
                g.drawString("The component don't display anything but this text !", 100, 100);
            }
        }
        
        repaint();
    }
    
    private void setImage(BufferedImage bi){
        image = bi;
        repaint();
    }
    
    public void setImageFromFrame(int frame){
        //frame to ms
        //timastamp >> duration ms
        //frame  >> frames long
        //timestamp = duration*frame/frames
        IPacket videoPacket = IPacket.make();
        double fpm = fps*1000;
        long timestamp = milliseconds * frame / frames;
        container.seekKeyFrame(videoStream, 1000, IContainer.SEEK_FLAG_FRAME);
        
        if (videoCoder != null) {
            if (videoCoder.getPixelType() != IPixelFormat.Type.BGR24){
                resampler = IVideoResampler.make(videoCoder.getWidth(), videoCoder.getHeight(), IPixelFormat.Type.BGR24,
                videoCoder.getWidth(), videoCoder.getHeight(), videoCoder.getPixelType());
            }
        }
        
        int count = 0;
        while(container.readNextPacket(videoPacket) >= 0 && count<1){
            
            if (videoPacket.getStreamIndex() == videoStream){
                IVideoPicture picture = IVideoPicture.make(
                        videoCoder.getPixelType(),
                        videoCoder.getWidth(),
                        videoCoder.getHeight());

                System.out.println("TEMOIN 1");
                if (picture.isComplete()){
                    IVideoPicture newPic = picture;
System.out.println("TEMOIN 2");
                    if (resampler != null){
                        newPic = IVideoPicture.make(resampler.getOutputPixelFormat(), picture.getWidth(), picture.getHeight());
                        setImage(Utils.videoPictureToImage(newPic));
                        count += 1;System.out.println("TEMOIN 3");
                    }
                }
            }
            
        }
        
        
        
    }
    
    @Override
    public void paint(Graphics g){
        if (image!=null){
            g.drawImage(image, 0, 0, null);
        }else{
            g.setColor(Color.cyan);
            g.fillRect(0, 0, getWidth(), getHeight());
        }
    }
    
}
