
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[UpdateAfter(typeof(TankMoveSystem))]
[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct SafetySystem : ISystem
{
   public void OnCreate(ref SystemState state)
   {
      state.RequireForUpdate<Config>();
      state.RequireForUpdate<Tank>();
   }

   [BurstCompile]
   public void OnUpdate(ref SystemState state)
   {
      float radiusSqr = SystemAPI.GetSingleton<Config>().safetyRadiusSqr;
      var job = new SafetyJob()
      {
         radiusSqr = radiusSqr
      };
      job.ScheduleParallel();
   }
}

[BurstCompile]
[WithOptions(EntityQueryOptions.IgnoreComponentEnabledState)]
// [WithAll(typeof(Shooting))]
public partial struct SafetyJob : IJobEntity
{
   public float radiusSqr;

   void Execute(RefRO<LocalToWorld> transformMatrix, EnabledRefRW<Shooting> shooting)
   {
      shooting.ValueRW = math.lengthsq(transformMatrix.ValueRO.Position) > radiusSqr;
   }
}
