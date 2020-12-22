using RPiButtons.MatrixButtons.Common;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Text;

namespace RPiButtons.MatrixButtons
{
    public class ButtonsManager
    {
        private GpioController _gpioController = null;
        private List<MatrixButton> _buttons = null;

        private List<int> OutputPins => _buttons.Select(b => b.RowPin).Distinct().ToList();
        private List<int> InputPins => _buttons.Select(b => b.ColumnPin).Distinct().ToList();

        public void Init(List<MatrixButton> buttons, GpioController controller)
        {
            _gpioController = controller;
            _buttons = buttons;

            foreach (int pinNo in OutputPins)
            {
                _gpioController.OpenPin(pinNo, PinMode.Output);
                _gpioController.Write(pinNo, PinValue.Low);
            }

            foreach (int pinNo in InputPins)
            {
                _gpioController.OpenPin(pinNo, PinMode.InputPullDown);
            }
        }

        public bool IsPressed(MatrixButton buttonToCheck)
        {
            if (!_buttons.Contains(buttonToCheck))
                return false;

            foreach (int pinNo in OutputPins)
                _gpioController.Write(pinNo, PinValue.Low);

            _gpioController.Write(buttonToCheck.RowPin, PinValue.High);
            var state = _gpioController.Read(buttonToCheck.ColumnPin);
            _gpioController.Write(buttonToCheck.RowPin, PinValue.Low);

            return state == PinValue.High;
        }

        public void Cleanup()
        {
            foreach (int pinNo in OutputPins)
            {
                _gpioController.ClosePin(pinNo);
            }

            foreach (int pinNo in InputPins)
            {
                _gpioController.ClosePin(pinNo);
            }
            _gpioController = null;
        }
    }
}
