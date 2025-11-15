using System;
using Ksi;
using UnityEngine;

namespace KsiTanks.State
{
    [ExplicitCopy, DynSized, TempAlloc]
    public struct FrameState
    {
        public float DeltaTime;
        public PlayerInputs Inputs;
        public TempRefList<Directions> EnterConstraints;
    }

    public static class ConstraintsExtensions
    {
        public static void Reset(ref this TempRefList<Directions> self, in Specs specs)
        {
            self.Clear();
            self.AppendDefault(specs.BoardSize.x * specs.BoardSize.y);

            foreach (ref readonly var pos in specs.Obstacles.RefReadonlyIter())
                self.RefAtCell(specs, pos) = Directions.All;
        }

        public static Directions AtCell(in this TempRefList<Directions> self, in Specs specs, Vector2Int cell)
        {
            var rect = new RectInt(Vector2Int.zero, specs.BoardSize);
            if (!rect.Contains(cell))
                return Directions.All;

            return self.RefReadonlyAt(cell.y * specs.BoardSize.x + cell.x);
        }

        [RefPath("self", "!", "[n]")]
        public static ref Directions RefAtCell([DynNoResize] ref this TempRefList<Directions> self, in Specs specs, Vector2Int cell)
        {
            var rect = new RectInt(Vector2Int.zero, specs.BoardSize);
            if (!rect.Contains(cell))
                throw new IndexOutOfRangeException();

            return ref self.RefAt(cell.y * specs.BoardSize.x + cell.x);
        }
    }
}