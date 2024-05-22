// #define _SIMPLE
// #define _COLOR
#define _COLOR_GROUP

using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

[BurstCompile]
[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct TankSpawnSystem : ISystem
{
   private Random random;
   public void OnCreate(ref SystemState state)
   {
      state.RequireForUpdate<Config>();
      random = new Random(1);
   }

   public void OnUpdate(ref SystemState state)
   {
      #if _SIMPLE
      if (SystemAPI.HasSingleton<Config>())
      {
         var config = SystemAPI.GetSingleton<Config>();
         for (int i = 0; i < config.tankCntToSpawn; i++)
         {
            var spawnedTank = state.EntityManager.Instantiate(config.tankEntityPrefab);
            LocalTransform newLocalTransform = LocalTransform.FromRotation(Quaternion.Euler(0, random.NextFloat(0, 360.0f), 0));
            state.EntityManager.SetComponentData(spawnedTank, newLocalTransform);
         }

         state.Enabled = false;
      }
      #endif
      
      #if _COLOR
      if (SystemAPI.HasSingleton<Config>())
      {
         var config = SystemAPI.GetSingleton<Config>();
         var tankArray = new NativeArray<Entity>(config.tankCntToSpawn, Allocator.Temp);
         state.EntityManager.Instantiate(config.tankEntityPrefab, tankArray);

         foreach (var tank in tankArray)
         {
            LocalTransform newLocalTransform =
               LocalTransform.FromRotation(Quaternion.Euler(0, random.NextFloat(0, 360.0f), 0));
            state.EntityManager.SetComponentData(tank, newLocalTransform);
            state.EntityManager.SetComponentData(tank, new URPMaterialPropertyBaseColor()
            {
               Value = RandomColor(ref random)
            });
         }
         
         state.Enabled = false;
      }
      #endif
      
      #if _COLOR_GROUP
      if (SystemAPI.HasSingleton<Config>())
      {
         var config = SystemAPI.GetSingleton<Config>();
         var tankArray = new NativeArray<Entity>(config.tankCntToSpawn, Allocator.Temp);
         var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged);
         
         ecb.Instantiate(config.tankEntityPrefab, tankArray);

         var query = SystemAPI.QueryBuilder().WithAll<URPMaterialPropertyBaseColor>().Build();
         var queryMask = query.GetEntityQueryMask();

         foreach (var tank in tankArray)
         {
            LocalTransform newLocalTransform =
               LocalTransform.FromRotation(quaternion.Euler(0, random.NextFloat(0, 360.0f), 0));
            ecb.SetComponent(tank, newLocalTransform);
            ecb.SetComponentForLinkedEntityGroup(tank, queryMask, new URPMaterialPropertyBaseColor()
            {
               Value = RandomColor(ref random)
            });
         }
         state.Enabled = false;
      }
      #endif
   }

   private float4 RandomColor(ref Random random)
   {
      return (Vector4)Color.HSVToRGB(random.NextFloat(0, 1), 1, 1);
   }
}
