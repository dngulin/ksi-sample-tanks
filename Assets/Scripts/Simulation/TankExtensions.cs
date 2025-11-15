using KsiTanks.State;

namespace KsiTanks.Simulation
{
    public static class TankExtensions
    {
        public static bool IsMoving(this in Tank self) => self.RemainingMoveDistance > 0;
        public static bool CanShoot(this in Tank self) => self.ReloadCooldown <= 0;
    }
}