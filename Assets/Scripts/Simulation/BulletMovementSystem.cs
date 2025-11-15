using Ksi;
using KsiTanks.State;
using UnityEngine;

namespace KsiTanks.Simulation
{
    public static class BulletMovementSystem
    {
        private const float MaxBulletMovement = 0.2f;

        public static void Tick(in Specs specs, ref GameState state, ref FrameState frame)
        {
            foreach (ref var bullet in state.Bullets.RefIter())
            {
                var moveDistance = Mathf.Min(specs.BulletSpeed * frame.DeltaTime, MaxBulletMovement);
                bullet.Position += bullet.Direction.ToVector2() * moveDistance;
            }
        }
    }
}