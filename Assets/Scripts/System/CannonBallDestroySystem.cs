

using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
[UpdateAfter(typeof(TransformSystemGroup))]
public partial struct CannonBallDestroySystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<CannonBall>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged);
        foreach (var (cannonBall, entity) in SystemAPI.Query<RefRO<CannonBall>>().WithEntityAccess())
        {
            if (cannonBall.ValueRO.currentBounce >= cannonBall.ValueRO.maxBounce)
            {
                ecb.DestroyEntity(entity);
            }
        }
    }
    
}
