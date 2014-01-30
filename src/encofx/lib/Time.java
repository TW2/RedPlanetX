/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package encofx.lib;

/**
 *
 * @author Yves
 */
public class Time {
    
    private int hours = 0;
    private int minutes = 0;
    private int seconds = 0;
    private int milliseconds = 0;

    //OLD : Main instance of ProgramLine.Time, initialized at default.
    /** <p>Create a new Time<br />Crée un nouveau Time.</p> */
    public Time(){

    }

    //OLD : Main instance of ProgramLine.Time with our own initialization.
    /** <p>Create a new Time<br />Crée un nouveau Time.</p>
     * @param hours
     * @param minutes
     * @param seconds
     * @param milliseconds */
    public Time(int hours,int minutes,int seconds,int milliseconds){
        this.hours = hours;
        this.minutes = minutes;
        this.seconds = seconds;
        this.milliseconds = milliseconds;
    }

    // <editor-fold defaultstate="collapsed" desc=" get/set Time components ">

    /** <p>Get the hours.<br />Obtient les heures.</p>
     * @return  */
    public int getHours(){
        return hours;
    }

    /** <p>Get the minutes.<br />Obtient les minutes.</p>
     * @return  */
    public int getMinutes(){
        return minutes;
    }

    /** <p>Get the seconds.<br />Obtient les secondes.</p>
     * @return  */
    public int getSeconds(){
        return seconds;
    }

    /** <p>Get the milliseconds.<br />Obtient les millisecondes.</p>
     * @return  */
    public int getMilliseconds(){
        return milliseconds;
    }

    /** <p>Set the hours.<br />Définit les heures.</p>
     * @param hours */
    public void setHours(int hours){
        this.hours = hours;
    }

    /** <p>Set the minutes.<br />Définit les minutes.</p>
     * @param minutes */
    public void setMinutes(int minutes){
        this.minutes = minutes;
    }

    /** <p>Set the seconds.<br />Définit les secondes.</p>
     * @param seconds */
    public void setSeconds(int seconds){
        this.seconds = seconds;
    }

    /** <p>Set the milliseconds.<br />Définit les millisecondes.</p>
     * @param milliseconds */
    public void setMilliseconds(int milliseconds){
        this.milliseconds = milliseconds;
    }

    // </editor-fold>

    // <editor-fold defaultstate="collapsed" desc=" Addition / Substraction ">

    /** <p>Add Time t1 to Time t2.<br />Ajoute le temps t1 au temps t2.<br />
* <tt>t = t1 + t2</tt>.</p>
     * @param t1
     * @param t2
     * @return  */
    public Time addition(Time t1, Time t2){
        Time t;

        long lt1 = toMillisecondsTime(t1);
        long lt2 = toMillisecondsTime(t2);

        long lt = lt1 + lt2;

        t = fromMillisecondsTime(lt);

        return t;
    }

    /** <p>Substract Time t2 to Time t1.<br />Soustrait le temps t2 au temps t1.<br />
* <tt>t = t2 - t1</tt>. * </p>
     * @param t1
     * @param t2
     * @return  */
    public Time substract(Time t1, Time t2){
        Time t;

        long lt1 = toMillisecondsTime(t1);
        long lt2 = toMillisecondsTime(t2);

        long lt = lt2 - lt1;

        t = fromMillisecondsTime(lt);

        return t;
    }

    // </editor-fold>

    // <editor-fold defaultstate="collapsed" desc=" Conversion Time <> milliseconds ">

    /** <p>Convert Time object to milliseconds.<br />
* Convertit un objet Time en millisecondes.</p>
     * @param t
     * @return  */
    public static long toMillisecondsTime(Time t){
        long mst;

        mst = t.getHours()*3600000
                + t.getMinutes()*60000
                + t.getSeconds()*1000
                + t.getMilliseconds();

        return mst;
    }

    /** <p>Convert milliseconds to Time object.<br />
* Convertit des millisecondes en un objet Time.</p>
     * @param mst
     * @return  */
    public static Time fromMillisecondsTime(long mst){
        Time t = new Time();

        int hour = (int)(mst / 3600000);
        int min = (int)((mst - 3600000 * hour) / 60000);
        int sec = (int)((mst - 3600000 * hour - 60000 * min) / 1000);
        int mSec = (int)(mst - 3600000 * hour - 60000 * min - 1000 * sec);

        t.setHours(hour);
        t.setMinutes(min);
        t.setSeconds(sec);
        t.setMilliseconds(mSec);

        return t;
    }

    // </editor-fold>

    /** <p>Get Time.<br />Obtient le temps.</p>
     * @return  */
    public Time getTime(){
        return new Time(getHours(),getMinutes(),
                getSeconds(),getMilliseconds());
    }

    /** <p>Set Time.<br />Définit le temps.</p>
     * @param t */
    public void setTime(Time t){
        this.hours = t.getHours();
        this.minutes = t.getMinutes();
        this.seconds = t.getSeconds();
        this.milliseconds = t.getMilliseconds();
    }

    // <editor-fold defaultstate="collapsed" desc=" Import / export ">

    /** <p>Get Time in ASS format.<br />
    * Obtient le temps au format ASS.</p>
     * @return  */
    public String toASSTime(){
        String Smin, Ssec, Scent;

        int hour = getHours();
        int min = getMinutes();
        int sec = getSeconds();
        int cSec = getMilliseconds()/10;

        if (min<10){Smin = "0"+min;}else{Smin = String.valueOf(min);}
        if (sec<10){Ssec = "0"+sec;}else{Ssec = String.valueOf(sec);}
        if (cSec<10){Scent = "0"+cSec;}else{Scent = String.valueOf(cSec);}

        return hour + ":" + Smin + ":" + Ssec + "." + Scent;
    }
    
}
