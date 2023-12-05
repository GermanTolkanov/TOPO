using Microsoft.Win32;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace LR4_ExcLog
{
    internal class Program
    {
        public delegate void Serialize();
        static Registers registers = new Registers();
        static public event Serialize serEvent;
        static void Main(string[] args)
        {
            serEvent += SerializeInvoker;
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
                registers.RegistersList.Add(new RegisterXML(ex));
                registers.RegistersList.Add(new RegisterJSON(ex));
                registers.RegistersList.Add(new RegisterTXT(ex));
                Console.WriteLine(ex.Message);
            }
            catch(DSubZeroException ex)
            {
                registers.RegistersList.Add(new RegisterXML(ex));
                registers.RegistersList.Add(new RegisterJSON(ex));
                registers.RegistersList.Add(new RegisterTXT(ex));
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
                    registers.RegistersList.Add(new RegisterXML(ex));
                    registers.RegistersList.Add(new RegisterJSON(ex));
                    registers.RegistersList.Add(new RegisterTXT(ex));
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
                registers.RegistersList.Add(new RegisterXML(ex));
                registers.RegistersList.Add(new RegisterJSON(ex));
                registers.RegistersList.Add(new RegisterTXT(ex));
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
        static public void SerializeInvoker()
        {
            foreach(ISerialize serial in registers.RegistersList)
            {
                serial.Serialize();
            }
            registers.RegistersList.Clear();
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
    public class Registers
    {
        private List<ISerialize> registersList = new List<ISerialize>();

        public Registers() { }
        public List<ISerialize> RegistersList { get => registersList; set => registersList = value; }
    }
    public class RegisterTXT : ISerialize
    {
        public DateTime Time { get; set; }
        public string ExcName { get; set; }
        public string AppName { get; set; }
        public string StackTrace { get; set; }
        public RegisterTXT(){}
        public RegisterTXT(Exception ex)
        {
            Time = DateTime.Now;
            AppName = ex.Source;
            StackTrace = ex.StackTrace;
            ExcName = ex.ToString().Split(':')[0];
        }
        public void Serialize()
        {
            string info = $"Time - {Time}\nException name - {ExcName}\nApplication name - {AppName}\nStack Trace - {StackTrace}\n";
            byte[] buffer = Encoding.Default.GetBytes(info);
            using (FileStream filestream = new FileStream("saveFile.txt", FileMode.Append, FileAccess.Write))
            {
                filestream.Write(buffer, 0, buffer.Length);
                filestream.Flush();
                filestream.Close();
            }
        }
    }
    [Serializable]
    public class RegisterXML : ISerialize
    {
        [XmlElement("EXCEPTION")]
        public DateTime Time { get; set; }
        public string ExcName { get; set; }
        public string AppName { get; set; }
        public string StackTrace { get; set; }
        public RegisterXML() { }
        public RegisterXML(Exception ex)
        {
            Time = DateTime.Now;
            AppName = ex.Source;
            StackTrace = ex.StackTrace;
            ExcName = ex.ToString().Split(':')[0];
        }
        public void Serialize()
        {
            XmlSerializer xml = new XmlSerializer(typeof(RegisterXML));
            using FileStream filestream = new FileStream("saveFile.xml", FileMode.Append);
            xml.Serialize(filestream, this);
        }
    }
    [Serializable]
    public class RegisterJSON: ISerialize
    {
        [XmlElement("EXCEPTION")]
        public DateTime Time { get; set; }
        public string ExcName { get; set; }
        public string AppName { get; set; }
        public string StackTrace { get; set; }
        public RegisterJSON() { }
        public RegisterJSON(Exception ex)
        {
            Time = DateTime.Now;
            AppName = ex.Source;
            StackTrace = ex.StackTrace;
            ExcName = ex.ToString().Split(':')[0];
        }
        public void Serialize()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            using FileStream filestream = new FileStream("saveFile.json", FileMode.Append);
            JsonSerializer.Serialize(filestream, this, options);
        }
    }
    public interface ISerialize
    {
        public void Serialize();
    }
}