using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEditor.SceneManagement;
using UnityEngine;

public class TurretAuthoring : MonoBehaviour
{
    public GameObject cannonBallPrefab;
    public GameObject spawnPointObject;
    public float turretRotSpeed;
    
    public class TurretBaker : Baker<TurretAuthoring>
    {
        public override void Bake(TurretAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Turret()
            {
                cannonBallEntityPrefab = GetEntity(authoring.cannonBallPrefab, TransformUsageFlags.Dynamic),
                spawnPointEntity = GetEntity(authoring.spawnPointObject, TransformUsageFlags.Dynamic)
            });
            //
            AddComponent<Shooting>(entity);
            
            // Turret Rotation
            AddComponent(entity, new TurretRotation()
            {
                rotationSpeed = authoring.turretRotSpeed
            });
        }
    }
}
