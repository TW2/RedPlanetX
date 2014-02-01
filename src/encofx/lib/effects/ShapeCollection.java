/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.effects;

import encofx.lib.ObjectCollectionObject;
import encofx.lib.SubObjects;
import encofx.lib.graphics.GraphicsTextFX;
import encofx.lib.properties.AbstractProperty;
import encofx.lib.settings.SetupObject;
import java.awt.AlphaComposite;
import java.awt.Color;
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
public class ShapeCollection extends ObjectCollectionObject {
    
    List<Shape> texts = new ArrayList<>();
    private Font font = new Font("Arial", Font.PLAIN, 50);
    //Objects for the table of properties and settings
    List<AbstractProperty> properties = new ArrayList<>();
    
    private String notStrippedSentence = "";
    private int syllableIndex = -1;
    
    public ShapeCollection(){
        properties.add(propChild);
        properties.add(propString);
        properties.add(propAnchorX);
        properties.add(propAnchorY);
        properties.add(propAnchorPosition);
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
        SubObjects<Shape> so = new SubObjects();
        so.addAllObjects(texts);
        return so;
    }

    @Override
    public void setSubObjects(SubObjects subs) {
        texts = subs.getObjects();
    }
    
    public void sortByFrames(){
        Collections.sort(texts, new Comparator<Shape>() {
            @Override
            public int compare(Shape o1, Shape o2) {
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
        Collections.sort(texts, new Comparator<Shape>() {
            @Override
            public int compare(Shape o1, Shape o2) {
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
        return Type.Shape;
    }
    
    public Shape getBefore(int frame){
        Shape before = null;
        for(Shape tx : texts){
            if(tx.getFrame()<=frame){
                before = tx;
            }
        }
        return before;
    }
    
    public Shape getAfter(int frame){
        Shape after = null;
        sortByFrames_Reverse();
        for(Shape tx : texts){
            if(tx.getFrame()>=frame){
                after = tx;
            }
        }
        sortByFrames();
        return after;
    }
    
//    public Shape getAfter(int frame){
//        Shape after = null;
//        boolean afterHasBeenFound = false;
//        for(Shape tx : texts){
//            if(tx.getFrame()>=frame && afterHasBeenFound==false){
//                after = tx; afterHasBeenFound = true;
//            }
//        }
//        return after;
//    }
    
    public void setList(List<Shape> texts){
        this.texts = texts;
    }
    
    public List<Shape> getList(){
        return texts;
    }
    
    public void add(Shape obj){
        texts.add(obj);
    }
    
    public void remove(Shape obj){
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
        
        Shape before = getBefore(frame);
        Shape after = getAfter(frame);
        
        if(before==null | after == null){
            return graFX.getBlankImage();
        }else if(before!=null && after!=null){
            if(before.getFrame()==after.getFrame()){
                return graFX.getBlankImage();
            }            
        }
        
        //Propiétés dynamiques        
        //Couleur
        Color c = Shape.getActualColor(before, after, frame);
        //Taille de la fonte
        //float size = Shape.getActualSize(before, after, frame);
        //Position des ancres X et Y
        SetupObject<Float> soAX = (SetupObject)propAnchorX.getObject();
        SetupObject<Float> soAY = (SetupObject)propAnchorY.getObject();
        Point2D anchor = new Point2D.Float(soAX.get(), soAY.get());
        float x = Shape.getActualX(before, after, anchor, frame);
        float y = Shape.getActualY(before, after, anchor, frame);
        //Transparence
        float transparency = Shape.getActualTransparency(before, after, frame);
        //Echelle X et Y
        float scale_x = Shape.getActualScaleX(before, after, frame);
        float scale_y = Shape.getActualScaleY(before, after, frame);
        //Angle
        float angle = Shape.getActualAngle(before, after, frame);
        //Dégradé (deux cotés)
        Color[] twosidesgradient = Shape.getActualGradientColor(before, after, frame);
        //Dégradé (quatre cotés)
        Color[] foursidesgradient = Shape.getActualFourSidesGradientColor(before, after, frame);

        //Propiétés statiques
        //Nom de la fonte
        //SetupObject<String> soFontname = (SetupObject)propFontname.getObject();
        //String fontname = soFontname.get();
        //Style de la fonte
        //SetupObject<FontStyle> soFontstyle = (SetupObject)propFontstyle.getObject();
        //FontStyle fontstyle = soFontstyle.get();
        //Chaine de caractère
        SetupObject<String> soString = (SetupObject)propString.getObject();
        String string = soString.get();
        //Position prédéfinit pour les ancres
        SetupObject<AnchorPosition> soAP = (SetupObject)propAnchorPosition.getObject();
        AnchorPosition anchorposition = soAP.get();
        //Souligné
        //SetupObject<Boolean> soUnderline = (SetupObject)propUnderline.getObject();
        //boolean underline = soUnderline.get();
        //Barré
        //SetupObject<Boolean> soStrikeOut = (SetupObject)propStrikeOut.getObject();
        //boolean strikeout = soStrikeOut.get();
        //Type par défaut du  rendu
        SetupObject<GradientType> soGradientType = (SetupObject)propGradientType.getObject();
        GradientType gradienttype = soGradientType.get();
        
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
                        Logger.getLogger(ShapeCollection.class.getName()).log(Level.SEVERE, null, ex);
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
                //if(soChild.get().getFontnameUsage()){
                //    fontname = soChild.get().getFontname();
                //}
                //Style de la fonte
                //if(soChild.get().getFontstyleUsage()){
                //    fontstyle = soChild.get().getFontstyle();
                //}
                //Chaine de caractère
                //String string = soChild.get().getShape();
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
        graFX.setColor(c);
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
        graFX.setGradientType(gradienttype);
        graFX.setColorsForGradientPaint(twosidesgradient);
        graFX.setColorsForFourSidesGradientPaint(foursidesgradient);
        
        if(scriptObject!=null){
            return graFX.getImageFromScript(scriptObject, frame);
        }
        
        return graFX.getImageFromShape();
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
