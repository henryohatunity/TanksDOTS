using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class TankAuthoring : MonoBehaviour
{
    public float speed = 1.0f;
    public class TankBaker : Baker<TankAuthoring>
    {
        public override void Bake(TankAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Tank(){speed = authoring.speed});
        }
    }
}
