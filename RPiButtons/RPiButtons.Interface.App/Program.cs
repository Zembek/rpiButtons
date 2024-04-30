using RPiButtons.MatrixButtons;
using RPiButtons.MatrixButtons.Common;
using RPiButtons.SSD1306;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace RPiButtons.Interface.App
{
    class Program
    {
        private static List<int> _pinouts = new List<int> { 14, 15, 18, 23/*, 24, 25, 8, 7 */};
        private static List<MatrixButton> _matrixButtons = new List<MatrixButton>
        {
            new MatrixButton("One", 21, 16),
            new MatrixButton("Two", 20, 16),
            new MatrixButton("Three", 21, 12),
            new MatrixButton("Four", 20, 12)
        };

        static void Main(string[] args)
        {
            string apiHost = Environment.GetEnvironmentVariable("API_HOST");
            string apiProtocol = Environment.GetEnvironmentVariable("API_PROTOCOL");
            string apiPort = Environment.GetEnvironmentVariable("API_PORT");

            string apiUrl = $"{apiProtocol}://{apiHost}:{apiPort}/MarcoPolo";

            Console.WriteLine("App is up");
#if DEBUG
            Console.WriteLine("Waiting for debugger. Press any key to continue");
            Console.ReadKey();
#endif

            SSD1306Manager screenManager = new SSD1306Manager();
            GpioController gpioController = new GpioController();
            ButtonsManager buttonsManager = new ButtonsManager();

            InitializeScreen(screenManager);
            Dictionary<int, bool> enabledRelays = InitializeRelays(gpioController);

            screenManager.DrawPikachu(0, 0);
            Thread.Sleep(2000);


            //foreach (int pin in _pinouts)
            //{
            //    gpioController.Write(pin, PinValue.Low);
            //    Thread.Sleep(500);
            //    gpioController.Write(pin, PinValue.High);
            //    Thread.Sleep(500);
            //}

            InitializeMatrixButtons(gpioController, buttonsManager);
            ApplicationLoop(screenManager, gpioController, buttonsManager, enabledRelays);
            DeinitializeRelays(gpioController);
            DeinitializeMatrixButtons(buttonsManager);
            TurnOffScreen(screenManager);

            Console.WriteLine("App is down");
        }

        private static void TurnOffScreen(SSD1306Manager manager)
        {
            Console.WriteLine("Turn off screen");
            manager.TurnOff();
            Console.WriteLine("END Turn off screen");
        }

        private static void DeinitializeMatrixButtons(ButtonsManager buttonsManager)
        {
            Console.WriteLine("DeInitialize matrix buttons");
            buttonsManager.Cleanup();
            Console.WriteLine("END DeInitialize matrix buttons");
        }

        private static void DeinitializeRelays(GpioController controller)
        {
            Console.WriteLine("DeInitialize piouts");
            foreach (var pinNo in _pinouts)
            {
                controller.Write(pinNo, PinValue.High);
                controller.ClosePin(pinNo);
            }
            Console.WriteLine("END DeInitialize piouts");
        }

        private static async Task ApplicationLoop(SSD1306Manager manager, GpioController controller, ButtonsManager buttonsManager, Dictionary<int, bool> enabledRelays, string marcoPoloAPI)
        {
            HttpClient client = new HttpClient();
            Stopwatch watch = new Stopwatch();

            Console.WriteLine("Start application loop");
            SetRelayStatus(manager, enabledRelays, string.Empty, "0");
            while (true)
            {
                watch.Restart();
                var response = await client.GetStringAsync(marcoPoloAPI);
                watch.Stop();
                string elapsedMS = watch.ElapsedMilliseconds.ToString();
                SetRelayStatus(manager, enabledRelays, response, elapsedMS);

                List<MatrixButton> pressedButtons = buttonsManager.ArePressed();
                foreach (var button in pressedButtons)
                {
                    int buttonIndex = _matrixButtons.IndexOf(button);
                    if (buttonIndex < 0)
                        continue;

                    int relayToEnable = _pinouts[buttonIndex];
                    if (!enabledRelays[relayToEnable])
                    {
                        controller.Write(relayToEnable, PinValue.Low);
                        enabledRelays[relayToEnable] = true;
                    }
                    else
                    {
                        controller.Write(relayToEnable, PinValue.High);
                        enabledRelays[relayToEnable] = false;
                    }
                }

                if (pressedButtons.Count > 0)
                {
                    SetRelayStatus(manager, enabledRelays, response, elapsedMS);
                }

                Thread.Sleep(500);

                if (!enabledRelays.ContainsValue(false))
                    break;
            }
            Console.WriteLine("End application loop");
        }

        private static void InitializeMatrixButtons(GpioController controller, ButtonsManager buttonsManager)
        {
            Console.WriteLine("Initialize Matrix Buttons");
            buttonsManager.Init(_matrixButtons, controller);
            Console.WriteLine("END Initialize Matrix Buttons");
        }

        private static Dictionary<int, bool> InitializeRelays(GpioController controller)
        {
            Console.WriteLine("Initialize piouts");
            Dictionary<int, bool> enabledRelays = new Dictionary<int, bool>();
            foreach (var pinNo in _pinouts)
            {
                controller.OpenPin(pinNo, PinMode.Output);
                controller.Write(pinNo, PinValue.High);
                enabledRelays.Add(pinNo, false);
            }
            Console.WriteLine("END Initialize piouts");
            return enabledRelays;
        }

        private static void InitializeScreen(SSD1306Manager manager)
        {
            Console.WriteLine("Initialize Screen");
            manager.TurnOn();
            Console.WriteLine("END Initialize Screen");
        }

        private static void SetRelayStatus(SSD1306Manager manager, Dictionary<int, bool> enabledRelays, string apiResponse, string responseTime)
        {
            manager.Clear();
            manager.WriteMessage(0, 0, $"R1: {(enabledRelays[_pinouts[0]] ? "ON" : "OFF")}");
            manager.WriteMessage(0, 80, $"R2: {(enabledRelays[_pinouts[1]] ? "ON" : "OFF")}");
            manager.WriteMessage(1, 0, $"Message: Marco");
            manager.WriteMessage(2, 0, $"[{responseTime}ms] Respons: {apiResponse}");
            manager.WriteMessage(3, 0, $"R3: {(enabledRelays[_pinouts[2]] ? "ON" : "OFF")}");
            manager.WriteMessage(3, 80, $"R4: {(enabledRelays[_pinouts[3]] ? "ON" : "OFF")}");
            manager.Update();
        }
    }
}
