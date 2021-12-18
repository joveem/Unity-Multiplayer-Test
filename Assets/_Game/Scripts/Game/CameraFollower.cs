using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{

    [SerializeField] Transform cameraPivot;
    Transform target;

    void LateUpdate()
    {

        if (target != null)
            cameraPivot.position = Vector3.Lerp(cameraPivot.position, target.position, 0.9f);

    }

    public void SetCameraTarget(Transform _cameraTarget)
    {

        if (_cameraTarget != null)
        {

            target = _cameraTarget;

        }
        else
        {

            target = null;

        }

    }

}
