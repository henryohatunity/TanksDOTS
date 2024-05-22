
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[UpdateAfter(typeof(CannonBallSpawnSystem))]
[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct CannonBallMoveSystem : ISystem
{
    private float3 gravity;
    
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<CannonBall>();
        gravity = new float3(0, -9.81f, 0);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        var job = new CannonBallMoveJob()
        {
            deltaTime = deltaTime,
            gravity = gravity
        };
        job.ScheduleParallel();
    }
}

[BurstCompile]
[WithAll(typeof(CannonBall))]
public partial struct CannonBallMoveJob : IJobEntity
{
    public float deltaTime;
    public float3 gravity;
    public void Execute(ref LocalTransform localTransform, ref CannonBall cannonBall)
    {
        localTransform.Position += cannonBall.velocity * deltaTime;
        // Gravity
        cannonBall.velocity += gravity * deltaTime;
        
        // Bounce
        if (localTransform.Position.y < 0)
        {
            localTransform.Position.y = 0;
            cannonBall.velocity.y *= -1;
            cannonBall.currentBounce++;
        }
    }
}
