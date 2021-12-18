using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//

using JovDK.Debug;

public class Projectile : MonoBehaviour
{

    public int id = 0;
    [Space(15)]
    public bool isLocalEntinty = false;
    public string entityId = "";
    public string ownerId = "";

    [Space(15)]
    [SerializeField] float damage = 10f;
    [SerializeField] float velocity = 30f;
    [SerializeField] float lifeTime = 5f;

    Rigidbody rigidbody;


    private void Start()
    {

        if (rigidbody == null)
        {

            if (GetComponent<Rigidbody>() != null)
                rigidbody = GetComponent<Rigidbody>();
            else
                DebugExtension.DevLogError("Projectile HAVE NO Rigidbody COMPONENT!");


        }

        rigidbody.velocity = transform.forward * velocity; //* Time.deltaTime;
        Destroy(gameObject, lifeTime);

    }

    public void Setup(string _ownerId, string _entityId)
    {

        isLocalEntinty = (GameManager.instance.GetLocalPlayer().id == _ownerId);


    }

    void OnTriggerEnter(Collider other)
    {

        switch (other.tag)
        {

            case "Player":
                {

                    if (other.GetComponent<Player>() != null)
                    {

                        Player _hittedPlayer = other.GetComponent<Player>();

                        if (_hittedPlayer.id != ownerId)
                        {

                            // send player damage

                        }

                    }

                    break;

                }
            case "Projectile":
                {

                    if (other.GetComponent<Projectile>() != null)
                    {

                        Projectile _hittedProjectile = other.GetComponent<Projectile>();

                        if (_hittedProjectile.ownerId != ownerId)
                        {

                            // send destroy projectile

                        }

                    }

                    break;

                }

            case "Box":
                {

                    if (other.GetComponent<EnvironmentBox>() != null)
                    {

                        EnvironmentBox _hittedBox = other.GetComponent<EnvironmentBox>();

                        // send box damage

                    }

                    break;

                }

        }

    }


}
