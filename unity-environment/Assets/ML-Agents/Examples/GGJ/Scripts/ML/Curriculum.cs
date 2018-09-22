using System;
using System.Collections.Generic;
[Serializable]
public class Curriculum
{
    public string measure;
    public float[] thresholds;
    public int min_lesson_length;
    public bool signal_smoothing;
    public Dictionary<string,float[]> parameters;

    public static Curriculum GetFromJson()
    {
        //var dict = JSONObject. ("{\"measure\" : \"reward\",\"thresholds\" : [4, 3, 2, 2, 2, 2, 2, 2],\"min_lesson_length\" : 50,\"signal_smoothing\" : true, \"parameters\" :{\"max_angle\" : [5, 10, 15, 30, 50, 70, 100, 160, 160],\"min_power\" : [8 ,7, 6, 5, 4, 3, 2, 1, 0],\"max_power\" : [10 ,11, 12, 13, 14, 15, 16, 16, 16],\"start_no_of_dead\" : [300, 300, 300, 300, 300, 250, 200, 150, 100]}}") as Dictionary<string, object>;
       
        return null;
    }

}
