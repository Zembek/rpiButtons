using RPiButtons.SSD1306;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Threading;
using System.Threading.Tasks;

namespace RPiButtons.Interface.App
{
    class Program
    {
        private static List<int> _pinouts = new List<int> { 14, 15, 18, 23 };
        static async Task Main(string[] args)
        {
            Console.WriteLine("App is up");
#if DEBUG
            Console.WriteLine("Waiting for debugger. Press any key to continue");
            Console.ReadKey();
#endif
            SSD1306Manager manager = new SSD1306Manager();
            manager.TurnOn();
            manager.WriteMessage(0, 0, "Jebac pis");
            Thread.Sleep(1500);
            manager.DrawPikachu();
            Thread.Sleep(1500);

            GpioController controller = new GpioController();
            Console.WriteLine("Initialize piouts");
            foreach (var pinNo in _pinouts)
            {
                controller.OpenPin(pinNo, PinMode.Output);
                controller.Write(pinNo, PinValue.High);
            }
            Console.WriteLine("END Initialize piouts");

            foreach (var pinNo in _pinouts)
            {
                Console.WriteLine($"Set pin: {pinNo} value: Low");
                controller.Write(pinNo, PinValue.Low);
                Console.WriteLine($"Sleep 1500ms");
                Thread.Sleep(1500);
                Console.WriteLine($"Set pin: {pinNo} value: High");
                controller.Write(pinNo, PinValue.High);
                Console.WriteLine($"Sleep 1000ms");
                Thread.Sleep(1000);
            }

            Console.WriteLine("DeInitialize piouts");
            foreach (var pinNo in _pinouts)
            {
                controller.ClosePin(pinNo);
            }
            Console.WriteLine("END DeInitialize piouts");

            manager.TurnOff();
        }
    }
}
