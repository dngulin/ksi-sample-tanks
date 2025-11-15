using System;
using KsiTanks.State;
using UnityEngine;

namespace KsiTanks.View
{
    public class BulletView : EntityView<Bullet>
    {
        [SerializeField] private Sprite _red;
        [SerializeField] private Sprite _blue;

        [SerializeField] private SpriteRenderer _renderer;

        protected override void UpdateSide(Side side)
        {
            _renderer.sprite = side switch {
                Side.Bot => _red,
                Side.Player => _blue,
                _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
            };
        }

        public override void SetState(in Bullet state)
        {
            Side = state.Side;
            transform.SetLocalPositionAndRotation(state.Position, state.Direction.ToRotation());
        }
    }
}