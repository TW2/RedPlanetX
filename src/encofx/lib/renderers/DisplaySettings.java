/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.renderers;

import encofx.lib.settings.SetupObject;
import java.awt.Color;
import java.awt.Component;
import javax.swing.JLabel;
import javax.swing.JTable;
import javax.swing.table.TableCellRenderer;

/**
 *
 * @author Yves
 * @deprecated Replace by DisplaySettingsDeluxe
 * @see DisplaySettingsDeluxe
 */
public class DisplaySettings extends JLabel implements TableCellRenderer {
    
    public DisplaySettings(){
        setOpaque(true);
    }

    @Override
    public Component getTableCellRendererComponent(JTable table, Object value,
            boolean isSelected, boolean hasFocus, int row, int column){
        
        if(value instanceof SetupObject){
            SetupObject so = (SetupObject)value;
            if(so.getType()==SetupObject.Type.Color){
                setText("");
                Color c = (Color)so.get();
                setBackground(c);
            }else{
                setText(so.get().toString());
                setBackground(Color.white);
            }
        }
        
        return this;
    }
    
    
    
}
