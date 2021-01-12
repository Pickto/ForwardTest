using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;



namespace TestForward
{

    static class Program
    {
       
        static void Main()
        {
            string jsonString = File.ReadAllText("config.json");
            InternalCombustionEngineConfig config = JsonSerializer.Deserialize<InternalCombustionEngineConfig>(jsonString);
            InternalCombustionEngine engine = new InternalCombustionEngine(config);
            Console.WriteLine("Введите значение температуры окружающей среды:");
            double external_T;
            try
            {
                external_T = Convert.ToDouble(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine("Неверный формат температуры окружающей среды");
                return;
            }
            try
            {
                double time = engine.Start(external_T, 10000);
                Console.WriteLine($"Время перегрева - {time} секунд");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

        }
    }
}
