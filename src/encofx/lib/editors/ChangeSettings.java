/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.editors;

import encofx.lib.dialogs.HTMLColorDialog;
import encofx.lib.dialogs.ParentDialog;
import encofx.lib.dialogs.SidesGradientDialog;
import encofx.lib.effects.ParentCollection;
import encofx.lib.effects.TextCollection;
import encofx.lib.properties.Child;
import encofx.lib.settings.SetupObject;
import java.awt.Color;
import java.awt.Component;
import java.awt.Frame;
import java.awt.GraphicsEnvironment;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.List;
import javax.swing.AbstractCellEditor;
import javax.swing.DefaultComboBoxModel;
import javax.swing.JButton;
import javax.swing.JCheckBox;
import javax.swing.JComboBox;
import javax.swing.JSpinner;
import javax.swing.JTable;
import javax.swing.JTextField;
import javax.swing.SpinnerNumberModel;
import javax.swing.SwingUtilities;
import javax.swing.UIManager;
import javax.swing.UnsupportedLookAndFeelException;
import javax.swing.event.ChangeEvent;
import javax.swing.event.ChangeListener;
import javax.swing.plaf.nimbus.NimbusLookAndFeel;
import javax.swing.table.TableCellEditor;

/**
 *
 * @author Yves
 */
public class ChangeSettings extends AbstractCellEditor implements TableCellEditor {
    
    private final String EDIT = "edit";
    private SetupObject indicator = null;
    private Frame frame = null;
    
    private List<ParentCollection> parents = null;
    
    public void setParents(List<ParentCollection> parents){
        this.parents = parents;
    }
    
    // ===== FONTNAME ======================================================
    private final JComboBox comboFontname = new JComboBox();
    private final DefaultComboBoxModel modelFontname = new DefaultComboBoxModel();
    private SetupObject SO_Fontname = new SetupObject();
    
    public void comboFontnameActionPerformed(ActionEvent e){
        SO_Fontname.set(comboFontname.getSelectedItem());
        fireEditingStopped();
    }
    // ---------------------------------------------------------------------
    
    // ===== FONTSTYLE ======================================================
    private final JComboBox comboFontstyle = new JComboBox();
    private final DefaultComboBoxModel modelFontstyle = new DefaultComboBoxModel();
    private SetupObject SO_Fontstyle = new SetupObject();
    
    public void comboFontstyleActionPerformed(ActionEvent e){
        SO_Fontstyle.set(comboFontstyle.getSelectedItem());
        fireEditingStopped();
    }
    // ---------------------------------------------------------------------
    
    // ===== FONTSIZE ======================================================
    private final JSpinner spinFontsize = new JSpinner();
    private final SpinnerNumberModel modelFontsize = new SpinnerNumberModel(50d, -10000d, 10000d, 0.5d);
    private SetupObject SO_Fontsize = new SetupObject();
    
    public void spinFontsizeStateChanged(ChangeEvent e){
        SO_Fontsize.set(modelFontsize.getNumber().floatValue());
        fireEditingStopped();
    }
    // ---------------------------------------------------------------------
    
    // ===== POSITIONX ======================================================
    private final JSpinner spinPositionX = new JSpinner();
    private final SpinnerNumberModel modelPositionX = new SpinnerNumberModel(0d, -10000d, 10000d, 0.5d);
    private SetupObject SO_PositionX = new SetupObject();
    
    public void spinPositionXStateChanged(ChangeEvent e){
        SO_PositionX.set(modelPositionX.getNumber().floatValue());
        fireEditingStopped();
    }
    // ---------------------------------------------------------------------
    
    // ===== POSITIONY ======================================================
    private final JSpinner spinPositionY = new JSpinner();
    private final SpinnerNumberModel modelPositionY = new SpinnerNumberModel(0d, -10000d, 10000d, 0.5d);
    private SetupObject SO_PositionY = new SetupObject();
    
    public void spinPositionYStateChanged(ChangeEvent e){
        SO_PositionY.set(modelPositionY.getNumber().floatValue());
        fireEditingStopped();
    }
    // ---------------------------------------------------------------------
    
    // ===== TEXT ======================================================
    private final JTextField tfText = new JTextField();
    private SetupObject SO_Text = new SetupObject();
    
    public void tfTextActionPerformed(ActionEvent e){
        SO_Text.set(tfText.getText());
        fireEditingStopped();
    }
    // ---------------------------------------------------------------------
    
    // ===== ANCHORX ======================================================
    private final JSpinner spinAnchorX = new JSpinner();
    private final SpinnerNumberModel modelAnchorX = new SpinnerNumberModel(0d, -10000d, 10000d, 0.5d);
    private SetupObject SO_AnchorX = new SetupObject();
    
    public void spinAnchorXStateChanged(ChangeEvent e){
        SO_AnchorX.set(modelAnchorX.getNumber().floatValue());
        fireEditingStopped();
    }
    // ---------------------------------------------------------------------
    
    // ===== ANCHORY ======================================================
    private final JSpinner spinAnchorY = new JSpinner();
    private final SpinnerNumberModel modelAnchorY = new SpinnerNumberModel(0d, -10000d, 10000d, 0.5d);
    private SetupObject SO_AnchorY = new SetupObject();
    
    public void spinAnchorYStateChanged(ChangeEvent e){
        SO_AnchorY.set(modelAnchorY.getNumber().floatValue());
        fireEditingStopped();
    }
    // ---------------------------------------------------------------------
    
    // ===== ANCHORPOSITION ======================================================
    private final JComboBox comboAnchorPosition = new JComboBox();
    private final DefaultComboBoxModel modelAnchorPosition = new DefaultComboBoxModel();
    private SetupObject SO_AnchorPosition = new SetupObject();
    
    public void comboAnchorPositionActionPerformed(ActionEvent e){
        SO_AnchorPosition.set(comboAnchorPosition.getSelectedItem());
        fireEditingStopped();
    }
    // ---------------------------------------------------------------------
    
    // ===== COLOR ======================================================
    private final JButton buttonColor = new JButton();
    private final HTMLColorDialog htmlColor;
    private SetupObject<Color> SO_Color = new SetupObject();
    
    public void buttonColorActionPerformed(ActionEvent e){
        buttonColor.setBackground(SO_Color.get());
        htmlColor.showDialog();
        SO_Color.set(htmlColor.getColor());
        buttonColor.setBackground(SO_Color.get());
        fireEditingStopped();
    }
    // ---------------------------------------------------------------------
    
    // ===== TRANSPARENCY ==================================================
    private final JSpinner spinTransparency = new JSpinner();
    private final SpinnerNumberModel modelTransparency = new SpinnerNumberModel(0d, 0d, 1d, 0.01d);
    private SetupObject SO_Transparency = new SetupObject();
    
    public void spinTransparencyStateChanged(ChangeEvent e){
        SO_Transparency.set(modelTransparency.getNumber().floatValue());
        fireEditingStopped();
    }
    // ---------------------------------------------------------------------
    
    // ===== UNDERLINE ======================================================
    private final JCheckBox cbUnderline = new JCheckBox();
    private SetupObject<Boolean> SO_Underline = new SetupObject();
    
    public void cbUnderlineActionPerformed(ActionEvent e){
        SO_Underline.set(cbUnderline.isSelected());
        fireEditingStopped();
    }
    // ---------------------------------------------------------------------
    
    // ===== STRIKEOUT ======================================================
    private final JCheckBox cbStrikeOut = new JCheckBox();
    private SetupObject<Boolean> SO_StrikeOut = new SetupObject();
    
    public void cbStrikeOutActionPerformed(ActionEvent e){
        SO_StrikeOut.set(cbStrikeOut.isSelected());
        fireEditingStopped();
    }
    // ---------------------------------------------------------------------
    
    // ===== SCALEX ======================================================
    private final JSpinner spinScaleX = new JSpinner();
    private final SpinnerNumberModel modelScaleX = new SpinnerNumberModel(0d, 0d, 10000d, 1d);
    private SetupObject SO_ScaleX = new SetupObject();
    
    public void spinScaleXStateChanged(ChangeEvent e){
        SO_ScaleX.set(modelScaleX.getNumber().floatValue());
        fireEditingStopped();
    }
    // ---------------------------------------------------------------------
    
    // ===== SCALEY ======================================================
    private final JSpinner spinScaleY = new JSpinner();
    private final SpinnerNumberModel modelScaleY = new SpinnerNumberModel(0d, 0d, 10000d, 1d);
    private SetupObject SO_ScaleY = new SetupObject();
    
    public void spinScaleYStateChanged(ChangeEvent e){
        SO_ScaleY.set(modelScaleY.getNumber().floatValue());
        fireEditingStopped();
    }
    // ---------------------------------------------------------------------
    
    // ===== ANGLE ======================================================
    private final JSpinner spinAngle = new JSpinner();
    private final SpinnerNumberModel modelAngle = new SpinnerNumberModel(0d, -10000d, 10000d, 1d);
    private SetupObject SO_Angle = new SetupObject();
    
    public void spinAngleStateChanged(ChangeEvent e){
        SO_Angle.set(modelAngle.getNumber().floatValue());
        fireEditingStopped();
    }
    // ---------------------------------------------------------------------
    
    // ===== GRADIENTTYPE ======================================================
    private final JComboBox comboGradientType = new JComboBox();
    private final DefaultComboBoxModel modelGradientType = new DefaultComboBoxModel();
    private SetupObject SO_GradientType = new SetupObject();
    
    public void comboGradientTypeActionPerformed(ActionEvent e){
        SO_GradientType.set(comboGradientType.getSelectedItem());
        fireEditingStopped();
    }
    // ---------------------------------------------------------------------
    
    // ===== TWOSIDESGRADIENT ======================================================
    private final JButton buttonTwoSides = new JButton();
    private SidesGradientDialog twoSidesDialog = null;
    private SetupObject<Color[]> SO_TwoSides = new SetupObject();
    
    public void buttonTwoSidesActionPerformed(ActionEvent e){
        buttonTwoSides.setBackground(Color.gray);
        Color[] result = twoSidesDialog.showTwoSidesDialog(SO_TwoSides.get());
        SO_TwoSides.set(result);
        buttonTwoSides.setBackground(Color.gray);
        fireEditingStopped();
    }
    // ---------------------------------------------------------------------
    
    // ===== FOURSIDESGRADIENT ======================================================
    private final JButton buttonFourSides = new JButton();
    private SidesGradientDialog fourSidesDialog = null;
    private SetupObject<Color[]> SO_FourSides = new SetupObject();
    
    public void buttonFourSidesActionPerformed(ActionEvent e){
        buttonFourSides.setBackground(Color.gray);
        Color[] result = fourSidesDialog.showFourSidesDialog(SO_FourSides.get());
        SO_FourSides.set(result);
        buttonFourSides.setBackground(Color.gray);
        fireEditingStopped();
    }
    // ---------------------------------------------------------------------
    
    // ===== CHILD ======================================================
    private final JButton buttonChild = new JButton();
    private ParentDialog childDialog = null;
    private SetupObject<ParentCollection> SO_Child = new SetupObject();
    
    public void buttonChildActionPerformed(ActionEvent e){
        buttonChild.setBackground(Color.gray);
        ParentCollection result = childDialog.showDialog(SO_Child.get(), parents);
        SO_Child.set(result);
        buttonChild.setBackground(Color.gray);
        fireEditingStopped();
    }
    // ---------------------------------------------------------------------
    
    
    public ChangeSettings(Frame frame) {
        this.frame = frame;
        try {
            UIManager.setLookAndFeel(new NimbusLookAndFeel());
        } catch (UnsupportedLookAndFeelException ex) {
            //Do nothing
        }
        
        SwingUtilities.updateComponentTreeUI(comboFontname);
        SwingUtilities.updateComponentTreeUI(comboFontstyle);
        SwingUtilities.updateComponentTreeUI(spinFontsize);
        SwingUtilities.updateComponentTreeUI(spinPositionX);
        SwingUtilities.updateComponentTreeUI(spinPositionY);
        SwingUtilities.updateComponentTreeUI(tfText);
        SwingUtilities.updateComponentTreeUI(spinAnchorX);
        SwingUtilities.updateComponentTreeUI(spinAnchorY);
        SwingUtilities.updateComponentTreeUI(comboAnchorPosition);
        SwingUtilities.updateComponentTreeUI(buttonColor);
        SwingUtilities.updateComponentTreeUI(spinTransparency);
        SwingUtilities.updateComponentTreeUI(cbUnderline);
        SwingUtilities.updateComponentTreeUI(cbStrikeOut);
        SwingUtilities.updateComponentTreeUI(spinScaleX);
        SwingUtilities.updateComponentTreeUI(spinScaleY);
        SwingUtilities.updateComponentTreeUI(spinAngle);
        SwingUtilities.updateComponentTreeUI(comboGradientType);
        SwingUtilities.updateComponentTreeUI(buttonTwoSides);
        SwingUtilities.updateComponentTreeUI(buttonFourSides);
        SwingUtilities.updateComponentTreeUI(buttonChild);
        
        // ===== FONTNAME ======================================================
        comboFontname.setModel(modelFontname);
        comboFontname.setActionCommand(EDIT);
        
        for (String s : GraphicsEnvironment.getLocalGraphicsEnvironment().getAvailableFontFamilyNames()){
            modelFontname.addElement(s);
        }
        
        comboFontname.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                comboFontnameActionPerformed(e);
            }
        });
        // ---------------------------------------------------------------------
        
        // ===== FONTSTYLE ======================================================
        comboFontstyle.setModel(modelFontstyle);
        comboFontstyle.setActionCommand(EDIT);
        
        for (TextCollection.FontStyle fs : TextCollection.FontStyle.values()){
            modelFontstyle.addElement(fs);
        }
        
        comboFontstyle.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                comboFontstyleActionPerformed(e);
            }
        });
        // ---------------------------------------------------------------------
        
        // ===== FONTSIZE ======================================================
        spinFontsize.setModel(modelFontsize);
        spinFontsize.addChangeListener(new ChangeListener() {
            @Override
            public void stateChanged(ChangeEvent e) {
                spinFontsizeStateChanged(e);
            }
        });
        // ---------------------------------------------------------------------
        
        // ===== POSITIONX ======================================================
        spinPositionX.setModel(modelPositionX);
        spinPositionX.addChangeListener(new ChangeListener() {
            @Override
            public void stateChanged(ChangeEvent e) {
                spinPositionXStateChanged(e);
            }
        });
        // ---------------------------------------------------------------------
        
        // ===== POSITIONY ======================================================
        spinPositionY.setModel(modelPositionY);
        spinPositionY.addChangeListener(new ChangeListener() {
            @Override
            public void stateChanged(ChangeEvent e) {
                spinPositionYStateChanged(e);
            }
        });
        // ---------------------------------------------------------------------
        
        // ===== TEXT ======================================================
        tfText.setActionCommand(EDIT);
        tfText.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                tfTextActionPerformed(e);
            }
        });
        // ---------------------------------------------------------------------
        
        // ===== ANCHORX ======================================================
        spinAnchorX.setModel(modelAnchorX);
        spinAnchorX.addChangeListener(new ChangeListener() {
            @Override
            public void stateChanged(ChangeEvent e) {
                spinAnchorXStateChanged(e);
            }
        });
        // ---------------------------------------------------------------------
        
        // ===== ANCHORY ======================================================
        spinAnchorY.setModel(modelAnchorY);
        spinAnchorY.addChangeListener(new ChangeListener() {
            @Override
            public void stateChanged(ChangeEvent e) {
                spinAnchorYStateChanged(e);
            }
        });
        // ---------------------------------------------------------------------
        
        // ===== ANCHORPOSITION ======================================================
        comboAnchorPosition.setModel(modelAnchorPosition);
        comboAnchorPosition.setActionCommand(EDIT);
        
        for (TextCollection.AnchorPosition ap : TextCollection.AnchorPosition.values()){
            modelAnchorPosition.addElement(ap);
        }
        
        comboAnchorPosition.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                comboAnchorPositionActionPerformed(e);
            }
        });
        // ---------------------------------------------------------------------
        
        // ===== COLOR ======================================================
        htmlColor = new HTMLColorDialog(frame, true);
        buttonColor.setActionCommand(EDIT);
        buttonColor.setBorderPainted(true);
        buttonColor.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                buttonColorActionPerformed(e);
            }
        });
        // ---------------------------------------------------------------------
        
        // ===== TRANSPARENCY ==================================================
        spinTransparency.setModel(modelTransparency);
        spinTransparency.addChangeListener(new ChangeListener() {
            @Override
            public void stateChanged(ChangeEvent e) {
                spinTransparencyStateChanged(e);
            }
        });
        // ---------------------------------------------------------------------
        
        // ===== UNDERLINE ======================================================
        cbUnderline.setActionCommand(EDIT);
        cbUnderline.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                cbUnderlineActionPerformed(e);
            }
        });
        // ---------------------------------------------------------------------
        
        // ===== STRIKEOUT ======================================================
        cbStrikeOut.setActionCommand(EDIT);
        cbStrikeOut.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                cbStrikeOutActionPerformed(e);
            }
        });
        // ---------------------------------------------------------------------
        
        // ===== SCALEX ======================================================
        spinScaleX.setModel(modelScaleX);
        spinScaleX.addChangeListener(new ChangeListener() {
            @Override
            public void stateChanged(ChangeEvent e) {
                spinScaleXStateChanged(e);
            }
        });
        // ---------------------------------------------------------------------
        
        // ===== SCALEY ======================================================
        spinScaleY.setModel(modelScaleY);
        spinScaleY.addChangeListener(new ChangeListener() {
            @Override
            public void stateChanged(ChangeEvent e) {
                spinScaleYStateChanged(e);
            }
        });
        // ---------------------------------------------------------------------
        
        // ===== ANGLE ======================================================
        spinAngle.setModel(modelAngle);
        spinAngle.addChangeListener(new ChangeListener() {
            @Override
            public void stateChanged(ChangeEvent e) {
                spinAngleStateChanged(e);
            }
        });
        // ---------------------------------------------------------------------
        
        // ===== GRADIENTTYPE ======================================================
        comboGradientType.setModel(modelGradientType);
        comboGradientType.setActionCommand(EDIT);
        
        for (TextCollection.GradientType gt : TextCollection.GradientType.values()){
            modelGradientType.addElement(gt);
        }
        
        comboGradientType.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                comboGradientTypeActionPerformed(e);
            }
        });
        // ---------------------------------------------------------------------
        
        // ===== TWOSIDESGRADIENT ======================================================
        twoSidesDialog = new SidesGradientDialog(frame, true);
        buttonTwoSides.setActionCommand(EDIT);
        buttonTwoSides.setBorderPainted(true);
        buttonTwoSides.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                buttonTwoSidesActionPerformed(e);
            }
        });
        // ---------------------------------------------------------------------
        
        // ===== FOURSIDESGRADIENT ======================================================
        fourSidesDialog = new SidesGradientDialog(frame, true);
        buttonFourSides.setActionCommand(EDIT);
        buttonFourSides.setBorderPainted(true);
        buttonFourSides.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                buttonFourSidesActionPerformed(e);
            }
        });
        // ---------------------------------------------------------------------
        
        // ===== CHILD ======================================================
        childDialog = new ParentDialog(frame, true);
        buttonChild.setActionCommand(EDIT);
        buttonChild.setBorderPainted(true);
        buttonChild.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                buttonChildActionPerformed(e);
            }
        });
        // ---------------------------------------------------------------------
        
    }

    @Override
    public Object getCellEditorValue() {
        
        // ===== FONTNAME ======================================================
        if(indicator.getType()==SetupObject.Type.Fontname){
            return SO_Fontname;
        }
        // ---------------------------------------------------------------------
        
        // ===== FONTSTYLE ======================================================
        if(indicator.getType()==SetupObject.Type.Fontstyle){
            return SO_Fontstyle;
        }
        // ---------------------------------------------------------------------
        
        // ===== FONTSIZE ======================================================
        if(indicator.getType()==SetupObject.Type.Fontsize){
            return SO_Fontsize;
        }
        // ---------------------------------------------------------------------
        
        // ===== POSITIONX ======================================================
        if(indicator.getType()==SetupObject.Type.PositionX){
            return SO_PositionX;
        }
        // ---------------------------------------------------------------------
        
        // ===== POSITIONY ======================================================
        if(indicator.getType()==SetupObject.Type.PositionY){
            return SO_PositionY;
        }
        // ---------------------------------------------------------------------
        
        // ===== TEXT ======================================================
        if(indicator.getType()==SetupObject.Type.Text){
            return SO_Text;
        }
        // ---------------------------------------------------------------------
        
        // ===== ANCHORX ======================================================
        if(indicator.getType()==SetupObject.Type.AnchorX){
            return SO_AnchorX;
        }
        // ---------------------------------------------------------------------
        
        // ===== ANCHORY ======================================================
        if(indicator.getType()==SetupObject.Type.AnchorY){
            return SO_AnchorY;
        }
        // ---------------------------------------------------------------------
        
        // ===== ANCHORPOSITION ======================================================
        if(indicator.getType()==SetupObject.Type.AnchorPosition){
            return SO_AnchorPosition;
        }
        // ---------------------------------------------------------------------
        
        // ===== COLOR ======================================================
        if(indicator.getType()==SetupObject.Type.Color){
            return SO_Color;
        }
        // ---------------------------------------------------------------------
        
        // ===== TRANSPARENCY ==================================================
        if(indicator.getType()==SetupObject.Type.Transparency){
            return SO_Transparency;
        }
        // ---------------------------------------------------------------------
        
        // ===== UNDERLINE ======================================================
        if(indicator.getType()==SetupObject.Type.Underline){
            return SO_Underline;
        }
        // ---------------------------------------------------------------------
        
        // ===== STRIKEOUT ======================================================
        if(indicator.getType()==SetupObject.Type.StrikeOut){
            return SO_StrikeOut;
        }
        // ---------------------------------------------------------------------
        
        // ===== SCALEX ======================================================
        if(indicator.getType()==SetupObject.Type.ScaleX){
            return SO_ScaleX;
        }
        // ---------------------------------------------------------------------
        
        // ===== SCALEY ======================================================
        if(indicator.getType()==SetupObject.Type.ScaleY){
            return SO_ScaleY;
        }
        // ---------------------------------------------------------------------
        
        // ===== ANGLE ======================================================
        if(indicator.getType()==SetupObject.Type.Angle){
            return SO_Angle;
        }
        // ---------------------------------------------------------------------
        
        // ===== GRADIENTTYPE ======================================================
        if(indicator.getType()==SetupObject.Type.GradientType){
            return SO_GradientType;
        }
        // ---------------------------------------------------------------------
        
        // ===== TWOSIDESGRADIENT ======================================================
        if(indicator.getType()==SetupObject.Type.GradientPaint){
            return SO_TwoSides;
        }
        // ---------------------------------------------------------------------
        
        // ===== FOURSIDESGRADIENT ======================================================
        if(indicator.getType()==SetupObject.Type.FourSidesGradientPaint){
            return SO_FourSides;
        }
        // ---------------------------------------------------------------------
        
        // ===== CHILD ======================================================
        if(indicator.getType()==SetupObject.Type.Child){
            return SO_Child;
        }
        // ---------------------------------------------------------------------
        
        return null;
    }

    @Override
    public Component getTableCellEditorComponent(JTable table, Object value,
            boolean isSelected, int row, int column) {
        
        indicator = (SetupObject)value;
        
        if(indicator!=null){
            
            // ===== FONTNAME ======================================================            
            if(indicator.getType()==SetupObject.Type.Fontname){
                SO_Fontname = (SetupObject)value;
                comboFontname.setSelectedItem(SO_Fontname.toString());
                return comboFontname;
            }
            // ---------------------------------------------------------------------

            // ===== FONTSTYLE ======================================================            
            if(indicator.getType()==SetupObject.Type.Fontstyle){
                SO_Fontstyle = (SetupObject)value;
                comboFontstyle.setSelectedItem(SO_Fontstyle.toString());
                return comboFontstyle;
            }
            // ---------------------------------------------------------------------
        
            // ===== FONTSIZE ======================================================            
            if(indicator.getType()==SetupObject.Type.Fontsize){
                SO_Fontsize = (SetupObject)value;
                modelFontsize.setValue(Double.parseDouble(SO_Fontsize.toString()));
                return spinFontsize;
            }
            // ---------------------------------------------------------------------
            
            // ===== POSITIONX ======================================================            
            if(indicator.getType()==SetupObject.Type.PositionX){
                SO_PositionX = (SetupObject)value;
                modelPositionX.setValue(Double.parseDouble(SO_PositionX.toString()));
                return spinPositionX;
            }
            // ---------------------------------------------------------------------
            
            // ===== POSITIONY ======================================================            
            if(indicator.getType()==SetupObject.Type.PositionY){
                SO_PositionY = (SetupObject)value;
                modelPositionY.setValue(Double.parseDouble(SO_PositionY.toString()));
                return spinPositionY;
            }
            // ---------------------------------------------------------------------
            
            // ===== TEXT ======================================================
            if(indicator.getType()==SetupObject.Type.Text){
                SO_Text = (SetupObject)value;
                tfText.setText(SO_Text.toString());
                return tfText;
            }
            // ---------------------------------------------------------------------
            
            // ===== ANCHORX ======================================================            
            if(indicator.getType()==SetupObject.Type.AnchorX){
                SO_AnchorX = (SetupObject)value;
                modelAnchorX.setValue(Double.parseDouble(SO_AnchorX.toString()));
                return spinAnchorX;
            }
            // ---------------------------------------------------------------------
            
            // ===== ANCHORY ======================================================            
            if(indicator.getType()==SetupObject.Type.AnchorY){
                SO_AnchorY = (SetupObject)value;
                modelAnchorY.setValue(Double.parseDouble(SO_AnchorY.toString()));
                return spinAnchorY;
            }
            // ---------------------------------------------------------------------
            
            // ===== ANCHORPOSITION ======================================================            
            if(indicator.getType()==SetupObject.Type.AnchorPosition){
                SO_AnchorPosition = (SetupObject)value;
                comboAnchorPosition.setSelectedItem(SO_AnchorPosition.toString());
                return comboAnchorPosition;
            }
            // ---------------------------------------------------------------------
            
            // ===== COLOR ======================================================
            if(indicator.getType()==SetupObject.Type.Color){
                SO_Color = (SetupObject)value;
                buttonColor.setBackground(SO_Color.get());
                htmlColor.setColor(SO_Color.get());
                return buttonColor;
            }
            // ---------------------------------------------------------------------
            
            // ===== TRANSPARENCY ======================================================            
            if(indicator.getType()==SetupObject.Type.Transparency){
                SO_Transparency = (SetupObject)value;
                modelTransparency.setValue(Double.parseDouble(SO_Transparency.toString()));
                return spinTransparency;
            }
            // ---------------------------------------------------------------------
            
            // ===== UNDERLINE ======================================================
            if(indicator.getType()==SetupObject.Type.Underline){
                SO_Underline = (SetupObject)value;
                cbUnderline.setSelected(SO_Underline.get());
                return cbUnderline;
            }
            // ---------------------------------------------------------------------
            
            // ===== STRIKEOUT ======================================================
            if(indicator.getType()==SetupObject.Type.StrikeOut){
                SO_StrikeOut = (SetupObject)value;
                cbStrikeOut.setSelected(SO_StrikeOut.get());
                return cbStrikeOut;
            }
            // ---------------------------------------------------------------------
            
            // ===== SCALEX ======================================================            
            if(indicator.getType()==SetupObject.Type.ScaleX){
                SO_ScaleX = (SetupObject)value;
                modelScaleX.setValue(Double.parseDouble(SO_ScaleX.toString()));
                return spinScaleX;
            }
            // ---------------------------------------------------------------------
            
            // ===== SCALEY ======================================================            
            if(indicator.getType()==SetupObject.Type.ScaleY){
                SO_ScaleY = (SetupObject)value;
                modelScaleY.setValue(Double.parseDouble(SO_ScaleY.toString()));
                return spinScaleY;
            }
            // ---------------------------------------------------------------------
            
            // ===== ANGLE ======================================================            
            if(indicator.getType()==SetupObject.Type.Angle){
                SO_Angle = (SetupObject)value;
                modelAngle.setValue(Double.parseDouble(SO_Angle.toString()));
                return spinAngle;
            }
            // ---------------------------------------------------------------------
            
            // ===== GRADIENTTYPE ======================================================            
            if(indicator.getType()==SetupObject.Type.GradientType){
                SO_GradientType = (SetupObject)value;
                comboGradientType.setSelectedItem(SO_GradientType.toString());
                return comboGradientType;
            }
            // ---------------------------------------------------------------------
            
            // ===== TWOSIDESGRADIENT ======================================================
            if(indicator.getType()==SetupObject.Type.GradientPaint){
                SO_TwoSides = (SetupObject)value;
                buttonTwoSides.setBackground(Color.gray);
                //htmlColor.setColor(SO_Color.get());
                return buttonTwoSides;
            }
            // ---------------------------------------------------------------------
            
            // ===== FOURSIDESGRADIENT ======================================================
            if(indicator.getType()==SetupObject.Type.FourSidesGradientPaint){
                SO_FourSides = (SetupObject)value;
                buttonFourSides.setBackground(Color.gray);
                //htmlColor.setColor(SO_Color.get());
                return buttonFourSides;
            }
            // ---------------------------------------------------------------------
            
            // ===== CHILD ======================================================
            if(indicator.getType()==SetupObject.Type.Child){
                SO_Child = (SetupObject)value;
                buttonChild.setBackground(Color.gray);
                //htmlColor.setColor(SO_Color.get());
                return buttonChild;
            }
            // ---------------------------------------------------------------------
            
        }
        
        return null;
    }
    
}
