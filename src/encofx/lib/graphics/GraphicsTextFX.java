/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.graphics;

import encofx.lib.ObjectCollectionObject;
import encofx.lib.properties.ShapeType;
import encofx.lib.scripting.Scripting;
import java.awt.AlphaComposite;
import java.awt.Color;
import java.awt.Font;
import java.awt.FontMetrics;
import java.awt.GradientPaint;
import java.awt.Graphics2D;
import java.awt.Shape;
import java.awt.font.FontRenderContext;
import java.awt.font.LineBreakMeasurer;
import java.awt.font.TextAttribute;
import java.awt.font.TextLayout;
import java.awt.geom.AffineTransform;
import java.awt.geom.Ellipse2D;
import java.awt.geom.Rectangle2D;
import java.awt.geom.RoundRectangle2D;
import java.awt.image.BufferedImage;
import java.awt.image.BufferedImageOp;
import java.awt.image.ConvolveOp;
import java.text.AttributedCharacterIterator;
import java.text.AttributedString;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * Une classe pour créer de superbes graphique ou pas. :P
 * @author Yves
 */
public class GraphicsTextFX {
    
    private final BufferedImage image;
    private final Graphics2D g;
    private Rendering rendering = Rendering.Drawing;
    private Direction direction = Direction.Horizontal;
    
    private String fontname = "Arial";
    private float fontsize = 12f;
    private int fontstyle = Font.PLAIN;
    private Color color = Color.blue;
    private float transparency = 1f;
    private ObjectCollectionObject.AnchorPosition anchorPosition = ObjectCollectionObject.AnchorPosition.CornerLeftBottom;
    private boolean anchorSelected = false;
    private float x = 100f, xa = 100f; // x = position, xa = position à l'origine
    private float y = 100f, ya = 100f; // y = position, ya = position à l'origine
    private float scale_x = 100f;
    private float scale_y = 100f;
    private String string = "Sample";
    private boolean underline = false;
    private boolean strikeout = false;
    private float angle = 0f;
    private ObjectCollectionObject.GradientType gradientType = ObjectCollectionObject.GradientType.None;
    private Color[] gradientColors = new Color[]{Color.black, Color.white};
    private Color[] fourSidesGradientColors = new Color[]{Color.black, Color.white, Color.blue, Color.red};
    private ShapeType.ObjectShapeType oShapeType = ShapeType.ObjectShapeType.Free;
    
    /**
     * <p>Pour définir le rendu.<br />
     * Rendering = Drawing quand nous sommes en mode édition.<br />
     * Rendering = Encoding quand nous sommes en mode encodage.</p>
     */
    public enum Rendering{
        Drawing, Encoding;
    }
    
    public enum Direction{
        Vertical, Horizontal;
    }
    
    /**
     * Crée un nouveau objet GraphicsTextFX qui se prépare à dessiner 
     * sur une image transparente.
     * @param width La largeur de la zone peinte (largeur de l'image)
     * @param height La hauteur de la zone peinte (hauteur de l'image)
     */
    public GraphicsTextFX(int width, int height){
        image = new BufferedImage(width, height, BufferedImage.TYPE_INT_ARGB);
        g = image.createGraphics();
    }
    
    // <editor-fold defaultstate="collapsed" desc="===TEXTE HORIZONTAL===">
    /**
     * Obtient une image avec les modifications voulues.
     * @return Une image transparente avec le texte visible et ses effets
     */
    public BufferedImage getImageFromText(){
        AffineTransform at = new AffineTransform();
        AffineTransform oldAT = g.getTransform();
        
        //Fonte
        g.setFont(new Font(fontname, fontstyle, 12).deriveFont(fontsize));
        //Underline - Strikeout
        Map<TextAttribute, Object> USmap = new HashMap<>();
        if(underline == true && strikeout == true && direction == Direction.Horizontal){
            USmap.put(TextAttribute.KERNING, TextAttribute.KERNING_ON); //Mode Normal
            USmap.put(TextAttribute.UNDERLINE, TextAttribute.UNDERLINE_ON); //Souligné
            USmap.put(TextAttribute.STRIKETHROUGH, TextAttribute.STRIKETHROUGH_ON); //Barré
        }else if(underline == true && direction == Direction.Horizontal){
            USmap.put(TextAttribute.KERNING, TextAttribute.KERNING_ON); //Mode Normal
            USmap.put(TextAttribute.UNDERLINE, TextAttribute.UNDERLINE_ON); //Souligné
        }else if(strikeout == true && direction == Direction.Horizontal){
            USmap.put(TextAttribute.KERNING, TextAttribute.KERNING_ON); //Mode Normal
            USmap.put(TextAttribute.STRIKETHROUGH, TextAttribute.STRIKETHROUGH_ON); //Barré
        }else{
            USmap.put(TextAttribute.KERNING, TextAttribute.KERNING_ON); //Mode Normal
        }
        g.setFont(g.getFont().deriveFont(USmap));
        //Couleur
        g.setColor(color);
        //Transparence
        g.setComposite(makeComposite(transparency));
        //Echelle X et Y
        at.setToScale(scale_x/100, scale_y/100);        
        //Angle
        at.setToRotation(Math.toRadians(angle), xa, ya);
        
        g.setTransform(at);
        
        FontMetrics metrics = g.getFontMetrics(g.getFont());
        int w = metrics.stringWidth(string);
        int h = metrics.getHeight();
        
        w = Math.round(w*(scale_x/100));
        h = Math.round(h*(scale_y/100));
        
        switch(anchorPosition){
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
                y = y + h; //Do nothing x
                break;
            case Left: 
                y = y + h/2; //Do nothing x
                break;
            case Middle:
                x = x - w/2; y = y + h/2;
                break;
        }
        
        //Correction de la position de l'ancre
        y = y - metrics.getMaxDescent();
        
        if(gradientType==ObjectCollectionObject.GradientType.None){
            //Texte
            if(direction == Direction.Horizontal){
                //Correction dû à l'échelle
                float tx = 1f/(scale_x/100f);
                float ty = 1f/(scale_y/100f);            
                //Ecriture du texte            
                g.drawString(string, x*tx, y*ty);
            }else{
                int py = 0;
                for(char ch : string.toCharArray()){
                    g.drawString(Character.toString(ch), x, py+y);
                    py += h;
                }
            }
        }else if(gradientType==ObjectCollectionObject.GradientType.TwoSides){
            // Horizontal
            GradientPaint gp = new GradientPaint(
                    x, y, gradientColors[0],
                    x+w, y, gradientColors[1]);
            g.setPaint(gp);
            //Texte
            if(direction == Direction.Horizontal){
                //Correction dû à l'échelle
                float tx = 1f/(scale_x/100f);
                float ty = 1f/(scale_y/100f);            
                //Ecriture du texte            
                g.drawString(string, x*tx, y*ty);
            }else{
                int py = 0;
                for(char ch : string.toCharArray()){
                    g.drawString(Character.toString(ch), x, py+y);
                    py += h;
                }
            }
        }else if(gradientType==ObjectCollectionObject.GradientType.FourSides){
            // Horizontal
            GradientPaint gp2 = new GradientPaint(
                    x, y, fourSidesGradientColors[0],
                    x+w, y, fourSidesGradientColors[1]);
            g.setPaint(gp2);
            //Texte
            if(direction == Direction.Horizontal){
                //Correction dû à l'échelle
                float tx = 1f/(scale_x/100f);
                float ty = 1f/(scale_y/100f);            
                //Ecriture du texte            
                g.drawString(string, x*tx, y*ty);
            }else{
                int py = 0;
                for(char ch : string.toCharArray()){
                    g.drawString(Character.toString(ch), x, py+y);
                    py += h;
                }
            }
            // Vertical
            Color c3 = fourSidesGradientColors[2];
            Color c4 = fourSidesGradientColors[3];
            GradientPaint gp = new GradientPaint(
                    x, y-h, new Color(c3.getRed(), c3.getGreen(), c3.getBlue(), 127),
                    x, y, new Color(c4.getRed(), c4.getGreen(), c4.getBlue(), 127));
            g.setPaint(gp);
            //Texte
            if(direction == Direction.Horizontal){
                //Correction dû à l'échelle
                float tx = 1f/(scale_x/100f);
                float ty = 1f/(scale_y/100f);            
                //Ecriture du texte            
                g.drawString(string, x*tx, y*ty);
            }else{
                int py = 0;
                for(char ch : string.toCharArray()){
                    g.drawString(Character.toString(ch), x, py+y);
                    py += h;
                }
            }
        }
        
        
        //On cache l'ancre à l'encodage mais on ne la cache pas pour l'édition.
        if(rendering == Rendering.Drawing){
            g.setTransform(oldAT);
            if(anchorSelected == true){
                g.setColor(Color.magenta);
                g.fillRect(
                        Math.round(xa)-5,
                        Math.round(ya)-5,
                        10,
                        10);
            }
            g.setColor(Color.cyan);
            g.drawRect(
                    Math.round(xa)-5,
                    Math.round(ya)-5,
                    10,
                    10);
        }
            
//        BlurMatrix blurmatrix = new BlurMatrix();
//        blurmatrix.set5x5Matrix(0.01f, 0.01f, 0.01f, 0.01f, 0.01f, 0.01f, 0.01f, 0.01f, 0.01f);
//        BufferedImage new_image = useBlur(image, blurmatrix);
        
        return image;
    }
    // </editor-fold>
    
    // <editor-fold defaultstate="collapsed" desc="===TEXTE VERTICAL===">
    /**
     * Obtient une image avec les modifications voulues.
     * @return Une image transparente avec le texte visible et ses effets
     */
    public BufferedImage getImageFromVerticalText(){
        AffineTransform at = new AffineTransform();
        AffineTransform oldAT = g.getTransform();
        
        //Fonte
        g.setFont(new Font(fontname, fontstyle, 12).deriveFont(fontsize));
        //Underline - Strikeout
        Map<TextAttribute, Object> USmap = new HashMap<>();
        if(underline == true && strikeout == true && direction == Direction.Horizontal){
            USmap.put(TextAttribute.KERNING, TextAttribute.KERNING_ON); //Mode Normal
            USmap.put(TextAttribute.UNDERLINE, TextAttribute.UNDERLINE_ON); //Souligné
            USmap.put(TextAttribute.STRIKETHROUGH, TextAttribute.STRIKETHROUGH_ON); //Barré
        }else if(underline == true && direction == Direction.Horizontal){
            USmap.put(TextAttribute.KERNING, TextAttribute.KERNING_ON); //Mode Normal
            USmap.put(TextAttribute.UNDERLINE, TextAttribute.UNDERLINE_ON); //Souligné
        }else if(strikeout == true && direction == Direction.Horizontal){
            USmap.put(TextAttribute.KERNING, TextAttribute.KERNING_ON); //Mode Normal
            USmap.put(TextAttribute.STRIKETHROUGH, TextAttribute.STRIKETHROUGH_ON); //Barré
        }else{
            USmap.put(TextAttribute.KERNING, TextAttribute.KERNING_ON); //Mode Normal
        }
        g.setFont(g.getFont().deriveFont(USmap));
        //Couleur
        g.setColor(color);
        //Transparence
        g.setComposite(makeComposite(transparency));
        //Echelle X et Y
        at.setToScale(scale_x/100, scale_y/100);        
        //Angle
        at.setToRotation(Math.toRadians(angle), xa, ya);
        
        g.setTransform(at);
        
        FontMetrics metrics = g.getFontMetrics(g.getFont());
        int w = metrics.stringWidth(string);
        int h = metrics.getHeight();
        
        w = Math.round(w*(scale_x/100));
        h = Math.round(h*(scale_y/100));
        
        switch(anchorPosition){
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
                y = y + h; //Do nothing x
                break;
            case Left: 
                y = y + h/2; //Do nothing x
                break;
            case Middle:
                x = x - w/2; y = y + h/2;
                break;
        }
        
        //Correction de la position de l'ancre
        y = y - metrics.getMaxDescent();
        
        if(gradientType==ObjectCollectionObject.GradientType.None){
            //Texte
            if(direction == Direction.Vertical){
                int py = 0;
                for(char ch : string.toCharArray()){
                    g.drawString(Character.toString(ch), x, py+y);
                    py += h;
                }
            }
        }else if(gradientType==ObjectCollectionObject.GradientType.TwoSides){
            // Horizontal
            GradientPaint gp = new GradientPaint(
                    x, y, gradientColors[0],
                    x+w, y, gradientColors[1]);
            g.setPaint(gp);
            //Texte
            if(direction == Direction.Vertical){
                int py = 0;
                for(char ch : string.toCharArray()){
                    g.drawString(Character.toString(ch), x, py+y);
                    py += h;
                }
            }
        }else if(gradientType==ObjectCollectionObject.GradientType.FourSides){
            // Horizontal
            GradientPaint gp2 = new GradientPaint(
                    x, y, fourSidesGradientColors[0],
                    x+w, y, fourSidesGradientColors[1]);
            g.setPaint(gp2);
            //Texte
            if(direction == Direction.Vertical){
                int py = 0;
                for(char ch : string.toCharArray()){
                    g.drawString(Character.toString(ch), x, py+y);
                    py += h;
                }
            }
            // Vertical
            Color c3 = fourSidesGradientColors[2];
            Color c4 = fourSidesGradientColors[3];
            GradientPaint gp = new GradientPaint(
                    x, y-h, new Color(c3.getRed(), c3.getGreen(), c3.getBlue(), 127),
                    x, y, new Color(c4.getRed(), c4.getGreen(), c4.getBlue(), 127));
            g.setPaint(gp);
            //Texte
            if(direction == Direction.Vertical){
                int py = 0;
                for(char ch : string.toCharArray()){
                    g.drawString(Character.toString(ch), x, py+y);
                    py += h;
                }
            }
        }
        
        
        //On cache l'ancre à l'encodage mais on ne la cache pas pour l'édition.
        if(rendering == Rendering.Drawing){
            g.setTransform(oldAT);
            if(anchorSelected == true){
                g.setColor(Color.magenta);
                g.fillRect(
                        Math.round(xa)-5,
                        Math.round(ya)-5,
                        10,
                        10);
            }
            g.setColor(Color.cyan);
            g.drawRect(
                    Math.round(xa)-5,
                    Math.round(ya)-5,
                    10,
                    10);
        }
            
//        BlurMatrix blurmatrix = new BlurMatrix();
//        blurmatrix.set5x5Matrix(0.01f, 0.01f, 0.01f, 0.01f, 0.01f, 0.01f, 0.01f, 0.01f, 0.01f);
//        BufferedImage new_image = useBlur(image, blurmatrix);
        
        return image;
    }
    // </editor-fold>
    
    // <editor-fold defaultstate="collapsed" desc="===SCRIPT===">
    /**
     * Obtient une image avec les modifications voulues.
     * @param script L'object contenant la référence du script
     * @param frame
     * @return Une image transparente avec le texte visible et ses effets
     */
    public BufferedImage getImageFromScript(Object script, int frame){
        AffineTransform at = new AffineTransform();
        AffineTransform oldAT = g.getTransform();
        
        //Fonte
        g.setFont(new Font(fontname, fontstyle, 12).deriveFont(fontsize));
        //Underline - Strikeout
        Map<TextAttribute, Object> USmap = new HashMap<>();
        if(underline == true && strikeout == true && direction == Direction.Horizontal){
            USmap.put(TextAttribute.KERNING, TextAttribute.KERNING_ON); //Mode Normal
            USmap.put(TextAttribute.UNDERLINE, TextAttribute.UNDERLINE_ON); //Souligné
            USmap.put(TextAttribute.STRIKETHROUGH, TextAttribute.STRIKETHROUGH_ON); //Barré
        }else if(underline == true && direction == Direction.Horizontal){
            USmap.put(TextAttribute.KERNING, TextAttribute.KERNING_ON); //Mode Normal
            USmap.put(TextAttribute.UNDERLINE, TextAttribute.UNDERLINE_ON); //Souligné
        }else if(strikeout == true && direction == Direction.Horizontal){
            USmap.put(TextAttribute.KERNING, TextAttribute.KERNING_ON); //Mode Normal
            USmap.put(TextAttribute.STRIKETHROUGH, TextAttribute.STRIKETHROUGH_ON); //Barré
        }else{
            USmap.put(TextAttribute.KERNING, TextAttribute.KERNING_ON); //Mode Normal
        }
        g.setFont(g.getFont().deriveFont(USmap));
        //Couleur
        g.setColor(color);
        //Transparence
        g.setComposite(makeComposite(transparency));
        //Echelle X et Y
        at.setToScale(scale_x/100, scale_y/100);        
        //Angle
        at.setToRotation(Math.toRadians(angle), xa, ya);
        
        g.setTransform(at);
        
        FontMetrics metrics = g.getFontMetrics(g.getFont());
        int w = metrics.stringWidth(string);
        int h = metrics.getHeight();
        
        w = Math.round(w*(scale_x/100));
        h = Math.round(h*(scale_y/100));
        
        switch(anchorPosition){
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
                y = y + h; //Do nothing x
                break;
            case Left: 
                y = y + h/2; //Do nothing x
                break;
            case Middle:
                x = x - w/2; y = y + h/2;
                break;
        }
        
        //Correction de la position de l'ancre
        y = y - metrics.getMaxDescent();
        
                
        if(gradientType==ObjectCollectionObject.GradientType.None){
            
            Scripting sc = new Scripting();
            sc.setGraphics(g);
            sc.runScriptAndDo(script);
            
        }else if(gradientType==ObjectCollectionObject.GradientType.TwoSides){
            // Horizontal
            GradientPaint gp = new GradientPaint(
                    x, y, gradientColors[0],
                    x+w, y, gradientColors[1]);
            g.setPaint(gp);
            
            Scripting sc = new Scripting();
            sc.setGraphics(g);
            sc.runScriptAndDo(script);
            
        }else if(gradientType==ObjectCollectionObject.GradientType.FourSides){
            // Horizontal
            GradientPaint gp2 = new GradientPaint(
                    x, y, fourSidesGradientColors[0],
                    x+w, y, fourSidesGradientColors[1]);
            g.setPaint(gp2);
            
            Scripting sc = new Scripting();
            sc.setGraphics(g);
            sc.runScriptAndDo(script);
            
            // Vertical
            Color c3 = fourSidesGradientColors[2];
            Color c4 = fourSidesGradientColors[3];
            GradientPaint gp = new GradientPaint(
                    x, y-h, new Color(c3.getRed(), c3.getGreen(), c3.getBlue(), 127),
                    x, y, new Color(c4.getRed(), c4.getGreen(), c4.getBlue(), 127));
            g.setPaint(gp);
            
            sc.runScriptAndDo(script);     
            
        }
        
        
        //On cache l'ancre à l'encodage mais on ne la cache pas pour l'édition.
        if(rendering == Rendering.Drawing){
            g.setTransform(oldAT);
            if(anchorSelected == true){
                g.setColor(Color.magenta);
                g.fillRect(
                        Math.round(xa)-5,
                        Math.round(ya)-5,
                        10,
                        10);
            }
            g.setColor(Color.cyan);
            g.drawRect(
                    Math.round(xa)-5,
                    Math.round(ya)-5,
                    10,
                    10);
        }
        
        return image;
    }
    // </editor-fold>
    
    // <editor-fold defaultstate="collapsed" desc="===ZONE DE TEXTE HORIZONTALE===">
    /**
     * Obtient une image avec les modifications voulues.
     * @return Une image transparente avec le texte visible et ses effets
     */    
    public BufferedImage getImageFromTextArea(){
        AffineTransform at = new AffineTransform();
        AffineTransform oldAT = g.getTransform();
        TextLayout layout;
        Shape shape;
        
        //Fonte
        g.setFont(new Font(fontname, fontstyle, 12).deriveFont(fontsize));
        //Couleur
        g.setColor(color);
        //Transparence
        g.setComposite(makeComposite(transparency));
        //Echelle X et Y
        at.setToScale(scale_x/100, scale_y/100);        
        //Angle
        at.setToRotation(Math.toRadians(angle), xa, ya);
        
        g.setTransform(at);
        
        int lineBreak = 100;
        LineBreakMeasurer lineMeasurer;
        int paragraphStart, paragraphEnd;
        AttributedString as = new AttributedString(string);
        as.addAttribute(TextAttribute.FONT, g.getFont());
        as.addAttribute(TextAttribute.FOREGROUND, g.getColor());
        as.addAttribute(TextAttribute.KERNING, TextAttribute.KERNING_ON);
        if(underline == true && strikeout == true && direction == Direction.Horizontal){
            as.addAttribute(TextAttribute.UNDERLINE, TextAttribute.UNDERLINE_ON);
            as.addAttribute(TextAttribute.STRIKETHROUGH, TextAttribute.STRIKETHROUGH_ON);
        }else if(underline == true && direction == Direction.Horizontal){
            as.addAttribute(TextAttribute.UNDERLINE, TextAttribute.UNDERLINE_ON);
        }else if(strikeout == true && direction == Direction.Horizontal){
            as.addAttribute(TextAttribute.STRIKETHROUGH, TextAttribute.STRIKETHROUGH_ON);
        }
        as.addAttribute(TextAttribute.LIGATURES, TextAttribute.LIGATURES_ON);
        as.addAttribute(TextAttribute.TRANSFORM, at);
        
        // Create a new LineBreakMeasurer from the paragraph.
        // It will be cached and re-used.        
        AttributedCharacterIterator paragraph = as.getIterator();
        paragraphStart = paragraph.getBeginIndex();
        paragraphEnd = paragraph.getEndIndex();
        FontRenderContext frc = g.getFontRenderContext();
        lineMeasurer = new LineBreakMeasurer(paragraph, frc);
        
        // Set break width to width of Component.
        float breakWidth = (float)lineBreak;
        float drawPosY = y;
        // Set position to the index of the first character in the paragraph.
        lineMeasurer.setPosition(paragraphStart);
        
        // Emulation
        while (lineMeasurer.getPosition() < paragraphEnd) {
            layout = lineMeasurer.nextLayout(breakWidth);
            float drawPosX = layout.isLeftToRight() ? x : x + breakWidth - layout.getAdvance();
            // Move y-coordinate by the ascent of the layout.
            drawPosY += layout.getAscent();
            // Don't display text.....
            // Move y-coordinate in preparation for next layout.
            drawPosY += layout.getDescent() + layout.getLeading();
        }
        
        int w = lineBreak;
        int h = Math.round(drawPosY - ya);
        
        w = Math.round(w*(scale_x/100));
        h = Math.round(h*(scale_y/100));
        
        switch(anchorPosition){
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
                y = y + h; //Do nothing x
                break;
            case Left: 
                y = y + h/2; //Do nothing x
                break;
            case Middle:
                x = x - w/2; y = y + h/2;
                break;
        }
        
        y = y - h;
        
        // Set break width to width of Component.
        breakWidth = (float)lineBreak;
        drawPosY = y;
        // Set position to the index of the first character in the paragraph.
        lineMeasurer.setPosition(paragraphStart);
        
        List<TextAreaElement> elements = new ArrayList<>();
        
        while (lineMeasurer.getPosition() < paragraphEnd) {
            layout = lineMeasurer.nextLayout(breakWidth);
            float drawPosX = layout.isLeftToRight() ? x : x + breakWidth - layout.getAdvance();
            // Move y-coordinate by the ascent of the layout.
            drawPosY += layout.getAscent();
            // Display text.....
            shape = layout.getOutline(null);
            TextAreaElement tae = new TextAreaElement(shape, drawPosX, drawPosY);
            elements.add(tae);
            // Move y-coordinate in preparation for next layout.
            drawPosY += layout.getDescent() + layout.getLeading();
        }
        
        for(TextAreaElement tae : elements){
            AffineTransform atOrigin = g.getTransform();
            AffineTransform atx = new AffineTransform();
            atx.setToTranslation(tae.getX(), tae.getY());
            g.setTransform(atx);
            if(gradientType == ObjectCollectionObject.GradientType.None){
                g.fill(tae.getShape());
            }else if(gradientType == ObjectCollectionObject.GradientType.TwoSides){
                // Horizontal
                GradientPaint gp = new GradientPaint(
                        x, y, gradientColors[0],
                        x+w, y, gradientColors[1]);
                g.setPaint(gp);
                g.fill(tae.getShape());
            }else if(gradientType == ObjectCollectionObject.GradientType.FourSides){
                // Horizontal
                GradientPaint gp = new GradientPaint(
                        x, y, fourSidesGradientColors[0],
                        x+w, y, fourSidesGradientColors[1]);
                g.setPaint(gp);
                g.fill(tae.getShape());
                Color c3 = fourSidesGradientColors[2];
                Color c4 = fourSidesGradientColors[3];
                GradientPaint gp2 = new GradientPaint(
                        x, y-h, new Color(c3.getRed(), c3.getGreen(), c3.getBlue(), 127),
                        x, y, new Color(c4.getRed(), c4.getGreen(), c4.getBlue(), 127));
                g.setPaint(gp2);
                g.fill(tae.getShape());
            }
            g.setTransform(atOrigin);
        }
        
        //On cache l'ancre à l'encodage mais on ne la cache pas pour l'édition.
        if(rendering == Rendering.Drawing){
            g.setTransform(oldAT);
            if(anchorSelected == true){
                g.setColor(Color.magenta);
                g.fillRect(
                        Math.round(xa)-5,
                        Math.round(ya)-5,
                        10,
                        10);
            }
            g.setColor(Color.cyan);
            g.drawRect(
                    Math.round(xa)-5,
                    Math.round(ya)-5,
                    10,
                    10);
        }
        
        return image;
    }
    
    public class TextAreaElement{
        
        private final Shape myshape;
        private float x_location = 0f;
        private float y_location = 0f;
        
        public TextAreaElement(Shape shape, float x_location, float y_location){
            this.myshape = shape;
            this.x_location = x_location;
            this.y_location = y_location;
        }
        
        public Shape getShape(){
            return myshape;
        }
        
        public float getX(){
            return x_location;
        }
        
        public float getY(){
            return y_location;
        }
    }
    // </editor-fold>
    
    // <editor-fold defaultstate="collapsed" desc="===FORME RECTANGULAIRE===">
    public BufferedImage getImageFromRectangle(){
        AffineTransform at = new AffineTransform();
        AffineTransform oldAT = g.getTransform();
        
        //Fonte
        g.setFont(new Font(fontname, fontstyle, 12).deriveFont(fontsize));
        //Underline - Strikeout
        Map<TextAttribute, Object> USmap = new HashMap<>();
        if(underline == true && strikeout == true && direction == Direction.Horizontal){
            USmap.put(TextAttribute.KERNING, TextAttribute.KERNING_ON); //Mode Normal
            USmap.put(TextAttribute.UNDERLINE, TextAttribute.UNDERLINE_ON); //Souligné
            USmap.put(TextAttribute.STRIKETHROUGH, TextAttribute.STRIKETHROUGH_ON); //Barré
        }else if(underline == true && direction == Direction.Horizontal){
            USmap.put(TextAttribute.KERNING, TextAttribute.KERNING_ON); //Mode Normal
            USmap.put(TextAttribute.UNDERLINE, TextAttribute.UNDERLINE_ON); //Souligné
        }else if(strikeout == true && direction == Direction.Horizontal){
            USmap.put(TextAttribute.KERNING, TextAttribute.KERNING_ON); //Mode Normal
            USmap.put(TextAttribute.STRIKETHROUGH, TextAttribute.STRIKETHROUGH_ON); //Barré
        }else{
            USmap.put(TextAttribute.KERNING, TextAttribute.KERNING_ON); //Mode Normal
        }
        g.setFont(g.getFont().deriveFont(USmap));
        //Couleur
        g.setColor(color);
        //Transparence
        g.setComposite(makeComposite(transparency));
        //Echelle X et Y
        at.setToScale(scale_x/100, scale_y/100);        
        //Angle
        at.setToRotation(Math.toRadians(angle), xa, ya);
        
        g.setTransform(at);
        
        int w = 100;
        int h = 100;
        
        w = Math.round(w*(scale_x/100));
        h = Math.round(h*(scale_y/100));
        
        switch(anchorPosition){
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
                y = y + h; //Do nothing x
                break;
            case Left: 
                y = y + h/2; //Do nothing x
                break;
            case Middle:
                x = x - w/2; y = y + h/2;
                break;
        }
        
        //Correction de la position de l'ancre
        y = y - h;
        
        if(gradientType==ObjectCollectionObject.GradientType.None){
            g.fillRect((int)x, (int)y, w, h);
        }else if(gradientType==ObjectCollectionObject.GradientType.TwoSides){
            // Horizontal
            GradientPaint gp = new GradientPaint(
                    x, y, gradientColors[0],
                    x+w, y, gradientColors[1]);
            g.setPaint(gp);
            g.fillRect((int)x, (int)y, w, h);
        }else if(gradientType==ObjectCollectionObject.GradientType.FourSides){
            // Horizontal
            GradientPaint gp2 = new GradientPaint(
                    x, y, fourSidesGradientColors[0],
                    x+w, y, fourSidesGradientColors[1]);
            g.setPaint(gp2);
            g.fillRect((int)x, (int)y, w, h);
            // Vertical
            Color c3 = fourSidesGradientColors[2];
            Color c4 = fourSidesGradientColors[3];
            GradientPaint gp = new GradientPaint(
                    x, y, new Color(c3.getRed(), c3.getGreen(), c3.getBlue(), 127),
                    x, y+h, new Color(c4.getRed(), c4.getGreen(), c4.getBlue(), 127));
            g.setPaint(gp);
            g.fillRect((int)x, (int)y, w, h);
        }
        
        
        //On cache l'ancre à l'encodage mais on ne la cache pas pour l'édition.
        if(rendering == Rendering.Drawing){
            g.setTransform(oldAT);
            if(anchorSelected == true){
                g.setColor(Color.magenta);
                g.fillRect(
                        Math.round(xa)-5,
                        Math.round(ya)-5,
                        10,
                        10);
            }
            g.setColor(Color.cyan);
            g.drawRect(
                    Math.round(xa)-5,
                    Math.round(ya)-5,
                    10,
                    10);
        }
            
//        BlurMatrix blurmatrix = new BlurMatrix();
//        blurmatrix.set5x5Matrix(0.01f, 0.01f, 0.01f, 0.01f, 0.01f, 0.01f, 0.01f, 0.01f, 0.01f);
//        BufferedImage new_image = useBlur(image, blurmatrix);
        
        return image;
    }
    // </editor-fold>
    
    // <editor-fold defaultstate="collapsed" desc="===FORME RECTANGULAIRE ARRONDI AUX ANGLES===">
    public BufferedImage getImageFromRoundRectangle(){
        AffineTransform at = new AffineTransform();
        AffineTransform oldAT = g.getTransform();
        
        //Fonte
        g.setFont(new Font(fontname, fontstyle, 12).deriveFont(fontsize));
        //Underline - Strikeout
        Map<TextAttribute, Object> USmap = new HashMap<>();
        if(underline == true && strikeout == true && direction == Direction.Horizontal){
            USmap.put(TextAttribute.KERNING, TextAttribute.KERNING_ON); //Mode Normal
            USmap.put(TextAttribute.UNDERLINE, TextAttribute.UNDERLINE_ON); //Souligné
            USmap.put(TextAttribute.STRIKETHROUGH, TextAttribute.STRIKETHROUGH_ON); //Barré
        }else if(underline == true && direction == Direction.Horizontal){
            USmap.put(TextAttribute.KERNING, TextAttribute.KERNING_ON); //Mode Normal
            USmap.put(TextAttribute.UNDERLINE, TextAttribute.UNDERLINE_ON); //Souligné
        }else if(strikeout == true && direction == Direction.Horizontal){
            USmap.put(TextAttribute.KERNING, TextAttribute.KERNING_ON); //Mode Normal
            USmap.put(TextAttribute.STRIKETHROUGH, TextAttribute.STRIKETHROUGH_ON); //Barré
        }else{
            USmap.put(TextAttribute.KERNING, TextAttribute.KERNING_ON); //Mode Normal
        }
        g.setFont(g.getFont().deriveFont(USmap));
        //Couleur
        g.setColor(color);
        //Transparence
        g.setComposite(makeComposite(transparency));
        //Echelle X et Y
        at.setToScale(scale_x/100, scale_y/100);        
        //Angle
        at.setToRotation(Math.toRadians(angle), xa, ya);
        
        g.setTransform(at);
        
        int w = 100;
        int h = 100;
        
        w = Math.round(w*(scale_x/100));
        h = Math.round(h*(scale_y/100));
        
        switch(anchorPosition){
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
                y = y + h; //Do nothing x
                break;
            case Left: 
                y = y + h/2; //Do nothing x
                break;
            case Middle:
                x = x - w/2; y = y + h/2;
                break;
        }
        
        //Correction de la position de l'ancre
        y = y - h;
        
        if(gradientType==ObjectCollectionObject.GradientType.None){
            RoundRectangle2D rr2d = new RoundRectangle2D.Float(x, y, w, h, 30, 30);
            g.fill(rr2d);
        }else if(gradientType==ObjectCollectionObject.GradientType.TwoSides){
            // Horizontal
            GradientPaint gp = new GradientPaint(
                    x, y, gradientColors[0],
                    x+w, y, gradientColors[1]);
            g.setPaint(gp);
            RoundRectangle2D rr2d = new RoundRectangle2D.Float(x, y, w, h, 30, 30);
            g.fill(rr2d);
        }else if(gradientType==ObjectCollectionObject.GradientType.FourSides){
            RoundRectangle2D rr2d = new RoundRectangle2D.Float(x, y, w, h, 30, 30);
            // Horizontal
            GradientPaint gp2 = new GradientPaint(
                    x, y, fourSidesGradientColors[0],
                    x+w, y, fourSidesGradientColors[1]);
            g.setPaint(gp2);            
            g.fill(rr2d);
            // Vertical
            Color c3 = fourSidesGradientColors[2];
            Color c4 = fourSidesGradientColors[3];
            GradientPaint gp = new GradientPaint(
                    x, y, new Color(c3.getRed(), c3.getGreen(), c3.getBlue(), 127),
                    x, y+h, new Color(c4.getRed(), c4.getGreen(), c4.getBlue(), 127));
            g.setPaint(gp);
            g.fill(rr2d);
        }
        
        
        //On cache l'ancre à l'encodage mais on ne la cache pas pour l'édition.
        if(rendering == Rendering.Drawing){
            g.setTransform(oldAT);
            if(anchorSelected == true){
                g.setColor(Color.magenta);
                g.fillRect(
                        Math.round(xa)-5,
                        Math.round(ya)-5,
                        10,
                        10);
            }
            g.setColor(Color.cyan);
            g.drawRect(
                    Math.round(xa)-5,
                    Math.round(ya)-5,
                    10,
                    10);
        }
            
//        BlurMatrix blurmatrix = new BlurMatrix();
//        blurmatrix.set5x5Matrix(0.01f, 0.01f, 0.01f, 0.01f, 0.01f, 0.01f, 0.01f, 0.01f, 0.01f);
//        BufferedImage new_image = useBlur(image, blurmatrix);
        
        return image;
    }
    // </editor-fold>
    
    // <editor-fold defaultstate="collapsed" desc="===FORME QUELCONQUE===">
    /**
     * Obtient une image avec les modifications voulues.
     * @return Une image transparente avec le texte visible et ses effets
     */
    public BufferedImage getImageFromShape(){
        AffineTransform at = new AffineTransform();
        AffineTransform oldAT = g.getTransform();
        
        //Couleur
        g.setColor(color);
        //Transparence
        g.setComposite(makeComposite(transparency));
        //Echelle X et Y
        at.setToScale(scale_x/100, scale_y/100);        
        //Angle
        at.setToRotation(Math.toRadians(angle), xa, ya);
        
        g.setTransform(at);
        
        int w = 100;
        int h = 100;
        
        w = Math.round(w*(scale_x/100));
        h = Math.round(h*(scale_y/100));
        
        switch(anchorPosition){
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
                y = y + h; //Do nothing x
                break;
            case Left: 
                y = y + h/2; //Do nothing x
                break;
            case Middle:
                x = x - w/2; y = y + h/2;
                break;
        }
        
        //Correction de la position de l'ancre
        y = y - h;
        
        Shape shape = null;
        if(oShapeType==ShapeType.ObjectShapeType.Free){
            //TODO getGeneralPath
        }else if(oShapeType==ShapeType.ObjectShapeType.Rectangle){
            shape = new Rectangle2D.Float(x, y, w, h);
        }else if(oShapeType==ShapeType.ObjectShapeType.RoundRectangle){
            shape = new RoundRectangle2D.Float(x, y, w, h, 30, 30);
        }else if(oShapeType==ShapeType.ObjectShapeType.Ellipse){
            shape = new Ellipse2D.Float(x, y, w, h);
        }
        
        if(shape!=null){
            if(gradientType==ObjectCollectionObject.GradientType.None){
                g.fill(shape);
            }else if(gradientType==ObjectCollectionObject.GradientType.TwoSides){
                // Horizontal
                GradientPaint gp = new GradientPaint(
                        x, y, gradientColors[0],
                        x+w, y, gradientColors[1]);
                g.setPaint(gp);
                g.fill(shape);
            }else if(gradientType==ObjectCollectionObject.GradientType.FourSides){
                // Horizontal
                GradientPaint gp2 = new GradientPaint(
                        x, y, fourSidesGradientColors[0],
                        x+w, y, fourSidesGradientColors[1]);
                g.setPaint(gp2);
                g.fill(shape);
                // Vertical
                Color c3 = fourSidesGradientColors[2];
                Color c4 = fourSidesGradientColors[3];
                GradientPaint gp = new GradientPaint(
                        x, y, new Color(c3.getRed(), c3.getGreen(), c3.getBlue(), 127),
                        x, y+h, new Color(c4.getRed(), c4.getGreen(), c4.getBlue(), 127));
                g.setPaint(gp);
                g.fill(shape);
            }
        }       
        
        
        //On cache l'ancre à l'encodage mais on ne la cache pas pour l'édition.
        if(rendering == Rendering.Drawing){
            g.setTransform(oldAT);
            if(anchorSelected == true){
                g.setColor(Color.magenta);
                g.fillRect(
                        Math.round(xa)-5,
                        Math.round(ya)-5,
                        10,
                        10);
            }
            g.setColor(Color.cyan);
            g.drawRect(
                    Math.round(xa)-5,
                    Math.round(ya)-5,
                    10,
                    10);
        }
            
//        BlurMatrix blurmatrix = new BlurMatrix();
//        blurmatrix.set5x5Matrix(0.01f, 0.01f, 0.01f, 0.01f, 0.01f, 0.01f, 0.01f, 0.01f, 0.01f);
//        BufferedImage new_image = useBlur(image, blurmatrix);
        
        return image;
    }
    // </editor-fold>
    
    
    public BufferedImage getBlankImage(){
        return image;
    }
    
    /**
     * <p>Définit le rendu.<br />
     * Rendering = Drawing quand nous sommes en mode édition.<br />
     * Rendering = Encoding quand nous sommes en mode encodage.</p>
     * @param rendering Le mode de rendu
     */
    public void setRendering(Rendering rendering){
        this.rendering = rendering;
    }
    
    public void setDirection(Direction direction){
        this.direction = direction;
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
    
    /**
     * Définit la couleur.
     * @param color La couleur
     */
    public void setColor(Color color){
        this.color = color;
    }
    
    /**
     * Définit la transparence.
     * @param transparency La transparence de 0.00 (transparent) à 1.00 (opaque)
     */
    public void setTransparency(float transparency){
        this.transparency = transparency;
    }
    
    /**
     * Définit l'échelle sur X.
     * @param scale_x L'échelle sur X où 100 correspond à 100%
     */
    public void setScaleX(float scale_x){
        this.scale_x = scale_x;
    }
    
    /**
     * Définit l'échelle sur Y.
     * @param scale_y L'échelle sur Y où 100 correspond à 100%
     */
    public void setScaleY(float scale_y){
        this.scale_y = scale_y;
    }
    
    public void setX(float x){
        this.x = x; xa = x;
    }
    
    public void setY(float y){
        this.y = y; ya = y;
    }
    
    public void setAnchorPosition(ObjectCollectionObject.AnchorPosition anchorPosition){
        this.anchorPosition = anchorPosition;
    }
    
    public void setAnchorSelection(boolean anchorSelected){
        this.anchorSelected = anchorSelected;
    }
    
    public void setString(String string){
        this.string = string;
    }
    
    public void setUnderline(boolean underline){
        this.underline = underline;
    }
    
    public void setStrikeOut(boolean strikeout){
        this.strikeout = strikeout;
    }
    
    public void setAngle(float angle){
        this.angle = angle;
    }
    
    public void setGradientType(ObjectCollectionObject.GradientType gradientType){
        this.gradientType = gradientType;
    }
    
    public void setColorsForGradientPaint(Color[] gradientColors){
        this.gradientColors = gradientColors;
    }
    
    public void setColorsForFourSidesGradientPaint(Color[] fourSidesGradientColors){
        this.fourSidesGradientColors = fourSidesGradientColors;
    }
    
    public void setShapeType(ShapeType.ObjectShapeType oShapeType){
        this.oShapeType = oShapeType;
    }
    
    /**
     * Méthode de gestion de la transparence par son composite.
     */
    private AlphaComposite makeComposite(float alpha) {
        int type = AlphaComposite.SRC_OVER;
        return(AlphaComposite.getInstance(type, alpha));
    }
    
    private BufferedImage useBlur(BufferedImage source, BlurMatrix blurmatrix){
        BufferedImage destination = new BufferedImage(source.getWidth(), source.getHeight(), BufferedImage.TYPE_INT_ARGB);        
//        float[] matrix = new float[400];        
//	for (int i = 0; i < 400; i++){
//            matrix[i] = 1.0f/400.0f;
//        }        
//        BufferedImageOp op = new ConvolveOp( new Kernel(20, 20, matrix), ConvolveOp.EDGE_NO_OP, null );
        BufferedImageOp op = new ConvolveOp( blurmatrix.getKernel(), ConvolveOp.EDGE_NO_OP, null );
	return op.filter(source, destination);        
    }
    
}
