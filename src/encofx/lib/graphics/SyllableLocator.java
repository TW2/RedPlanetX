/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.graphics;

import java.awt.Font;
import java.awt.FontMetrics;
import java.awt.Graphics2D;
import java.awt.font.TextAttribute;
import java.awt.image.BufferedImage;
import java.util.HashMap;
import java.util.Map;

/**
 *
 * @author Yves
 */
public class SyllableLocator {
    
    private final BufferedImage image;
    private final Graphics2D g;
    
    private String fontname = "Arial";
    private float fontsize = 12f;
    private int fontstyle = Font.PLAIN;
    private float x = 100f, xa = 100f; // x = position, xa = position à l'origine
    private String string = "Sample";
    
    public SyllableLocator(int width, int height){
        image = new BufferedImage(width, height, BufferedImage.TYPE_INT_ARGB);
        g = image.createGraphics();
    }
    
    public Map<Integer, Float> getSyllablesLocation(){
        Map<Integer, Float> locations = new HashMap<>();
        
        String itemsText = string.replaceAll("\\{[^\\}]+\\}", "◆");
        String[] table = itemsText.split("◆");
        
        //Fonte
        g.setFont(new Font(fontname, fontstyle, 12).deriveFont(fontsize));
        Map<TextAttribute, Object> USmap = new HashMap<>();
        USmap.put(TextAttribute.KERNING, TextAttribute.KERNING_ON); //Mode Normal
        g.setFont(g.getFont().deriveFont(USmap));
        
        FontMetrics metrics = g.getFontMetrics(g.getFont());        
        
        String restruct = "";
        
        for (int i=1; i<table.length; i++){
            float w = metrics.stringWidth(restruct) + x;
            locations.put(i-1, w);
            restruct = restruct.concat(table[i]);
        }
        
        return locations;
    }
    
    public String[] getSyllables(String s){
        String itemsText = s.replaceAll("\\{[^\\}]+\\}", "◆");
        String[] table = itemsText.split("◆");        
        return table;
    }
    
    /**
     * Définit le nom de la fonte.
     * @param fontname Le nome de la fonte/police
     */
    public void setFontname(String fontname){
        this.fontname = fontname;
    }
    
    /**
     * Définit la taille de la fonte.
     * @param fontsize La taille de la fonte/police
     */
    public void setFontsize(float fontsize){
        this.fontsize = fontsize;
    }
    
    /**
     * Définit le style de la fonte.
     * @param fontstyle Le style de la fonte/police
     */
    public void setFontstyle(int fontstyle){
        this.fontstyle = fontstyle;
    }
    
    public void setX(float x){
        this.x = x; xa = x;
    }
    
    public void setString(String string){
        this.string = string;
    }
    
}
