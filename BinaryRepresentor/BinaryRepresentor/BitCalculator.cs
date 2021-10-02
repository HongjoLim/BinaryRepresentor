using System;
using System.Collections;

namespace BinaryRepresentor
{
    public class BitCalculator
    {
        public const string IllegalOperation = "Illegal Operation";

        public static string Calculate(byte[] data, bool isLittleEndiann, ushort bitOffset, ushort valueSize, int preScalingOffset, int postScalingOffset, uint multiplier, uint divider)
        {
            if (divider == 0)
            {
                return IllegalOperation;
            }

            int bitArrLength = data.Length * 8;

            if (bitArrLength < bitOffset + valueSize)
            {
                return IllegalOperation;
            }

            bool[] bits = GetBitsFromData(data, isLittleEndiann);
            bool[] bitsToChange = GetBitsToChangeFromSourceBits(bits, isLittleEndiann, bitOffset, valueSize);

            long result = 0;

            result = ((result + preScalingOffset) * multiplier / divider) + postScalingOffset;

            return result.ToString();
        }

        public static bool[] GetBitsFromData(byte[] data, bool isLittleEndian)
        {
            bool[] bits = new bool[data.Length * 8];

            if (isLittleEndian == false)
            {
                Array.Reverse(data);
            }

            new BitArray(data).CopyTo(bits, 0);

            return bits;
        }

        public static bool[] GetBitsToChangeFromSourceBits(bool[] bits, bool isLittleEndian, ushort bitOffset, ushort valueSize)
        {
            bool[] bitsToChange = new bool[valueSize];

            if (isLittleEndian == false)
            {
                for (int i = 0; i < bits.Length / 8; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        bool temp;
                        temp = bits[i * 8 + j];
                        bits[i * 8 + j] = bits[(i + 1) * 8 - j - 1];
                        bits[(i + 1) * 8 - j - 1] = temp;
                    }
                }
            }

            Array.Copy(bits, bitOffset, bitsToChange, 0, valueSize);
            return bitsToChange;
        }

        public static int GetNumOfBitsToHighlightBigEndian(int minIndex, int maxIndex, ushort valueSize, ushort bitOffset)
        {
            int numOfBitsToHighlight = 0;

            if (minIndex > bitOffset)
            {
                numOfBitsToHighlight = 0;
            }
            else if (bitOffset <= maxIndex)
            {
                numOfBitsToHighlight = valueSize <= maxIndex - bitOffset + 1 ? valueSize : maxIndex - bitOffset + 1;
            }
            else
            {
                int numOfBytesHighlighted = (int)Math.Ceiling((bitOffset - maxIndex) / 8m) + 1;
                int numOfBitsHighlighted = numOfBytesHighlighted * 8 - (bitOffset - maxIndex) - 7 + (numOfBytesHighlighted - 2) * 8;

                numOfBitsToHighlight = valueSize >= numOfBitsHighlighted + 8 ? 8 : valueSize - numOfBitsHighlighted >= 0 ? valueSize - numOfBitsHighlighted : 0;
            }

            return numOfBitsToHighlight;
        }

        public static int GetNumOfBitsToHighlightLittleEndian(int minIndex, int maxIndex, ushort valueSize, ushort bitOffset)
        {
            int numOfBitsToHighlight = 0;

            if (maxIndex < bitOffset || minIndex > bitOffset + valueSize)
            {
                numOfBitsToHighlight = 0;
            }
            else if (minIndex <= bitOffset && maxIndex >= bitOffset)
            {
                numOfBitsToHighlight = valueSize <= maxIndex + 1 - bitOffset ? valueSize : maxIndex + 1 - bitOffset;
            }
            else if (maxIndex < bitOffset + valueSize)
            {
                numOfBitsToHighlight = 8;
            }
            // The last bitset to highlight
            else
            {
                numOfBitsToHighlight = (bitOffset + valueSize) % 8;
            }

            return numOfBitsToHighlight;
        }
    }
}