using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//

using JovDK.Debug;

public class NetworkPlayerPositionDebugger : MonoBehaviour
{

    [SerializeField] Rigidbody destinationRigidbody;
    public Vector3 velocity;
    Rigidbody rigidbody;

    [Space(20)]
    [Header("Debugging")]
    [SerializeField] Vector3 destination;
    [SerializeField] Vector3 previousDestination;


    void Start()
    {

        if (GetComponent<Rigidbody>() != null)
            rigidbody = GetComponent<Rigidbody>();
        else
            DebugExtension.DevLogError("rigidbody component NOT FOUND!");


    }

    void Update()
    {

        //HandleDestinationPostions();
        SetPostion();

    }

    public void SetDestination(Vector3 _destination, Vector3 _velocity)
    {

        previousDestination = destination;
        destination = _destination;

        velocity = _velocity;

    }

    void HandleDestinationPostions()
    {

        if (rigidbody != null && destinationRigidbody.transform.position != destination)
        {

            previousDestination = destination;
            destination = destinationRigidbody.transform.position;

        }

    }


    void SetPostion()
    {

        if (rigidbody != null)
        {

            Vector3 _startPostion = rigidbody.position;
            Vector3 _positionOffset = _startPostion - destination;

            float _velocityMultiplier = Vector3.Distance(_startPostion, destination) / (PlayerController.instance.playerMaxVelocity * 0.09f);

            Vector3 _moveOffset = Vector3.Normalize(_positionOffset) * PlayerController.instance.playerMaxVelocity * _velocityMultiplier * Time.deltaTime;
            _moveOffset = Vector3.ClampMagnitude(_moveOffset, _positionOffset.magnitude);

            rigidbody.transform.position = (_startPostion - _moveOffset);

        }

    }

}
