using Ksi;
using KsiTanks.State;
using UnityEngine;

namespace KsiTanks.Simulation
{
    public static class BotSpawningSystem
    {
        private const int TargetBotCount = 3;

        public static void Tick(in Specs specs, ref GameState state, in FrameState frame)
        {
            if (state.BotSpawnCooldown > 0)
                state.BotSpawnCooldown -= frame.DeltaTime;

            var botCount = state.Tanks.Count();
            if (botCount > 0 && state.Tanks.RefAt(0).Side == Side.Player)
                botCount--;

            if (state.BotSpawnCooldown > 0 || botCount >= TargetBotCount)
                return;

            var pos = specs.BoardSize - Vector2Int.one;
            var constraint = frame.EnterConstraints.AtCell(specs, pos);
            if (!constraint.Is(Directions.None))
                return;

            state.BotSpawnCooldown = specs.SpawnPeriod;

            ref var tank = ref state.Tanks.RefAdd();
            tank.Position = pos;
            tank.Direction = Direction.Down;
            tank.HitPoints = 1;
        }
    }
}