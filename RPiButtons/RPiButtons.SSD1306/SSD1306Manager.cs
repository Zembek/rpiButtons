using RPiButtons.SSD1306.Core;

namespace RPiButtons.SSD1306
{
    public class SSD1306Manager
    {
        private readonly Display _display = new Display();

        public void TurnOn()
        {
            _display.Init(new Common.DisplayConfiguration());
        }

        public void TurnOff()
        {
            _display.TurnOffDisplay();
        }

        public void WriteMessage(uint line, uint column, string message)
        {
            _display.WriteLineDisplayBuf(message, column, line);
            _display.DisplayUpdate();
        }

        public void DrawPikachu(uint line, uint column)
        {
            _display.WriteImageDisplayBuf(DisplayImages.Pikachu, 0, 0);
            _display.DisplayUpdate();
        }

        public void Clear()
        {
            _display.ClearDisplayBuf();
            _display.DisplayUpdate();
        }
    }
}
