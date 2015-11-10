using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Mathx
{
    public static double CalculateStdDev(IEnumerable<double> values)
    {
        double ret = 0;
        if (values.Count() > 0)
        {
            //Compute the Average      
            double avg = values.Average();
            //Perform the Sum of (value-avg)_2_2      
            double sum = values.Sum(d => Math.Pow(d - avg, 2));
            //Put it all together      
            ret = Math.Sqrt((sum) / (values.Count() - 1));
        }
        return ret;
    }

    public static float LerpBearing(float a, float b, float t)
    {
        var diff = b - a;

        if (diff > Mathf.PI)
            a += 2*Mathf.PI;
        else if (diff < -Mathf.PI)
            a -= 2*Mathf.PI;

        return a + (b - a)*t;
    }
}
