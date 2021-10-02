using BinaryRepresentor;
using Xunit;

namespace BinaryRepresentorTest.BitCalculatorTests
{
    public class GetNumOfBitsToHighlightBigEndianTest
    {
        [Fact]
        public void GetNumOfBitsToHighlight_WithMinIndexGreaterThanBitOffset_ShouldReturn0()
        {
            // Assert
            int minIndex = 16;
            int maxIndex = 23;
            ushort valueSize = 1;
            ushort bitOffset = 13;

            // Act
            var result = BitCalculator.GetNumOfBitsToHighlightBigEndian(minIndex, maxIndex, valueSize, bitOffset);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void GetNumOfBitsToHighlight_WithBitOffsetGreaterThanMinIndexLessThanOrEqualToMaxIndex_WithValueSizeLessThanOrEqualToMaxIndexMinusBitOffsetPlus1_ShouldReturnValueSize()
        {
            // Assert
            int minIndex = 8;
            int maxIndex = 15;
            ushort valueSize = 1;
            ushort bitOffset = 15;

            // Act
            var result = BitCalculator.GetNumOfBitsToHighlightBigEndian(minIndex, maxIndex, valueSize, bitOffset);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void GetNumOfBitsToHighlight_WithBitOffsetGreaterThanMinIndexLessThanOrEqualToMaxIndex_WithValueSizeGreaterThanMaxIndexMinusBitOffsetPlus1_ShouldReturnMaxIndexMinusBitOffsetPlus1()
        {
            // Assert
            int minIndex = 8;
            int maxIndex = 15;
            ushort valueSize = 8;
            ushort bitOffset = 15;

            // Act
            var result = BitCalculator.GetNumOfBitsToHighlightBigEndian(minIndex, maxIndex, valueSize, bitOffset);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void GetNumOfBitsToHighlight_WithValueSizeGreaterThanNumOfBitsHighlighted_ByMoreThan8Bits()
        {
            // Assert
            int minIndex = 8;
            int maxIndex = 15;
            ushort valueSize = 15;
            ushort bitOffset = 18;

            // Act
            var result = BitCalculator.GetNumOfBitsToHighlightBigEndian(minIndex, maxIndex, valueSize, bitOffset);

            // Assert
            Assert.Equal(8, result);
        }

        [Fact]
        public void GetNumOfBitsToHighlight_WithValueSizeGreaterThanNumOfBitsHighlighted_ByLessThan8Bits()
        {
            // Assert
            int minIndex = 0;
            int maxIndex = 7;
            ushort valueSize = 15;
            ushort bitOffset = 18;

            // Act
            var result = BitCalculator.GetNumOfBitsToHighlightBigEndian(minIndex, maxIndex, valueSize, bitOffset);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void GetNumOfBitsToHighlight_WithValueSizeLessThanNumOfBitsHighlighted_ShouldReturn0()
        {
            // Assert
            int minIndex = 0;
            int maxIndex = 7;
            ushort valueSize = 15;
            ushort bitOffset = 24;

            // Act
            var result = BitCalculator.GetNumOfBitsToHighlightBigEndian(minIndex, maxIndex, valueSize, bitOffset);

            // Assert
            Assert.Equal(0, result);
        }
    }
}
