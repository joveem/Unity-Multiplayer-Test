using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//

using NetworkModels;
using JovDK.Debug;

public class NetworkManager : MonoBehaviour
{

    public static NetworkManager instance;

    public NetworkManager()
    {

        if (instance == null)
            instance = this;

    }

    public float latencyAverage;
    [SerializeField] Dictionary<string, Action<byte[]>> eventsCallbacks = new Dictionary<string, Action<byte[]>>();

    int serverTickRate = 20;
    float tickRatePassedTime = 0f;
    private void Start()
    {

        SetupEvents();

    }

    void Update()
    {

        HandleServerTickRate();

    }

    void SetupEvents()
    {

        SetEventCallback("send-ping-to-client", OnReceivePingResponse);
        SetEventCallback("send-all-players-positions-to-client", OnReceiveAllPlayersPositions);
        SetEventCallback("send-shot-to-client", OnReceiveShot);

    }

    void HandleServerTickRate()
    {

        tickRatePassedTime += Time.deltaTime;

        if (tickRatePassedTime >= (1f / (float)serverTickRate))
        {

            tickRatePassedTime -= (1f / (float)serverTickRate);
            FixedTickRateUpdate();

        }

    }

    void FixedTickRateUpdate()
    {

        SendPlayerPosition();
        SendPing();

    }

    void SendPing()
    {

        string _randomId = GetRandomId(8);

        PingRequest _pingRequest = new PingRequest()
        {

            id = _randomId,
            time = Time.time

        };

        EmitEvent("send-ping-to-server", _pingRequest.SerializeObjectToByte());

    }

    public void SendShot(string _ownerId, string _entityId, int _projectileId, Vector3 _position, Quaternion _rotation)
    {

        ShotInformations _shotInformations = new ShotInformations()
        {

            ownerId = _ownerId,
            entityId = _entityId,
            projectileId = _projectileId,
            position = new SerializableVector3(_position),
            rotation = new SerializableVector3(_rotation.eulerAngles)

        };

        EmitEvent("send-shot-to-server", _shotInformations.SerializeObjectToByte());

    }

    public string GetRandomId(int _length)
    {

        string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        string _value = "";

        for (int i = 0; i < _length; i++)
        {
            _value += characters[UnityEngine.Random.Range(0, characters.Length)];
        }


        return _value;

    }

    void SendPlayerPosition()
    {

        PlayerPosition _playerPosition = new PlayerPosition()
        {

            position = new SerializableVector3(NetworkDebugger.instance.localPlayeInputRigidbody.position),
            eulerRotation = new SerializableVector3(NetworkDebugger.instance.localPlayeInputRigidbody.rotation.eulerAngles),
            velocity = new SerializableVector3(NetworkDebugger.instance.localPlayeInputRigidbody.velocity),
            latencyAverage = latencyAverage

        };

        PlayerPositionState _playerPositionState = new PlayerPositionState()
        {

            playerId = GameManager.instance.GetLocalPlayer().id,
            playerPosition = _playerPosition

        };

        EmitEvent("send-player-position-to-server", _playerPositionState.SerializeObjectToByte());

    }

    void EmitEvent(string _eventName, byte[] _data)
    {

        FakeWebsocketServer.instance.ApplyEventAfterFakeLatency(_eventName, _data);

    }

    // ( event name = "send-all-players-positions-to-client" )
    void OnReceiveAllPlayersPositions(byte[] _data)
    {

        PlayerPositionState[] _playersPositionStatesList = _data.DeserializeByteToObject<PlayerPositionState[]>();

        string _localPlayerId = "";

        if (GameManager.instance.GetLocalPlayer() != null)
            _localPlayerId = GameManager.instance.GetLocalPlayer().id;

        foreach (PlayerPositionState _playerPositionState in _playersPositionStatesList)
        {

            if (_playerPositionState.playerId == _localPlayerId)
            {

                Vector3 _velocityOffset = _playerPositionState.playerPosition.velocity.Vector3 * _playerPositionState.playerPosition.latencyAverage;

                NetworkDebugger.instance.networkPlayerInputTransform.position = _playerPositionState.playerPosition.position.Vector3;
                NetworkDebugger.instance.networkPlayerInputTransform.rotation = Quaternion.Euler(_playerPositionState.playerPosition.eulerRotation.Vector3);

                NetworkDebugger.instance.networkPlayerPredictedInputTransform.position = _playerPositionState.playerPosition.position.Vector3 + _velocityOffset;
                NetworkDebugger.instance.networkPlayerPredictedInputTransform.rotation = Quaternion.Euler(_playerPositionState.playerPosition.eulerRotation.Vector3);

                NetworkDebugger.instance.networkPlayerPositionView.SetDestination(_playerPositionState.playerPosition.position.Vector3 + _velocityOffset, _playerPositionState.playerPosition.eulerRotation.Vector3, _playerPositionState.playerPosition.velocity.Vector3, _playerPositionState.playerPosition.latencyAverage);

            }
            else
            {

                // apply network player position

            }

        }

    }

    // ( event name = "send-ping-to-client" )
    void OnReceivePingResponse(byte[] _data)
    {

        PingRequest _pingResponse = _data.DeserializeByteToObject<PingRequest>();

        float _latency = Time.time - _pingResponse.time;

        UpdateLatencyAverage(_latency);

    }

    // ( event name = "send-shot-to-client" )
    void OnReceiveShot(byte[] _data)
    {

        ShotInformations _shotInformations = _data.DeserializeByteToObject<ShotInformations>();

        GameManager.instance.InstantiateShot(_shotInformations.ownerId, _shotInformations.entityId, _shotInformations.projectileId, _shotInformations.position.Vector3, Quaternion.Euler(_shotInformations.rotation.Vector3));

    }

    void UpdateLatencyAverage(float _lastLatencyValue)
    {

        latencyAverage = (latencyAverage * 9f + _lastLatencyValue) / ((float)serverTickRate / 2f);

    }

    void SetEventCallback(string _eventName, Action<byte[]> _callBack)
    {

        if (!eventsCallbacks.ContainsKey(_eventName))
            eventsCallbacks.Add(_eventName, _callBack);
        else
            DebugExtension.DevLogError("_eventName ALREADY EXISTS! ( _eventName = " + _eventName + " )");

    }

    public void ApplyEventReceive(string _eventName, byte[] _data)
    {

        OnEventReceived(_eventName, _data);

    }

    void OnEventReceived(string _eventName, byte[] _data)
    {

        //DebugExtension.DevLog("event RECEIVED!".ToColor(GoodCollors.orange) + " ( _eventName = " + _eventName + " )");

        if (eventsCallbacks.ContainsKey(_eventName))
            eventsCallbacks[_eventName](_data);
        else
            DebugExtension.DevLogError("_eventName NOT FOUND! ( _eventName = " + _eventName + " )");

    }

}
