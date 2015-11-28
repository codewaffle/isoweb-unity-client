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
            Debug.Log("Registering CraftInterface");
            Global.CraftInterface = this;
            Global.Client.RegisterPacketHandler(PacketType.CRAFT_LIST, UpdateCraftList);
            Global.Client.RegisterPacketHandler(PacketType.CRAFT_VIEW, UpdateCraftView);
        }

        private void UpdateCraftView(PacketReader pr)
        {

        }

        private void UpdateCraftList(PacketReader pr)
        {
            var obj = pr.ReadJsonObject();
            foreach (JSONArray rec in obj.AsArray)
            {
                Debug.Log(rec);
            }
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

        void OnExpandTab()
        {
            Global.Client.RequestCraftList();
        }
    }
}