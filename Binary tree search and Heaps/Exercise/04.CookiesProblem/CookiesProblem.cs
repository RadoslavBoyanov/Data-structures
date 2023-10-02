using System;
using System.Linq;
using _03.MinHeap;
using Wintellect.PowerCollections;

namespace _04.CookiesProblem
{
    public class CookiesProblem
    {
        public int Solve(int minSweetness, int[] cookies)
        {
            var priorityQueue = new OrderedBag<int>();

            foreach (var cookie in cookies)
            {
                priorityQueue.Add(cookie);
            }

            int currentSweetnes = priorityQueue[0];

            int numberOfOperations = 0;

            while (currentSweetnes > minSweetness && priorityQueue.Count > 0) 
            {
                int leastSweetCookie = priorityQueue.RemoveFirst();
                int secondLeastSweetCookie = priorityQueue.RemoveFirst();

                int specialCombinedCookie = leastSweetCookie + 2 * secondLeastSweetCookie;

                priorityQueue.Add(specialCombinedCookie);
                
                currentSweetnes= priorityQueue[0];

                numberOfOperations++;
            }

            if (currentSweetnes < minSweetness)
            {
                return -1;
            }
            return numberOfOperations;
        }
    }
}
