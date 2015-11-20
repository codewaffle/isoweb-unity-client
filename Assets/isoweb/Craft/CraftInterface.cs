using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace isoweb
{
    public class CraftInterface : MonoBehaviour
    {
        void Start()
        {
            transform.parent = Global.UICanvas.transform;

            Debug.Log("Registering CraftInterface");
            Global.CraftInterface = this;
        }

        void OnEnable()
        {
            Debug.Log("OnEnable");
        }

        void Update()
        {
            var rt = (RectTransform)transform;
            rt.anchoredPosition = Vector2.zero;
        }
    }
}