using System;
using Ksi;
using KsiTanks.Simulation;
using KsiTanks.State;
using KsiTanks.View;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace KsiTanks
{
    public class GameLoop : MonoBehaviour
    {
        private readonly ExclusiveAccess<GameState> _state = new ExclusiveAccess<GameState>();
        private readonly ExclusiveAccess<Specs> _specs = new ExclusiveAccess<Specs>();

        [SerializeField] private TankView _tankPrefab;
        [SerializeField] private BulletView _bulletPrefab;
        [SerializeField] private GameObject _obstaclePrefab;

        private GameView _gameView;

        private void OnDestroy()
        {
            using var stateAccessor = _state.Mutable;
            using var specsAccessor = _specs.Mutable;
            try
            {
                stateAccessor.Value.Dealloc();
                specsAccessor.Value.Dealloc();
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
            }
        }

        private void Start()
        {
            using var specAccessor = _specs.Mutable;
            using var stateAccessor = _state.Mutable;

            ref var specs = ref specAccessor.Value;
            ref var state = ref stateAccessor.Value;

            InitSpecs(ref specs);
            GameView.SpawnObstacles(specs, _obstaclePrefab, transform);

            GameLogic.SetupInitialState(ref state, (uint)Random.Range(int.MinValue, int.MaxValue));
            _gameView = new GameView(transform, _tankPrefab, _bulletPrefab);
            _gameView.Update(state);
        }

        private static void InitSpecs(ref Specs specs)
        {
            specs.BoardSize = new Vector2Int(7, 7);
            specs.TankSpeed = 2;
            specs.BulletSpeed = 4;
            specs.ReloadTime = 0.6f;
            specs.SpawnPeriod = 5;
            specs.Obstacles.Add(new Vector2Int(1, 4));
            specs.Obstacles.Add(new Vector2Int(3, 2));
            specs.Obstacles.Add(new Vector2Int(5, 5));
        }

        private void Update()
        {
            using var specsAccessor = _specs.ReadOnly;
            using var stateAccessor = _state.Mutable;

            ref readonly var specs = ref specsAccessor.Value;
            ref var state = ref stateAccessor.Value;
            var frameState = new FrameState
            {
                DeltaTime = Time.deltaTime,
                Inputs = ReadPlayerInputs()
            };

            GameLogic.Tick(specs, ref state, ref frameState);
            _gameView.Update(state);
        }

        private static PlayerInputs ReadPlayerInputs()
        {
            var inputs = PlayerInputs.None;

            if (Keyboard.current.wKey.isPressed) inputs |= PlayerInputs.Up;
            if (Keyboard.current.dKey.isPressed) inputs |= PlayerInputs.Right;
            if (Keyboard.current.sKey.isPressed) inputs |= PlayerInputs.Down;
            if (Keyboard.current.aKey.isPressed) inputs |= PlayerInputs.Left;

            if (Keyboard.current.spaceKey.isPressed)
                inputs |= PlayerInputs.Fire;

            return inputs;
        }
    }
}