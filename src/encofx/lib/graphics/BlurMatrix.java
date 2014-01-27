/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib.graphics;

import java.awt.image.Kernel;

/**
 *
 * @author Yves
 */
public class BlurMatrix {
    
    private float[] matrix;
//    private float c1, c2, c3, c4, c5, c6, c7, c8, c9;
    private int width = 3, height = 3;
    
    public BlurMatrix(){
        float c1, c2, c3, c4, c5, c6, c7, c8, c9;
        c7 = 1f; c8 = 1f; c9 = 1f;
        c4 = 1f; c5 = 1f; c6 = 1f;
        c1 = 1f; c2 = 1f; c3 = 1f;
        matrix = new float[]{
            c1, c2, c3,
            c4, c5, c6,
            c7, c8, c9
        };
    }
    
    public void set3x3Matrix(
            float c1, float c2, float c3,
            float c4, float c5, float c6,
            float c7, float c8, float c9){
        matrix = new float[]{
            c1, c2, c3,
            c4, c5, c6,
            c7, c8, c9
        };
    }
    
    public void set5x5Matrix(
            float c1, float c2, float c3,
            float c4, float c5, float c6,
            float c7, float c8, float c9){
        matrix = new float[]{
            c1,                 getMiddle(c1,c2),           c2,                 getMiddle(c2,c3),           c3,
            getMiddle(c1,c4),   getMiddle(c1, c2, c4, c5),  getMiddle(c2,c5),   getMiddle(c2, c3, c5, c6),  getMiddle(c3,c6),
            c4,                 getMiddle(c4,c5),           c5,                 getMiddle(c5,c6),           c6,
            getMiddle(c4,c7),   getMiddle(c4, c5, c7, c8),  getMiddle(c5,c8),   getMiddle(c5, c6, c8, c9),  getMiddle(c6,c9),
            c7,                 getMiddle(c7,c8),           c8,                 getMiddle(c8,c9),           c9
        };
        width = 5;
        height = 5;
    }
    
    public void setAnotherMatrix(float[] matrix){
        this.matrix = matrix;
        width = (int)Math.sqrt(matrix.length);
        height = (int)Math.sqrt(matrix.length);
    }
    
    private float getMiddle(float a, float b){
        return a+b/2f;
    }
    
    private float getMiddle(float a, float b, float c, float d){
        return a+b+c+d/4f;
    }
    
    public Kernel getKernel(){
        Kernel kernel = new Kernel(width, height, matrix);
        return kernel;
    }
    
}
