// #define _SIMPLE
// #define _RANDOM
#define _JOB

using System.ComponentModel;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEditor;
using UnityEngine;
using Random = Unity.Mathematics.Random;

[BurstCompile]
[UpdateAfter(typeof(TankSpawnSystem))]
[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct TankMoveSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Tank>();
    }
    
    public void OnUpdate(ref SystemState state)
    {
        #if _SIMPLE
        foreach (var (localTransform, tank, entity) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<Tank>>().WithAll<Tank>().WithEntityAccess())
        {
            float3 forwardDirection = localTransform.ValueRO.Forward();
            float3 position = localTransform.ValueRO.Position;
            float3 nextPosition = position + forwardDirection * tank.ValueRO.speed * SystemAPI.Time.DeltaTime;
            state.EntityManager.SetComponentData(entity, LocalTransform.FromPositionRotation(nextPosition, localTransform.ValueRO.Rotation));
        }
        #endif
        
        #if _RANDOM
        foreach (var (localTransform, tank, entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Tank>>()
                     .WithAll<Tank>().WithEntityAccess())
        {
            var pos = localTransform.ValueRO.Position;
            pos.y = entity.Index;
            var angle = (0.5f + noise.cnoise(pos / 20.0f)) * 4.0f * math.PI;
            var dir = float3.zero;
            math.sincos(angle, out dir.x, out dir.z);
            var newPosition = localTransform.ValueRO.Position + dir * tank.ValueRO.speed * SystemAPI.Time.DeltaTime;
            localTransform.ValueRW.Position = newPosition;
            localTransform.ValueRW.Rotation = quaternion.RotateY(angle);
        }
        #endif
        
        #if _JOB
        var job = new TankMoveJob()
        {
            deltaTime = SystemAPI.Time.DeltaTime
        };
        job.ScheduleParallel();
        #endif
    }

    [BurstCompile]
    [WithAll(typeof(Tank))]
    public partial struct TankMoveJob : IJobEntity
    {
        public float deltaTime;
        public void Execute(Entity entity, ref LocalTransform localTransform, ref Tank tank)
        {
            var pos = localTransform.Position;
            pos.y = entity.Index;
            var angle = (0.5f + noise.cnoise(pos / 20.0f)) * 4.0f * math.PI;
            var dir = float3.zero;
            math.sincos(angle, out dir.x, out dir.z);
            var newPosition = localTransform.Position + dir * tank.speed * deltaTime;
            localTransform.Position = newPosition;
            localTransform.Rotation = quaternion.RotateY(angle);
        }
    }
}
