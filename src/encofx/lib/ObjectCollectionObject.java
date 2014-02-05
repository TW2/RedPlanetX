/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib;

import encofx.lib.effects.ParentCollection;
import encofx.lib.properties.Child;
import encofx.lib.properties.ShapeType;
import encofx.lib.properties.TextAnchorPosition;
import encofx.lib.properties.TextAnchorX;
import encofx.lib.properties.TextAnchorY;
import encofx.lib.properties.TextFontname;
import encofx.lib.properties.TextFontstyle;
import encofx.lib.properties.TextGradientType;
import encofx.lib.properties.TextStrikeOut;
import encofx.lib.properties.TextString;
import encofx.lib.properties.TextUnderline;
import encofx.lib.settings.SetupObject;
import java.awt.Font;
import java.awt.geom.Point2D;

/**
 *
 * @author Yves
 */
public abstract class ObjectCollectionObject implements ObjectCollectionInterface {
    
    protected boolean anchorIsSelected = false;
    
    public ObjectCollectionObject(){
        
    }

    @Override
    public Type getType() {
        return Type.Unknown;
    }

    @Override
    public SubObjects getSubObjects() {
        /*
        Doit impérativement être implémentée de la façon suivante :
        SubObjects<type d'objet> so = new SubObjects();
        so.addAllObjects(liste relative au type d'objet);
        return so;
        exemple :
        SubObjects<Text> so = new SubObjects();
        so.addAllObjects(texts);
        return so;
        */
        return null;
    }

    @Override
    public void setSubObjects(SubObjects subs) {
        /*
        Doit impérativement être implémentée de la façon suivante :
        liste relative au type d'objet = subs.getObjects();
        exemple :
        texts = subs.getObjects();
        */
    }
    
    public void setAnchorSelection(boolean anchorIsSelected){
        this.anchorIsSelected = anchorIsSelected;
    }
    
    public boolean isAnchorSelected(){
        return anchorIsSelected;
    }
    
    public boolean isNearOfAnchor(Point2D p){
        SetupObject<Float> soAX = (SetupObject)propAnchorX.getObject();
        SetupObject<Float> soAY = (SetupObject)propAnchorY.getObject();
        Point2D anchor = new Point2D.Float(soAX.get(), soAY.get());
        if(p.distance(anchor)<=10){
            return true;
        }
        return false;
    }
    
    //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    //=====  Spécifique aux COLLECTIONS de TEXTES  =============================
    //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    
    protected final Child propChild = new Child();
    protected final TextFontname propFontname = new TextFontname();
    protected final TextFontstyle propFontstyle = new TextFontstyle();
    protected final TextString propString = new TextString();
    protected final TextAnchorX propAnchorX = new TextAnchorX();
    protected final TextAnchorY propAnchorY = new TextAnchorY();
    protected final TextAnchorPosition propAnchorPosition = new TextAnchorPosition();
    protected final TextUnderline propUnderline = new TextUnderline();
    protected final TextStrikeOut propStrikeOut = new TextStrikeOut();
    protected final TextGradientType propGradientType = new TextGradientType();
    protected final ShapeType propShapeType = new ShapeType();
    
    public enum AnchorPosition{
        CornerLeftBottom("Corner Left-Bottom"),
        Bottom("Bottom"),
        CornerRightBottom("Corner Right-Bottom"),
        Right("Right"),
        CornerRightTop("Corner Right-Top"),
        Top("Top"),
        CornerLeftTop("Corner Left-Top"),
        Left("Left"),
        Middle("Middle");
        
        String name = "";
        
        AnchorPosition(String name){
            this.name = name;
        }
        
        @Override
        public String toString(){
            return name;
        }
    }
    
    public enum FontStyle{
        Plain("Plain", Font.PLAIN),
        Bold("Bold", Font.BOLD),
        Italic("Italic", Font.ITALIC),
        BoldItalic("Bold Italic", Font.BOLD+Font.ITALIC);
        
        String name = "Unknown";
        int style = Font.PLAIN;
        
        FontStyle(String name, int style){
            this.name = name;
            this.style = style;
        }
        
        public int getStyle(){
            return style;
        }
        
        @Override
        public String toString(){
            return name;
        }
    
    }
    
    public enum GradientType{
        None("Without gradient"),
        TwoSides("Gradient"),
        FourSides("4 sides gradient");
        //Linear("Linear gradient"),
        //Radial("Radial gradient"),
        //Multiple("Multiple gradient");
        
        String name = "";
        
        GradientType(String name){
            this.name = name;
        }
        
        @Override
        public String toString(){
            return name;
        }
    }
    
    //==========================================================================
    // ----- CHILD -------------------------------------------------------------
    public void setParent(ParentCollection pc){
        SetupObject<ParentCollection> so = (SetupObject)propChild.getObject();
        so.set(pc);
    }
    
    public ParentCollection getParent(){
        SetupObject<ParentCollection> so = (SetupObject)propChild.getObject();
        return so.get();
    }
    // -------------------------------------------------------------------------
    //==========================================================================
    
    public void setFontname(String fontname){
        SetupObject<String> so = (SetupObject)propFontname.getObject();
        so.set(fontname);
    }
    
    public String getFontname(){
        SetupObject<String> so = (SetupObject)propFontname.getObject();
        return so.get();
    }
    
    public void setFontstyle(FontStyle fontstyle){
        SetupObject<FontStyle> so = (SetupObject)propFontstyle.getObject();
        so.set(fontstyle);
    }
    
    public FontStyle getFontstyle(){
        SetupObject<FontStyle> so = (SetupObject)propFontstyle.getObject();
        return so.get();
    }
    
    public void setText(String text){
        SetupObject<String> soString = (SetupObject)propString.getObject();
        soString.set(text);
    }
    
    public String getText(){
        SetupObject<String> so = (SetupObject)propString.getObject();
        return so.get();
    }
    
    public void setAnchor(Point2D anchor){
        SetupObject<Float> soAX = (SetupObject)propAnchorX.getObject();
        SetupObject<Float> soAY = (SetupObject)propAnchorY.getObject();
        soAX.set(Float.parseFloat(Double.toString(anchor.getX())));
        soAY.set(Float.parseFloat(Double.toString(anchor.getY())));
    }
    
    public Point2D getAnchor(){
        SetupObject<Float> soAX = (SetupObject)propAnchorX.getObject();
        SetupObject<Float> soAY = (SetupObject)propAnchorY.getObject();
        Point2D anchor = new Point2D.Float(soAX.get(), soAY.get());
        return anchor;
    }
    
    public void setAnchorPosition(AnchorPosition anchorposition){
        SetupObject<AnchorPosition> soAP = (SetupObject)propAnchorPosition.getObject();
        soAP.set(anchorposition);
    }
    
    public AnchorPosition getAnchorPosition(){
        SetupObject<AnchorPosition> soAP = (SetupObject)propAnchorPosition.getObject();
        return soAP.get();
    }
    
    public void setUnderline(boolean b){
        SetupObject<Boolean> soUnderline = (SetupObject)propUnderline.getObject();
        soUnderline.set(b);
    }
    
    public boolean getUnderline(){
        SetupObject<Boolean> soUnderline = (SetupObject)propUnderline.getObject();
        return soUnderline.get();
    }
    
    public void setStrikeOut(boolean b){
        SetupObject<Boolean> soStrikeOut = (SetupObject)propStrikeOut.getObject();
        soStrikeOut.set(b);
    }
    
    public boolean getStrikeOut(){
        SetupObject<Boolean> soStrikeOut = (SetupObject)propStrikeOut.getObject();
        return soStrikeOut.get();
    }
    
    public void setGradientType(GradientType gradienttype){
        SetupObject<GradientType> soGT = (SetupObject)propGradientType.getObject();
        soGT.set(gradienttype);
    }
    
    public GradientType getGradientType(){
        SetupObject<GradientType> soGT = (SetupObject)propGradientType.getObject();
        return soGT.get();
    }
    
    public void setShapeType(ShapeType.ObjectShapeType ost){
        SetupObject<ShapeType.ObjectShapeType> soGT = (SetupObject)propShapeType.getObject();
        soGT.set(ost);
    }
    
    public ShapeType.ObjectShapeType getShapeType(){
        SetupObject<ShapeType.ObjectShapeType> soGT = (SetupObject)propShapeType.getObject();
        return soGT.get();
    }
}
