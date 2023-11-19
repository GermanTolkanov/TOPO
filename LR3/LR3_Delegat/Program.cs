namespace LR3_Delegat
{
    internal class Program
    {
        delegate void CalcTable(int x, int y);
        static void Main(string[] args)
        {
            Console.Write("Введите x = ");
            int x = Int32.Parse(Console.ReadLine());
            Console.Write("Введите y = ");
            int y = Int32.Parse(Console.ReadLine());
            CalcTable calcTable = Addition;  
            calcTable += Multiplication;
            calcTable(x, y);
            Console.ReadLine();
        }
        public static void Addition (int x, int y)
        {
            Console.Write("Сложение\n\t");
            for (int i = 1; i < x+1; i++)
            {
                Console.Write($"{i}\t");
            }
            Console.WriteLine();
            for (int i = 1; i < x+1; i++)
            {
                Console.Write($"{i}\t");
                for (int j = 1; j < y+1; j++)
                {
                    if(j == y)
                        Console.Write($"{i + j}\n");
                    else
                        Console.Write($"{i + j}\t");
                } 
            }
        }
        public static void Multiplication(int x, int y)
        {
            Console.Write("Умножение\n\t");
            for (int i = 1; i < x + 1; i++)
            {
                Console.Write($"{i}\t");
            }
            Console.WriteLine();
            for (int i = 1; i < x + 1; i++)
            {
                Console.Write($"{i}\t");
                for (int j = 1; j < y + 1; j++)
                {
                    if (j == y)
                        Console.Write($"{i * j}\n");
                    else
                        Console.Write($"{i * j}\t");
                }
            }
        }
    }
}