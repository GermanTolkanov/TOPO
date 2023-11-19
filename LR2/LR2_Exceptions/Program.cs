using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace LR2_Exceptions
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double[] input;
            Console.WriteLine("Программа решения квадратного уравнения");
        Begining:
            input = GetData();
            try
            {
                Solution(input);
            }
            catch(AEqZeroException ex) 
            {
                Console.WriteLine(ex.Message);
                goto Begining;
            }
            catch(DSubZeroException ex)
            {
                Console.WriteLine(ex.Message);
                goto Begining;
            }
        }

        static double[] GetData()
        {
            string str, path;
            double[] data = new double[3];
            Begining:
            Console.WriteLine("Введите путь файла с коэффициентами a, b, c");
            while (true)
            {
                try
                {
                    path = Console.ReadLine();
                    if (path != "") 
                    {
                        str = File.ReadAllText(path);
                        break;
                    }
                }
                catch (FileNotFoundException ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Введите путь заново");
                }
            }
            Regex rg = new Regex(@"^-?\d+(,\d+)?[|\s]+-?\d+(,\d+)?[|\s]+-?\d+(,\d+)?$");
            Regex splitRg = new Regex(@"[|\s]+");
            try
            {
                if (!rg.IsMatch(str))
                {
                    throw new WrongParamsException("Неверный формат данных");
                }
                string[] strMassive= splitRg.Split(str);
                for (int i = 0; i < strMassive.Length; i++)
                {
                    data[i] = Double.Parse(strMassive[i]);
                }
            }
            catch(WrongParamsException ex)
            {
                Console.WriteLine($"{ex.Message}");
                goto Begining;
            }
            return data;
        }

        static void Solution(double[] input)
        {
            if (input[0] == 0)
            {
                throw new AEqZeroException("Уравнение не квадратное (а = 0)");
            }
            double D = Math.Pow(input[1], 2) - 4 * input[0] * input[2];
            if (D < 0)
            {
                throw new DSubZeroException("Дискриминант меньше нуля");
            }
            string answer = $"Решение квадратного уравнения {input[0]}*x^2";
            if (input[1] < 0)
            {
                answer += $"{input[1]}*x";
            }
            else
            {
                answer += $"+{input[1]}*x";
            }
            if (input[2] < 0)
            {
                answer += $"{input[2]}=0:\n";
            }
            else
            {
                answer += $"+{input[2]}=0:\n";
            }
            Console.Write(answer);
            if (D == 0)
            {
                double x = (-input[1] + Math.Sqrt(D)) / (2 * input[0]);
                Console.WriteLine($"x={Math.Round(x,3)}");
            }
            else
            {
                double x1 = (-input[1] + Math.Sqrt(D)) / (2 * input[0]);
                double x2 = (-input[1] - Math.Sqrt(D)) / (2 * input[0]);
                Console.WriteLine($"x1={Math.Round(x1, 3)}   x2={Math.Round(x2, 3)}");

            }
        }

        public class WrongParamsException: Exception 
        {
            public WrongParamsException(string? message) : base(message) { }
        }
        public class AEqZeroException : Exception
        {
            public AEqZeroException(string message) : base(message) { }
        }
        public class DSubZeroException : Exception
        {
            public DSubZeroException(string message) : base(message) { }
        }
    }
}