using Problem01.CircularQueue;

namespace _06.CalculateSequenceWithAQueue
{
    public class Program
    {
        static void Main(string[] args)
        {
            int current = int.Parse(Console.ReadLine());

            var queue = new Queue<int>();
            queue.Enqueue(current);

            for (int i = 0; i < 50; i++)
            {
                var s1 = current + 1;
                var s2 = 2 * current + 1;
                var s3 = current + 2;

                queue.Enqueue(s1);
                queue.Enqueue(s2);
                queue.Enqueue(s3);
                current = queue.ElementAtOrDefault(i);
            }

            Console.WriteLine(string.Join(" ", queue.ToArray()));
        }
    }
}