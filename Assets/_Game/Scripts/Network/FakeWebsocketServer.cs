using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//

using NetworkModels;
using JovDK.Debug;

public class FakeWebsocketServer : MonoBehaviour
{

    public static FakeWebsocketServer instance;

    public FakeWebsocketServer()
    {

        if (instance == null)
            instance = this;

    }
    public List<PlayerState> playersStatesList = new List<PlayerState>();


    [Space(15)]
    [Header("Server configurations")]
    [SerializeField] int serverTickRate = 20;


    [Space(15)]
    [Header("Fake latency (ms)")]
    public int fakeLatencyInMilliseconds = 100;
    public bool fixLatency = false;

    float tickRatePassedTime = 0f;

    void Update()
    {

        HandleServerTickRate();

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

        SendAllPlayersStates();

    }

    void SendAllPlayersStates()
    {

        List<PlayerPositionState> _playersPositionStatesList = new List<PlayerPositionState>();

        foreach (PlayerState _playerState in playersStatesList)
        {

            _playersPositionStatesList.Add(new PlayerPositionState()
            {

                playerId = _playerState.playerId,
                playerPosition = _playerState.playerPosition

            });

        }

        Emit("send-all-players-positions-to-client", _playersPositionStatesList.ToArray().SerializeObjectToByte());

    }

    public void ApplyEventAfterFakeLatency(string _eventName, byte[] _data)
    {

        int _randomLatency = fakeLatencyInMilliseconds;

        if (!fixLatency)
        {

            int _minimumLatency = fakeLatencyInMilliseconds - (fakeLatencyInMilliseconds / 5);
            int _maximumLatency = fakeLatencyInMilliseconds + (fakeLatencyInMilliseconds / 5);

            _randomLatency = Mathf.Clamp(Random.Range(_minimumLatency, _maximumLatency), 1, 999999);

        }

        StartCoroutine(ApplyEventAfterFakeLatency(_eventName, _data, _randomLatency));

    }

    IEnumerator ApplyEventAfterFakeLatency(string _eventName, byte[] _data, int _latency)
    {

        yield return new WaitForSeconds((float)_latency * 0.001f);

        OnEventReceived(_eventName, _data);

    }

    void OnEventReceived(string _eventName, byte[] _data)
    {

        //DebugExtension.DevLog("event RECEIVED!".ToColor(GoodCollors.green) + " ( _eventName = " + _eventName + " )");

        switch (_eventName)
        {

            case "send-player-position-to-server":
                {

                    PlayerPositionState _receivedPlayerPositionState = _data.DeserializeByteToObject<PlayerPositionState>();

                    if (GetPlayerStateById(_receivedPlayerPositionState.playerId) != null)
                    {

                        PlayerPositionState _currentPlayer = GetPlayerStateById(_receivedPlayerPositionState.playerId);

                        _currentPlayer.playerPosition = _receivedPlayerPositionState.playerPosition;

                    }
                    break;

                }

            case "send-ping-to-server":
                {

                    Emit("send-ping-to-client", _data);
                    break;

                }

            case "send-shot-to-server":
                {

                    Emit("send-shot-to-client", _data);
                    break;

                }

            default:
                {

                    DebugExtension.DevLogError("_eventName NOT FOUND! ( _eventName = " + _eventName + " )");
                    break;

                }

        }

    }

    void Emit(string _eventName, byte[] _data)
    {

        NetworkManager.instance.ApplyEventReceive(_eventName, _data);

    }

    PlayerState GetPlayerStateById(string _playerId)
    {

        PlayerState _value = null;

        foreach (PlayerState _playerState in playersStatesList)
        {

            if (_playerState.playerId == _playerId)
            {

                _value = _playerState;
                break;

            }

        }

        return _value;

    }

}
