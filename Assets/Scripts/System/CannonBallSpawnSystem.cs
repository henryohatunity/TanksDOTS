
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[UpdateAfter(typeof(SafetySystem))]
[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct CannonBallSpawnSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Shooting>();
    }
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Velocity
        var config = SystemAPI.GetSingleton<Config>();
        
        // foreach (var (turret, localTransform, baseColor) in SystemAPI.Query<RefRO<Turret>, RefRO<LocalTransform>, RefRO<URPMaterialPropertyBaseColor>>()
        //              .WithAll<Shooting>())
        foreach (var (turret, baseColor) 
                 in SystemAPI.Query<RefRO<Turret>, RefRO<URPMaterialPropertyBaseColor>>().WithAll<Shooting>())
        {
            // Instantiate
            var cannonBallEntity = state.EntityManager.Instantiate(turret.ValueRO.cannonBallEntityPrefab);
            
            // Which one is faster?
            // float3 position = SystemAPI.GetComponent<LocalToWorld>(turret.ValueRO.spawnPointEntity).Position;
            // quaternion rotation = quaternion.identity;
            // float scale = SystemAPI.GetComponent<LocalToWorld>(turret.ValueRO.cannonBallEntityPrefab).Value.Scale().x;

            var spawnPointLocalToWorld =
                state.EntityManager.GetComponentData<LocalToWorld>(turret.ValueRO.spawnPointEntity);
            float3 position = spawnPointLocalToWorld.Position;
            quaternion rotation = quaternion.identity;
            // float scale = state.EntityManager.GetComponentData<LocalToWorld>(turret.ValueRO.cannonBallEntityPrefab)
                // .Value.Scale().x;
            float scale = 1.0f;
            state.EntityManager.SetComponentData(cannonBallEntity, new LocalTransform()
            {
                Position = position,
                Rotation = rotation,
                Scale = scale
            });
            
            // Color
            state.EntityManager.SetComponentData(cannonBallEntity, new URPMaterialPropertyBaseColor()
            {
                Value = baseColor.ValueRO.Value
            });
            
            state.EntityManager.SetComponentData(cannonBallEntity, new CannonBall()
            {
                velocity =  spawnPointLocalToWorld.Up * config.cannonBallSpeed,
                maxBounce = 5,
                currentBounce = 0
            });
        }
    }
}
