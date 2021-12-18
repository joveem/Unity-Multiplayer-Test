using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//

using JovDK.Debug;

public class PlayerController : MonoBehaviour
{

    public static PlayerController instance;

    public PlayerController()
    {

        if (instance == null)
            instance = this;

    }

    public float playerMaxVelocity = 5f;
    [SerializeField] float futurePlayerPositionDeltaTime = 0.1f;


    void Update()
    {

        HandleMovementInputs();
        HandleFireInputs();

    }

    void FixedUpdate()
    {


    }

    void HandleMovementInputs()
    {

        float _screenHeight = Screen.height;
        float _screenWidth = Screen.width;

        Vector3 _movementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 _cameraInput = new Vector3(Input.mousePosition.x - (_screenWidth / 2), 0, Input.mousePosition.y - (_screenHeight / 2));

        _movementInput = Vector3.ClampMagnitude(_movementInput, 1f);

        if (NetworkDebugger.instance.localPlayeInputRigidbody != null)
        {

            Vector3 _futureMovement = _movementInput * playerMaxVelocity * Time.deltaTime;
            Vector3 _destinationPosition = NetworkDebugger.instance.localPlayeInputRigidbody.position + _futureMovement;



            NetworkDebugger.instance.localPlayeInputRigidbody.MovePosition(_destinationPosition);


            if (_cameraInput != Vector3.zero)
            {

                Quaternion _lookRotation = Quaternion.LookRotation(_cameraInput);

                NetworkDebugger.instance.localPlayeInputRigidbody.MoveRotation(_lookRotation);
                GameManager.instance.GetLocalPlayer().SetDestination(_destinationPosition, _lookRotation.eulerAngles);

            }
            else
            {

                GameManager.instance.GetLocalPlayer().SetDestination(_destinationPosition);

            }


        }


    }

    void HandleFireInputs()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            Transform _gunPivot = GameManager.instance.GetLocalPlayer().gunPivot;

            int _projectileId = 0;

            NetworkManager.instance.SendShot(GameManager.instance.GetLocalPlayer().id, NetworkManager.instance.GetRandomId(8), _projectileId, _gunPivot.position, _gunPivot.rotation);

        }

    }

}
