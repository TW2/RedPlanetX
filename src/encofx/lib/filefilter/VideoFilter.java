/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.filefilter;

import java.io.File;

/**
 *
 * @author Yves
 */
public class VideoFilter extends javax.swing.filechooser.FileFilter {

    @Override
    public boolean accept(File pathname) {
        //Voir les dossiers dans le FileChooser
        if (pathname.isDirectory()) {
            return true;
        }

        //Voir les fichiers images dans le FileChooser
        if(pathname.getName().endsWith(".avi")
                | pathname.getName().endsWith(".flv")
                | pathname.getName().endsWith(".mp4")
                | pathname.getName().endsWith(".mkv")){
            return true;
        }

        //Ne rien voir d'autres
        return false;
    }

    @Override
    public String getDescription() {
        //Montrer "Images" dans le sélecteur
        return "Videos";
    }
}
