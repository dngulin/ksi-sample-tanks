using Ksi;
using KsiTanks.State;
using UnityEngine;

namespace KsiTanks.Simulation
{
    public static class BulletCollisionSystem
    {
        private const float BulletRadius = 0.1f;
        private const float ObjectRadius = 0.4f;

        private const float BulletToBulletDistance = BulletRadius + BulletRadius;
        private const float BulletToObjectDistance = BulletRadius + ObjectRadius;

        private const float BulletToBulletSqrDistance = BulletToBulletDistance * BulletToBulletDistance;
        private const float BulletToObjectSqrDistance = BulletToObjectDistance * BulletToObjectDistance;

        public static void Tick(in Specs specs, ref GameState state)
        {
            BulletToWallsCollisions(specs, ref state);
            BulletToObstacleCollisions(specs, ref state);
            BulletToBulletCollisions(ref state);
            BulletToTankCollisions(ref state);
        }

        private static void BulletToWallsCollisions(in Specs specs, ref GameState state)
        {
            var bounds = new Rect(Vector2.zero - Vector2.one * 0.5f, specs.BoardSize);

            for (var i = state.Bullets.Count() - 1; i >= 0; i--)
            {
                if (!bounds.Contains(state.Bullets.RefReadonlyAt(i).Position))
                    state.Bullets.RemoveAt(i);
            }
        }

        private static void BulletToObstacleCollisions(in Specs specs, ref GameState state)
        {
            for (var i = state.Bullets.Count() - 1; i >= 0; i--)
            {
                foreach (var pos in specs.Obstacles.RefReadonlyIter())
                {
                    if ((state.Bullets.RefReadonlyAt(i).Position - pos).sqrMagnitude <= BulletToObjectSqrDistance)
                    {
                        state.Bullets.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        private static void BulletToBulletCollisions(ref GameState state)
        {
            for (var i = state.Bullets.Count() - 1; i >= 0; i--)
            {
                var posA = state.Bullets.RefAt(i).Position;
                var removedIdx = int.MaxValue;

                for (var j = state.Bullets.Count() - 1; j >= 0; j--)
                {
                    if (i == j)
                        continue;

                    var posB = state.Bullets.RefReadonlyAt(j).Position;
                    if ((posA - posB).sqrMagnitude <= BulletToBulletSqrDistance)
                    {
                        state.Bullets.RemoveAt(j);
                        removedIdx = j;
                        break;
                    }
                }

                if (removedIdx < i)
                    i--;

                if (removedIdx < int.MaxValue)
                    state.Bullets.RemoveAt(i);
            }
        }

        private static void BulletToTankCollisions(ref GameState state)
        {
            for (var bIdx = state.Bullets.Count() - 1; bIdx >= 0; bIdx--)
            {
                ref var bullet = ref state.Bullets.RefAt(bIdx);
                var bulletSide = bullet.Side;
                var bulletPos = bullet.Position;

                for (var tIdx = 0; tIdx < state.Tanks.Count(); tIdx++)
                {
                    ref var tank = ref state.Tanks.RefAt(tIdx);
                    if ((bulletPos - tank.Position).sqrMagnitude <= BulletToObjectSqrDistance)
                    {
                        if (tank.Side != bulletSide)
                            tank.HitPoints -= 1;

                        if (tank.HitPoints <= 0)
                            state.Tanks.RemoveAt(tIdx);

                        state.Bullets.RemoveAt(bIdx);
                        break;
                    }
                }
            }
        }
    }
}