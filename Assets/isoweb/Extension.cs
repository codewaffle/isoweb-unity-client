using UnityEngine;
using System.Collections;

namespace isoweb
{
    public static class Extension
    {
        public static Transform FindFirstRecursive(this Transform t, string n)
        {
            var ret = t.Find(n);

            // FOUND IT
            if (ret != null)
                return ret;

            // search children
            foreach (Transform c in t)
            {
                ret = c.FindFirstRecursive(n);
                if (ret != null)
                    return ret;
            }

            // it's nowhere :(
            return null;
        }
    }
}