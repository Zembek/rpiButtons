using RPiButtons.SSD1306.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unosquare.RaspberryIO;

namespace RPiButtons.SSD1306
{
    public class SSD1306Manager
    {
        public async Task Test()
        {
            var display = new Display();
            display.Init(new Common.DisplayConfiguration());
        }
    }
}
