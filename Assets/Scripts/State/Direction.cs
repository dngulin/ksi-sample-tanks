using System;
using UnityEngine;

namespace KsiTanks.State
{
    public enum Direction : byte
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3,
    }

    [Flags]
    public enum Directions : byte
    {
        None = 0,
        Up = 1 << 0,
        Right = 1 << 1,
        Down = 1 << 2,
        Left = 1 << 3,
        All =  Up | Right | Down | Left
    }

    public static class DirectionExtensions
    {
        public static Vector2 ToVector2(this Direction self)
        {
            return self switch
            {
                Direction.Up => Vector2.up,
                Direction.Right => Vector2.right,
                Direction.Down => Vector2.down,
                Direction.Left => Vector2.left,
                _ => throw new ArgumentOutOfRangeException(nameof(self), self, null)
            };
        }

        public static Vector2Int ToVector2Int(this Direction self)
        {
            return self switch
            {
                Direction.Up => Vector2Int.up,
                Direction.Right => Vector2Int.right,
                Direction.Down => Vector2Int.down,
                Direction.Left => Vector2Int.left,
                _ => throw new ArgumentOutOfRangeException(nameof(self), self, null)
            };
        }

        public static Directions ToFlag(this Direction self)
        {
            return self switch
            {
                Direction.Up => Directions.Up,
                Direction.Right => Directions.Right,
                Direction.Down => Directions.Down,
                Direction.Left => Directions.Left,
                _ => throw new ArgumentOutOfRangeException(nameof(self), self, null)
            };
        }

        public static Quaternion ToRotation(this Direction self)
        {
            return self switch
            {
                Direction.Up => Quaternion.AngleAxis(0, Vector3.back),
                Direction.Right => Quaternion.AngleAxis(90, Vector3.back),
                Direction.Down => Quaternion.AngleAxis(180, Vector3.back),
                Direction.Left => Quaternion.AngleAxis(270, Vector3.back),
                _ => throw new ArgumentOutOfRangeException(nameof(self), self, null)
            };
        }
    }

    public static class DirectionsExtensions
    {
        public static bool Contains(this Directions self, Directions other) => (self & other) == other;

        public static bool Is(this Directions self, Directions other) => self == other;
    }
}