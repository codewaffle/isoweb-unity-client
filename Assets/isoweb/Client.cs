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
    WebSocket webSocket;
    private ushort _syncId;
    private float _currentStamp;
    private double _offset;
    private double _latency;
    private Dictionary<ushort, float> _syncSessions = new Dictionary<ushort, float>();
    private int _syncIter;
    private double _rt_total;
    private double _offset_total;

    void Awake()
    {
        if(Config.Client != null)
            Debug.LogError("UGH WHY");

        Config.Client = this;
        pb = new PacketBuilder();
        pr = new PacketReader();
    }

    void RequestTimeSync()
    {
        // Request Time Sync
        pb.PushPacketType(PacketType.PING);
        _syncId = (ushort)Random.Range(0, 65535);
        pb.PushUint16(_syncId);
        webSocket.Send(pb.Build());
        _syncSessions[_syncId] = Time.time;
    }

    IEnumerator Start()
    {
        webSocket = new WebSocket(new Uri("ws://96.40.72.113:10000/player"));

        yield return StartCoroutine(webSocket.Connect());

        Debug.Log("Connected");
        StartCoroutine(SyncTime());

        while (true)
        {
            var msg = webSocket.Recv();

            if (msg != null)
            {
                pr.SetData(msg);
                HandleMessage();
            }

            if (webSocket.error != null)
            {
                Debug.LogError("Error: " + webSocket.error);
                break;
            }
            yield return 0;
        }
        webSocket.Close();
    }

    private void HandleMessage()
    {
        var packetType = pr.ReadPacketType();

        while (packetType != 0)
        {
            _currentStamp = pr.ReadFloat32();

            switch (packetType)
            {
                case PacketType.META:
                    HandleMeta(pr.ReadJsonObject().AsObject);
                    break;
                case PacketType.PONG:
                    HandlePong(pr);
                    break;
                case PacketType.DO_ASSIGN_CONTROL:
                    pr.ReadEntityId().TakeControl();
                    break;
                case PacketType.ENTITYDEF_UPDATE:
                    HandleEntityDefUpdate();
                    break;
                case PacketType.ENTITY_ENABLE:
                    pr.ReadEntityId().Enable();
                    break;
                case PacketType.ENTITY_DISABLE:
                    pr.ReadEntityId().Disable();
                    break;
                case PacketType.ENTITY_UPDATE:
                    HandleEntityUpdate();
                    break;
                default:
                    Debug.LogError("UNKNOWN PACKET: " + packetType);
                    Debug.Break();
                    break;
            }

            packetType = pr.ReadPacketType();
        }
    }

    private void HandleEntityUpdate()
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
                    ent.PushPositionUpdate(
                        _currentStamp,
                        pr.ReadFloat32(),
                        pr.ReadFloat32(),
                        pr.ReadFloat32()
                        );

                    pr.ReadFloat32();
                    pr.ReadFloat32();

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
    }

    private void HandleEntityDefUpdate()
    {
        var def = pr.ReadEntityDefId();
        var parsed = pr.ReadJsonObject();
        def.Update(parsed);
    }

    private IEnumerator SyncTime()
    {
        _syncIter = 0;
        _rt_total = 0;
        _offset_total = 0;

        for (var i=0;i<10;++i)
        {
            RequestTimeSync();
            yield return new WaitForSeconds(0.375f);
        }

        while (_syncIter < 10)
        {
            yield return new WaitForSeconds(0.05f);
        }

        _latency = _rt_total/20f;
        _offset = _offset_total/10f;

        Debug.Log("Avg Offset: " + _offset);
        Debug.Log("Avg Latency: " + _latency);
    }

    public float ServerTime
    {
        get { return (float)(Time.time + _offset); }
    }

    public float AdjustedServerTime
    {
        get { return ServerTime - Config.Interpolate; }
    }

    private void HandlePong(PacketReader packetReader)
    {
        var syncId = pr.ReadUint16();
        var t0 = _syncSessions[syncId];
        _syncSessions.Remove(syncId);
        var t1 = pr.ReadFloat64();
        var t2 = _currentStamp;
        var t3 = Time.time;

        var rt = (t3 - t0) - (t2 - t1);
        var off = ((t1 - t0) + (t2 - t3))/2f;

        _latency = rt/2f;
        _offset = off;
        _rt_total += rt;
        _offset_total += off;
        _syncIter += 1;
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

    public void SendCmdContextualPosition(float x, float y)
    {
        pb.PushByte((byte)PacketType.CMD_CONTEXTUAL_POSITION);
        pb.PushFloat32(x);
        pb.PushFloat32(y);
        webSocket.Send(pb.Build());
    }
}
