using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssetsManager : MonoBehaviour
{

    public static GameAssetsManager instance;

    public GameAssetsManager()
    {

        if (instance == null)
            instance = this;

    }

    [SerializeField] List<Projectile> projectilesList = new List<Projectile>();

    public Projectile GetProjectileById(int _projectileId)
    {

        Projectile _value = null;

        foreach (Projectile _projectile in projectilesList)
        {

            if (_projectile.id == _projectileId)
            {

                _value = _projectile;
                break;

            }

        }

        return _value;

    }


}
