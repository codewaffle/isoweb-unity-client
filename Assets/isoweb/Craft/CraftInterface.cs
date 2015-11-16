using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace isoweb
{
    public class CraftInterface : MonoBehaviour
    {
        protected Dictionary<ulong, CraftRecipe> _recipes;

        void Start()
        {
            transform.parent = Global.UICanvas.transform;
            Debug.Log("Registering CraftInterface");
            Global.CraftInterface = this;
        }

        public void UpdateRecipe(ulong hash, JSONNode data)
        {
            CraftRecipe recipe;

        }
    }

    public class CraftRecipe
    {
        public ulong Hash;
        public string Name;
    }
}