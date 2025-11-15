using Ksi;
using KsiTanks.State;

namespace KsiTanks.Simulation
{
    public static class TankControlSystem
    {
        public static void Tick(in Specs specs, ref GameState state, ref FrameState frame)
        {
            InitMovementConstraints(specs, state, ref frame);
            ControlPlayerTank(specs, ref state, ref frame);
            ControlBotTanks(specs, ref state, ref frame);
        }

        private static void InitMovementConstraints(in Specs specs, in GameState state, ref FrameState frame)
        {
            frame.EnterConstraints.Reset(specs);

            foreach (ref readonly var tank in state.Tanks.RefReadonlyIter())
            {
                if (!tank.IsMoving())
                {
                    frame.EnterConstraints.RefAtCell(specs, tank.Position.ToVector2Int()) = Directions.All;
                    continue;
                }

                var cellTo = (tank.Position + tank.Direction.ToVector2() * tank.RemainingMoveDistance).ToVector2Int();
                var cellFrom = cellTo - tank.Direction.ToVector2Int();

                var constraint = ~tank.Direction.ToFlag();
                frame.EnterConstraints.RefAtCell(specs, cellFrom) = constraint;
                frame.EnterConstraints.RefAtCell(specs, cellTo) = constraint;
            }
        }

        private static void ControlPlayerTank(in Specs specs, ref GameState state, ref FrameState frame)
        {
            if (state.Tanks.Count() == 0 || frame.Inputs.Is(PlayerInputs.None))
                return;

            ref var tank = ref state.Tanks.RefAt(0);
            if (tank.Side != Side.Player)
                return;

            if (!tank.IsMoving() && frame.Inputs.TryGetDirection(out var dir))
                TryStartMovement(specs, ref tank, dir, ref frame);

            if (tank.ReloadCooldown > 0)
                tank.ReloadCooldown -= frame.DeltaTime;

            if (tank.CanShoot() && frame.Inputs.Contains(PlayerInputs.Fire))
                SpawnBullet(specs, ref state.Bullets, ref tank);
        }

        private static void ControlBotTanks(in Specs specs, ref GameState state, ref FrameState frame)
        {
            foreach (ref var tank in state.Tanks.RefIter())
            {
                if (tank.Side != Side.Bot)
                    continue;

                if (!tank.IsMoving())
                    TryStartMovement(specs, ref tank, (Direction)state.Random.NextInt(4), ref frame);

                if (tank.ReloadCooldown > 0)
                    tank.ReloadCooldown -= frame.DeltaTime;

                if (tank.CanShoot())
                    SpawnBullet(specs, ref state.Bullets, ref tank);
            }
        }

        private static void TryStartMovement(in Specs specs, ref Tank tank, Direction dir, ref FrameState frame)
        {
            var cellFrom = tank.Position.ToVector2Int();
            var cellTo = cellFrom + dir.ToVector2Int();

            var constraints = frame.EnterConstraints.AtCell(specs, cellTo);

            var dirFlag = dir.ToFlag();
            if (!constraints.Contains(dirFlag))
            {
                tank.Direction = dir;
                tank.RemainingMoveDistance = 1.0f;

                var constraint = ~dirFlag;
                frame.EnterConstraints.RefAtCell(specs, cellFrom) = constraint;
                frame.EnterConstraints.RefAtCell(specs, cellTo) = constraint;
            }
        }

        private static void SpawnBullet(in Specs specs, ref RefList<Bullet> bullets, ref Tank tank)
        {
            ref var bullet = ref bullets.RefAdd();
            bullet.Position = tank.Position + tank.Direction.ToVector2() * 0.5f;
            bullet.Direction = tank.Direction;
            bullet.Side = tank.Side;

            tank.ReloadCooldown = specs.ReloadTime;
        }
    }
}