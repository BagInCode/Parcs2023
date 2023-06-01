using Parcs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParcsCpppp
{
    internal class Executor : IModule
    {
        public static int factorial(int x)
        {
            int result = 1;

            for (int i = 2; i <= x; i++)
            {
                result *= i;
            }

            return result;
        }
        public void GenerateOrder(int orderId, List <int> basicOrder, ref List<int> resultOrder)
        {
            List<int> basicOrderCopy = basicOrder;

            int fact = factorial(basicOrder.Count()-1);

            for (int i = basicOrderCopy.Count()-1; i > 0; i--)
            {
                int pos = orderId / fact;

                resultOrder.Add(basicOrderCopy[pos]);

                basicOrderCopy.RemoveAt(pos);

                orderId %= fact;
                fact /= i;
            }

            resultOrder.Add(basicOrderCopy[0]);
        }

        public void printOrder( ref List<int> items)
        {
            Console.WriteLine("Order: ");
            Console.Write(" [");
            for (int i = 0; i < items.Count(); i++)
            {
                Console.Write(" "); 
                Console.Write(items[i]);
            }
            Console.WriteLine(" ]");

            return;
        }

        public List<Bin> FFalgo(int capacity, ref List<int> items)
        {
            List<Bin> result = new List<Bin>();

            foreach(int item in items)
            {
                bool binWasFounded = false;
                for(int i = 0; i < result.Count(); i++)
                {
                    if( result[i].getCapacity() >= item)
                    {
                        binWasFounded = true;
                        result[i].addItem(item);
                        break;
                    }
                }

                if(!binWasFounded)
                {
                    result.Add(new Bin(capacity));
                    result[result.Count()-1].addItem(item);
                }
            }

            for(int i = 0; i < result.Count(); i++)
            {
                result[i].sort();
            }

            result.Sort(new BinComparer());

            return result;
        }

        public void printResult(ref List<Bin> result)
        {
            Console.WriteLine("Result: ");
            Console.Write(" [");
            for(int i = 0; i < result.Count(); i++)
            {
                result[i].print();
            }
            Console.WriteLine(" ]");

            return;
        }

        public static void updateAnswer(ref int minBinCount, ref List<List<Bin>> answer, ref List<Bin> orderResult)
        {
            if(minBinCount < orderResult.Count())
            {
                return;
            }
            if(minBinCount > orderResult.Count())
            {
                minBinCount = orderResult.Count();
                answer = new List<List<Bin>>();
                answer.Add(orderResult);
                
                return;
            }
            foreach(List<Bin> result in answer)
            {
                BinComparer comparer = new BinComparer();
                bool unique = false;

                for (int i = 0; i < result.Count(); i++)
                {
                    if (comparer.Compare(result[i], orderResult[i]) != 0)
                    {
                        unique = true;
                        break;
                    }
                }

                if (!unique) return;
            }
            answer.Add(orderResult);
            
            return;
        }

        public void printAnswer(ref List<List<Bin>> answer)
        {
            Console.WriteLine("Answer: ");
            foreach (List<Bin> result in answer)
            {
                Console.Write(" [");

                foreach (Bin bin in result)
                {
                    Console.Write(" [");
                    foreach (int item in bin.getItems())
                    {
                        Console.Write(' ');
                        Console.Write(item);
                    }
                    Console.Write(" ]");
                }
                Console.Write(" ]");
                Console.WriteLine();
            }
        }

        public void Run(ModuleInfo info, CancellationToken token = default)
        {
            List<int> items = info.Parent.ReadObject<List<int>>();
            int capacity = info.Parent.ReadObject<int>();
            int indexStart = info.Parent.ReadObject<int>();
            int indexEnd = info.Parent.ReadObject<int>();

            int minBinCount = items.Count();
            List<List<Bin>> answer = new List<List<Bin>>();

            for (int i = indexStart; i < indexEnd; i++)
            {
                List<int> order = new List<int>();
                List<int> itemsCopy = new List<int>(items);
                GenerateOrder(i, itemsCopy, ref order);
                printOrder(ref order);
                printOrder(ref items);

                List < Bin > orderResult = FFalgo(capacity, ref order);
                printResult(ref orderResult);
                
                updateAnswer(ref minBinCount, ref answer, ref orderResult);
                printAnswer(ref answer);
            }
            printAnswer(ref answer);
            info.Parent.WriteObject(answer);

            return;
        }
    }
}
