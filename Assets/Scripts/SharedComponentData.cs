
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.Rendering;

// Global variables
public struct Config : IComponentData
{
    public Entity tankEntityPrefab;
    public int tankCntToSpawn;
    public float safetyRadiusSqr;
    public float cannonBallSpeed;
}

// Components
public struct Tank : IComponentData
{
    public float speed;
}

public struct CannonBall : IComponentData
{
    public float3 velocity;
    public int maxBounce;
    public int currentBounce;
}

public struct Turret : IComponentData
{
    public Entity cannonBallEntityPrefab;
    public Entity spawnPointEntity;
}

public struct TurretRotation : IComponentData
{
    public float rotationSpeed;
}

public struct Shooting : IComponentData, IEnableableComponent{}


