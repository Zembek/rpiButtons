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
        private static List<int> _pinouts = new List<int> { 14, 15, 18, 23/*, 24, 25, 8, 7 */};
        private static List<MatrixButton> _matrixButtons = new List<MatrixButton>
        {
            new MatrixButton("One", 26, 16),
            new MatrixButton("Two", 20, 16),
            new MatrixButton("Three", 26, 12),
            new MatrixButton("Four", 20, 12)
        };

        static void Main(string[] args)
        {
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
                controller.ClosePin(pinNo);
            }
            Console.WriteLine("END DeInitialize piouts");
        }

        private static void ApplicationLoop(SSD1306Manager manager, GpioController controller, ButtonsManager buttonsManager, Dictionary<int, bool> enabledRelays)
        {
            Console.WriteLine("Start application loop");
            SetRelayStatus(manager, enabledRelays);
            while (true)
            {
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
                    SetRelayStatus(manager, enabledRelays);
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

        private static void SetRelayStatus(SSD1306Manager manager, Dictionary<int, bool> enabledRelays)
        {
            manager.Clear();
            manager.WriteMessage(0, 0, $"R1: {(enabledRelays[_pinouts[0]] ? "ON" : "OFF")}");
            manager.WriteMessage(0, 80, $"R2: {(enabledRelays[_pinouts[1]] ? "ON" : "OFF")}");
            manager.WriteMessage(2, 0, $"R3: {(enabledRelays[_pinouts[2]] ? "ON" : "OFF")}");
            manager.WriteMessage(2, 80, $"R4: {(enabledRelays[_pinouts[3]] ? "ON" : "OFF")}");
            manager.Update();
        }
    }
}
