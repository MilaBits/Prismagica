using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.Systems.RuntimeSet
{
    public abstract class RuntimeSet<T> : ScriptableObject
    {
        protected List<T> items = new List<T>();

        public void Initialize() => items.Clear();
        public int Count => items.Count;
        public virtual T GetItemAtIndex(int index) => items[index];

        public UnityEvent ItemsChanged;

        public void AddToSet(T item)
        {
            if (!items.Contains(item))
            {
                items.Add(item);
                ItemsChanged.Invoke();
            }
        }

        public void RemoveFromSet(T item)
        {
            if (items.Contains(item))
            {
                items.Remove(item);
                ItemsChanged.Invoke();
            }
        }
    }
}