using Ksi;
using KsiTanks.State;
using Unity.Burst;

namespace KsiTanks.Simulation
{
    [BurstCompile]
    public static class GameLogic
    {
        [BurstCompile]
        public static void Tick(in Specs specs, ref GameState state, ref FrameState frame)
        {
            TankMovementSystem.Tick(specs, ref state, ref frame);
            BulletMovementSystem.Tick(specs, ref state, ref frame);
            BulletCollisionSystem.Tick(specs, ref state);
            TankControlSystem.Tick(specs, ref state, ref frame);
            BotSpawningSystem.Tick(specs, ref state, frame);
        }

        [BurstCompile]
        public static void SetupInitialState(ref GameState state, uint seed)
        {
            state.Deallocated() = default;
            state.Random.SetSeed(seed);

            ref var playerTank = ref state.Tanks.RefAdd();
            playerTank.HitPoints = 3;
            playerTank.Side = Side.Player;
        }
    }
}