/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.renderers;

import encofx.lib.effects.ParentCollection;
import encofx.lib.settings.SetupObject;
import java.awt.BorderLayout;
import java.awt.Color;
import java.awt.Component;
import javax.swing.JCheckBox;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JSpinner;
import javax.swing.JTable;
import javax.swing.SwingUtilities;
import javax.swing.UIManager;
import javax.swing.UnsupportedLookAndFeelException;
import javax.swing.plaf.nimbus.NimbusLookAndFeel;
import javax.swing.table.TableCellRenderer;

/**
 *
 * @author Yves
 */
public class DisplaySettingsDeluxe extends JPanel implements TableCellRenderer {
    
    JLabel label = new JLabel();
    JCheckBox checkbox = new JCheckBox();
    JSpinner spinner = new JSpinner();
    TwoSidesGradientPanel twoSides = null;
    FourSidesGradientPanel fourSides = null;
    
    public DisplaySettingsDeluxe(){
        init();
    }
    
    private void init(){
        try {
            UIManager.setLookAndFeel(new NimbusLookAndFeel());
            SwingUtilities.updateComponentTreeUI(this);
        } catch (UnsupportedLookAndFeelException ex) {
            //Do nothing
        }
        
        SwingUtilities.updateComponentTreeUI(label);
        SwingUtilities.updateComponentTreeUI(checkbox);
        SwingUtilities.updateComponentTreeUI(spinner);
        
        setLayout(new BorderLayout());
        setOpaque(true);
        setBackground(Color.white);
    }

    @Override
    public Component getTableCellRendererComponent(JTable table, Object value,
            boolean isSelected, boolean hasFocus, int row, int column) {
        
        removeAll();
        
        if(value instanceof SetupObject){
            SetupObject so = (SetupObject)value;
//            if(so.getType()==SetupObject.Type.Color){
//                label.setOpaque(true);
//                label.setText("");
//                Color c = (Color)so.get();
//                label.setBackground(c);
//                add(label, BorderLayout.CENTER);
//            }else if(so.getType()==SetupObject.Type.Underline){
//                checkbox.setText(so.get().toString());
//                checkbox.setBackground(Color.white);
//                checkbox.setSelected((Boolean)so.get());
//                add(checkbox, BorderLayout.CENTER);
//            }else{
//                label.setText(so.get().toString());
//                label.setBackground(Color.white);
//                add(label, BorderLayout.CENTER);
//            }
            
            if(so.get() instanceof Color){
                label.setOpaque(true);
                label.setText("");
                Color c = (Color)so.get();
                label.setBackground(c);
                add(label, BorderLayout.CENTER);
            }else if(so.get() instanceof Float){
                spinner.setValue((Float)so.get());
                spinner.setBackground(Color.white);
                add(spinner, BorderLayout.CENTER);
            }else if(so.get() instanceof Boolean){
                checkbox.setText(so.get().toString());
                checkbox.setBackground(Color.white);
                checkbox.setSelected((Boolean)so.get());
                add(checkbox, BorderLayout.CENTER);
            }else if(so.getType()==SetupObject.Type.GradientPaint){
                twoSides = new TwoSidesGradientPanel(table.getWidth()/2, table.getRowHeight());
                Color[] cs = (Color[])so.get();
                twoSides.setColor1(cs[0]);
                twoSides.setColor2(cs[1]);
                add(twoSides, BorderLayout.CENTER);
            }else if(so.getType()==SetupObject.Type.FourSidesGradientPaint){
                fourSides = new FourSidesGradientPanel(table.getWidth()/2, table.getRowHeight());
                Color[] cs = (Color[])so.get();
                fourSides.setColor1(cs[0]);
                fourSides.setColor2(cs[1]);
                fourSides.setColor3(cs[2]);
                fourSides.setColor4(cs[3]);
                add(fourSides, BorderLayout.CENTER);
            }else if(so.getType()==SetupObject.Type.Child){
                ParentCollection pc = (ParentCollection)so.get();
                if(pc==null){
                    label.setText("None");
                }else{
                    label.setText(so.get().toString());
                }                
                label.setBackground(Color.white);
                add(label, BorderLayout.CENTER);
            }else{
                label.setText(so.get().toString());
                label.setBackground(Color.white);
                add(label, BorderLayout.CENTER);
            }
        }
        
        return this;
    }
    
}
