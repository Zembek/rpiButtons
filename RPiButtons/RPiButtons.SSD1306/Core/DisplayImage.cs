﻿namespace RPiButtons.SSD1306.Core
{
    // column major, little endian.
    public class DisplayImage
    {
        public readonly uint ImageWidthPx;
        public readonly uint ImageHeightBytes;
        public readonly byte[] ImageData;

        public DisplayImage(uint imageHeightBytes, byte[] imageData)
        {
            ImageWidthPx = (uint)imageData.Length / imageHeightBytes;
            ImageHeightBytes = imageHeightBytes;
            ImageData = imageData;
        }
    }

    // Images were generated with http://dotmatrixtool.com
    // Find the source at https://github.com/stefangordon/dotmatrixtool
    // Column Major, Little Endian, for 2 byte tall images use "16px" height in dotmatrixtool.
    public static class DisplayImages
    {
        public static DisplayImage Connected = new DisplayImage(2, new byte[]
           { 0x00, 0x00, 0x30, 0x00, 0x30, 0x00, 0x18, 0x01, 0x98, 0x01, 0xcc, 0x08, 0xcc, 0x0c, 0xcc, 0x6c, 0xcc, 0x6c, 0xcc, 0x0c, 0xcc, 0x08, 0x98, 0x01, 0x18, 0x01, 0x30, 0x00, 0x30, 0x00, 0x00, 0x00 });

        public static DisplayImage ClockUp = new DisplayImage(2, new byte[]
          { 0x10, 0x0e, 0x98, 0x31, 0x9c, 0x20, 0x5e, 0x40, 0x5f, 0x47, 0x5e, 0x44, 0x9c, 0x24, 0x98, 0x31, 0x10, 0x0e, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });

        public static DisplayImage ClockDown = new DisplayImage(2, new byte[]
          { 0x01, 0x0e, 0x83, 0x31, 0x87, 0x20, 0x4f, 0x40, 0x5f, 0x47, 0x4f, 0x44, 0x87, 0x24, 0x83, 0x31, 0x01, 0x0e, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });

        public static DisplayImage Pikachu = new DisplayImage(4, new byte[]
            { 0x00, 0x00, 0x00, 0x00, 0xf0, 0x00, 0x00, 0x00, 0x3c, 0x07, 0x00, 0x00, 0x1c, 0x1c, 0x60, 0x00, 0x18, 0x30, 0xdf, 0x83, 0x30, 0xe0, 0x00, 0xc6, 0x60, 0x60, 0x00, 0x6c, 0x40, 0x00, 0x60, 0x38, 0x80, 0x01, 0x60, 0x00, 0x00, 0x87, 0x03, 0x00, 0x00, 0x84, 0x02, 0x00, 0x00, 0x86, 0x02, 0x00, 0x00, 0x82, 0x01, 0x00, 0x00, 0x02, 0x80, 0x03, 0x00, 0x02, 0x48, 0x02, 0x00, 0x02, 0x48, 0x02, 0x00, 0x02, 0xc0, 0x03, 0x00, 0x06, 0x00, 0x00, 0x00, 0x84, 0x01, 0x00, 0x00, 0x84, 0x02, 0x00, 0x00, 0x88, 0x02, 0x00, 0x00, 0x88, 0x03, 0x00, 0x00, 0x0c, 0x00, 0x00, 0x00, 0x06, 0x30, 0x00, 0x00, 0x03, 0x30, 0x00, 0x80, 0x01, 0x00, 0x00, 0x80, 0xf0, 0x01, 0x00, 0xc0, 0x18, 0xfe, 0x00, 0x40, 0x0c, 0x80, 0x03, 0x40, 0x06, 0x00, 0x0c, 0xe0, 0x03, 0x00, 0x70, 0xe0, 0x00, 0x00, 0x80 });
    }
}
