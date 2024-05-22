using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct TurretRotationSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<TurretRotation>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        var job = new TurretRotationJob()
        {
            deltaTime = deltaTime
        };
        job.ScheduleParallel();
    }
}

[BurstCompile]
[WithAll(typeof(TurretRotation))]
public partial struct TurretRotationJob : IJobEntity
{
    // public float rotSpeed;  // deg/sec
    public float deltaTime;
    public void Execute(RefRW<LocalTransform> localTransform)
    {
        float rot = 90.0f * deltaTime;
        var rotQuaternion = quaternion.RotateY(math.radians(rot));
        localTransform.ValueRW.Rotation = math.mul(rotQuaternion, localTransform.ValueRO.Rotation);
    }
}
