using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace ParcsCpppp
{
    public class BinComparer : IComparer<Bin>
    {
        public int Compare(Bin x, Bin y)
        {
            if (x.getItems().Count() > y.getItems().Count())
            {
                return 1;
            }
            if (x.getItems().Count() < y.getItems().Count())
            {
                return -1;
            }
            
            for(int i = 0; i < x.getItems().Count(); i++)
            {
                if (x.getItems()[i] > y.getItems()[i])
                {
                    return 1;
                }
                if (x.getItems()[i] < y.getItems()[i])
                {
                    return -1;
                }
            }

            return 0;
        }
    }
    [Serializable]
    public class Bin
    {
        private List<int> items;
        private int capacity;

        public Bin(int _capacity) 
        {
            items = new List<int>();
            capacity = _capacity;
        }

        public int getCapacity() { return capacity; }
        public ref List<int> getItems() { return ref items; }

        public void addItem(int item) 
        {
            items.Add(item);
            capacity -= item;
        }

        public void sort() 
        {
            items.Sort();
        }

        public void print()
        {
            Console.Write(" [");
            for (int i = 0; i < items.Count(); i++)
            {
                Console.Write(" ");
                Console.Write(items[i]);
            }
            Console.Write(" ]");
        }
    }
}
