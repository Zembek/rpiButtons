using RPiButtons.SSD1306.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RPiButtons.SSD1306
{
    public class SSD1306Manager
    {
        private readonly Display _display = new Display();

        public void Init()
        {
            _display.Init(new Common.DisplayConfiguration());
        }

        public void DeInit()
        {
            _display.TurnOffDisplay();
        }

        public void WriteMessage(uint line, uint column, string message)
        {
            _display.WriteLineDisplayBuf(message, column, line);
            _display.DisplayUpdate();
        }
    }
}
