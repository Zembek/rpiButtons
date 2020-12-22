using RPiButtons.MatrixButtons;
using RPiButtons.MatrixButtons.Common;
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
            manager.Clear();
            manager.WriteMessage(0, 20, "O kurla dziala");
            manager.DrawPikachu(1, 0);
            Thread.Sleep(1500);

            GpioController controller = new GpioController();
            Console.WriteLine("Initialize piouts");
            Dictionary<int, bool> enabledRelays = new Dictionary<int, bool>();
            foreach (var pinNo in _pinouts)
            {
                controller.OpenPin(pinNo, PinMode.Output);
                controller.Write(pinNo, PinValue.High);
                enabledRelays.Add(pinNo, false);
            }

            Console.WriteLine("END Initialize piouts");

            Console.WriteLine("Initialize Matrix Buttons");
            ButtonsManager buttonsManager = new ButtonsManager();
            MatrixButton buttonOne = new MatrixButton("One", 21, 16);
            MatrixButton buttonTwo = new MatrixButton("Two", 20, 16);
            MatrixButton buttonThree = new MatrixButton("Three", 21, 12);
            MatrixButton buttonFour = new MatrixButton("Four", 20, 12);
            List<MatrixButton> buttonList = new List<MatrixButton> { buttonOne, buttonTwo, buttonThree, buttonFour };
            buttonsManager.Init(buttonList, controller);

            while (true)
            {
                Console.WriteLine($"Checking pins: {DateTime.Now.ToUniversalTime()}");
                for (int pinIndex = 0; pinIndex < buttonList.Count; pinIndex++)
                {
                    MatrixButton buttonToCheck = buttonList[pinIndex];
                    bool isPressed = buttonsManager.IsPressed(buttonToCheck);
                    Console.WriteLine($"Input no {buttonList[pinIndex].Name} is {isPressed}");
                }
                Console.WriteLine("Sleep for 1.5s");
                Thread.Sleep(1500);
            }

            //while (true)
            //{
            //    for (int pinIndex = 0; pinIndex < _inputPins.Count; pinIndex++)
            //    {
            //        if (controller.Read(_inputPins[pinIndex]) == PinValue.High)
            //        {
            //            manager.Clear();
            //            manager.WriteMessage(0, 0, $"Pin no: {_inputPins[pinIndex]}");
            //            int relayToEnable = _pinouts[pinIndex];

            //            if (!enabledRelays[relayToEnable])
            //            {
            //                controller.Write(relayToEnable, PinValue.Low);
            //                enabledRelays[relayToEnable] = true;
            //            }
            //            else
            //            {
            //                controller.Write(relayToEnable, PinValue.High);
            //                enabledRelays[relayToEnable] = false;
            //            }
            //        }
            //    }
            //}

            //foreach (var pinNo in _pinouts)
            //{
            //    Console.WriteLine($"Set pin: {pinNo} value: Low");
            //    controller.Write(pinNo, PinValue.Low);
            //    Console.WriteLine($"Sleep 1500ms");
            //    Thread.Sleep(1500);
            //    Console.WriteLine($"Set pin: {pinNo} value: High");
            //    controller.Write(pinNo, PinValue.High);
            //    Console.WriteLine($"Sleep 1000ms");
            //    Thread.Sleep(1000);
            //}

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
