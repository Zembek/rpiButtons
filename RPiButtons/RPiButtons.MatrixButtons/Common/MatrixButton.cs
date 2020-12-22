using System;
using System.Collections.Generic;
using System.Text;

namespace RPiButtons.MatrixButtons.Common
{
    public class MatrixButton
    {
        public int ColumnPin { get; private set; }
        public int RowPin { get; private set; }
        public string Name { get; private set; }

        public MatrixButton(string name, int columnPin, int rowPin)
        {
            Name = name;
            ColumnPin = columnPin;
            RowPin = rowPin;
        }

    }
}
