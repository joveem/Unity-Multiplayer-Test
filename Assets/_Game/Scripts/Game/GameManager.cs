using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//

using JovDK.Debug;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public List<Player> playersList = new List<Player>();

    public GameManager()
    {

        if (instance == null)
            instance = this;

    }

    public Material localPlayerMaterial;
    public Material networkPlayerMaterial;
    [SerializeField] private CameraFollower cameraFollower;

    [Space(15)]
    [Header("Prefabs")]
    public Player playerPrefab;


    void Start()
    {

        if (cameraFollower != null)
        {

            if (GetLocalPlayer() != null)
                cameraFollower.SetCameraTarget(GetLocalPlayer().transform);
            else
                DebugExtension.DevLogWarning("Local player not found");

        }

    }

    public void InstantiateShot(string _ownerId, string _entityId, int _projectileId, Vector3 _position, Quaternion _rotation)
    {

        if (GameAssetsManager.instance.GetProjectileById(_projectileId) != null)
        {

            Projectile _instantiatedProjectile = Instantiate(GameAssetsManager.instance.GetProjectileById(_projectileId), _position, _rotation);

            _instantiatedProjectile.Setup(_ownerId, _entityId);

        }
        else
        {

            DebugExtension.DevLogError("Projectile NOT FOUND!( _projectileId = " + _projectileId + " )");

        }

    }

    public Player GetPlayerById(string _playerId)
    {

        Player _value = null;

        foreach (Player _player in playersList)
        {

            if (_player.id == _playerId)
            {

                _value = _player;
                break;

            }

        }

        return _value;

    }

    public Player GetLocalPlayer()
    {

        Player _value = null;

        foreach (Player _player in playersList)
        {

            if (_player.isLocalPlayer)
            {

                _value = _player;
                break;

            }

        }

        return _value;

    }

}
