/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.dialogs;

import encofx.lib.IO;
import encofx.lib.IO.EventLine;
import encofx.lib.effects.Text;
import encofx.lib.effects.TextArea;
import encofx.lib.effects.TextAreaCollection;
import encofx.lib.effects.TextCollection;
import encofx.lib.effects.VText;
import encofx.lib.effects.VTextCollection;
import encofx.lib.filefilter.SubtitleFilter;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.logging.Level;
import java.util.logging.Logger;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
import javax.swing.JFileChooser;
import javax.swing.SwingUtilities;
import javax.swing.filechooser.FileFilter;
import javax.swing.table.DefaultTableModel;
import javax.swing.table.TableColumn;

/**
 *
 * @author Yves
 */
public class AddTextDialog extends javax.swing.JDialog {

    private ButtonPressed bp;
    private DefaultTableModel subsModel;
    
    public enum ButtonPressed{
        NONE, OK_BUTTON, CANCEL_BUTTON;
    }
    
    /**
     * Creates new form AddTextDialog
     * @param parent
     * @param modal
     */
    public AddTextDialog(java.awt.Frame parent, boolean modal) {
        super(parent, modal);
        initComponents();
        init();
    }
    
    private void init(){
        bp = ButtonPressed.NONE;
        
        String[] subsHead = new String[]{"T", "L", "Marg.", "Start", "End",
                "Style", "Name", "Effect", "Text"};
        
        subsModel = new DefaultTableModel(
                null,
                subsHead
        ){
            Class[] types = new Class [] {
                    java.lang.String.class, java.lang.String.class, java.lang.String.class,
                    java.lang.String.class, java.lang.String.class, java.lang.String.class,
                    java.lang.String.class, java.lang.String.class, java.lang.String.class};
            boolean[] canEdit = new boolean [] {
                    false, true, true,
                    true, true, true,
                    true, true, true};
            @Override
            public Class getColumnClass(int columnIndex) {return types [columnIndex];}
            @Override
            public boolean isCellEditable(int rowIndex, int columnIndex) {return canEdit [columnIndex];}
        };
        
        subsTable.setModel(subsModel);
        
        TableColumn column;
        for (int i = 0; i < 9; i++) {
            column = subsTable.getColumnModel().getColumn(i);
            switch(i){
                case 0:
                    column.setPreferredWidth(30);
                    break; //Type
                case 1:
                    column.setPreferredWidth(30);
                    break; //Layer
                case 2:
                    column.setPreferredWidth(60);
                    break; //Margins
                case 3:
                    column.setPreferredWidth(90);
                    break; //Start
                case 4:
                    column.setPreferredWidth(90);
                    break; //End
                case 5:
                    column.setPreferredWidth(80);
                    break; //Style
                case 6:
                    column.setPreferredWidth(80);
                    break; //Name
                case 7:
                    column.setPreferredWidth(20);
                    break; //Effects
                case 8:
                    column.setPreferredWidth(700);
                    break; //Text                    
            }
        }
    }
    
    public List<TextCollection> showDialogForHText(){
        List<TextCollection> tcs = new ArrayList<>();
        setLocationRelativeTo(null);
        setVisible(true);
        
        if(bp.equals(ButtonPressed.OK_BUTTON)){
            if(rbNormalText.isSelected()){
                TextCollection tc = new TextCollection();
                tc.setText(tfNormalText.getText());
                tcs.add(tc);
                return tcs;
            }else{
                for(int i : subsTable.getSelectedRows()){
                    TextCollection tc = new TextCollection();
                    tc.setText((String)subsTable.getValueAt(i, 8));
                    
                    int start = Integer.parseInt(IO.getFrame((String)subsTable.getValueAt(i, 3), getFPS()));
                    int end = Integer.parseInt(IO.getFrame((String)subsTable.getValueAt(i, 4), getFPS()));
                    
                    Text before = new Text();
                    before.setFrame(start);
                    tc.add(before);
                    Text after = new Text();
                    after.setFrame(end);
                    tc.add(after);
                    
                    tcs.add(tc);
                }
                return tcs;
            }
        }
        return null;
    }
    
    public List<VTextCollection> showDialogForVText(){
        List<VTextCollection> vtcs = new ArrayList<>();
        setLocationRelativeTo(null);
        setVisible(true);
        
        if(bp.equals(ButtonPressed.OK_BUTTON)){
            if(rbNormalText.isSelected()){
                VTextCollection vtc = new VTextCollection();
                vtc.setText(tfNormalText.getText());
                vtcs.add(vtc);
                return vtcs;
            }else{
                for(int i : subsTable.getSelectedRows()){
                    VTextCollection vtc = new VTextCollection();
                    vtc.setText((String)subsTable.getValueAt(i, 8));
                    vtcs.add(vtc);
                }
                return vtcs;
            }
        }
        return null;
    }
    
    public List<TextAreaCollection> showDialogForTextArea(){
        List<TextAreaCollection> tcs = new ArrayList<>();
        setLocationRelativeTo(null);
        setVisible(true);
        
        if(bp.equals(ButtonPressed.OK_BUTTON)){
            if(rbNormalText.isSelected()){
                TextAreaCollection tc = new TextAreaCollection();
                tc.setText(tfNormalText.getText());
                tcs.add(tc);
                return tcs;
            }else{
                for(int i : subsTable.getSelectedRows()){
                    TextAreaCollection tc = new TextAreaCollection();
                    tc.setText((String)subsTable.getValueAt(i, 8));
                    
                    int start = Integer.parseInt(IO.getFrame((String)subsTable.getValueAt(i, 3), getFPS()));
                    int end = Integer.parseInt(IO.getFrame((String)subsTable.getValueAt(i, 4), getFPS()));
                    
                    TextArea before = new TextArea();
                    before.setFrame(start);
                    tc.add(before);
                    TextArea after = new TextArea();
                    after.setFrame(end);
                    tc.add(after);
                    
                    tcs.add(tc);
                }
                return tcs;
            }
        }
        return null;
    }
    
    public void setFPS(double fps){
        if(fps!=0d){
            tfFPS.setText(fps+"");
        }
    }
    
    public double getFPS(){
        try{
            return Double.parseDouble(tfFPS.getText());
        }catch(NumberFormatException e){
            return 25d;
        }        
    }
    
    public List<TextCollection> getSyllablesOnTextCollection(){
        if(rbSubsText.isSelected()){
            List<TextCollection> ltc = new ArrayList<>();
            for(int i : subsTable.getSelectedRows()){
                String text = (String)subsTable.getValueAt(i, 8);                
                if(text.contains("{\\")==true){                    
                    Pattern p = Pattern.compile("\\{([^\\}]+)\\}([A-Za-z0-9 ]+)");
                    Matcher m = p.matcher(text);
                    long ms_start = IO.getMilliseconds((String)subsTable.getValueAt(i, 3));
                    long ms_end = IO.getMilliseconds((String)subsTable.getValueAt(i, 4));
                    long ms_count = ms_start;
                    
                    int index = 0;
                    while(m.find()){
                        long ms_syl = IO.getSyllableMillisenconds(m.group(1).replace("\\k", ""));
                        
                        String syl = m.group(2);
                        TextCollection tc = new TextCollection();
                        tc.setNotStrippedSentence(text);
                        tc.setText(syl);
                        tc.setSyllableIndex(index);
                        
                        Text before = new Text();
                        before.setFrame(Integer.parseInt(IO.getFrame(ms_count, getFPS())));
                        before.setSyllable(true);
                        before.setSyllableIndex(index);
                        tc.add(before);
                        
                        Text after = new Text();
                        ms_count += ms_syl;
                        after.setFrame(Integer.parseInt(IO.getFrame(ms_count, getFPS())));
                        after.setSyllable(true);
                        after.setSyllableIndex(index);
                        tc.add(after);
                        
                        ltc.add(tc);
                        index += 1;
                    }                    
                }
            }
            return ltc;
        }
        return null;
    }
    
    public List<VTextCollection> getSyllablesOnVTextCollection(){
        if(rbSubsText.isSelected()){
            List<VTextCollection> ltc = new ArrayList<>();
            for(int i : subsTable.getSelectedRows()){
                String text = (String)subsTable.getValueAt(i, 8);                
                if(text.contains("{\\")==true){                    
                    Pattern p = Pattern.compile("\\{([^\\}]+)\\}([A-Za-z0-9 ]+)");
                    Matcher m = p.matcher(text);
                    long ms_start = IO.getMilliseconds((String)subsTable.getValueAt(i, 3));
                    long ms_end = IO.getMilliseconds((String)subsTable.getValueAt(i, 4));
                    long ms_count = ms_start;
                    
                    int index = 0;
                    while(m.find()){
                        long ms_syl = IO.getSyllableMillisenconds(m.group(1).replace("\\k", ""));
                        
                        String syl = m.group(2);
                        VTextCollection tc = new VTextCollection();
                        tc.setNotStrippedSentence(text);
                        tc.setText(syl);
                        tc.setSyllableIndex(index);
                        
                        VText before = new VText();
                        before.setFrame(Integer.parseInt(IO.getFrame(ms_count, getFPS())));
                        before.setSyllable(true);
                        before.setSyllableIndex(index);
                        tc.add(before);
                        
                        VText after = new VText();
                        ms_count += ms_syl;
                        after.setFrame(Integer.parseInt(IO.getFrame(ms_count, getFPS())));
                        after.setSyllable(true);
                        after.setSyllableIndex(index);
                        tc.add(after);
                        
                        ltc.add(tc);
                        index += 1;
                    }                    
                }
            }
            return ltc;
        }
        return null;
    }
    
    public List<TextAreaCollection> getSyllablesOnTextAreaCollection(){
        if(rbSubsText.isSelected()){
            List<TextAreaCollection> ltc = new ArrayList<>();
            for(int i : subsTable.getSelectedRows()){
                String text = (String)subsTable.getValueAt(i, 8);                
                if(text.contains("{\\")==true){                    
                    Pattern p = Pattern.compile("\\{([^\\}]+)\\}([A-Za-z0-9 ]+)");
                    Matcher m = p.matcher(text);
                    long ms_start = IO.getMilliseconds((String)subsTable.getValueAt(i, 3));
                    long ms_end = IO.getMilliseconds((String)subsTable.getValueAt(i, 4));
                    long ms_count = ms_start;
                    
                    int index = 0;
                    while(m.find()){
                        long ms_syl = IO.getSyllableMillisenconds(m.group(1).replace("\\k", ""));
                        
                        String syl = m.group(2);
                        TextAreaCollection tc = new TextAreaCollection();
                        tc.setNotStrippedSentence(text);
                        tc.setText(syl);
                        tc.setSyllableIndex(index);
                        
                        TextArea before = new TextArea();
                        before.setFrame(Integer.parseInt(IO.getFrame(ms_count, getFPS())));
                        before.setSyllable(true);
                        before.setSyllableIndex(index);
                        tc.add(before);
                        
                        TextArea after = new TextArea();
                        ms_count += ms_syl;
                        after.setFrame(Integer.parseInt(IO.getFrame(ms_count, getFPS())));
                        after.setSyllable(true);
                        after.setSyllableIndex(index);
                        tc.add(after);
                        
                        ltc.add(tc);
                        index += 1;
                    }                    
                }
            }
            return ltc;
        }
        return null;
    }

    /**
     * This method is called from within the constructor to initialize the form.
     * WARNING: Do NOT modify this code. The content of this method is always
     * regenerated by the Form Editor.
     */
    @SuppressWarnings("unchecked")
    // <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
    private void initComponents() {

        bgText = new javax.swing.ButtonGroup();
        fcText = new javax.swing.JFileChooser();
        OK_Button = new javax.swing.JButton();
        Cancel_Button = new javax.swing.JButton();
        jPanel5 = new javax.swing.JPanel();
        tfNormalText = new javax.swing.JTextField();
        rbNormalText = new javax.swing.JRadioButton();
        rbSubsText = new javax.swing.JRadioButton();
        jButton1 = new javax.swing.JButton();
        tfFPS = new javax.swing.JTextField();
        jLabel1 = new javax.swing.JLabel();
        jScrollPane4 = new javax.swing.JScrollPane();
        subsTable = new javax.swing.JTable();

        setDefaultCloseOperation(javax.swing.WindowConstants.DISPOSE_ON_CLOSE);

        OK_Button.setText("OK");
        OK_Button.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                OK_ButtonActionPerformed(evt);
            }
        });

        Cancel_Button.setText("Cancel");
        Cancel_Button.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                Cancel_ButtonActionPerformed(evt);
            }
        });

        tfNormalText.setText("And Java is the way you choose to add text !");

        bgText.add(rbNormalText);
        rbNormalText.setSelected(true);
        rbNormalText.setText("Normal text");

        bgText.add(rbSubsText);
        rbSubsText.setText("Subtitle's text");

        jButton1.setText("Open...");
        jButton1.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton1ActionPerformed(evt);
            }
        });

        tfFPS.setText("25");

        jLabel1.setText("FPS : ");

        javax.swing.GroupLayout jPanel5Layout = new javax.swing.GroupLayout(jPanel5);
        jPanel5.setLayout(jPanel5Layout);
        jPanel5Layout.setHorizontalGroup(
            jPanel5Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel5Layout.createSequentialGroup()
                .addContainerGap()
                .addGroup(jPanel5Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(jPanel5Layout.createSequentialGroup()
                        .addComponent(rbNormalText)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)
                        .addComponent(tfNormalText, javax.swing.GroupLayout.DEFAULT_SIZE, 623, Short.MAX_VALUE))
                    .addGroup(jPanel5Layout.createSequentialGroup()
                        .addComponent(rbSubsText)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                        .addComponent(jLabel1)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addComponent(tfFPS, javax.swing.GroupLayout.PREFERRED_SIZE, 50, javax.swing.GroupLayout.PREFERRED_SIZE)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addComponent(jButton1, javax.swing.GroupLayout.PREFERRED_SIZE, 100, javax.swing.GroupLayout.PREFERRED_SIZE)))
                .addContainerGap())
        );
        jPanel5Layout.setVerticalGroup(
            jPanel5Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel5Layout.createSequentialGroup()
                .addContainerGap()
                .addGroup(jPanel5Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(tfNormalText, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(rbNormalText))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(jPanel5Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(rbSubsText)
                    .addComponent(jButton1)
                    .addComponent(tfFPS, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(jLabel1))
                .addContainerGap(javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
        );

        subsTable.setModel(new javax.swing.table.DefaultTableModel(
            new Object [][] {
                {null, null, null, null},
                {null, null, null, null},
                {null, null, null, null},
                {null, null, null, null}
            },
            new String [] {
                "Title 1", "Title 2", "Title 3", "Title 4"
            }
        ));
        jScrollPane4.setViewportView(subsTable);

        javax.swing.GroupLayout layout = new javax.swing.GroupLayout(getContentPane());
        getContentPane().setLayout(layout);
        layout.setHorizontalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addComponent(jPanel5, javax.swing.GroupLayout.Alignment.TRAILING, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
            .addComponent(jScrollPane4, javax.swing.GroupLayout.DEFAULT_SIZE, 726, Short.MAX_VALUE)
            .addGroup(javax.swing.GroupLayout.Alignment.TRAILING, layout.createSequentialGroup()
                .addContainerGap(javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                .addComponent(Cancel_Button, javax.swing.GroupLayout.PREFERRED_SIZE, 100, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addComponent(OK_Button, javax.swing.GroupLayout.PREFERRED_SIZE, 100, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addContainerGap())
        );
        layout.setVerticalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(layout.createSequentialGroup()
                .addComponent(jPanel5, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addComponent(jScrollPane4, javax.swing.GroupLayout.PREFERRED_SIZE, 250, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(OK_Button)
                    .addComponent(Cancel_Button))
                .addContainerGap())
        );

        pack();
    }// </editor-fold>//GEN-END:initComponents

    private void OK_ButtonActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_OK_ButtonActionPerformed
        bp = ButtonPressed.OK_BUTTON;
        dispose();
    }//GEN-LAST:event_OK_ButtonActionPerformed

    private void Cancel_ButtonActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_Cancel_ButtonActionPerformed
        bp = ButtonPressed.CANCEL_BUTTON;
        dispose();
    }//GEN-LAST:event_Cancel_ButtonActionPerformed

    private void jButton1ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton1ActionPerformed
        for (FileFilter f : fcText.getChoosableFileFilters()){
            fcText.removeChoosableFileFilter(f);
        }
        fcText.addChoosableFileFilter(new SubtitleFilter());
        fcText.setDialogTitle("Choose the video...");
        SwingUtilities.updateComponentTreeUI(fcText);
        int z = fcText.showOpenDialog(this);
        if (z == JFileChooser.APPROVE_OPTION){
            try {
                List<EventLine> events = IO.extractASS(fcText.getSelectedFile().getAbsolutePath());
                for(EventLine ev : events){
                    Object[] row = {
                        ev.getType(),
                        ev.getLayer(),
                        ev.getMarginL()+","+ev.getMarginR()+","+ev.getMarginV(),
                        ev.getStart(),
                        ev.getEnd(),
                        ev.getStyle(),
                        ev.getName(),
                        ev.getEffect(),
                        ev.getText()
                    };
                    subsModel.addRow(row);
                }
            } catch (IOException ex) {
                Logger.getLogger(AddTextDialog.class.getName()).log(Level.SEVERE, null, ex);
            }
        }
    }//GEN-LAST:event_jButton1ActionPerformed
    
    /**
     * @param args the command line arguments
     */
    public static void main(String args[]) {
        /* Set the Nimbus look and feel */
        //<editor-fold defaultstate="collapsed" desc=" Look and feel setting code (optional) ">
        /* If Nimbus (introduced in Java SE 6) is not available, stay with the default look and feel.
         * For details see http://download.oracle.com/javase/tutorial/uiswing/lookandfeel/plaf.html 
         */
        try {
            for (javax.swing.UIManager.LookAndFeelInfo info : javax.swing.UIManager.getInstalledLookAndFeels()) {
                if ("Nimbus".equals(info.getName())) {
                    javax.swing.UIManager.setLookAndFeel(info.getClassName());
                    break;
                }
            }
        } catch (ClassNotFoundException | InstantiationException | IllegalAccessException | javax.swing.UnsupportedLookAndFeelException ex) {
            java.util.logging.Logger.getLogger(AddTextDialog.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        }
        //</editor-fold>

        /* Create and display the dialog */
        java.awt.EventQueue.invokeLater(new Runnable() {
            @Override
            public void run() {
                AddTextDialog dialog = new AddTextDialog(new javax.swing.JFrame(), true);
                dialog.addWindowListener(new java.awt.event.WindowAdapter() {
                    @Override
                    public void windowClosing(java.awt.event.WindowEvent e) {
                        System.exit(0);
                    }
                });
                dialog.setVisible(true);
            }
        });
    }

    // Variables declaration - do not modify//GEN-BEGIN:variables
    private javax.swing.JButton Cancel_Button;
    private javax.swing.JButton OK_Button;
    private javax.swing.ButtonGroup bgText;
    private javax.swing.JFileChooser fcText;
    private javax.swing.JButton jButton1;
    private javax.swing.JLabel jLabel1;
    private javax.swing.JPanel jPanel5;
    private javax.swing.JScrollPane jScrollPane4;
    private javax.swing.JRadioButton rbNormalText;
    private javax.swing.JRadioButton rbSubsText;
    private javax.swing.JTable subsTable;
    private javax.swing.JTextField tfFPS;
    private javax.swing.JTextField tfNormalText;
    // End of variables declaration//GEN-END:variables
}
