using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[UpdateAfter(typeof(CannonBallDestroySystem))]
public partial struct CannonBallCountSystem : ISystem
{
    private int cnt;
    
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<CannonBall>();
        cnt = 0;
        // state.Enabled = false;
    }

    // [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        int currentCnt = SystemAPI.QueryBuilder().WithAll<CannonBall>().Build().CalculateEntityCount();
        if (currentCnt != cnt)
        {
            cnt = currentCnt;
            UIManager.Instance.UpdateBallCount(cnt);
        }
    }
}
