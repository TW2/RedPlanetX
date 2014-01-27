/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.effects;

import encofx.lib.ObjectCollectionInterface;
import encofx.lib.ObjectCollectionObject;
import encofx.lib.SubObjects;
import encofx.lib.properties.AbstractProperty;
import encofx.lib.settings.SetupObject;
import java.awt.Color;
import java.awt.Font;
import java.awt.FontMetrics;
import java.awt.Graphics2D;
import java.awt.geom.Point2D;
import java.awt.image.BufferedImage;
import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;
import java.util.List;

/**
 *
 * @author Yves
 */
public class VTextCollection extends ObjectCollectionObject {
    
    List<VText> vtexts = new ArrayList<>();
    private Font font = new Font("Arial", Font.PLAIN, 50);
    //Objects for the table of properties and settings
    List<AbstractProperty> properties = new ArrayList<>();
    
    public VTextCollection(){
        properties.add(propFontname);
        properties.add(propFontstyle);
        properties.add(propString);
        properties.add(propAnchorX);
        properties.add(propAnchorY);
        properties.add(propAnchorPosition);
    }

    @Override
    public SubObjects getSubObjects() {
        SubObjects<VText> so = new SubObjects();
        so.addAllObjects(vtexts);
        return so;
    }

    @Override
    public void setSubObjects(SubObjects subs) {
        vtexts = subs.getObjects();
    }
    
    public void sortByFrames(){
        Collections.sort(vtexts, new Comparator<VText>() {
            @Override
            public int compare(VText o1, VText o2) {
                if(o1.getFrame()==o2.getFrame()){
                    return 0;
                }else if(o1.getFrame()>o2.getFrame()){
                    return 1;
                }else{
                    return -1;
                }
            }
        });
    }
    
    public void sortByFrames_Reverse(){
        Collections.sort(vtexts, new Comparator<VText>() {
            @Override
            public int compare(VText o1, VText o2) {
                if(o1.getFrame()==o2.getFrame()){
                    return 0;
                }else if(o1.getFrame()>o2.getFrame()){
                    return -1;
                }else{
                    return 1;
                }
            }
        });
    }

    @Override
    public ObjectCollectionInterface.Type getType() {
        return ObjectCollectionInterface.Type.Text;
    }
    
    public VText getBefore(int frame){
        VText before = null;
        for(VText tx : vtexts){
            if(tx.getFrame()<=frame){
                before = tx;
            }
        }
        return before;
    }
    
    public VText getAfter(int frame){
        VText after = null;
        sortByFrames_Reverse();
        for(VText tx : vtexts){
            if(tx.getFrame()>=frame){
                after = tx;
            }
        }
        sortByFrames();
        return after;
    }
    
//    public Text getAfter(int frame){
//        Text after = null;
//        boolean afterHasBeenFound = false;
//        for(Text tx : texts){
//            if(tx.getFrame()>=frame && afterHasBeenFound==false){
//                after = tx; afterHasBeenFound = true;
//            }
//        }
//        return after;
//    }
    
    public void setList(List<VText> vtexts){
        this.vtexts = vtexts;
    }
    
    public List<VText> getList(){
        return vtexts;
    }
    
    public void add(VText obj){
        vtexts.add(obj);
    }
    
    public void remove(VText obj){
        vtexts.remove(obj);
    }
    
    public void clear(){
        vtexts.clear();
    }
    
    public void setFont(Font font){
        this.font = font;
    }
    
    public Font getFont(){
        return font;
    }
    
    public BufferedImage getFX(int frame, int imageWidth, int imageHeight, boolean encoding) {
        BufferedImage image = new BufferedImage(imageWidth, imageHeight, BufferedImage.TYPE_INT_ARGB);
        
        VText before = getBefore(frame);
        VText after = getAfter(frame);
        
        Color c = VText.getActualColor(before, after, frame);
        
        float size = VText.getActualSize(before, after, frame);        
        
        SetupObject<Float> soAX = (SetupObject)propAnchorX.getObject();
        SetupObject<Float> soAY = (SetupObject)propAnchorY.getObject();
        Point2D anchor = new Point2D.Float(soAX.get(), soAY.get());
        float x = VText.getActualX(before, after, anchor, frame);
        float y = VText.getActualY(before, after, anchor, frame);
        
        Graphics2D g = image.createGraphics();
        
        if(c!=null){
            //Fontname
            SetupObject<String> soFontname = (SetupObject)propFontname.getObject();
            String settingsfontname = soFontname.get();
            Font saveFont = g.getFont();            
            //Fontstyle
            SetupObject<ObjectCollectionObject.FontStyle> soFontstyle = (SetupObject)propFontstyle.getObject();
            ObjectCollectionObject.FontStyle settingsfontstyle = soFontstyle.get();
            //Text
            SetupObject<String> soString = (SetupObject)propString.getObject();
            String settingsstring = soString.get();
            //Anchor ---
            //AnchorPosition
            SetupObject<ObjectCollectionObject.AnchorPosition> soAP = (SetupObject)propAnchorPosition.getObject();
            ObjectCollectionObject.AnchorPosition anchorposition = soAP.get();
            
            g.setFont(new Font(settingsfontname, saveFont.getStyle(), saveFont.getSize()));
            g.setFont(g.getFont().deriveFont(settingsfontstyle.getStyle()));
            g.setFont(g.getFont().deriveFont(size));
            g.setColor(c);
            
            FontMetrics metrics = g.getFontMetrics(g.getFont());
            int w = metrics.stringWidth(settingsstring);
            int h = metrics.getHeight();
            switch(anchorposition){
                case CornerLeftBottom:
                    //Do nothing x and y
                    break;
                case Bottom:
                    x = x - w/2; //Do nothing y
                    break;
                case CornerRightBottom:
                    x = x - w; //Do nothing y
                    break;
                case Right:
                    x = x - w; y = y + h/2;
                    break;
                case CornerRightTop:
                    x = x - w; y = y + h;
                    break;
                case Top:
                    x = x - w/2; y = y + h;
                    break;
                case CornerLeftTop:
                    y = y + h; // Do nothing x
                    break;
                case Left: 
                    y = y + h/2; // Do nothing x
                    break;
                case Middle:
                    x = x - w/2; y = y + h/2;
                    break;
            }
            
            int py = 0;
            for(char ch : settingsstring.toCharArray()){
                g.drawString(Character.toString(ch), x, py+y);
                py += h;
            }            
            
            //On cache l'ancre à l'encodage mais on ne la cache pas pour l'édition.
            if(encoding==false){
                g.setColor(Color.cyan);
                g.drawRect(
                        Math.round(Float.parseFloat(Double.toString(anchor.getX())))-5,
                        Math.round(Float.parseFloat(Double.toString(anchor.getY())))-5,
                        10,
                        10);

                if(isAnchorSelected()){
                    g.setColor(Color.magenta);
                    g.fillRect(
                            Math.round(Float.parseFloat(Double.toString(anchor.getX())))-5,
                            Math.round(Float.parseFloat(Double.toString(anchor.getY())))-5,
                            10,
                            10);
                }
            }
            
        }
        
        
        return image;
    }
    
    @Override
    public String toString(){
        return getText();
    }
    
    public List<AbstractProperty> getProperties(){
        return properties;
    }
    
}
