using System;
using System.Collections.Generic;
using System.Linq;

namespace Algo;

public static class Utils
{
    public delegate double PointPlacementMethod(double start, double end, int step, int steps);

    public static readonly Dictionary<string, PointPlacementMethod> PointPlacementMethods = new()
    {
        {
            "Равноотносящиеся",
            new((start, end, step, steps) => start + (end - start) * (double)step / steps)
        },
        {
            "Чебышевского",
            new((start, end, step, steps) => (start + end) / 2 + Math.Cos((double) step / steps * Math.PI) * (end - start) / 2)
        }
    };

    public static readonly string[] PointPlacementMethodsNames = PointPlacementMethods.Keys.ToArray();
}