using Unity.Mathematics;

namespace KsiTanks.Simulation
{
    public struct XorShiftRand
    {
        public uint State;
    }

    // Code copied from UnityMathematics.Random and moved to extension methods
    public static class XorShiftRandExtensions
    {
        public static void SetSeed(ref this XorShiftRand self, uint seed)
        {
            self.State = seed;
            self.Next();
        }

        public static int NextInt(ref this XorShiftRand self) => (int)self.Next() ^ -2147483648;
        public static int NextInt(ref this XorShiftRand self, int max) => (int)((self.Next() * (ulong)max) >> 32);
        public static int NextInt(ref this XorShiftRand self, int min, int max) => self.NextInt(max - min) + min;

        public static float NextFloat(ref this XorShiftRand self) => math.asfloat(0x3f800000 | (self.Next() >> 9)) - 1.0f;

        private static uint Next(ref this XorShiftRand self)
        {
            var result = self.State;

            ref var state = ref self.State;
            state ^= state << 13;
            state ^= state >> 17;
            state ^= state << 5;

            return result;
        }
    }
}