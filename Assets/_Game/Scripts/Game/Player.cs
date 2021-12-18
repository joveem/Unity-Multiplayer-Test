using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//

using JovDK.Debug;

public class Player : MonoBehaviour
{

    public bool isLocalPlayer;

    public string id;
    public GameObject playerMesh;
    public Transform gunPivot;

    Vector3 destination;
    Vector3 destinationRotation;
    Vector3 velocity;
    Rigidbody rigidbody;
    float latency = 0f;

    void Start()
    {

        if (rigidbody == null)
        {

            if (GetComponent<Rigidbody>() != null)
                rigidbody = GetComponent<Rigidbody>();
            else
                DebugExtension.DevLogError("Player HAVE NO Rigidbody COMPONENT!");

        }

    }

    public void Setup(string _id, bool _isLocalPlayer)
    {

        id = _id;

        isLocalPlayer = _isLocalPlayer;

        if (isLocalPlayer)
            playerMesh.GetComponent<MeshRenderer>().sharedMaterial = GameManager.instance.localPlayerMaterial;
        else
            playerMesh.GetComponent<MeshRenderer>().sharedMaterial = GameManager.instance.networkPlayerMaterial;

    }

    void Update()
    {

        HandleMovement();


    }

    void HandleMovement()
    {

        SetPostion();

    }

    void SetPostion()
    {

        if (rigidbody != null)
        {

            if (isLocalPlayer)
            {

                Vector3 _startPostion = rigidbody.position;
                Vector3 _positionOffset = _startPostion - destination;

                float _velocityMultiplier = Mathf.Clamp(Vector3.Distance(_startPostion, destination), 0f, (PlayerController.instance.playerMaxVelocity * 0.1f)) / (PlayerController.instance.playerMaxVelocity * 0.1f);

                Vector3 _moveOffset = Vector3.Normalize(_positionOffset) * PlayerController.instance.playerMaxVelocity * _velocityMultiplier * Time.deltaTime;
                _moveOffset = Vector3.ClampMagnitude(_moveOffset, _positionOffset.magnitude);

                rigidbody.MovePosition(_startPostion - _moveOffset);


                Quaternion _startRotation = playerMesh.transform.rotation;
                Quaternion _finalRotaion = Quaternion.Euler(destinationRotation);

                playerMesh.transform.rotation = (Quaternion.LerpUnclamped(_startRotation, _finalRotaion, 0.8f));

            }
            else
            {

                Vector3 _startPostion = rigidbody.position;
                Vector3 _positionOffset = _startPostion - destination;

                float _velocityMultiplier = Vector3.Distance(_startPostion, destination) / (PlayerController.instance.playerMaxVelocity * 0.09f);

                //float _velocityMultiplier = Mathf.Clamp(Vector3.Distance(_startPostion, destination), 0f, (PlayerController.instance.playerMaxVelocity * 0.1f)) / (PlayerController.instance.playerMaxVelocity * 0.1f);

                Vector3 _moveOffset = Vector3.Normalize(_positionOffset) * PlayerController.instance.playerMaxVelocity * _velocityMultiplier * Time.deltaTime;
                _moveOffset = Vector3.ClampMagnitude(_moveOffset, _positionOffset.magnitude);

                rigidbody.MovePosition(_startPostion - _moveOffset);


                Quaternion _startRotation = playerMesh.transform.rotation;
                Quaternion _finalRotaion = Quaternion.Euler(destinationRotation);

                playerMesh.transform.rotation = (Quaternion.LerpUnclamped(_startRotation, _finalRotaion, Time.deltaTime * 32f));

            }


        }

    }

    public void SetDestination(Vector3 _destination, Vector3? _destinationRotation = null)
    {

        destination = _destination;

        if (_destinationRotation != null)
            destinationRotation = (Vector3)_destinationRotation;

    }

    public void SetDestination(Vector3 _destination, Vector3 _destinationRotation, Vector3 _velocity, float _latency)
    {

        destination = _destination;
        destinationRotation = _destinationRotation;

        velocity = _velocity;
        latency = _latency;

    }

}
