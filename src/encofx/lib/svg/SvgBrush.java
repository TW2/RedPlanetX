/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.svg;

import com.kitfox.svg.SVGDiagram;
import com.kitfox.svg.SVGException;
import com.kitfox.svg.SVGUniverse;
import java.awt.Graphics2D;
import java.io.File;
import java.net.MalformedURLException;
import java.net.URI;

/**
 *
 * @author Yves
 */
public class SvgBrush {
    
    private final SVGUniverse universe = new SVGUniverse();
    private SVGDiagram diagram = null;
    
    public SvgBrush(String SVG_BrushFile) throws MalformedURLException{
        File f = new File(SVG_BrushFile);
        URI address = universe.loadSVG(f.toURI().toURL());
        diagram = universe.getDiagram(address);
        diagram.setIgnoringClipHeuristic(true);
    }
    
    public SvgBrush(File f) throws MalformedURLException{
        URI address = universe.loadSVG(f.toURI().toURL());
        diagram = universe.getDiagram(address);
        diagram.setIgnoringClipHeuristic(true);
    }
    
    public void paintWithBrush(Graphics2D g2) throws SVGException{
        diagram.render(g2);
    }
    
}
