using System.Collections.Generic;
using UnityEngine;

namespace KsiTanks.View
{
    public class EntityViewPool<TState> where TState : struct
    {
        private readonly Stack<EntityView<TState>> _stack;

        private readonly EntityView<TState> _prefab;
        private readonly Transform _parent;
        private readonly int _poolSize;

        public EntityViewPool(EntityView<TState> prefab, Transform parent, int poolSize)
        {
            _prefab = prefab;
            _parent = parent;
            _poolSize = poolSize;
            _stack = new Stack<EntityView<TState>>(poolSize);

            for (var i = 0; i < poolSize; i++)
            {
                var view = Object.Instantiate(_prefab, _parent);
                view.gameObject.SetActive(false);
                _stack.Push(view);
            }
        }

        public EntityView<TState> GetView()
        {
            if (_stack.Count == 0)
                return Object.Instantiate(_prefab, _parent);

            var view = _stack.Pop();
            view.gameObject.SetActive(true);
            return view;
        }

        public void ReturnView(EntityView<TState> view)
        {
            if (_stack.Count >= _poolSize)
            {
                view.DestroyGameObject();
                return;
            }

            view.gameObject.SetActive(false);
            _stack.Push(view);
        }
    }
}