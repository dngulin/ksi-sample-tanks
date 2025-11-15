using System;
using KsiTanks.State;
using UnityEngine;

namespace KsiTanks.View
{
    public class TankView : EntityView<Tank>
    {
        [SerializeField] private Sprite _redBody;
        [SerializeField] private Sprite _redTurret;

        [SerializeField] private Sprite _blueBody;
        [SerializeField] private Sprite _blueTurret;

        [SerializeField] private SpriteRenderer _bodyRenderer;
        [SerializeField] private SpriteRenderer _turretRenderer;

        protected override void UpdateSide(Side side)
        {
            switch (side)
            {
                case Side.Bot:
                    _bodyRenderer.sprite = _redBody;
                    _turretRenderer.sprite = _redTurret;
                    break;

                case Side.Player:
                    _bodyRenderer.sprite = _blueBody;
                    _turretRenderer.sprite = _blueTurret;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }
        }

        public override void SetState(in Tank state)
        {
            Side = state.Side;
            transform.SetLocalPositionAndRotation(state.Position, state.Direction.ToRotation());
        }
    }
}