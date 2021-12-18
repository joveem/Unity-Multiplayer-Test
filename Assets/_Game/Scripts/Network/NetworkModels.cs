using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkModels
{

    [Serializable]
    public class PlayerPosition
    {

        public SerializableVector3 position;
        public SerializableVector3 eulerRotation;
        public SerializableVector3 velocity;
        public float latencyAverage;

    }

    [Serializable]
    public class PlayerPositionState
    {

        [SerializeField] public string playerId;
        public PlayerPosition playerPosition;

    }

    [Serializable]
    public class PlayerState : PlayerPositionState
    {
        public float health;
        public int score;
        public int armorId;
        public int ammoAmount;

    }

    [Serializable]
    public class PingRequest
    {

        public string id;
        public float time;

    }

    [Serializable]
    public class ShotInformations
    {

        public string ownerId;
        public string entityId;
        public int projectileId;

        public SerializableVector3 position;
        public SerializableVector3 rotation;


    }

}