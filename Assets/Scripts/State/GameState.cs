using Ksi;
using KsiTanks.Simulation;
using UnityEngine;

namespace KsiTanks.State
{
    [ExplicitCopy, DynSized, Dealloc]
    public struct GameState
    {
        public RefList<Tank> Tanks;
        public RefList<Bullet> Bullets;
        public XorShiftRand Random;
        public float BotSpawnCooldown;
    }

    public struct Tank
    {
        public Vector2 Position;
        public Direction Direction;
        public float RemainingMoveDistance;

        public int HitPoints;
        public float ReloadCooldown;

        public Side Side;
    }

    public struct Bullet
    {
        public Vector2 Position;
        public Direction Direction;
        public Side Side;
    }

    public enum Side : byte
    {
        Bot,
        Player,
    }
}