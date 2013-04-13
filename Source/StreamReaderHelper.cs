using System;
using System.IO;

namespace woanware
{
    /// <summary>
    /// 
    /// </summary>
    internal class StreamReaderHelper
    {
        #region Memory Stream
        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        public static byte ReadByte(MemoryStream memoryStream)
        {
            byte[] data = new byte[1];
            memoryStream.Read(data, 0, 1);

            Console.WriteLine(data[0]);
            return data[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        public static Int16 ReadInt16(MemoryStream memoryStream)
        {
            byte[] data = new byte[2];
            memoryStream.Read(data, 0, 2);
            return BitConverter.ToInt16(data, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        public static Int32 ReadInt32(MemoryStream memoryStream)
        {
            byte[] data = new byte[4];
            memoryStream.Read(data, 0, 4);
            return BitConverter.ToInt32(data, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        public static Int64 ReadInt64(MemoryStream memoryStream)
        {
            byte[] data = new byte[8];
            memoryStream.Read(data, 0, 8);
            return BitConverter.ToInt64(data, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        public static sbyte ReadSByte(MemoryStream memoryStream)
        {
            byte[] data = new byte[1];
            memoryStream.Read(data, 0, 1);
            return (sbyte)data[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        public static UInt16 ReadUInt16(MemoryStream memoryStream)
        {
            byte[] data = new byte[2];
            memoryStream.Read(data, 0, 2);
            return BitConverter.ToUInt16(data, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        public static UInt16 ReadUInt16Reversed(MemoryStream memoryStream)
        {
            byte[] data = new byte[2];
            memoryStream.Read(data, 0, 2);
            Array.Reverse(data);
            return BitConverter.ToUInt16(data, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        public static UInt32 ReadUInt32(MemoryStream memoryStream)
        {
            byte[] data = new byte[4];
            memoryStream.Read(data, 0, 4);
            return BitConverter.ToUInt32(data, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        public static UInt64 ReadUInt64(MemoryStream memoryStream)
        {
            byte[] data = new byte[8];
            memoryStream.Read(data, 0, 8);
            return BitConverter.ToUInt64(data, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        public static float ReadFloat(MemoryStream memoryStream)
        {
            byte[] data = new byte[4];
            memoryStream.Read(data, 0, 4);
            return BitConverter.ToSingle(data, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        public static double ReadDouble(MemoryStream memoryStream)
        {
            byte[] data = new byte[8];
            memoryStream.Read(data, 0, 8);
            return BitConverter.ToDouble(data, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        public static DateTime ReadDateTime(MemoryStream memoryStream)
        {
            byte[] data = new byte[8];
            memoryStream.Read(data, 0, 8);
            long temp = BitConverter.ToInt64(data, 0);
            return DateTime.FromFileTimeUtc(temp);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string ReadString(MemoryStream memoryStream, int length)
        {
            byte[] data = new byte[length];
            memoryStream.Read(data, 0, length);
            return Text.ByteArray2String(data, false, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] ReadByteArray(MemoryStream memoryStream, int length)
        {
            byte[] data = new byte[length];
            memoryStream.Read(data, 0, length);
            return data;
        }
        #endregion

        #region File Stream
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public static byte ReadByte(FileStream fileStream)
        {
            byte[] data = new byte[1];
            fileStream.Read(data, 0, 1);
            return data[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public static Int16 ReadInt16(FileStream fileStream)
        {
            byte[] data = new byte[2];
            fileStream.Read(data, 0, 2);
            return BitConverter.ToInt16(data, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public static Int32 ReadInt32(FileStream fileStream)
        {
            byte[] data = new byte[4];
            fileStream.Read(data, 0, 4);
            return BitConverter.ToInt32(data, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public static Int64 ReadInt64(FileStream fileStream)
        {
            byte[] data = new byte[8];
            fileStream.Read(data, 0, 8);
            return BitConverter.ToInt64(data, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public static sbyte ReadSByte(FileStream fileStream)
        {
            byte[] data = new byte[1];
            fileStream.Read(data, 0, 1);
            return (sbyte)data[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public static UInt16 ReadUInt16(FileStream fileStream)
        {
            byte[] data = new byte[2];
            fileStream.Read(data, 0, 2);
            return BitConverter.ToUInt16(data, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public static UInt32 ReadUInt32(FileStream fileStream)
        {
            byte[] data = new byte[4];
            fileStream.Read(data, 0, 4);
            return BitConverter.ToUInt32(data, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public static UInt64 ReadUInt64(FileStream fileStream)
        {
            byte[] data = new byte[8];
            fileStream.Read(data, 0, 8);
            return BitConverter.ToUInt64(data, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public static DateTime ReadDateTime(FileStream fileStream)
        {
            byte[] data = new byte[8];
            fileStream.Read(data, 0, 8);
            long temp = BitConverter.ToInt64(data, 0);
            return DateTime.FromFileTimeUtc(temp);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string ReadString(FileStream fileStream, int length)
        {
            byte[] data = new byte[length];
            fileStream.Read(data, 0, length);
            return Text.ByteArray2String(data, false, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] ReadByteArray(FileStream fileStream, int length)
        {
            byte[] data = new byte[length];
            fileStream.Read(data, 0, length);
            return data;
        }
        #endregion
    }
}
