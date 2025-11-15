using System;

namespace KsiTanks.State
{
    [Flags]
    public enum PlayerInputs : byte
    {
        None = 0,
        Up = 1 << 0,
        Right = 1 << 1,
        Down = 1 << 2,
        Left = 1 << 3,
        Fire = 1 << 4,
    }

    public static class PlayerInputsExtensions
    {
        public static bool Contains(this PlayerInputs self, PlayerInputs other) => (self & other) == other;

        public static bool Is(this PlayerInputs self, PlayerInputs other) => self == other;

        public static bool TryGetDirection(this PlayerInputs self, out Direction direction)
        {
            if (self.Contains(PlayerInputs.Up))
            {
                direction = Direction.Up;
                return true;
            }

            if (self.Contains(PlayerInputs.Right))
            {
                direction = Direction.Right;
                return true;
            }

            if (self.Contains(PlayerInputs.Down))
            {
                direction = Direction.Down;
                return true;
            }

            if (self.Contains(PlayerInputs.Left))
            {
                direction = Direction.Left;
                return true;
            }

            direction = default;
            return false;
        }
    }
}