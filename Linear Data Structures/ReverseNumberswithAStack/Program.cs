namespace _05.ReverseNumberswithAStack
{
    public class Program
    {
        static void Main(string[] args)
        {
            int[] input = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();
            var stack = new Stack<int>();

            foreach (var number in input)
            {
                stack.Push(number);
            }

            Console.WriteLine(string.Join(" ", stack));

        }
    }
}