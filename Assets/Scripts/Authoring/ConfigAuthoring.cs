using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ConfigAuthoring : MonoBehaviour
{
    public GameObject tankPrefab;
    public int tankCntToSpawn;
    public float safetyRadius;
    public float cannonBallSpeed = 10;
    public class ConfigBaker : Baker<ConfigAuthoring>
    {
        public override void Bake(ConfigAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new Config()
            {
                tankCntToSpawn = authoring.tankCntToSpawn,
                safetyRadiusSqr = authoring.safetyRadius * authoring.safetyRadius,
                tankEntityPrefab = GetEntity(authoring.tankPrefab, TransformUsageFlags.Dynamic),
                cannonBallSpeed = authoring.cannonBallSpeed
            });
        }
    }
}
