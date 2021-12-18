using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

using JovDK.Debug;

public class NetworkDebugger : MonoBehaviour
{

    public static NetworkDebugger instance;

    public NetworkDebugger()
    {

        if (instance == null)
            instance = this;

    }


    public Rigidbody localPlayeInputRigidbody;
    public Transform networkPlayerInputTransform;
    public Transform networkPlayerPredictedInputTransform;
    public Player networkPlayerPositionView;

    [Space(15)]

    [SerializeField] bool isShowingPanel = false;
    [SerializeField] GameObject debuggingPanel;
    [SerializeField] Button debuggingPanelSwitchButton;

    [Space(5)]

    int minimumLatency = 1;
    int maximumLatency = 600;
    [SerializeField] Slider latencySlider;
    [SerializeField] TextMeshProUGUI currentLatencyText;
    [SerializeField] Toggle fixedLattencyToggle;

    [Space(5)]

    [SerializeField] Toggle localPlayeInputToggle;
    [SerializeField] Toggle networkPlayerInputToggle;
    [SerializeField] Toggle networkPlayerPredictedInputToggle;
    [SerializeField] Toggle networkPlayerPositionViewToggle;


    void Start()
    {

        SetupButtons();
        HideAllDebuggingViews();
        ResetPanel();

        ResetAllDebugginPositions();

    }

    void ResetAllDebugginPositions()
    {

        if (GameManager.instance.GetLocalPlayer() != null)
        {

            Player _localPlayer = GameManager.instance.GetLocalPlayer();

            if (localPlayeInputRigidbody != null)
                localPlayeInputRigidbody.position = _localPlayer.transform.position;

            if (networkPlayerInputTransform != null)
                networkPlayerInputTransform.position = _localPlayer.transform.position;

            if (networkPlayerPredictedInputTransform != null)
                networkPlayerPredictedInputTransform.position = _localPlayer.transform.position;

            if (networkPlayerPositionView != null)
                networkPlayerPositionView.SetDestination(_localPlayer.transform.position);

        }
        else
        {

            if (localPlayeInputRigidbody != null)
                localPlayeInputRigidbody.position = Vector3.zero;

            if (networkPlayerInputTransform != null)
                networkPlayerInputTransform.position = Vector3.zero;

            if (networkPlayerPredictedInputTransform != null)
                networkPlayerPredictedInputTransform.position = Vector3.zero;

            if (networkPlayerPositionView != null)
                networkPlayerPositionView.SetDestination(Vector3.zero);

        }

    }

    void SetupButtons()
    {

        if (debuggingPanelSwitchButton != null)
        {

            debuggingPanelSwitchButton.onClick.AddListener(() =>
            {

                DebuggingPanelSwitchButton();

            });

        }
        else
        {

            DebugExtension.DevLogError("debuggingPanelSwitchButton IS NULL!");

        }

    }

    void SetupSlider()
    {

        if (latencySlider != null)
        {

            float _latencyRange = (float)maximumLatency - (float)minimumLatency;

            latencySlider.value = ((float)FakeWebsocketServer.instance.fakeLatencyInMilliseconds / _latencyRange);

            latencySlider.onValueChanged.AddListener(OnLatencySliderValueChange);

        }
        else
            DebugExtension.DevLogError("latencySlider IS NULL!");

        if (currentLatencyText != null)
            currentLatencyText.text = FakeWebsocketServer.instance.fakeLatencyInMilliseconds.ToString() + "ms";
        else
            DebugExtension.DevLogError("currentLatencyText IS NULL!");

    }

    void OnLatencySliderValueChange(float _value)
    {

        float _latencyRange = (float)maximumLatency - (float)minimumLatency;
        FakeWebsocketServer.instance.fakeLatencyInMilliseconds = Mathf.Clamp(Mathf.RoundToInt(_value * _latencyRange), minimumLatency, maximumLatency);

        if (currentLatencyText != null)
            currentLatencyText.text = FakeWebsocketServer.instance.fakeLatencyInMilliseconds.ToString() + "ms";
        else
            DebugExtension.DevLogError("currentLatencyText IS NULL!");

    }

    void SetupToggles()
    {

        if (fixedLattencyToggle != null)
            fixedLattencyToggle.onValueChanged.AddListener(OnChangeFixedLatencyToggle);
        else
            DebugExtension.DevLogError("fixedLattencyToggle IS NULL!");


        if (localPlayeInputToggle != null)
            localPlayeInputToggle.onValueChanged.AddListener(OnChangeLocalPlayeInputToggle);
        else
            DebugExtension.DevLogError("localPlayeInputToggle IS NULL!");

        if (networkPlayerInputToggle != null)
            networkPlayerInputToggle.onValueChanged.AddListener(OnChangeNetworkPlayerInputToggle);
        else
            DebugExtension.DevLogError("networkPlayerInputToggle IS NULL!");

        if (networkPlayerPredictedInputToggle != null)
            networkPlayerPredictedInputToggle.onValueChanged.AddListener(OnChangeNetworkPlayerPredictedInputToggle);
        else
            DebugExtension.DevLogError("networkPlayerPredictedInputToggle IS NULL!");

        if (networkPlayerPositionViewToggle != null)
            networkPlayerPositionViewToggle.onValueChanged.AddListener(OnChangeNetworkPlayerPositionViewToggle);
        else
            DebugExtension.DevLogError("networkPlayerPositionViewToggle IS NULL!");

    }

    void ResetToggles()
    {

        if (fixedLattencyToggle != null)
            fixedLattencyToggle.isOn = false;
        else
            DebugExtension.DevLogError("fixedLattencyToggle IS NULL!");


        if (localPlayeInputToggle != null)
            localPlayeInputToggle.isOn = false;
        else
            DebugExtension.DevLogError("localPlayeInputToggle IS NULL!");

        if (networkPlayerInputToggle != null)
            networkPlayerInputToggle.isOn = false;
        else
            DebugExtension.DevLogError("networkPlayerInputToggle IS NULL!");

        if (networkPlayerPredictedInputToggle != null)
            networkPlayerPredictedInputToggle.isOn = false;
        else
            DebugExtension.DevLogError("networkPlayerPredictedInputToggle IS NULL!");

        if (networkPlayerPositionViewToggle != null)
            networkPlayerPositionViewToggle.isOn = false;
        else
            DebugExtension.DevLogError("networkPlayerPositionViewToggle IS NULL!");

    }

    void HideAllDebuggingViews()
    {

        if (localPlayeInputRigidbody != null)
            localPlayeInputRigidbody.transform.GetChild(0).gameObject.SetActive(false);
        else
            DebugExtension.DevLogError("localPlayeInputRigidbody IS NULL");

        if (networkPlayerInputTransform != null)
            networkPlayerInputTransform.GetChild(0).gameObject.SetActive(false);
        else
            DebugExtension.DevLogError("networkPlayerInputTransform IS NULL");

        if (networkPlayerPredictedInputTransform != null)
            networkPlayerPredictedInputTransform.GetChild(0).gameObject.SetActive(false);
        else
            DebugExtension.DevLogError("networkPlayerPredictedInputTransform IS NULL");

        if (networkPlayerPositionView != null)
            networkPlayerPositionView.transform.GetChild(0).gameObject.SetActive(false);
        else
            DebugExtension.DevLogError("networkPlayerPositionView IS NULL");

    }

    void OnChangeFixedLatencyToggle(bool _togleValue)
    {

        FakeWebsocketServer.instance.fixLatency = _togleValue;

    }

    void OnChangeLocalPlayeInputToggle(bool _togleValue)
    {

        if (localPlayeInputRigidbody != null)
            localPlayeInputRigidbody.transform.GetChild(0).gameObject.SetActive(_togleValue);
        else
            DebugExtension.DevLogError("localPlayeInputRigidbody IS NULL");

    }

    void OnChangeNetworkPlayerInputToggle(bool _togleValue)
    {

        if (networkPlayerInputTransform != null)
            networkPlayerInputTransform.GetChild(0).gameObject.SetActive(_togleValue);
        else
            DebugExtension.DevLogError("networkPlayerInputTransform IS NULL");

    }

    void OnChangeNetworkPlayerPredictedInputToggle(bool _togleValue)
    {

        if (networkPlayerPredictedInputTransform != null)
            networkPlayerPredictedInputTransform.GetChild(0).gameObject.SetActive(_togleValue);
        else
            DebugExtension.DevLogError("networkPlayerPredictedInputTransform IS NULL");

    }

    void OnChangeNetworkPlayerPositionViewToggle(bool _togleValue)
    {

        if (networkPlayerPositionView != null)
            networkPlayerPositionView.transform.GetChild(0).gameObject.SetActive(_togleValue);
        else
            DebugExtension.DevLogError("networkPlayerPositionView IS NULL");

    }

    void ResetPanel()
    {

        SetupSlider();
        SetupToggles();
        ResetToggles();
        HidePanel();

    }

    void DebuggingPanelSwitchButton()
    {

        SwitchPanelShowing();

    }

    void ShowPanel()
    {

        if (debuggingPanel != null)
            debuggingPanel.SetActive(true);
        else
            DebugExtension.DevLogError("debuggingPanel IS NULL!");

        isShowingPanel = true;

    }
    void HidePanel()
    {

        if (debuggingPanel != null)
            debuggingPanel.SetActive(false);
        else
            DebugExtension.DevLogError("debuggingPanel IS NULL!");

        isShowingPanel = false;

    }

    void SwitchPanelShowing()
    {

        if (isShowingPanel)
            HidePanel();
        else
            ShowPanel();

    }

}
