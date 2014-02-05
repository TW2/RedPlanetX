/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.settings;

/**
 *
 * @author Yves
 * @param <T> The type of the setting.
 */
public class SetupObject<T> {
    
    private T setting = null;
    private Type type = Type.None;
    
    public SetupObject(){
        
    }
    
    public SetupObject(T setting){
        this.setting = setting;
    }
    
    public enum Type{
        None, Fontname, Fontsize, Fontstyle, Text, PositionX, PositionY, 
        AnchorPosition, AnchorX, AnchorY, Color, Transparency, Underline,
        StrikeOut, ScaleX, ScaleY, Angle, BlurType, BlurValue, GradientType,
        GradientPaint, FourSidesGradientPaint, Child, ShapeType;
    }
    
    public void set(T setting){
        this.setting = setting;
    }
    
    public T get(){
        return setting;
    }
    
    public void setType(Type type){
        this.type = type;
    }
    
    public Type getType(){
        return type;
    }
    
    @Override
    public String toString(){
        if(type==Type.Fontname | type==Type.Text){
            return (String)setting;
        }else if(type==Type.Fontstyle | type==Type.Fontsize |
                type==Type.PositionX | type==Type.PositionY |
                type==Type.AnchorX | type==Type.AnchorY |
                type==Type.AnchorPosition | type==Type.Color |
                type==Type.Transparency | type==Type.Underline |
                type==Type.StrikeOut | type==Type.ScaleX |
                type==Type.ScaleY | type==Type.Angle |
                type==Type.BlurType | type==Type.BlurValue |
                type==Type.GradientType | type==Type.GradientPaint |
                type==Type.FourSidesGradientPaint | type==Type.Child |
                type==Type.ShapeType){
            return setting.toString();
        }else{
            return "";
        }        
    }
    
}
