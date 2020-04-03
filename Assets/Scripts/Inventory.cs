using System.Collections;
using System.Collections.Generic;
using DeloG.Items;

namespace DeloG
{
    public interface IReadOnlyInventory : IEnumerable<Item>
    {
        Item CurrentItem { get; }
    }
    public class Inventory : IReadOnlyInventory
    {
        public Item CurrentItem => Items.Count == 0 ? null : Items.First.Value;

        readonly int Capacity;
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
            return true;
        }
        public bool Remove(Item item) => Items.Remove(item);
        public void Shift(int count)
        {
            if (count == 0) return;

            if (count > 0)
                for (int i = 0; i < System.Math.Abs(count); i++)
                {
                    var last = Items.Last;
                    Items.RemoveLast();
                    Items.AddFirst(last);
                }
            else
                for (int i = 0; i < System.Math.Abs(count); i++)
                {
                    var first = Items.First;
                    Items.RemoveFirst();
                    Items.AddLast(first);
                }
        }

        public IEnumerator<Item> GetEnumerator() => Items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}