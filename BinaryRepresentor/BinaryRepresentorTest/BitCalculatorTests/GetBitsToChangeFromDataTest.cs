using BinaryRepresentor;
using Xunit;

namespace UniversalAutomotiveTester.Tests.OutputCalculatorTests
{
    public class GetBitsToChangeFromDataTest
    {
        [Fact]
        public void GetBitsToChangeFromData_WithLittleEndianTrue_WithBitOffset12_ValueSizeUpto8()
        {
            // Assert
            byte[] data = new byte[8] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
            bool isLittleEndian = true;
            ushort valueSize = 8;
            ushort bitOffset = 12;

            // Act
            var result = BitCalculator.GetBitsToChangeFromData(data, isLittleEndian, bitOffset, valueSize);

            // Assert - 0x52 in bits
            Assert.False(result[0]);
            Assert.True(result[1]);
            Assert.False(result[2]);
            Assert.True(result[3]);

            Assert.False(result[4]);
            Assert.False(result[5]);
            Assert.True(result[6]);
            Assert.False(result[7]);
        }

        [Fact]
        public void GetBitsToChangeFromData_WithLittleEndianTrue_WithBitOffset12_ValueSizeGreaterThan8()
        {
            // Assert
            byte[] data = new byte[8] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
            bool isLittleEndian = true;
            ushort valueSize = 16;
            ushort bitOffset = 12;

            // Act
            var result = BitCalculator.GetBitsToChangeFromData(data, isLittleEndian, bitOffset, valueSize);

            // Assert - 0x7452 in bits - 0111 0100 0101 0010
            Assert.Equal(16, result.Length);
            Assert.False(result[0]);
            Assert.True(result[1]);
            Assert.True(result[2]);
            Assert.True(result[3]);

            Assert.False(result[4]);
            Assert.True(result[5]);
            Assert.False(result[6]);
            Assert.False(result[7]);

            Assert.False(result[8]);
            Assert.True(result[9]);
            Assert.False(result[10]);
            Assert.True(result[11]);

            Assert.False(result[12]);
            Assert.False(result[13]);
            Assert.True(result[14]);
            Assert.False(result[15]);
        }

        [Fact]
        public void GetBitsToChangeFromData_WithLittleEndianFalse_WithBitOffset12_ValueSizeUpTo8()
        {
            // Assert
            byte[] data = new byte[8] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
            bool isLittleEndian = false;
            ushort valueSize = 8;
            ushort bitOffset = 12;

            // Act
            var result = BitCalculator.GetBitsToChangeFromData(data, isLittleEndian, bitOffset, valueSize);

            // Assert - 0x12 in bits
            Assert.False(result[0]);
            Assert.False(result[1]);
            Assert.False(result[2]);
            Assert.True(result[3]);

            Assert.False(result[4]);
            Assert.False(result[5]);
            Assert.True(result[6]);
            Assert.False(result[7]);
        }

        [Fact]
        public void GetBitsToChangeFromData_WithLittleEndianFalse_WithBitOffset20_ValueSizeGreaterThan8()
        {
            // Assert
            byte[] data = new byte[8] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
            bool isLittleEndian = false;
            ushort valueSize = 16;
            ushort bitOffset = 20;

            // Act
            var result = BitCalculator.GetBitsToChangeFromData(data, isLittleEndian, bitOffset, valueSize);

            // Assert - 0x1234 in bits
            Assert.False(result[0]);
            Assert.False(result[1]);
            Assert.False(result[2]);
            Assert.True(result[3]);

            Assert.False(result[4]);
            Assert.False(result[5]);
            Assert.True(result[6]);
            Assert.False(result[7]);

            Assert.False(result[8]);
            Assert.False(result[9]);
            Assert.True(result[10]);
            Assert.True(result[11]);

            Assert.False(result[12]);
            Assert.True(result[13]);
            Assert.False(result[14]);
            Assert.False(result[15]);
        }
    }
}
