// SUPER UGLY STUFF IN HERE
// you've been warned.
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using Random = UnityEngine.Random;

public class Client : MonoBehaviour {
    PacketBuilder pb;
    PacketReader pr;

    IEnumerator Start()
    {
        pb = new PacketBuilder();
        pr = new PacketReader();

        var w = new WebSocket(new Uri("ws://96.40.72.113:10000/player"));

        yield return StartCoroutine(w.Connect());

        Debug.Log("Connected");

        // Request Time Sync
        pb.PushByte(0);
        var rand = (ushort)Random.Range(0, 65535);
        pb.PushUint16(rand);
        w.Send(pb.Build());

        while (true)
        {
            var msg = w.Recv();

            if (msg != null)
            {
                pr.SetData(msg);
                var packetType = pr.ReadPacketType();

                while (packetType != 0)
                {
                    var stamp = pr.ReadFloat32();

                    switch (packetType)
                    {
                        case PacketType.META:
                            HandleMeta(pr.ReadJsonObject().AsObject);
                            break;
                        case PacketType.DO_ASSIGN_CONTROL:
                        { 
                            var ent = pr.ReadEntityId();
                            ent.TakeControl();
                            break;
                        }
                        case PacketType.ENTITYDEF_UPDATE:
                        { 
                            var def = pr.ReadEntityDefId();
                            var parsed = pr.ReadJsonObject();
                            def.Update(parsed);
                            break;
                        }
                        case PacketType.ENTITY_ENABLE:
                        {
                            pr.ReadEntityId().Enable();
                            break;
                        }
                        case PacketType.ENTITY_UPDATE:
                        { 
                            var ent = pr.ReadEntityId();
                            var updateType = pr.ReadPacketType();

                            while (updateType != PacketType.NULL)
                            {

                                switch (updateType)
                                {
                                    case PacketType.STRING_UPDATE:
                                    {
                                        var attr = pr.ReadSmallString();
                                        var val = pr.ReadString();

                                        ent.SetAttribute(attr, val);
                                        break;
                                    }
                                    case PacketType.FLOAT_UPDATE:
                                    {
                                        var attr = pr.ReadSmallString();
                                        var val = pr.ReadFloat32();

                                        ent.SetAttribute(attr, val);
                                        break;
                                    }
                                    case PacketType.PARENT_UPDATE:
                                    {
                                        var parent = pr.ReadEntityId();
                                        ent.SetParent(parent);
                                        break;
                                    }
                                    case PacketType.ENTITYDEF_HASH_UPDATE:
                                    {
                                        var def = pr.ReadEntityDefId();
                                        ent.SetDefinition(def);
                                        break;
                                    }
                                    case PacketType.POSITION_UPDATE:
                                    {
                                        ent.UpdatePosition(
                                            pr.ReadFloat32(),
                                            pr.ReadFloat32(),
                                            pr.ReadFloat32(),
                                            pr.ReadFloat32(),
                                            pr.ReadFloat32()
                                        );
                                        break;
                                    }
                                    case PacketType.ENTITY_UPDATE:
                                    {
                                        ent.UpdateAttributes(pr.ReadJsonObject());
                                        break;
                                    }
                                    default:
                                        Debug.LogError("Unknown UPDATE type: " + updateType);
                                        Debug.Break();
                                        break;
                                }
                                updateType = pr.ReadPacketType();
                            }
                            break;
                        }
                        default:
                            Debug.LogError("UNKNOWN PACKET: " + packetType);
                            Debug.Break();
                            break;
                    }

                    packetType = pr.ReadPacketType();
                }
            }

            if (w.error != null)
            {
                Debug.LogError("Error: " + w.error);
                break;
            }
            yield return 0;
        }
        w.Close();
    }

    private void HandleMeta(JSONClass data)
    {
        foreach (KeyValuePair<string, JSONNode> kvp in data)
        {
            switch (kvp.Key)
            {
                case "asset_base":
                    Config.AssetBase = kvp.Value;
                    break;
            }
        }
    }
}
