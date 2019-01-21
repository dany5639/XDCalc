using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDCalc
{
    class ValidCharacters
    {
        public static List<byte> ValidChars_all = new List<byte>
        {
            0x41, // A
            0x42, // B
            0x43, // C
            0x44, // D
            0x45, // E
            0x46, // F
            0x61, // 0
            0x62, // 1
            0x63, // 2
            0x64, // 3
            0x65, // 4
            0x66, // 5
            0x30, // 6
            0x31, // 7
            0x32, // 8
            0x33, // 9
            0x34, // a
            0x35, // b
            0x36, // c
            0x37, // d
            0x38, // e
            0x39, // f
            0x2A, // *
            0x2B, // +
            0x2F, // /
            0x2D, // -
            0x20, // space
        };

        public static List<byte> ValidChars_hex = new List<byte>
        {
            0x41, // A
            0x42, // B
            0x43, // C
            0x44, // D
            0x45, // E
            0x46, // F
            0x34, // a
            0x35, // b
            0x36, // c
            0x37, // d
            0x38, // e
            0x39, // f
        };

        public static List<byte> ValidChars_ops = new List<byte>
        {
            0x2A, // *
            0x2B, // +
            0x2F, // /
            0x2D, // -
        };

        public static List<byte> ValidChars_numbers = new List<byte>
        {
            0x41, // A
            0x42, // B
            0x43, // C
            0x44, // D
            0x45, // E
            0x46, // F
            0x61, // 0
            0x62, // 1
            0x63, // 2
            0x64, // 3
            0x65, // 4
            0x66, // 5
            0x30, // 6
            0x31, // 7
            0x32, // 8
            0x33, // 9
            0x34, // a
            0x35, // b
            0x36, // c
            0x37, // d
            0x38, // e
            0x39, // f
        };
    }
}
