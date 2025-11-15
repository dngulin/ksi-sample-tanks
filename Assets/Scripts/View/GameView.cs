using System.Collections.Generic;
using Ksi;
using KsiTanks.State;
using UnityEngine;
using Object = UnityEngine.Object;

namespace KsiTanks.View
{
    public class GameView
    {
        private readonly EntityViewPool<Tank> _tankPool;
        private readonly EntityViewPool<Bullet> _bulletPool;

        private readonly List<EntityView<Tank>> _tanks = new List<EntityView<Tank>>(4);
        private readonly List<EntityView<Bullet>> _bullets = new List<EntityView<Bullet>>(10);

        public GameView(Transform root, TankView tankPrefab, BulletView bulletPrefab)
        {
            _tankPool = new EntityViewPool<Tank>(tankPrefab, root, 4);
            _bulletPool = new EntityViewPool<Bullet>(bulletPrefab, root, 10);
        }

        public static void SpawnObstacles(in Specs specs, GameObject prefab, Transform parent)
        {
            foreach (ref readonly var pos in specs.Obstacles.RefReadonlyIter())
            {
                var obstacle = Object.Instantiate(prefab, parent);
                obstacle.transform.localPosition = (Vector2)pos;
            }
        }

        public void Update(in GameState state)
        {
            MapEntities(state.Tanks, _tanks, _tankPool);
            MapEntities(state.Bullets, _bullets, _bulletPool);
        }

        private static void MapEntities<TState>(
            in RefList<TState> states, List<EntityView<TState>> views, EntityViewPool<TState> pool
        )
            where TState : unmanaged
        {
            var syncIterations = Mathf.Min(views.Count, states.Count());
            for (var i = 0; i < syncIterations; i++)
                views[i].SetState(states.RefReadonlyAt(i));

            while (views.Count < states.Count())
                views.Add(pool.GetView().WithState(states.RefReadonlyAt(views.Count)));

            while (views.Count > states.Count())
                pool.ReturnView(views.Pop());
        }
    }
}