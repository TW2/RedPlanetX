/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib;

import encofx.lib.properties.ImageOnFrames;
import encofx.lib.properties.ImageOntoFrames;
import encofx.lib.properties.TextAngle;
import encofx.lib.properties.TextColor;
import encofx.lib.properties.TextFontsize;
import encofx.lib.properties.TextFourSidesGradientPaint;
import encofx.lib.properties.TextGradientPaint;
import encofx.lib.properties.TextPositionX;
import encofx.lib.properties.TextPositionY;
import encofx.lib.properties.TextScaleX;
import encofx.lib.properties.TextScaleY;
import encofx.lib.properties.TextTransparency;
import encofx.lib.properties.VideoOntoFrames;
import encofx.lib.settings.SetupObject;
import encofx.lib.xuggle.VideoEmulation;
import java.awt.Color;
import java.awt.image.BufferedImage;

/**
 *
 * @author Yves
 */
public abstract class FXObject implements FXInterface {
    
    protected String name = "Generic effect";
    protected String displayname = "Generic effect";
    protected Type type = Type.Unknown;
    protected int frame = 0;
    
    public FXObject(){
        
    }
    
    @Override
    public void setName(String name) {
        this.name = name;
    }

    @Override
    public String getName() {
        return name;
    }

    @Override
    public void setDisplayName(String displayname) {
        this.displayname = displayname;
    }

    @Override
    public String getDisplayName() {
        return displayname;
    }

    @Override
    public void setType(Type type) {
        this.type = type;
    }

    @Override
    public Type getType() {
        return type;
    }
    
    @Override
    public void setFrame(int frame){
        this.frame = frame;
    }
    
    @Override
    public int getFrame(){
        return frame;
    }
    
    //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    //=====  Spécifique au TEXTE  ==============================================
    //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    
    protected final TextFontsize propFontsize = new TextFontsize();
    protected final TextPositionX propX = new TextPositionX();
    protected final TextPositionY propY = new TextPositionY();
    protected final TextColor propColor = new TextColor();
    protected final TextTransparency propTrans = new TextTransparency();
    protected final TextScaleX propScaleX = new TextScaleX();
    protected final TextScaleY propScaleY = new TextScaleY();
    protected final TextAngle propAngle = new TextAngle();
    protected final TextGradientPaint propGradient = new TextGradientPaint();
    protected final TextFourSidesGradientPaint propFourSidesGradient = new TextFourSidesGradientPaint();
    protected ImageOnFrames propImageOnFrames = new ImageOnFrames();
    protected ImageOntoFrames propImageOntoFrames = new ImageOntoFrames();
    protected VideoOntoFrames propVideoOntoFrames = new VideoOntoFrames();
    
    public void setColor(Color color){
        SetupObject<Color> so = (SetupObject)propColor.getObject();
        so.set(color);
    }
    
    public Color getColor(){
        SetupObject<Color> so = (SetupObject)propColor.getObject();
        return so.get();
    }
    
    public void setSize(float fontsize){
        SetupObject so = (SetupObject)propFontsize.getObject();
        so.set(fontsize);
    }
    
    public float getSize(){
        SetupObject so = (SetupObject)propFontsize.getObject();
        return (Float)so.get();
    }
    
    public void setX(float x){
        SetupObject so = (SetupObject)propX.getObject();
        so.set(x);
    }
    
    public float getX(){
        SetupObject so = (SetupObject)propX.getObject();
        return (Float)so.get();
    }
    
    public void setY(float y){
        SetupObject so = (SetupObject)propY.getObject();
        so.set(y);
    }
    
    public float getY(){
        SetupObject so = (SetupObject)propY.getObject();
        return (Float)so.get();
    }
    
    public void setTransparency(float f){
        SetupObject so = (SetupObject)propTrans.getObject();
        so.set(f);
    }
    
    public float getTransparency(){
        SetupObject so = (SetupObject)propTrans.getObject();
        return (Float)so.get();
    }
    
    public void setScaleX(float f){
        SetupObject so = (SetupObject)propScaleX.getObject();
        so.set(f);
    }
    
    public float getScaleX(){
        SetupObject so = (SetupObject)propScaleX.getObject();
        return (Float)so.get();
    }
    
    public void setScaleY(float f){
        SetupObject so = (SetupObject)propScaleY.getObject();
        so.set(f);
    }
    
    public float getScaleY(){
        SetupObject so = (SetupObject)propScaleY.getObject();
        return (Float)so.get();
    }
    
    public void setAngle(float f){
        SetupObject so = (SetupObject)propAngle.getObject();
        so.set(f);
    }
    
    public float getAngle(){
        SetupObject so = (SetupObject)propAngle.getObject();
        return (Float)so.get();
    }
    
    public void setGradient(Color[] cs){
        SetupObject<Color[]> so = (SetupObject)propGradient.getObject();
        so.set(cs);
    }
    
    public Color[] getGradient(){
        SetupObject<Color[]> so = (SetupObject)propGradient.getObject();
        return so.get();
    }
    
    public void setFourSidesGradient(Color[] cs){
        SetupObject<Color[]> so = (SetupObject)propFourSidesGradient.getObject();
        so.set(cs);
    }
    
    public Color[] getFourSidesGradient(){
        SetupObject<Color[]> so = (SetupObject)propFourSidesGradient.getObject();
        return so.get();
    }
    
    public void setImageOnFrames(BufferedImage image){
        SetupObject<BufferedImage> so = (SetupObject)propImageOnFrames.getObject();
        so.set(image);
    }
    
    public BufferedImage getImageOnFrames(){
        SetupObject<BufferedImage> so = (SetupObject)propImageOnFrames.getObject();
        return so.get();
    }
    
    public void setImageOntoFrames(BufferedImage image){
        SetupObject<BufferedImage> so = (SetupObject)propImageOntoFrames.getObject();
        so.set(image);
    }
    
    public BufferedImage getImageOntoFrames(){
        SetupObject<BufferedImage> so = (SetupObject)propImageOntoFrames.getObject();
        return so.get();
    }
    
    public void setVideoOntoFrames(VideoEmulation video){
        SetupObject<VideoEmulation> so = (SetupObject)propVideoOntoFrames.getObject();
        so.set(video);
    }
    
    public VideoEmulation getVideoOntoFrames(){
        SetupObject<VideoEmulation> so = (SetupObject)propVideoOntoFrames.getObject();
        return so.get();
    }
}
