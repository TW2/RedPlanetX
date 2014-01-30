/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.effects;

import encofx.lib.graphics.GraphicsTextFX;
import encofx.lib.ObjectCollectionObject;
import encofx.lib.SubObjects;
import encofx.lib.properties.AbstractProperty;
import encofx.lib.settings.SetupObject;
import java.awt.AlphaComposite;
import java.awt.BasicStroke;
import java.awt.Color;
import java.awt.Font;
import java.awt.FontMetrics;
import java.awt.Graphics2D;
import java.awt.Stroke;
import java.awt.geom.AffineTransform;
import java.awt.geom.Point2D;
import java.awt.image.BufferedImage;
import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;
import java.util.List;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 *
 * @author Yves
 */
public class TextCollection extends ObjectCollectionObject {
    
    List<Text> texts = new ArrayList<>();
    private Font font = new Font("Arial", Font.PLAIN, 50);
    //Objects for the table of properties and settings
    List<AbstractProperty> properties = new ArrayList<>();
    
    private String notStrippedSentence = "";
    private int syllableIndex = -1;
    
    public TextCollection(){
        properties.add(propChild);
        properties.add(propFontname);
        properties.add(propFontstyle);
        properties.add(propString);
        properties.add(propAnchorX);
        properties.add(propAnchorY);
        properties.add(propAnchorPosition);
        properties.add(propUnderline);
        properties.add(propStrikeOut);
        properties.add(propGradientType);
    }

    public void setNotStrippedSentence(String s){
        notStrippedSentence = s;
    }
    
    public String getNotStrippedSentence(){
        return notStrippedSentence;
    }
    
    public void setSyllableIndex(int index){
        syllableIndex = index;
    }
    
    public int getSyllableIndex(){
        return syllableIndex;
    }
    
    @Override
    public SubObjects getSubObjects() {
        SubObjects<Text> so = new SubObjects();
        so.addAllObjects(texts);
        return so;
    }

    @Override
    public void setSubObjects(SubObjects subs) {
        texts = subs.getObjects();
    }
    
    public void sortByFrames(){
        Collections.sort(texts, new Comparator<Text>() {
            @Override
            public int compare(Text o1, Text o2) {
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
        Collections.sort(texts, new Comparator<Text>() {
            @Override
            public int compare(Text o1, Text o2) {
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
    public Type getType() {
        return Type.Text;
    }
    
    public Text getBefore(int frame){
        Text before = null;
        for(Text tx : texts){
            if(tx.getFrame()<=frame){
                before = tx;
            }
        }
        return before;
    }
    
    public Text getAfter(int frame){
        Text after = null;
        sortByFrames_Reverse();
        for(Text tx : texts){
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
    
    public void setList(List<Text> texts){
        this.texts = texts;
    }
    
    public List<Text> getList(){
        return texts;
    }
    
    public void add(Text obj){
        texts.add(obj);
    }
    
    public void remove(Text obj){
        texts.remove(obj);
    }
    
    public void clear(){
        texts.clear();
    }
    
    public void setFont(Font font){
        this.font = font;
    }
    
    public Font getFont(){
        return font;
    }
    
    public BufferedImage getFX_(int frame, int imageWidth, int imageHeight, boolean encoding) {
        BufferedImage image = new BufferedImage(imageWidth, imageHeight, BufferedImage.TYPE_INT_ARGB);
        
        Text before = getBefore(frame);
        Text after = getAfter(frame);
        
        Color c = Text.getActualColor(before, after, frame);
        
        float size = Text.getActualSize(before, after, frame);        
        
        SetupObject<Float> soAX = (SetupObject)propAnchorX.getObject();
        SetupObject<Float> soAY = (SetupObject)propAnchorY.getObject();
        Point2D anchor = new Point2D.Float(soAX.get(), soAY.get());
        float x = Text.getActualX(before, after, anchor, frame);
        float y = Text.getActualY(before, after, anchor, frame);
        
        float transparency = Text.getActualTransparency(before, after, frame);        
        float scale_x = Text.getActualScaleX(before, after, frame);
        float scale_y = Text.getActualScaleY(before, after, frame);
        
        Graphics2D g = image.createGraphics();
        
        if(c!=null){
            //Fontname
            SetupObject<String> soFontname = (SetupObject)propFontname.getObject();
            String settingsfontname = soFontname.get();
            Font saveFont = g.getFont();            
            //Fontstyle
            SetupObject<FontStyle> soFontstyle = (SetupObject)propFontstyle.getObject();
            FontStyle settingsfontstyle = soFontstyle.get();
            //Text
            SetupObject<String> soString = (SetupObject)propString.getObject();
            String settingsstring = soString.get();
            //Anchor
//            SetupObject<Float> soAX = (SetupObject)propAnchorX.getObject();
//            SetupObject<Float> soAY = (SetupObject)propAnchorY.getObject();
//            Point2D anchor = new Point2D.Float(soAX.get(), soAY.get());
            //AnchorPosition
            SetupObject<AnchorPosition> soAP = (SetupObject)propAnchorPosition.getObject();
            AnchorPosition anchorposition = soAP.get();
            //Underline
            SetupObject<Boolean> soUnderline = (SetupObject)propUnderline.getObject();
            boolean settingsunderline = soUnderline.get();
            //Underline
            SetupObject<Boolean> soStrikeOut = (SetupObject)propStrikeOut.getObject();
            boolean settingsstrikeout = soStrikeOut.get();
            
            g.setFont(new Font(settingsfontname, saveFont.getStyle(), saveFont.getSize()));
            g.setFont(g.getFont().deriveFont(settingsfontstyle.getStyle()));
            g.setFont(g.getFont().deriveFont(size));
            g.setColor(c);
            
            g.setComposite(makeComposite(transparency));
            
            //Scale X + Y
            AffineTransform at = new AffineTransform();
            at.setToScale(scale_x/100, scale_y/100);
            g.setTransform(at);
            
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
            
            
            g.drawString(settingsstring, x, y);
            
            //Underline
            if(settingsunderline == true){
                Stroke stroke = g.getStroke();
                g.setStroke(new BasicStroke(2f));
                g.drawLine(Math.round(x), Math.round(y)+10, Math.round(x)+w, Math.round(y)+10);
                g.setStroke(stroke);
            }
            
            //StrikeOut
            if(settingsstrikeout == true){
                Stroke stroke = g.getStroke();
                g.setStroke(new BasicStroke(2f));
                g.drawLine(Math.round(x), Math.round(y)-h/2, Math.round(x)+w, Math.round(y)-h/2);
                g.setStroke(stroke);
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
    
    public BufferedImage getFX(int frame, int imageWidth, int imageHeight, boolean encoding) {
        GraphicsTextFX graFX = new GraphicsTextFX(imageWidth, imageHeight);
        
        boolean isRelative = true;
        
        Text before = getBefore(frame);
        Text after = getAfter(frame);
        
        if(before==null | after == null){
            return graFX.getBlankImage();
        }else if(before!=null && after!=null){
            if(before.getFrame()==after.getFrame()){
                return graFX.getBlankImage();
            }            
        }
        
        //Propiétés dynamiques        
        //Couleur
        Color c = Text.getActualColor(before, after, frame);
        //Taille de la fonte
        float size = Text.getActualSize(before, after, frame);
        //Position des ancres X et Y
        SetupObject<Float> soAX = (SetupObject)propAnchorX.getObject();
        SetupObject<Float> soAY = (SetupObject)propAnchorY.getObject();
        Point2D anchor = new Point2D.Float(soAX.get(), soAY.get());
        float x = Text.getActualX(before, after, anchor, frame);
        float y = Text.getActualY(before, after, anchor, frame);
        //Transparence
        float transparency = Text.getActualTransparency(before, after, frame);
        //Echelle X et Y
        float scale_x = Text.getActualScaleX(before, after, frame);
        float scale_y = Text.getActualScaleY(before, after, frame);
        //Angle
        float angle = Text.getActualAngle(before, after, frame);
        //Dégradé (deux cotés)
        Color[] twosidesgradient = Text.getActualGradientColor(before, after, frame);
        //Dégradé (quatre cotés)
        Color[] foursidesgradient = Text.getActualFourSidesGradientColor(before, after, frame);

        //Propiétés statiques
        //Nom de la fonte
        SetupObject<String> soFontname = (SetupObject)propFontname.getObject();
        String fontname = soFontname.get();
        //Style de la fonte
        SetupObject<FontStyle> soFontstyle = (SetupObject)propFontstyle.getObject();
        FontStyle fontstyle = soFontstyle.get();
        //Chaine de caractère
        SetupObject<String> soString = (SetupObject)propString.getObject();
        String string = soString.get();
        //Position prédéfinit pour les ancres
        SetupObject<AnchorPosition> soAP = (SetupObject)propAnchorPosition.getObject();
        AnchorPosition anchorposition = soAP.get();
        //Souligné
        SetupObject<Boolean> soUnderline = (SetupObject)propUnderline.getObject();
        boolean underline = soUnderline.get();
        //Barré
        SetupObject<Boolean> soStrikeOut = (SetupObject)propStrikeOut.getObject();
        boolean strikeout = soStrikeOut.get();
        //Type par défaut du  rendu
        SetupObject<GradientType> soGradientType = (SetupObject)propGradientType.getObject();
        GradientType gradienttype = soGradientType.get();
        
        if(propChild!=null){
            SetupObject<ParentCollection> soChild = (SetupObject<ParentCollection>)propChild.getObject();
            if(soChild.get()!=null){
                
                Parent parent_before = soChild.get().getBefore(frame);
                Parent parent_after = soChild.get().getAfter(frame);
                
                if(isRelative==true && before!=null && after!=null){
                    try {
                        parent_before = (Parent)soChild.get().getBefore(frame).clone();
                        parent_after = (Parent)soChild.get().getAfter(frame).clone();
                        parent_before.setFrame(before.getFrame());
                        parent_after.setFrame(after.getFrame());                        
                    } catch (CloneNotSupportedException ex) {
                        Logger.getLogger(TextCollection.class.getName()).log(Level.SEVERE, null, ex);
                        parent_before = soChild.get().getBefore(frame);
                        parent_after = soChild.get().getAfter(frame);
                    }                    
                }
                

                //Propiétés dynamiques        
                //Couleur
                if(soChild.get().getColorUsage()){
                    c = Parent.getActualColor(parent_before, parent_after, frame);
                    gradienttype = GradientType.None;
                }
                //Taille de la fonte
                if(soChild.get().getFontsizeUsage()){
                    size = Parent.getActualSize(parent_before, parent_after, frame);
                }
                //Position des ancres X et Y
                Point2D anchorXY = soChild.get().getAnchor();
                if(soChild.get().getXUsage()){
                    x = Parent.getActualX(parent_before, parent_after, anchorXY, frame);
                }
                if(soChild.get().getYUsage()){
                    y = Parent.getActualY(parent_before, parent_after, anchorXY, frame);
                }
                //Transparence
                if(soChild.get().getTransparencyUsage()){
                    transparency = Parent.getActualTransparency(parent_before, parent_after, frame);
                }
                //Echelle X et Y
                if(soChild.get().getScaleXUsage()){
                    scale_x = Parent.getActualScaleX(parent_before, parent_after, frame);
                }
                if(soChild.get().getScaleYUsage()){
                    scale_y = Parent.getActualScaleY(parent_before, parent_after, frame);
                }
                //Angle
                if(soChild.get().getAngleUsage()){
                    angle = Parent.getActualAngle(parent_before, parent_after, frame);
                }
                //Dégradé (deux cotés)
                if(soChild.get().getGradientUsage()){
                    twosidesgradient = Parent.getActualGradientColor(parent_before, parent_after, frame);
                    gradienttype = GradientType.TwoSides;
                }
                //Dégradé (quatre cotés)
                if(soChild.get().getFourSidesUsage()){
                    foursidesgradient = Parent.getActualFourSidesGradientColor(parent_before, parent_after, frame);
                    gradienttype = GradientType.FourSides;
                }

                //Propiétés statiques
                //Nom de la fonte
                if(soChild.get().getFontnameUsage()){
                    fontname = soChild.get().getFontname();
                }
                //Style de la fonte
                if(soChild.get().getFontstyleUsage()){
                    fontstyle = soChild.get().getFontstyle();
                }
                //Chaine de caractère
                //String string = soChild.get().getText();
                //Position prédéfinit pour les ancres
                if(soChild.get().getPositionUsage()){
                    anchorposition = soChild.get().getAnchorPosition();
                }
                //Souligné
                if(soChild.get().getUnderlineUsage()){
                    underline = soChild.get().getUnderline();
                }
                //Barré
                if(soChild.get().getStrikeOutUsage()){
                    strikeout = soChild.get().getStrikeOut();
                }
            }
            
        }
        //Règles
        graFX.setRendering(encoding==true ? GraphicsTextFX.Rendering.Encoding : GraphicsTextFX.Rendering.Drawing);
        graFX.setDirection(GraphicsTextFX.Direction.Horizontal);

        //Assignations
        graFX.setColor(c);
        graFX.setFontname(fontname);
        graFX.setFontstyle(fontstyle.getStyle());
        graFX.setFontsize(size);
        graFX.setX(x);
        graFX.setY(y);
        graFX.setAnchorPosition(anchorposition);
        graFX.setAnchorSelection(anchorIsSelected);
        graFX.setString(string);
        graFX.setTransparency(transparency);
        graFX.setScaleX(scale_x);
        graFX.setScaleY(scale_y);
        graFX.setUnderline(underline);
        graFX.setStrikeOut(strikeout);
        graFX.setAngle(angle);
        graFX.setGradientType(gradienttype);
        graFX.setColorsForGradientPaint(twosidesgradient);
        graFX.setColorsForFourSidesGradientPaint(foursidesgradient);
        
        return graFX.getImage();
    }
    
    @Override
    public String toString(){
        return getText();
    }
    
    public List<AbstractProperty> getProperties(){
        return properties;
    }
    
    // Gestion de la transparence
    private AlphaComposite makeComposite(float alpha) {
        int type = AlphaComposite.SRC_OVER;
        return(AlphaComposite.getInstance(type, alpha));
    }
}
