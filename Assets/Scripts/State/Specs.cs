using Ksi;
using UnityEngine;

namespace KsiTanks.State
{
    [ExplicitCopy, DynSized, Dealloc]
    public struct Specs
    {
        public Vector2Int BoardSize;
        public RefList<Vector2Int> Obstacles;

        public float TankSpeed;
        public float BulletSpeed;

        public float ReloadTime;
        public float SpawnPeriod;
    }
}