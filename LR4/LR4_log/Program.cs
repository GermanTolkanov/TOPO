using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace LR4_ExcLog
{
    internal class Program
    {
        public delegate void Serialize();
        static Registers registers = new Registers();
        static public event Serialize serEvent = null;
        static void Main(string[] args)
        {
            serEvent += SerializeXML;
            serEvent += SerializeJSON;
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
                registers.RegistersList.Add(new Register(ex));
                Console.WriteLine(ex.Message);
            }
            catch(DSubZeroException ex)
            {
                registers.RegistersList.Add(new Register(ex));
                Console.WriteLine(ex.Message);
            }
            serEvent.Invoke();
            goto Begining;
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
                    registers.RegistersList.Add(new Register(ex));
                    Console.WriteLine(ex.Message);
                    serEvent.Invoke();
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
                registers.RegistersList.Add(new Register(ex));
                Console.WriteLine($"{ex.Message}");
                serEvent.Invoke();
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
        static void SerializeXML()
        {
            XmlSerializer xml = new XmlSerializer(typeof(Registers));
            using FileStream filestream = new FileStream("saveFile.xml", FileMode.OpenOrCreate);
            xml.Serialize(filestream, registers);
        }
        static void SerializeJSON()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            using FileStream filestream = new FileStream("saveFile.json", FileMode.Create);
            JsonSerializer.Serialize(filestream, registers, options);
        }
    }
    public class WrongParamsException : Exception
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
    [Serializable]
    public class Register
    {
        [XmlElement("EXCEPTION")]
        public DateTime Time { get; set; }
        public string ExcName { get; set; }
        public string AppName { get; set; }
        public string StackTrace { get; set; }
        public Register(){}
        public Register(Exception ex)
        {
            Time = DateTime.Now;
            AppName = ex.Source;
            StackTrace = ex.StackTrace;
            ExcName = ex.ToString().Split(':')[0];
        }
    }
    [Serializable]
    public class Registers
    {
        public Registers() { }
        public List<Register> RegistersList { get; set; } = new List<Register>();
    }
}