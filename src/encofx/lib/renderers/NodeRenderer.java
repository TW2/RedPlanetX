/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.renderers;

import encofx.lib.FXObject;
import encofx.lib.effects.ParentCollection;
import encofx.lib.effects.Text;
import encofx.lib.effects.TextCollection;
import encofx.lib.effects.VTextCollection;
import encofx.lib.xuggle.VideoInfo;
import java.awt.Component;
import javax.swing.Icon;
import javax.swing.ImageIcon;
import javax.swing.JTree;
import javax.swing.tree.DefaultMutableTreeNode;
import javax.swing.tree.DefaultTreeCellRenderer;

/**
 *
 * @author Yves
 */
public class NodeRenderer extends DefaultTreeCellRenderer {
    
    private VideoInfo vi = null;
    
    public NodeRenderer(VideoInfo vi){
        this.vi = vi;
    }
    
    @Override
    public Component getTreeCellRendererComponent(JTree tree, Object value,
                        boolean sel, boolean expanded, boolean leaf,
                        int row, boolean hasFocus) {
        
        super.getTreeCellRendererComponent(tree, value, sel, expanded, leaf, row, hasFocus);
        
        DefaultMutableTreeNode node = (DefaultMutableTreeNode)value;
        
        if(node.getUserObject() instanceof TextCollection){
            TextCollection tc = (TextCollection)node.getUserObject();
            if(expanded){
                Icon icon = new ImageIcon(getClass().getResource("16px-h-textj.png"));
                setIcon(icon);
            }else{
                Icon icon = new ImageIcon(getClass().getResource("16px-h-text.png"));
                setIcon(icon);
            }
        }else if(node.getUserObject() instanceof VTextCollection){
            VTextCollection vtc = (VTextCollection)node.getUserObject();
            if(expanded){
                Icon icon = new ImageIcon(getClass().getResource("16px-v-textj.png"));
                setIcon(icon);
            }else{
                Icon icon = new ImageIcon(getClass().getResource("16px-v-text.png"));
                setIcon(icon);
            }
        }else if(node.getUserObject() instanceof FXObject){
            FXObject fx = (FXObject)node.getUserObject();
            if(fx.getFrame()==0){
                Icon icon = new ImageIcon(getClass().getResource("16px-point.png"));
                setIcon(icon);
            }else if(fx.getFrame()==vi.getFrames()){
                Icon icon = new ImageIcon(getClass().getResource("16px-point-red.png"));
                setIcon(icon);
            }else{
                Icon icon = new ImageIcon(getClass().getResource("16px-point-blue.png"));
                setIcon(icon);
            }
            if(fx instanceof Text){
                Text t = (Text)fx;
                if(t.isSyllable()==true){
                    if(t.getFrame()==0){
                        Icon icon = new ImageIcon(getClass().getResource("16px-triangle-green.png"));
                        setIcon(icon);
                    }else if(t.getFrame()==vi.getFrames()){
                        Icon icon = new ImageIcon(getClass().getResource("16px-triangle-red.png"));
                        setIcon(icon);
                    }else{
                        Icon icon = new ImageIcon(getClass().getResource("16px-triangle-blue.png"));
                        setIcon(icon);
                    }
                }
            }
        }else if(node.getUserObject() instanceof ParentCollection){
            ParentCollection pc = (ParentCollection)node.getUserObject();
            if(pc.getScript()==null){
                Icon icon = new ImageIcon(getClass().getResource("16px-Crystal_Clear_app_kllckety.png"));
                setIcon(icon);
            }else{
                Icon icon = new ImageIcon(getClass().getResource("16px-Crystal_Clear_app_kcmdf.png"));
                setIcon(icon);
            }            
        }
        
        return this;
    }
    
    public void updateVideoInfo(VideoInfo vi){
        this.vi = vi;
        updateUI();
    }
    
}
