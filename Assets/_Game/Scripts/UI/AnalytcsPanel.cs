using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class AnalytcsPanel : MonoBehaviour
{

    public static AnalytcsPanel instance;

    public AnalytcsPanel()
    {

        if (instance == null)
            instance = this;

    }

    [SerializeField] TextMeshProUGUI latencyText;

    void Start()
    {
        if (latencyText != null)
            latencyText.text = "latency: ---";


    }

    void Update()
    {

        HandleNetworkStatistics();

    }

    void HandleNetworkStatistics()
    {

        SetLatencyText(NetworkManager.instance.latencyAverage);

    }

    public void SetLatencyText(float _latencyTime)
    {

        if (latencyText != null)
        {
            int _latencyInMilliseconds = Mathf.Clamp(Mathf.RoundToInt(_latencyTime * 1000), 1, 9999);

            string _latencyColor = _latencyInMilliseconds < 80 ? GoodCollors.green
                : (_latencyInMilliseconds < 180 ? GoodCollors.yellow
                : (_latencyInMilliseconds < 600 ? GoodCollors.orange
                : GoodCollors.red));

            latencyText.text = "latency: " + (_latencyInMilliseconds + "ms").ToColor(_latencyColor);

        }

    }


}
