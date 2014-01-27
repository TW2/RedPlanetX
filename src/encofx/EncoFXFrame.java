/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx;

import encofx.lib.Configuration;
import encofx.lib.FXObject;
import encofx.lib.ObjectCollectionInterface;
import encofx.lib.SubObjects;
import encofx.lib.VTD2;
import encofx.lib.dialogs.AddTextDialog;
import encofx.lib.dialogs.InputDialog;
import encofx.lib.editors.ChangeSettings;
import encofx.lib.effects.Parent;
import encofx.lib.effects.ParentCollection;
import encofx.lib.effects.Text;
import encofx.lib.effects.TextCollection;
import encofx.lib.effects.VText;
import encofx.lib.effects.VTextCollection;
import encofx.lib.filefilter.VideoFilter;
import encofx.lib.properties.AbstractProperty;
import encofx.lib.renderers.DisplaySettingsDeluxe;
import encofx.lib.renderers.NodeRenderer;
import encofx.lib.settings.SetupObject;
import encofx.lib.xuggle.VideoInfo;
import java.awt.Desktop;
import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.swing.JFileChooser;
import javax.swing.JOptionPane;
import javax.swing.SwingUtilities;
import javax.swing.UIManager;
import javax.swing.UnsupportedLookAndFeelException;
import javax.swing.filechooser.FileFilter;
import javax.swing.plaf.nimbus.NimbusLookAndFeel;
import javax.swing.table.DefaultTableModel;
import javax.swing.table.TableColumn;
import javax.swing.tree.DefaultMutableTreeNode;
import javax.swing.tree.DefaultTreeModel;
import org.json.simple.parser.ParseException;

/**
 *
 * @author Yves
 */
public class EncoFXFrame extends javax.swing.JFrame {
    
    private final VTD2 vtd = new VTD2();
    private final List<ObjectCollectionInterface> collection = new ArrayList<>();
    private long end_frame = 100;
    private VideoInfo videoInfo = new VideoInfo();
    private String CONFIG_FOLDER = "";
    private Configuration configuration = null;
    private List<ParentCollection> parents = new ArrayList<>();
    
    //For tree    
    private final DefaultMutableTreeNode root = new DefaultMutableTreeNode("Objects");
    private final DefaultTreeModel treeModel = new DefaultTreeModel(root);
    private final NodeRenderer noderenderer = new NodeRenderer(videoInfo);
    
    //For table
    private DefaultTableModel tableModel;
    private ChangeSettings changeSettings = null;

    /**
     * Creates new form EncoFXFrame
     */
    public EncoFXFrame() {
        initComponents();        
        init();
    }
    
    private void init(){
        CONFIG_FOLDER = getApplicationDirectory()+File.separator+"settings";
        File configFolder = new File(CONFIG_FOLDER);
        if(configFolder.exists()==false){
            configFolder.mkdir();
        }
        configuration = new Configuration();
        
        objectsTree.setModel(treeModel);
        objectsTree.setCellRenderer(noderenderer);
        
        String[] head = new String[]{"Properties", "Settings"};
        
        tableModel = new DefaultTableModel(
                null,
                head
        ){
            Class[] types = new Class [] {
                    java.lang.String.class, SetupObject.class};
            boolean[] canEdit = new boolean [] {
                    false, true};
            @Override
            public Class getColumnClass(int columnIndex) {return types [columnIndex];}
            @Override
            public boolean isCellEditable(int rowIndex, int columnIndex) {return canEdit [columnIndex];}
        };
        
        propTable.setModel(tableModel);
        
        TableColumn column;
        for (int i = 0; i < 2; i++) {
            column = propTable.getColumnModel().getColumn(i);
            switch(i){
                case 0:
                    column.setPreferredWidth(50);
                    break; //Properties
                case 1:
                    column.setPreferredWidth(50);
                    break; //Settings
            }
        }
        
        changeSettings = new ChangeSettings(this);
        changeSettings.setParents(parents);
        
        propTable.setDefaultEditor(SetupObject.class, changeSettings);
        propTable.setDefaultRenderer(SetupObject.class, new DisplaySettingsDeluxe());
        propTable.setRowHeight(25);
        
        vtd.init();
        jPanel1.add(vtd);
        configureVTD();
        
//        TextCollection col001 = new TextCollection();
//        Text textAtStart = new Text();
//        textAtStart.setFrame(0);
//        textAtStart.setSize(50f);
//        textAtStart.setColor(Color.yellow);
//        Text textAtEnd = new Text();
//        textAtEnd.setFrame(50);
//        textAtEnd.setSize(300f);
//        textAtEnd.setColor(Color.pink);
//        Text textAfter = new Text();
//        textAfter.setFrame(100);
//        textAfter.setSize(150f);
//        textAfter.setColor(Color.cyan);
//        col001.add(textAtStart);
//        col001.add(textAtEnd);
//        col001.add(textAfter);
//        collection.add(col001);
//        
//        vtd.setCollections(collection);
        
        try {
            UIManager.setLookAndFeel(new NimbusLookAndFeel());
            SwingUtilities.updateComponentTreeUI(this);
        } catch (UnsupportedLookAndFeelException ex) {
            //Do nothing
        }
        
        setLocationRelativeTo(null);
        
//        ParentCollection pc = new ParentCollection();
//        pc.setText("None");
//        parents.add(pc);
    }
    
    private void forceUpdate(){
        vtd.setFrame(jSlider1.getValue());
    }
    
    private String getApplicationDirectory(){
        if(System.getProperty("os.name").equalsIgnoreCase("Mac OS X")){
            java.io.File file = new java.io.File("");
            return file.getAbsolutePath();
        }
        String path = System.getProperty("user.dir");
        if(path.toLowerCase().contains("jre")){
            File f = new File(getClass().getProtectionDomain()
                    .getCodeSource().getLocation().toString()
                    .substring(6));
            path = f.getParent();
        }
        return path;
    }
    
    private void configureVTD(){
        if(videoInfo!=null){
            vtd.setWidth(videoInfo.getVideoWidth());
            vtd.setHeight(videoInfo.getVideoHeight());
            int xWidth = jPanel1.getWidth()<videoInfo.getVideoWidth() ? jPanel1.getWidth() : videoInfo.getVideoWidth();
            int xHeight = jPanel1.getHeight()<videoInfo.getVideoHeight()? jPanel1.getHeight(): videoInfo.getVideoHeight();
            System.out.println(xWidth+" / "+xHeight);
            vtd.setVirtualHeight(xHeight);
            vtd.setVirtualWidth(xWidth);
            int x = (jPanel1.getWidth() - xWidth) / 2;
            int y = (jPanel1.getHeight() - xHeight) / 2;
            vtd.setLocation(x, y);
            jPanel1.updateUI();
            forceUpdate();
        }
    }
    
    private void expandTree(){
        for (int i=0; i<objectsTree.getRowCount(); i++){
            objectsTree.expandRow(i);
        }
    }
    
    public void expandTree(int from, int to){
        for (int i=0; i<objectsTree.getRowCount(); i++){
            if(i>=from && i<=to){
                objectsTree.expandRow(i);
            }            
        }
    }
    
    private void collapseTree(){
        for (int i=0; i<objectsTree.getRowCount(); i++){
            objectsTree.collapseRow(i);
        }
    }
    
    public void collapseTree(int from, int to){
        for (int i=0; i<objectsTree.getRowCount(); i++){
            if(i>=from && i<=to){
                objectsTree.collapseRow(i);
            }            
        }
    }
    
    private void updateTree(){
        root.removeAllChildren();
        for (ObjectCollectionInterface obj : collection){
            //Add the collection
            DefaultMutableTreeNode node = new DefaultMutableTreeNode(obj);
            root.add(node);
            for(int i=0 ; i<obj.getSubObjects().getObjects().size(); i++){
                Object o = obj.getSubObjects().getObjects().get(i);
                DefaultMutableTreeNode subnode = new DefaultMutableTreeNode(o);
                node.add(subnode);
            }
        }
        objectsTree.updateUI();
    }

    /**
     * This method is called from within the constructor to initialize the form.
     * WARNING: Do NOT modify this code. The content of this method is always
     * regenerated by the Form Editor.
     */
    @SuppressWarnings("unchecked")
    // <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
    private void initComponents() {

        treePopup = new javax.swing.JPopupMenu();
        popmExpandAll = new javax.swing.JMenuItem();
        popmCollapseAll = new javax.swing.JMenuItem();
        jSeparator2 = new javax.swing.JPopupMenu.Separator();
        popmExpand = new javax.swing.JMenuItem();
        popmCollapse = new javax.swing.JMenuItem();
        jSeparator1 = new javax.swing.JPopupMenu.Separator();
        popmAddEvent = new javax.swing.JMenuItem();
        popmRemoveEvent = new javax.swing.JMenuItem();
        jSeparator3 = new javax.swing.JPopupMenu.Separator();
        popmParent = new javax.swing.JMenuItem();
        fcOpenVideo = new javax.swing.JFileChooser();
        fcSaveFolder = new javax.swing.JFileChooser();
        jSlider1 = new javax.swing.JSlider();
        jPanel1 = new javax.swing.JPanel();
        jToolBar1 = new javax.swing.JToolBar();
        btnAddHText = new javax.swing.JButton();
        btnAddVText = new javax.swing.JButton();
        btnAddTextArea = new javax.swing.JButton();
        btnAddRectangle = new javax.swing.JButton();
        btnAddRoundRectangle = new javax.swing.JButton();
        btnAddEllipse = new javax.swing.JButton();
        btnAddFreeShape = new javax.swing.JButton();
        btnAddDrawing = new javax.swing.JButton();
        btnAddPicture = new javax.swing.JButton();
        btnAddVideo = new javax.swing.JButton();
        jScrollPane1 = new javax.swing.JScrollPane();
        objectsTree = new javax.swing.JTree();
        jScrollPane2 = new javax.swing.JScrollPane();
        propTable = new javax.swing.JTable();
        lblFrame = new javax.swing.JLabel();
        jMenuBar1 = new javax.swing.JMenuBar();
        jMenu1 = new javax.swing.JMenu();
        mnuOpenVideo = new javax.swing.JMenuItem();
        mnuGoToSaveFolder = new javax.swing.JMenuItem();
        mnuEncodeVideo = new javax.swing.JMenuItem();
        jMenu2 = new javax.swing.JMenu();

        popmExpandAll.setIcon(new javax.swing.ImageIcon(getClass().getResource("/images/20px-Crystal_Clear_action_forward.png"))); // NOI18N
        popmExpandAll.setText("Expand all");
        popmExpandAll.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                popmExpandAllActionPerformed(evt);
            }
        });
        treePopup.add(popmExpandAll);

        popmCollapseAll.setIcon(new javax.swing.ImageIcon(getClass().getResource("/images/20px-Crystal_Clear_action_back.png"))); // NOI18N
        popmCollapseAll.setText("Collapse all");
        popmCollapseAll.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                popmCollapseAllActionPerformed(evt);
            }
        });
        treePopup.add(popmCollapseAll);
        treePopup.add(jSeparator2);

        popmExpand.setIcon(new javax.swing.ImageIcon(getClass().getResource("/images/20px-Crystal_Clear_action_forward.png"))); // NOI18N
        popmExpand.setText("Expand branch");
        popmExpand.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                popmExpandActionPerformed(evt);
            }
        });
        treePopup.add(popmExpand);

        popmCollapse.setIcon(new javax.swing.ImageIcon(getClass().getResource("/images/20px-Crystal_Clear_action_back.png"))); // NOI18N
        popmCollapse.setText("Collapse branch");
        popmCollapse.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                popmCollapseActionPerformed(evt);
            }
        });
        treePopup.add(popmCollapse);
        treePopup.add(jSeparator1);

        popmAddEvent.setIcon(new javax.swing.ImageIcon(getClass().getResource("/images/20px-Crystal_Clear_action_edit_add.png"))); // NOI18N
        popmAddEvent.setText("Add event");
        popmAddEvent.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                popmAddEventActionPerformed(evt);
            }
        });
        treePopup.add(popmAddEvent);

        popmRemoveEvent.setIcon(new javax.swing.ImageIcon(getClass().getResource("/images/20px-Crystal_Clear_action_edit_remove.png"))); // NOI18N
        popmRemoveEvent.setText("Remove event or object");
        popmRemoveEvent.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                popmRemoveEventActionPerformed(evt);
            }
        });
        treePopup.add(popmRemoveEvent);
        treePopup.add(jSeparator3);

        popmParent.setIcon(new javax.swing.ImageIcon(getClass().getResource("/images/20px-Crystal_Clear_app_kllckety.png"))); // NOI18N
        popmParent.setText("Create parent...");
        popmParent.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                popmParentActionPerformed(evt);
            }
        });
        treePopup.add(popmParent);

        setDefaultCloseOperation(javax.swing.WindowConstants.EXIT_ON_CLOSE);
        setTitle("RedPlanet Xpress \"Little Phoenix\"");
        addComponentListener(new java.awt.event.ComponentAdapter() {
            public void componentResized(java.awt.event.ComponentEvent evt) {
                formComponentResized(evt);
            }
        });

        jSlider1.addChangeListener(new javax.swing.event.ChangeListener() {
            public void stateChanged(javax.swing.event.ChangeEvent evt) {
                jSlider1StateChanged(evt);
            }
        });

        jPanel1.setBackground(new java.awt.Color(51, 153, 255));

        javax.swing.GroupLayout jPanel1Layout = new javax.swing.GroupLayout(jPanel1);
        jPanel1.setLayout(jPanel1Layout);
        jPanel1Layout.setHorizontalGroup(
            jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGap(0, 0, Short.MAX_VALUE)
        );
        jPanel1Layout.setVerticalGroup(
            jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGap(0, 0, Short.MAX_VALUE)
        );

        jToolBar1.setFloatable(false);
        jToolBar1.setRollover(true);

        btnAddHText.setIcon(new javax.swing.ImageIcon(getClass().getResource("/images/32px-h-text.png"))); // NOI18N
        btnAddHText.setToolTipText("Add text");
        btnAddHText.setFocusable(false);
        btnAddHText.setHorizontalTextPosition(javax.swing.SwingConstants.CENTER);
        btnAddHText.setVerticalTextPosition(javax.swing.SwingConstants.BOTTOM);
        btnAddHText.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                btnAddHTextActionPerformed(evt);
            }
        });
        jToolBar1.add(btnAddHText);

        btnAddVText.setIcon(new javax.swing.ImageIcon(getClass().getResource("/images/32px-v-text.png"))); // NOI18N
        btnAddVText.setFocusable(false);
        btnAddVText.setHorizontalTextPosition(javax.swing.SwingConstants.CENTER);
        btnAddVText.setVerticalTextPosition(javax.swing.SwingConstants.BOTTOM);
        btnAddVText.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                btnAddVTextActionPerformed(evt);
            }
        });
        jToolBar1.add(btnAddVText);

        btnAddTextArea.setIcon(new javax.swing.ImageIcon(getClass().getResource("/images/32px-z-text.png"))); // NOI18N
        btnAddTextArea.setFocusable(false);
        btnAddTextArea.setHorizontalTextPosition(javax.swing.SwingConstants.CENTER);
        btnAddTextArea.setVerticalTextPosition(javax.swing.SwingConstants.BOTTOM);
        jToolBar1.add(btnAddTextArea);

        btnAddRectangle.setIcon(new javax.swing.ImageIcon(getClass().getResource("/images/32px-rect-shape.png"))); // NOI18N
        btnAddRectangle.setFocusable(false);
        btnAddRectangle.setHorizontalTextPosition(javax.swing.SwingConstants.CENTER);
        btnAddRectangle.setVerticalTextPosition(javax.swing.SwingConstants.BOTTOM);
        jToolBar1.add(btnAddRectangle);

        btnAddRoundRectangle.setIcon(new javax.swing.ImageIcon(getClass().getResource("/images/32px-rrect-shape.png"))); // NOI18N
        btnAddRoundRectangle.setFocusable(false);
        btnAddRoundRectangle.setHorizontalTextPosition(javax.swing.SwingConstants.CENTER);
        btnAddRoundRectangle.setVerticalTextPosition(javax.swing.SwingConstants.BOTTOM);
        jToolBar1.add(btnAddRoundRectangle);

        btnAddEllipse.setIcon(new javax.swing.ImageIcon(getClass().getResource("/images/32px-ellipse-shape.png"))); // NOI18N
        btnAddEllipse.setFocusable(false);
        btnAddEllipse.setHorizontalTextPosition(javax.swing.SwingConstants.CENTER);
        btnAddEllipse.setVerticalTextPosition(javax.swing.SwingConstants.BOTTOM);
        jToolBar1.add(btnAddEllipse);

        btnAddFreeShape.setIcon(new javax.swing.ImageIcon(getClass().getResource("/images/32px-free-shape.png"))); // NOI18N
        btnAddFreeShape.setFocusable(false);
        btnAddFreeShape.setHorizontalTextPosition(javax.swing.SwingConstants.CENTER);
        btnAddFreeShape.setVerticalTextPosition(javax.swing.SwingConstants.BOTTOM);
        jToolBar1.add(btnAddFreeShape);

        btnAddDrawing.setIcon(new javax.swing.ImageIcon(getClass().getResource("/images/32px-Crystal_Clear_app_kcoloredit.png"))); // NOI18N
        btnAddDrawing.setFocusable(false);
        btnAddDrawing.setHorizontalTextPosition(javax.swing.SwingConstants.CENTER);
        btnAddDrawing.setVerticalTextPosition(javax.swing.SwingConstants.BOTTOM);
        jToolBar1.add(btnAddDrawing);

        btnAddPicture.setIcon(new javax.swing.ImageIcon(getClass().getResource("/images/32px-Crystal_Clear_app_kpaint.png"))); // NOI18N
        btnAddPicture.setFocusable(false);
        btnAddPicture.setHorizontalTextPosition(javax.swing.SwingConstants.CENTER);
        btnAddPicture.setVerticalTextPosition(javax.swing.SwingConstants.BOTTOM);
        jToolBar1.add(btnAddPicture);

        btnAddVideo.setIcon(new javax.swing.ImageIcon(getClass().getResource("/images/32px-Crystal_Clear_app_camera.png"))); // NOI18N
        btnAddVideo.setFocusable(false);
        btnAddVideo.setHorizontalTextPosition(javax.swing.SwingConstants.CENTER);
        btnAddVideo.setVerticalTextPosition(javax.swing.SwingConstants.BOTTOM);
        jToolBar1.add(btnAddVideo);

        objectsTree.setComponentPopupMenu(treePopup);
        objectsTree.addTreeSelectionListener(new javax.swing.event.TreeSelectionListener() {
            public void valueChanged(javax.swing.event.TreeSelectionEvent evt) {
                objectsTreeValueChanged(evt);
            }
        });
        jScrollPane1.setViewportView(objectsTree);

        jScrollPane2.setVerticalScrollBarPolicy(javax.swing.ScrollPaneConstants.VERTICAL_SCROLLBAR_ALWAYS);

        propTable.setModel(new javax.swing.table.DefaultTableModel(
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
        propTable.addPropertyChangeListener(new java.beans.PropertyChangeListener() {
            public void propertyChange(java.beans.PropertyChangeEvent evt) {
                propTablePropertyChange(evt);
            }
        });
        jScrollPane2.setViewportView(propTable);

        lblFrame.setFont(new java.awt.Font("Tahoma", 1, 18)); // NOI18N
        lblFrame.setHorizontalAlignment(javax.swing.SwingConstants.CENTER);
        lblFrame.setText("50");

        jMenu1.setText("File");

        mnuOpenVideo.setText("Open a video...");
        mnuOpenVideo.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                mnuOpenVideoActionPerformed(evt);
            }
        });
        jMenu1.add(mnuOpenVideo);

        mnuGoToSaveFolder.setText("Go to save folder");
        mnuGoToSaveFolder.setEnabled(false);
        mnuGoToSaveFolder.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                mnuGoToSaveFolderActionPerformed(evt);
            }
        });
        jMenu1.add(mnuGoToSaveFolder);

        mnuEncodeVideo.setText("Encode video");
        mnuEncodeVideo.setEnabled(false);
        mnuEncodeVideo.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                mnuEncodeVideoActionPerformed(evt);
            }
        });
        jMenu1.add(mnuEncodeVideo);

        jMenuBar1.add(jMenu1);

        jMenu2.setText("Edit");
        jMenuBar1.add(jMenu2);

        setJMenuBar(jMenuBar1);

        javax.swing.GroupLayout layout = new javax.swing.GroupLayout(getContentPane());
        getContentPane().setLayout(layout);
        layout.setHorizontalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(layout.createSequentialGroup()
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING, false)
                    .addComponent(jScrollPane1, javax.swing.GroupLayout.DEFAULT_SIZE, 258, Short.MAX_VALUE)
                    .addComponent(jScrollPane2, javax.swing.GroupLayout.PREFERRED_SIZE, 0, Short.MAX_VALUE))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addComponent(jPanel1, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                    .addGroup(layout.createSequentialGroup()
                        .addComponent(jSlider1, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addComponent(lblFrame, javax.swing.GroupLayout.PREFERRED_SIZE, 100, javax.swing.GroupLayout.PREFERRED_SIZE))))
            .addComponent(jToolBar1, javax.swing.GroupLayout.DEFAULT_SIZE, 1162, Short.MAX_VALUE)
        );
        layout.setVerticalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(javax.swing.GroupLayout.Alignment.TRAILING, layout.createSequentialGroup()
                .addComponent(jToolBar1, javax.swing.GroupLayout.PREFERRED_SIZE, 34, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(layout.createSequentialGroup()
                        .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING, false)
                            .addComponent(jSlider1, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                            .addComponent(lblFrame, javax.swing.GroupLayout.DEFAULT_SIZE, 31, Short.MAX_VALUE))
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addComponent(jPanel1, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
                    .addGroup(layout.createSequentialGroup()
                        .addComponent(jScrollPane1, javax.swing.GroupLayout.PREFERRED_SIZE, 308, javax.swing.GroupLayout.PREFERRED_SIZE)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addComponent(jScrollPane2, javax.swing.GroupLayout.DEFAULT_SIZE, 299, Short.MAX_VALUE))))
        );

        pack();
    }// </editor-fold>//GEN-END:initComponents

    private void jSlider1StateChanged(javax.swing.event.ChangeEvent evt) {//GEN-FIRST:event_jSlider1StateChanged
        vtd.setFrame(jSlider1.getValue());
        lblFrame.setText(jSlider1.getValue()+"");
    }//GEN-LAST:event_jSlider1StateChanged

    private void btnAddHTextActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_btnAddHTextActionPerformed
        AddTextDialog atd = new AddTextDialog(this, true);
        TextCollection tc = atd.showDialogForHText();
        if(tc!=null){
//            //Add the collection
//            DefaultMutableTreeNode textnode = new DefaultMutableTreeNode(tc);
//            root.add(textnode);
            
            //If list is empty (normal text case)
            if(tc.getList().isEmpty()){
                Text before = new Text();
                before.setFrame(0);
                tc.add(before);
                Text after = new Text();
                after.setFrame(Integer.parseInt(Long.toString(end_frame)));
                tc.add(after);
            }
            
//            //Add any Text object of the collection to the tree
//            for(Text t : tc.getList()){
//                DefaultMutableTreeNode property = new DefaultMutableTreeNode(t);
//                textnode.add(property);
//            }
            
            //Add the collection to the program
            collection.add(tc);
            
            //Refresh the VTD
            vtd.setCollections(collection);
            
            updateTree();
            expandTree();
        }
    }//GEN-LAST:event_btnAddHTextActionPerformed

    private void objectsTreeValueChanged(javax.swing.event.TreeSelectionEvent evt) {//GEN-FIRST:event_objectsTreeValueChanged
        //Clear
        try{
            for (int i=tableModel.getRowCount()-1;i>=0;i--){
                tableModel.removeRow(i);
            }
        }catch(Exception exc){}
        //Fill
        try{
            DefaultMutableTreeNode tn = (DefaultMutableTreeNode)objectsTree.getSelectionPath().getLastPathComponent();
            if(tn.getUserObject() instanceof TextCollection){
                TextCollection tc = (TextCollection)tn.getUserObject();
                List<AbstractProperty> properties = tc.getProperties();
                for(AbstractProperty p : properties){
                    Object[] row = new Object[]{p, p.getObject()};
                    tableModel.addRow(row);
                }
            }else if(tn.getUserObject() instanceof VTextCollection){
                VTextCollection vtc = (VTextCollection)tn.getUserObject();
                List<AbstractProperty> properties = vtc.getProperties();
                for(AbstractProperty p : properties){
                    Object[] row = new Object[]{p, p.getObject()};
                    tableModel.addRow(row);
                }
            }else if(tn.getUserObject() instanceof ParentCollection){
                ParentCollection pc = (ParentCollection)tn.getUserObject();
                List<AbstractProperty> properties = pc.getProperties();
                for(AbstractProperty p : properties){
                    Object[] row = new Object[]{p, p.getObject()};
                    tableModel.addRow(row);
                }
            }
            if(tn.getUserObject() instanceof Text){
                Text t = (Text)tn.getUserObject();
                List<AbstractProperty> properties = t.getProperties();
                for(AbstractProperty p : properties){
                    Object[] row = new Object[]{p, p.getObject()};
                    tableModel.addRow(row);
                }
            }else if(tn.getUserObject() instanceof VText){
                VText vt = (VText)tn.getUserObject();
                List<AbstractProperty> properties = vt.getProperties();
                for(AbstractProperty p : properties){
                    Object[] row = new Object[]{p, p.getObject()};
                    tableModel.addRow(row);
                }
            }else if(tn.getUserObject() instanceof Parent){
                Parent pr = (Parent)tn.getUserObject();
                List<AbstractProperty> properties = pr.getProperties();
                for(AbstractProperty p : properties){
                    Object[] row = new Object[]{p, p.getObject()};
                    tableModel.addRow(row);
                }
            }
            forceUpdate();
        }catch(Exception e){}
    }//GEN-LAST:event_objectsTreeValueChanged

    private void propTablePropertyChange(java.beans.PropertyChangeEvent evt) {//GEN-FIRST:event_propTablePropertyChange
        forceUpdate();
    }//GEN-LAST:event_propTablePropertyChange

    private void mnuOpenVideoActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_mnuOpenVideoActionPerformed
        for (FileFilter f : fcOpenVideo.getChoosableFileFilters()){
            fcOpenVideo.removeChoosableFileFilter(f);
        }
        fcOpenVideo.addChoosableFileFilter(new VideoFilter());
        fcSaveFolder.setDialogTitle("Choose the video...");
        SwingUtilities.updateComponentTreeUI(fcOpenVideo);
        int z = fcOpenVideo.showOpenDialog(this);
        if (z == JFileChooser.APPROVE_OPTION){            
            videoInfo.setVideo(fcOpenVideo.getSelectedFile().getAbsolutePath());
            
            File configFolder = new File(CONFIG_FOLDER);
            File searchFor = new File(configFolder,fcOpenVideo.getSelectedFile().getName()+".json");
            boolean isInConf = configuration.isInConfigFolder(searchFor);
            if(isInConf==true){
                try {
                    videoInfo = configuration.fromJSON(searchFor);
                    end_frame = videoInfo.getFrames();
                    jSlider1.setMaximum(Integer.parseInt(Long.toString(end_frame)));
                    vtd.setVideoInfo(videoInfo);
                    mnuGoToSaveFolder.setEnabled(true);
                    mnuEncodeVideo.setEnabled(true);
                } catch ( IOException | ParseException ex) {
                    Logger.getLogger(EncoFXFrame.class.getName()).log(Level.SEVERE, null, ex);
                }
            }
            
            File savePath = new File(videoInfo.getSaveFolder());
            if(isInConf==false || savePath.listFiles().length == 0){
                for (FileFilter f : fcSaveFolder.getChoosableFileFilters()){
                    fcSaveFolder.removeChoosableFileFilter(f);
                }
                fcSaveFolder.setFileSelectionMode(JFileChooser.DIRECTORIES_ONLY);
                fcSaveFolder.setDialogTitle("Choose the save folder...");
                SwingUtilities.updateComponentTreeUI(fcSaveFolder);
                int y = fcSaveFolder.showSaveDialog(this);
                if (y == JFileChooser.APPROVE_OPTION){
                    videoInfo.setSaveFolder(fcSaveFolder.getSelectedFile().getAbsolutePath());
                    videoInfo.extractVideo();
                    end_frame = videoInfo.getFrames();
                    System.out.println(end_frame);
                    jSlider1.setMaximum(Integer.parseInt(Long.toString(end_frame)));
                    vtd.setVideoInfo(videoInfo);
                    mnuGoToSaveFolder.setEnabled(true);
                    mnuEncodeVideo.setEnabled(true);
                    try {
                        configuration.createJSON(videoInfo);
                    } catch (IOException ex) {
                        Logger.getLogger(EncoFXFrame.class.getName()).log(Level.SEVERE, null, ex);
                    }
                    forceUpdate();
                }
            }
            configureVTD();
            noderenderer.updateVideoInfo(videoInfo);
        }
    }//GEN-LAST:event_mnuOpenVideoActionPerformed

    private void popmAddEventActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_popmAddEventActionPerformed
        DefaultMutableTreeNode tn = (DefaultMutableTreeNode)objectsTree.getSelectionPath().getLastPathComponent();
        if(tn.getUserObject() instanceof TextCollection){
            TextCollection tc = (TextCollection)tn.getUserObject();
            Text text = new Text();
            text.setFrame(jSlider1.getValue());
            tc.add(text);
            tc.sortByFrames();
            updateTree();
            expandTree();
        }
        if(tn.getUserObject() instanceof VTextCollection){
            VTextCollection vtc = (VTextCollection)tn.getUserObject();
            VText text = new VText();
            text.setFrame(jSlider1.getValue());
            vtc.add(text);
            vtc.sortByFrames();
            updateTree();
            expandTree();
        }
    }//GEN-LAST:event_popmAddEventActionPerformed

    private void popmRemoveEventActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_popmRemoveEventActionPerformed
        DefaultMutableTreeNode tn = (DefaultMutableTreeNode)objectsTree.getSelectionPath().getLastPathComponent();
        int z = JOptionPane.showConfirmDialog(
                this,
                "Do you really want to delete this object ?",
                "Delete this object ?",
                JOptionPane.YES_NO_OPTION,
                JOptionPane.QUESTION_MESSAGE);
        if(z == JOptionPane.YES_OPTION){
            if(tn.getUserObject() instanceof FXObject){
                FXObject fx = (FXObject)tn.getUserObject();
                for(ObjectCollectionInterface oci : collection){
                    SubObjects so = oci.getSubObjects();
                    if(so.contains(fx)){
                        so.removeObject(fx);
                        oci.setSubObjects(so);
                    }
                }
            }else if(tn.getUserObject() instanceof ObjectCollectionInterface){
                ObjectCollectionInterface oci = (ObjectCollectionInterface)tn.getUserObject();
                collection.remove(oci);
            }
            updateTree();
            expandTree();
        }
    }//GEN-LAST:event_popmRemoveEventActionPerformed

    private void mnuEncodeVideoActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_mnuEncodeVideoActionPerformed
        videoInfo.setVirtualTimeDisplay(vtd);
        videoInfo.encodeVideo();
    }//GEN-LAST:event_mnuEncodeVideoActionPerformed

    private void mnuGoToSaveFolderActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_mnuGoToSaveFolderActionPerformed
        try {
            Desktop.getDesktop().open(new File(videoInfo.getSaveFolder()));
        } catch (IOException ex) {
            Logger.getLogger(EncoFXFrame.class.getName()).log(Level.SEVERE, null, ex);
        }
    }//GEN-LAST:event_mnuGoToSaveFolderActionPerformed

    private void formComponentResized(java.awt.event.ComponentEvent evt) {//GEN-FIRST:event_formComponentResized
        configureVTD();
    }//GEN-LAST:event_formComponentResized

    private void popmExpandAllActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_popmExpandAllActionPerformed
        expandTree();
    }//GEN-LAST:event_popmExpandAllActionPerformed

    private void popmCollapseAllActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_popmCollapseAllActionPerformed
        collapseTree();
    }//GEN-LAST:event_popmCollapseAllActionPerformed

    private void popmExpandActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_popmExpandActionPerformed
        int[] rows = objectsTree.getSelectionRows();
        expandTree(rows[0], rows[rows.length-1]);
    }//GEN-LAST:event_popmExpandActionPerformed

    private void popmCollapseActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_popmCollapseActionPerformed
        int[] rows = objectsTree.getSelectionRows();
        collapseTree(rows[0], rows[rows.length-1]);
    }//GEN-LAST:event_popmCollapseActionPerformed

    private void btnAddVTextActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_btnAddVTextActionPerformed
        AddTextDialog atd = new AddTextDialog(this, true);
        VTextCollection vtc = atd.showDialogForVText();
        if(vtc!=null){
            //If list is empty (normal text case)
            if(vtc.getList().isEmpty()){
                VText before = new VText();
                before.setFrame(0);
                vtc.add(before);
                VText after = new VText();
                after.setFrame(Integer.parseInt(Long.toString(end_frame)));
                vtc.add(after);
            }
            //Add the collection to the program
            collection.add(vtc);
            //Refresh the VTD
            vtd.setCollections(collection);
            //Refesh tree
            updateTree();
            expandTree();
        }
    }//GEN-LAST:event_btnAddVTextActionPerformed

    private void popmParentActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_popmParentActionPerformed
        ParentCollection pc = new ParentCollection();
        Parent before = new Parent();
        before.setFrame(0);
        pc.add(before);
        Parent after = new Parent();
        after.setFrame(Integer.parseInt(Long.toString(end_frame)));
        pc.add(after);
        //Obtient un nom
        InputDialog id = new InputDialog(this, true);
        String name = id.showDialog();
        pc.setText(name==null ? "PC "+pc.hashCode() : "PC "+name);
        //Ajout à la liste des parents
        parents.add(pc);
        
        //Add the collection to the program
        collection.add(pc);
        //Refresh the VTD
        vtd.setCollections(collection);
        //Refesh tree
        updateTree();
        expandTree();
    }//GEN-LAST:event_popmParentActionPerformed

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
            java.util.logging.Logger.getLogger(EncoFXFrame.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        }
        //</editor-fold>

        /* Create and display the form */
        java.awt.EventQueue.invokeLater(new Runnable() {
            @Override
            public void run() {
                new EncoFXFrame().setVisible(true);
            }
        });
    }

    // Variables declaration - do not modify//GEN-BEGIN:variables
    private javax.swing.JButton btnAddDrawing;
    private javax.swing.JButton btnAddEllipse;
    private javax.swing.JButton btnAddFreeShape;
    private javax.swing.JButton btnAddHText;
    private javax.swing.JButton btnAddPicture;
    private javax.swing.JButton btnAddRectangle;
    private javax.swing.JButton btnAddRoundRectangle;
    private javax.swing.JButton btnAddTextArea;
    private javax.swing.JButton btnAddVText;
    private javax.swing.JButton btnAddVideo;
    private javax.swing.JFileChooser fcOpenVideo;
    private javax.swing.JFileChooser fcSaveFolder;
    private javax.swing.JMenu jMenu1;
    private javax.swing.JMenu jMenu2;
    private javax.swing.JMenuBar jMenuBar1;
    private javax.swing.JPanel jPanel1;
    private javax.swing.JScrollPane jScrollPane1;
    private javax.swing.JScrollPane jScrollPane2;
    private javax.swing.JPopupMenu.Separator jSeparator1;
    private javax.swing.JPopupMenu.Separator jSeparator2;
    private javax.swing.JPopupMenu.Separator jSeparator3;
    private javax.swing.JSlider jSlider1;
    private javax.swing.JToolBar jToolBar1;
    private javax.swing.JLabel lblFrame;
    private javax.swing.JMenuItem mnuEncodeVideo;
    private javax.swing.JMenuItem mnuGoToSaveFolder;
    private javax.swing.JMenuItem mnuOpenVideo;
    private javax.swing.JTree objectsTree;
    private javax.swing.JMenuItem popmAddEvent;
    private javax.swing.JMenuItem popmCollapse;
    private javax.swing.JMenuItem popmCollapseAll;
    private javax.swing.JMenuItem popmExpand;
    private javax.swing.JMenuItem popmExpandAll;
    private javax.swing.JMenuItem popmParent;
    private javax.swing.JMenuItem popmRemoveEvent;
    private javax.swing.JTable propTable;
    private javax.swing.JPopupMenu treePopup;
    // End of variables declaration//GEN-END:variables

    
}
