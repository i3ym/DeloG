using System;
using System.Collections;
using System.Collections.Generic;
using DeloG.Items;

namespace DeloG
{
    public interface IReadOnlyInventory : IEnumerable<Item>
    {
        Item CurrentItem { get; }
        int Count { get; }
        int Capacity { get; }
    }
    public class Inventory : IReadOnlyInventory
    {
        public event Action OnChange = delegate { };

        public Item CurrentItem => Items.Count == 0 ? null : Items.First.Value;
        public int Count => Items.Count;
        public int Capacity { get; }
        readonly LinkedList<Item> Items;

        public Inventory(int capacity)
        {
            Items = new LinkedList<Item>();
            Capacity = capacity;
        }

        public bool TryAdd(Item item)
        {
            if (Items.Count >= Capacity) return false;

            Items.AddFirst(item);
            OnChange();
            return true;
        }
        public bool Remove(Item item)
        {
            var remove = Items.Remove(item);
            OnChange();
            return remove;
        }
        public void Shift(int count)
        {
            if (count == 0) return;

            if (count > 0)
                for (int i = 0; i < Math.Abs(count); i++)
                {
                    var last = Items.Last;
                    Items.RemoveLast();
                    Items.AddFirst(last);
                }
            else
                for (int i = 0; i < Math.Abs(count); i++)
                {
                    var first = Items.First;
                    Items.RemoveFirst();
                    Items.AddLast(first);
                }

            OnChange();
        }

        public bool Contains(Item item) => Items.Contains(item);

        public IEnumerator<Item> GetEnumerator() => Items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}