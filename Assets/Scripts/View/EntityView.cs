using System.Collections.Generic;
using KsiTanks.State;
using UnityEngine;

namespace KsiTanks.View
{
    public abstract class EntityView<TState> : MonoBehaviour where TState : struct
    {
        [SerializeField]
        private Side _side;

        public Side Side {
            get => _side;
            set
            {
                if (value != _side)
                {
                    _side = value;
                    UpdateSide(_side);
                }
            }
        }

        protected abstract void UpdateSide(Side side);

        public abstract void SetState(in TState state);
    }

    public static class EntityViewExtensions
    {
        public static EntityView<TState> WithState<TState>(this EntityView<TState> self, in TState state)
            where TState : struct
        {
            self.SetState(state);
            return self;
        }

        public static void DestroyGameObject<TState>(this EntityView<TState> self) where TState : struct
        {
            if (self.gameObject != null)
                Object.Destroy(self.gameObject);
        }

        public static EntityView<TState> Pop<TState>(this List<EntityView<TState>> self) where TState : struct
        {
            var idx = self.Count - 1;
            var view = self[idx];
            self.RemoveAt(idx);
            return view;
        }
    }
}