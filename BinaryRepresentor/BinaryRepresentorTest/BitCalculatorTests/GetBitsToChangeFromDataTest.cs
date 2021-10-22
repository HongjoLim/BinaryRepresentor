using BinaryRepresentor;
using Xunit;

namespace UniversalAutomotiveTester.Tests.OutputCalculatorTests
{
    public class GetBitsToChangeFromDataTest
    {
        [Fact]
        public void GetBitsToChangeFromData_WithLittleEndianTrue_WithBitOffset12_ValueSizeUpto8()
        {
            // Arrange
            byte[] data = new byte[8] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
            bool isLittleEndian = true;
            ushort valueSize = 8;
            ushort bitOffset = 12;

            // Act
            var result = BitCalculator.GetBitsToChangeFromData(data, isLittleEndian, bitOffset, valueSize);

            // Assert - 0x52 in bits
            Assert.Equal(0x52, BitCalculator.BitArrayToLong(result));
        }

        [Fact]
        public void GetBitsToChangeFromData_WithLittleEndianTrue_WithBitOffset12_ValueSizeGreaterThan8()
        {
            // Arrange
            byte[] data = new byte[8] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
            bool isLittleEndian = true;
            ushort valueSize = 16;
            ushort bitOffset = 12;

            // Act
            var result = BitCalculator.GetBitsToChangeFromData(data, isLittleEndian, bitOffset, valueSize);

            // Assert - 0x7452 in bits - 0111 0100 0101 0010
            Assert.Equal(0x7452, BitCalculator.BitArrayToLong(result));
        }

        [Fact]
        public void GetBitsToChangeFromData_WithLittleEndianFalse_WithBitOffset12_ValueSizeUpTo8()
        {
            // Arrange
            byte[] data = new byte[8] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
            bool isLittleEndian = false;
            ushort valueSize = 8;
            ushort bitOffset = 12;

            // Act
            var result = BitCalculator.GetBitsToChangeFromData(data, isLittleEndian, bitOffset, valueSize);

            // Assert - 0x12 in bits
            Assert.Equal(0x12, BitCalculator.BitArrayToLong(result));
        }

        [Fact]
        public void GetBitsToChangeFromData_WithLittleEndianFalse_WithBitOffset20_ValueSizeGreaterThan8()
        {
            // Arrange
            byte[] data = new byte[8] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
            bool isLittleEndian = false;
            ushort valueSize = 16;
            ushort bitOffset = 20;

            // Act
            var result = BitCalculator.GetBitsToChangeFromData(data, isLittleEndian, bitOffset, valueSize);

            // Assert - 0x1234 in bits
            Assert.Equal(0x1234, BitCalculator.BitArrayToLong(result));
        }
    }
}
