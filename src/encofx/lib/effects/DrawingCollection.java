/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.effects;

import encofx.lib.ObjectCollectionInterface;
import encofx.lib.ObjectCollectionObject;
import encofx.lib.SubObjects;
import encofx.lib.graphics.GraphicsTextFX;
import encofx.lib.properties.AbstractProperty;
import encofx.lib.settings.SetupObject;
import java.awt.Font;
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
public class DrawingCollection extends ObjectCollectionObject {
    
    List<Drawing> texts = new ArrayList<>();
    private Font font = new Font("Arial", Font.PLAIN, 50);
    //Objects for the table of properties and settings
    List<AbstractProperty> properties = new ArrayList<>();
    
    private String notStrippedSentence = "";
    private int syllableIndex = -1;
    
    public DrawingCollection(){
        properties.add(propChild);
        properties.add(propString);
        properties.add(propAnchorX);
        properties.add(propAnchorY);
        properties.add(propAnchorPosition);
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
        SubObjects<Drawing> so = new SubObjects();
        so.addAllObjects(texts);
        return so;
    }

    @Override
    public void setSubObjects(SubObjects subs) {
        texts = subs.getObjects();
    }
    
    public void sortByFrames(){
        Collections.sort(texts, new Comparator<Drawing>() {
            @Override
            public int compare(Drawing o1, Drawing o2) {
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
        Collections.sort(texts, new Comparator<Drawing>() {
            @Override
            public int compare(Drawing o1, Drawing o2) {
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
    
    public Drawing getBefore(int frame){
        Drawing before = null;
        for(Drawing tx : texts){
            if(tx.getFrame()<=frame){
                before = tx;
            }
        }
        return before;
    }
    
    public Drawing getAfter(int frame){
        Drawing after = null;
        sortByFrames_Reverse();
        for(Drawing tx : texts){
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
    
    public void setList(List<Drawing> texts){
        this.texts = texts;
    }
    
    public List<Drawing> getList(){
        return texts;
    }
    
    public void add(Drawing obj){
        texts.add(obj);
    }
    
    public void remove(Drawing obj){
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
    
    public BufferedImage getFX(int frame, int imageWidth, int imageHeight, boolean encoding) {
        GraphicsTextFX graFX = new GraphicsTextFX(imageWidth, imageHeight);
        
        boolean isRelative = true;
        Object scriptObject = null;
        
        Drawing before = getBefore(frame);
        Drawing after = getAfter(frame);
        
        if(before==null | after == null){
            return graFX.getBlankImage();
        }else if(before!=null && after!=null){
//            if(before.getFrame()==after.getFrame()){
//                return graFX.getBlankImage();
//            }            
        }
        
        //Propiétés dynamiques        
        //Couleur
        //Color c = Drawing.getActualColor(before, after, frame);
        //Taille de la fonte
        //float size = Drawing.getActualSize(before, after, frame);
        //Position des ancres X et Y
        SetupObject<Float> soAX = (SetupObject)propAnchorX.getObject();
        SetupObject<Float> soAY = (SetupObject)propAnchorY.getObject();
        Point2D anchor = new Point2D.Float(soAX.get(), soAY.get());
        float x = Drawing.getActualX(before, after, anchor, frame);
        float y = Drawing.getActualY(before, after, anchor, frame);
        //Transparence
        float transparency = Drawing.getActualTransparency(before, after, frame);
        //Echelle X et Y
        float scale_x = Drawing.getActualScaleX(before, after, frame);
        float scale_y = Drawing.getActualScaleY(before, after, frame);
        //Angle
        float angle = Drawing.getActualAngle(before, after, frame);
        //Dégradé (deux cotés)
        //Color[] twosidesgradient = Drawing.getActualGradientColor(before, after, frame);
        //Dégradé (quatre cotés)
        //Color[] foursidesgradient = Drawing.getActualFourSidesGradientColor(before, after, frame);
        //Image
        BufferedImage paintedImage = Drawing.getActualImage(before, frame);
        

        //Propiétés statiques
        //Nom de la fonte
        //SetupObject<String> soFontname = (SetupObject)propFontname.getObject();
        //String fontname = soFontname.get();
        //Style de la fonte
        //SetupObject<ObjectCollectionObject.FontStyle> soFontstyle = (SetupObject)propFontstyle.getObject();
        //ObjectCollectionObject.FontStyle fontstyle = soFontstyle.get();
        //Chaine de caractère
        SetupObject<String> soString = (SetupObject)propString.getObject();
        String string = soString.get();
        //Position prédéfinit pour les ancres
        SetupObject<ObjectCollectionObject.AnchorPosition> soAP = (SetupObject)propAnchorPosition.getObject();
        ObjectCollectionObject.AnchorPosition anchorposition = soAP.get();
        //Souligné
        //SetupObject<Boolean> soUnderline = (SetupObject)propUnderline.getObject();
        //boolean underline = soUnderline.get();
        //Barré
        //SetupObject<Boolean> soStrikeOut = (SetupObject)propStrikeOut.getObject();
        //boolean strikeout = soStrikeOut.get();
        //Type par défaut du  rendu
        //SetupObject<ObjectCollectionObject.GradientType> soGradientType = (SetupObject)propGradientType.getObject();
        //ObjectCollectionObject.GradientType gradienttype = soGradientType.get();
        
        if(propChild!=null){
            SetupObject<ParentCollection> soChild = (SetupObject<ParentCollection>)propChild.getObject();
            if(soChild.get()!=null){
                
                scriptObject = soChild.get().getScript();
                
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
                //if(soChild.get().getColorUsage()){
                //    c = Parent.getActualColor(parent_before, parent_after, frame);
                //    gradienttype = ObjectCollectionObject.GradientType.None;
                //}
                //Taille de la fonte
                //if(soChild.get().getFontsizeUsage()){
                //    size = Parent.getActualSize(parent_before, parent_after, frame);
                //}
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
                //if(soChild.get().getGradientUsage()){
                //    twosidesgradient = Parent.getActualGradientColor(parent_before, parent_after, frame);
                //    gradienttype = ObjectCollectionObject.GradientType.TwoSides;
                //}
                //Dégradé (quatre cotés)
                //if(soChild.get().getFourSidesUsage()){
                //    foursidesgradient = Parent.getActualFourSidesGradientColor(parent_before, parent_after, frame);
                //    gradienttype = ObjectCollectionObject.GradientType.FourSides;
                //}

                //Propiétés statiques
                //Nom de la fonte
                //if(soChild.get().getFontnameUsage()){
                //    fontname = soChild.get().getFontname();
                //}
                //Style de la fonte
                //if(soChild.get().getFontstyleUsage()){
                //    fontstyle = soChild.get().getFontstyle();
                //}
                //Chaine de caractère
                //String string = soChild.get().getText();
                //Position prédéfinit pour les ancres
                if(soChild.get().getPositionUsage()){
                    anchorposition = soChild.get().getAnchorPosition();
                }
                //Souligné
                //if(soChild.get().getUnderlineUsage()){
                //    underline = soChild.get().getUnderline();
                //}
                //Barré
                //if(soChild.get().getStrikeOutUsage()){
                //    strikeout = soChild.get().getStrikeOut();
                //}
            }
            
        }
        //Règles
        graFX.setRendering(encoding==true ? GraphicsTextFX.Rendering.Encoding : GraphicsTextFX.Rendering.Drawing);
        graFX.setDirection(GraphicsTextFX.Direction.Horizontal);

        //Assignations
        //graFX.setColor(c);
        //graFX.setFontname(fontname);
        //graFX.setFontstyle(fontstyle.getStyle());
        //graFX.setFontsize(size);
        graFX.setX(x);
        graFX.setY(y);
        graFX.setAnchorPosition(anchorposition);
        graFX.setAnchorSelection(anchorIsSelected);
        graFX.setString(string);
        graFX.setTransparency(transparency);
        graFX.setScaleX(scale_x);
        graFX.setScaleY(scale_y);
        //graFX.setUnderline(underline);
        //graFX.setStrikeOut(strikeout);
        graFX.setAngle(angle);
        //graFX.setGradientType(gradienttype);
        //graFX.setColorsForGradientPaint(twosidesgradient);
        //graFX.setColorsForFourSidesGradientPaint(foursidesgradient);
        graFX.setImageOnFrames(paintedImage);
        
        if(scriptObject!=null){
            return graFX.getImageFromScript(scriptObject, frame);
        }
        
        return graFX.getImageFromDrawing();
    }
    
    @Override
    public String toString(){
        return getText();
    }
    
    public List<AbstractProperty> getProperties(){
        return properties;
    }
}