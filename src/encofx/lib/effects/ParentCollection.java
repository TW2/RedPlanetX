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
import java.awt.Color;
import java.awt.Font;
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
public class ParentCollection extends ObjectCollectionObject {
    
    List<Parent> parents = new ArrayList<>();
    private Font font = new Font("Arial", Font.PLAIN, 50);
    //Objects for the table of properties and settings
    List<AbstractProperty> properties = new ArrayList<>();
    
    private boolean useFontname = false;
    private boolean useFontstyle = false;
    private boolean useAnchorX = false;
    private boolean useAnchorY = false;
    private boolean usePosition = false;
    private boolean useUnderline = false;
    private boolean useStrikeOut = false;
    
    private boolean useFontsize = false;
    private boolean useX = false;
    private boolean useY = false;
    private boolean useColor = false;
    private boolean useTransparency = false;
    private boolean useScaleX = false;
    private boolean useScaleY = false;
    private boolean useAngle = false;
    private boolean useGradient = false;
    private boolean useFourSides = false;
    
    private Object script = null;
    
    public ParentCollection(){
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

    @Override
    public SubObjects getSubObjects() {
        SubObjects<Parent> so = new SubObjects();
        so.addAllObjects(parents);
        return so;
    }

    @Override
    public void setSubObjects(SubObjects subs) {
        parents = subs.getObjects();
    }
    
    public void sortByFrames(){
        Collections.sort(parents, new Comparator<Parent>() {
            @Override
            public int compare(Parent o1, Parent o2) {
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
        Collections.sort(parents, new Comparator<Parent>() {
            @Override
            public int compare(Parent o1, Parent o2) {
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
        return Type.Parent;
    }
    
    public Parent getBefore(int frame){
        Parent before = null;
        for(Parent tx : parents){
            if(tx.getFrame()<=frame){
                before = tx;
            }
        }
        return before;
    }
    
    public Parent getAfter(int frame){
        Parent after = null;
        sortByFrames_Reverse();
        for(Parent tx : parents){
            if(tx.getFrame()>=frame){
                after = tx;
            }
        }
        sortByFrames();
        return after;
    }
    
    public void setList(List<Parent> parents){
        this.parents = parents;
    }
    
    public List<Parent> getList(){
        return parents;
    }
    
    public void add(Parent obj){
        parents.add(obj);
    }
    
    public void remove(Parent obj){
        parents.remove(obj);
    }
    
    public void clear(){
        parents.clear();
    }
    
    public void setFont(Font font){
        this.font = font;
    }
    
    public Font getFont(){
        return font;
    }
    
    public BufferedImage getFX(int frame, int imageWidth, int imageHeight, boolean encoding) {
        GraphicsTextFX graFX = new GraphicsTextFX(imageWidth, imageHeight);
        
        Parent before = getBefore(frame);
        Parent after = getAfter(frame);
        
        //Propiétés dynamiques
        //Couleur
        Color c = Parent.getActualColor(before, after, frame);
        //Taille de la fonte
        float size = Parent.getActualSize(before, after, frame);
        //Position des ancres X et Y
        SetupObject<Float> soAX = (SetupObject)propAnchorX.getObject();
        SetupObject<Float> soAY = (SetupObject)propAnchorY.getObject();
        Point2D anchor = new Point2D.Float(soAX.get(), soAY.get());
        float x = Parent.getActualX(before, after, anchor, frame);
        float y = Parent.getActualY(before, after, anchor, frame);
        //Transparence
        float transparency = Parent.getActualTransparency(before, after, frame);
        //Echelle X et Y
        float scale_x = Parent.getActualScaleX(before, after, frame);
        float scale_y = Parent.getActualScaleY(before, after, frame);
        //Angle
        float angle = Parent.getActualAngle(before, after, frame);
        //Dégradé (deux cotés)
        Color[] twosidesgradient = Parent.getActualGradientColor(before, after, frame);
        //Dégradé (quatre cotés)
        Color[] foursidesgradient = Parent.getActualFourSidesGradientColor(before, after, frame);
        
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
    
    //==========================================================================
    // Pour définir une propriété en faisant un lien
    //==========================================================================
    
    public void setFontnameUsage(boolean b){
        useFontname = b;
    }
    
    public boolean getFontnameUsage(){
        return useFontname;
    }
    
    public void setFontstyleUsage(boolean b){
        useFontstyle = b;
    }
    
    public boolean getFontstyleUsage(){
        return useFontstyle;
    }
    
    public void setAnchorXUsage(boolean b){
        useAnchorX = b;
    }
    
    public boolean getAnchorXUsage(){
        return useAnchorX;
    }
    
    public void setAnchorYUsage(boolean b){
        useAnchorY = b;
    }
    
    public boolean getAnchorYUsage(){
        return useAnchorY;
    }
    
    public void setPositionUsage(boolean b){
        usePosition = b;
    }
    
    public boolean getPositionUsage(){
        return usePosition;
    }
    
    public void setUnderlineUsage(boolean b){
        useUnderline = b;
    }
    
    public boolean getUnderlineUsage(){
        return useUnderline;
    }
    
    public void setStrikeOutUsage(boolean b){
        useStrikeOut = b;
    }
    
    public boolean getStrikeOutUsage(){
        return useStrikeOut;
    }
    
    public void setFontsizeUsage(boolean b){
        useFontsize = b;
    }
    
    public boolean getFontsizeUsage(){
        return useFontsize;
    }
    
    public void setXUsage(boolean b){
        useX = b;
    }
    
    public boolean getXUsage(){
        return useX;
    }
    
    public void setYUsage(boolean b){
        useY = b;
    }
    
    public boolean getYUsage(){
        return useY;
    }
    
    public void setColorUsage(boolean b){
        useColor = b;
    }
    
    public boolean getColorUsage(){
        return useColor;
    }
    
    public void setTransparencyUsage(boolean b){
        useTransparency = b;
    }
    
    public boolean getTransparencyUsage(){
        return useTransparency;
    }
    
    public void setScaleXUsage(boolean b){
        useScaleX = b;
    }
    
    public boolean getScaleXUsage(){
        return useScaleX;
    }
    
    public void setScaleYUsage(boolean b){
        useScaleY = b;
    }
    
    public boolean getScaleYUsage(){
        return useScaleY;
    }
    
    public void setAngleUsage(boolean b){
        useAngle = b;
    }
    
    public boolean getAngleUsage(){
        return useAngle;
    }
    
    public void setGradientUsage(boolean b){
        useGradient = b;
    }
    
    public boolean getGradientUsage(){
        return useGradient;
    }
    
    public void setFourSidesUsage(boolean b){
        useFourSides = b;
    }
    
    public boolean getFourSidesUsage(){
        return useFourSides;
    }
    
    public void setScript(Object o){
        script = o;
    }
    
    public Object getScript(){
        return script;
    }
    
    //==========================================================================
    // Pour maîtriser le temps
    //==========================================================================
    
    public enum VirtualTime{
        Start, End, Middle, OneQuart, ThreeQuart, Default, Another;
    }
    
//    public FXObject getBefore(int frame){
//        FXObject before = null;
//        for(FXObject tx : parents){
//            if(tx.getFrame()<=frame){
//                before = tx;
//            }
//        }
//        return before;
//    }
//    
//    public Parent getAfter(int frame){
//        Parent after = null;
//        sortByFrames_Reverse();
//        for(Parent tx : parents){
//            if(tx.getFrame()>=frame){
//                after = tx;
//            }
//        }
//        sortByFrames();
//        return after;
//    }
//    
//    public Parent getParentFromVirtual(VirtualTime vt, List<FXObject> fxList, int wantedframe){
//        FXObject newFX;
//        switch(vt){
//            case Start: 
//                newFX = fxList.
//                break;
//            case End: 
//                break;
//            case Middle:
//                break;
//            case OneQuart: 
//                break;
//            case ThreeQuart: 
//                break;
//            case Default: 
//                break;
//            case Another:
//                break;
//        }
//    }
}
