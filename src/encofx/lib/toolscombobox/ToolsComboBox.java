/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.toolscombobox;

import encofx.lib.VTD2;
import encofx.lib.paintdrawing.PaintTool;
import java.awt.BorderLayout;
import java.awt.Color;
import java.awt.Component;
import javax.swing.DefaultComboBoxModel;
import javax.swing.ImageIcon;
import javax.swing.JComboBox;
import javax.swing.JLabel;
import javax.swing.JList;
import javax.swing.JPanel;
import javax.swing.ListCellRenderer;

/**
 *
 * @author Yves
 */
public class ToolsComboBox extends JComboBox {
    
    private DefaultComboBoxModel model = new DefaultComboBoxModel();
    private ToolsComboBoxRenderer rend = new ToolsComboBoxRenderer();
    
    public ToolsComboBox(){
        setModel(model);
        setRenderer(rend);
        
        model.addElement(new Item());
        model.addElement(new Item(new ImageIcon(getClass().getResource("line2.png")), VTD2.ShapeSelection.Line));
        model.addElement(new Item(new ImageIcon(getClass().getResource("curve2.png")), VTD2.ShapeSelection.Curve));
        for(PaintTool.Tool tool : PaintTool.Tool.values()){
            model.addElement(new Item(new ImageIcon(getClass().getResource("32px-Crystal_Clear_app_kcoloredit.png")), tool));
        }
    }
    
    public DefaultComboBoxModel getDefaultModel(){
        return model;
    }
    
    public Object getFromSelectedItem(){
        Item item = (Item)getSelectedItem();
        return item.getObject();
    }
    
    public class Item{
        
        private ImageIcon icon = new ImageIcon();
        private Object object = new Object();
        
        public Item(){
            icon = null;
            object = "Please select a tool...";
        }
        
        public Item(ImageIcon icon, Object object){
            this.icon = icon;
            this.object = object;
        }
        
        public void setItem(ImageIcon icon, Object object){
            this.icon = icon;
            this.object = object;
        }
        
        public ImageIcon getIcon(){
            return icon;
        }
        
        public Object getObject(){
            return object;
        }
        
    }
    
    public class ToolsComboBoxRenderer extends JPanel implements ListCellRenderer {
        
        JLabel lblIcon = new JLabel();
        JLabel lblDisplay = new JLabel();
        
        public ToolsComboBoxRenderer(){
            setLayout(new BorderLayout());
            lblIcon.setSize(20, 20);
            add(lblIcon, BorderLayout.WEST);
            add(lblDisplay, BorderLayout.CENTER);
        }
        
        @Override
        public Component getListCellRendererComponent(JList list, Object value,
                int index, boolean isSelected, boolean cellHasFocus) {
            
            if(value instanceof Item){
                Item item = (Item)value;
                lblIcon.setIcon(item.getIcon());
                lblDisplay.setText(" "+item.getObject().toString());
            }
            
            if(isSelected){
                setBackground(Color.red);
            }else{
                setBackground(Color.white);
            }
            
            return this;
        }
        
    }
}
