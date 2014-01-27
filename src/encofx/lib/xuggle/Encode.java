/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.xuggle;

import com.xuggle.mediatool.IMediaReader;
import com.xuggle.mediatool.IMediaViewer;
import com.xuggle.mediatool.IMediaWriter;
import com.xuggle.mediatool.MediaToolAdapter;
import com.xuggle.mediatool.ToolFactory;
import com.xuggle.mediatool.event.IVideoPictureEvent;
import com.xuggle.mediatool.event.VideoPictureEvent;
import com.xuggle.xuggler.IVideoPicture;
import com.xuggle.xuggler.video.BgrConverter;
import encofx.lib.VTD2;
import java.awt.Graphics2D;
import java.awt.image.BufferedImage;

/**
 * Encodeur
 * Voir : http://www.jochus.be/site/2010-10-12/java/converting-resizing-videos-in-java-xuggler
 * @author Yves
 */
public class Encode {
    
    VTD2 vtd = null;
    VideoInfo videoInfo = null;
    private int lastFrame = -1;
//    private ViewerThread vtENCODING = null;
    
    public Encode(VTD2 vtd, VideoInfo videoInfo){
        this.vtd = vtd;
        this.videoInfo = videoInfo;        
    }
    
    public void startEncode(String output) {
        lastFrame = -1;
        
        //Lecture
        ImageLayers il = new ImageLayers();
        IMediaReader reader = ToolFactory.makeReader(videoInfo.getVideoPath());
        reader.setBufferedImageTypeToGenerate(BufferedImage.TYPE_3BYTE_BGR);
        reader.addListener(il);
        
        //Ecriture
        IMediaWriter writer = ToolFactory.makeWriter(output, reader);
        il.addListener(writer);
        
        //Visualisation
        IMediaViewer viewer = ToolFactory.makeViewer();
        viewer.willShowStatsWindow();
        writer.addListener(viewer);
        
        //Encodage
        while (reader.readPacket() == null) {
            //encode
        }
    }
    
    private class ImageLayers extends MediaToolAdapter {        
        
        public ImageLayers(){
            
        }
        
        @Override
        public void onVideoPicture(IVideoPictureEvent event) {
            //On compte chaque image (la source est une copie conforme de la sortie)
            lastFrame += 1;
            
            //On récupère l'image source
            BufferedImage sourceImage = event.getImage();            
            
            //On ajoute les images/couches
            Graphics2D g = sourceImage.createGraphics();
            vtd.draw(g, lastFrame);
            
            //Conversion
            int w = videoInfo.getVideoWidth();
            int h = videoInfo.getVideoHeight();
            BgrConverter converter = new BgrConverter(event.getPicture().getPixelType(), w, h, w, h);
            IVideoPicture newVideo = converter.toPicture(sourceImage, event.getTimeStamp());
            
            
            IVideoPictureEvent vpe = new VideoPictureEvent(event.getSource(), newVideo, event.getStreamIndex());
            super.onVideoPicture(vpe);
        }
        
    }
    
    private class Hack extends MediaToolAdapter implements Runnable {
        
        ProcessViewer pvw = new ProcessViewer();
        VideoInfo vi = null;
        Thread myHack = new Thread();
        
        public Hack(VideoInfo vi){
            this.vi = vi;
            myHack.start();
        }
        
        @Override
        public void onVideoPicture(IVideoPictureEvent event) {
            //Ceci ne fait pas partie du hack
            super.onVideoPicture(event);
        }

        @Override
        public void run() {
            pvw.setCurrentFrame(lastFrame);

            if(lastFrame+1 == vi.getFrames()){
                myHack.interrupt();
                pvw.dispose();
            }
        }
        
    }
    
//    public class ViewerThread extends Thread implements Runnable {
//        
//        ProcessViewer pvw = new ProcessViewer();
//        VideoInfo vi = null;
//        
//        public ViewerThread(VideoInfo vi){
//            this.vi = vi;
//        }
//        
//        @Override
//        public void run() {
//            pvw.setCurrentFrame(lastFrame);
//
//            if(lastFrame+1 == vi.getFrames()){
//                interrupt();
//                pvw.dispose();
//            }
//        }
//        
//    }
    
}
