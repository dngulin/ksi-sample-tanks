using UnityEngine;

namespace KsiTanks.Simulation
{
    public static class Vector2Extensions
    {
        public static void Snap(ref this Vector2 self)
        {
            self.x = Mathf.Round(self.x);
            self.y = Mathf.Round(self.y);
        }

        public static Vector2Int ToVector2Int(in this Vector2 self)
        {
            return Vector2Int.RoundToInt(self);
        }
    }
}