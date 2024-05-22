using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class CannonBallAuthoring : MonoBehaviour
{
    public int maxBounce;
    public class CannonBallBaker : Baker<CannonBallAuthoring>
    {
        public override void Bake(CannonBallAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CannonBall()
            {
                velocity = float3.zero,
                currentBounce = 0,
                maxBounce = authoring.maxBounce
            });
        }
    }
}
