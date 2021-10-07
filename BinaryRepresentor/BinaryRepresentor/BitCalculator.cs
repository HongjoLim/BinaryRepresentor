using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BinaryRepresentor
{
    public class BitCalculator
    {
        public const string IllegalOperation = "Illegal Operation";
        public const string InvalidInputs = "Cannot compute the result. \nPlease check your bit offset and value size inputs.";

        public static string Calculate(byte[] data, bool isLittleEndian, ushort bitOffset, ushort valueSize, int preScalingOffset, int postScalingOffset, uint multiplier, uint divider)
        {
            if (divider == 0)
            {
                return IllegalOperation;
            }

            int bitArrLength = data.Length * 8;

            if (isLittleEndian && bitArrLength < bitOffset + valueSize)
            {
                return InvalidInputs;
            }

            if (isLittleEndian == false && bitOffset + 8 < valueSize)
            {
                return InvalidInputs;
            }

            try
            {
                bool[] bitsToChange = GetBitsToChangeFromData(data, isLittleEndian, bitOffset, valueSize);
                long value = BitArrayToLong(bitsToChange);
                double result = ((value + preScalingOffset) * multiplier / divider) + postScalingOffset;
                return result.ToString();
            }
            catch
            {
                return InvalidInputs;
            }
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

        public static bool[] GetBitsToChangeFromData(byte[] data, bool isLittleEndian, ushort bitOffset, ushort valueSize)
        {
            List<bool> bitsToChange = new List<bool>();

            for (int i = 0; i < data.Length; i++)
            {
                bool[] bits = ByteToBoolArray(data[i]);
                Array.Reverse(bits);

                int minIndex = i * 8; // 0, 8, 16, 24 ...
                int maxIndex = (i + 1) * 8 - 1; // 7, 15, 23, 31 ...
                int numOfBitsToHighlight = 0;
                int numOfBitsToSkip = 0;

                if (minIndex <= bitOffset && maxIndex >= bitOffset)
                {
                    numOfBitsToSkip = bitOffset % 8;
                }

                if (isLittleEndian)
                {
                    numOfBitsToHighlight = GetNumOfBitsToHighlightLittleEndian(minIndex: minIndex, maxIndex: maxIndex, valueSize: valueSize, bitOffset: bitOffset);
                    bool[] bits2 = bits.Skip(numOfBitsToSkip).Take(numOfBitsToHighlight).ToArray();
                    bitsToChange.AddRange(bits2);
                }
                else
                {
                    numOfBitsToHighlight = GetNumOfBitsToHighlightBigEndian(minIndex: minIndex, maxIndex: maxIndex, valueSize: valueSize, bitOffset: bitOffset);
                    bool[] bits2 = bits.Skip(numOfBitsToSkip).Take(numOfBitsToHighlight).ToArray();
                    bitsToChange.InsertRange(0, bits2);
                }
            }

            bitsToChange.Reverse();

            return bitsToChange.ToArray();
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

        public static long BitArrayToLong(bool[] boolArray)
        {
            long result = 0;

            for (int i = 0; i < boolArray.Length; i++)
            {
                if (boolArray[i])
                {
                    result |= (long)(1 << (boolArray.Length - 1 - i));
                }
            }
            return result;
        }

        public static bool[] ByteToBoolArray(byte b)
        {
            // prepare the return result
            bool[] result = new bool[8];

            // check each bit in the byte. if 1 set to true, if 0 set to false
            for (int i = 0; i < 8; i++)
                result[i] = (b & (1 << i)) == 0 ? false : true;

            // reverse the array
            Array.Reverse(result);

            return result;
        }
    }
}