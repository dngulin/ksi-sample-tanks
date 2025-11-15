using Ksi;
using KsiTanks.State;
using UnityEngine;

namespace KsiTanks.Simulation
{
    public static class TankMovementSystem
    {
        public static void Tick(in Specs specs, ref GameState state, ref FrameState frame)
        {
            foreach (ref var tank in state.Tanks.RefIter())
            {
                if (!tank.IsMoving())
                    continue;

                var moveDistance = Mathf.Min(specs.TankSpeed * frame.DeltaTime, tank.RemainingMoveDistance);
                tank.Position += tank.Direction.ToVector2() * moveDistance;
                tank.RemainingMoveDistance -= moveDistance;

                if (tank.IsMoving())
                    continue;

                tank.Position.Snap();
                tank.RemainingMoveDistance = 0;
            }
        }
    }
}