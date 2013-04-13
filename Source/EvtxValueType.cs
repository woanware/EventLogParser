using System;
using System.Collections.Generic;
using System.IO;

namespace woanware
{
    /// <summary>
    /// 
    /// </summary>
    internal enum ValueTypes
    {
        NullType = 0x00, 
        StringType = 0x01,
        AnsiStringType = 0x02, 
        Int8Type = 0x03, 
        UInt8Type = 0x04, 
        Int16Type = 0x05, 
        UInt16Type = 0x06, 
        Int32Type = 0x07, 
        UInt32Type = 0x08, 
        Int64Type = 0x09, 
        UInt64Type = 0x0A, 
        Real32Type = 0x0B, 
        Real64Type = 0x0C, 
        BoolType = 0x0D, 
        BinaryType = 0x0E, 
        GuidType = 0x0F, 
        SizeTType = 0x10, 
        FileTimeType = 0x11, 
        SysTimeType = 0x12, 
        SidType = 0x13, 
        HexInt32Type = 0x14, 
        HexInt64Type = 0x15, 
        BinXmlType = 0x21, 
        StringArrayType = 0x81, 
        AnsiStringArrayType = 0x82, 
        Int8ArrayType = 0x83, 
        UInt8ArrayType = 0x84, 
        Int16ArrayType = 0x85, 
        UInt16ArrayType = 0x86, 
        Int32ArrayType = 0x87, 
        UInt32ArrayType = 0x88, 
        Int64ArrayType = 0x89, 
        UInt64ArrayType = 0x8A, 
        Real32ArrayType = 0x8B, 
        Real64ArrayType = 0x8C, 
        BoolArrayType = 0x8D, 
        GuidArrayType = 0x8F, 
        SizeTArrayType = 0x90, 
        FileTimeArrayType = 0x91, 
        SysTimeArrayType = 0x92, 
        SidArrayType = 0x93, 
        HexInt32ArrayType = 0x94, 
        HexInt64ArrayType = 0x95, 
    }

    /// <summary>
    /// 
    /// </summary>
    internal interface EvtxValueType
    {
        string Data { get; set; }
        int Length { get; set; }

        void Parse(MemoryStream memoryStream);
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeNull : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeNull()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            memoryStream.Seek(this.Length, SeekOrigin.Current);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeString : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        public EvtxValueTypeString()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            this.Data = Text.ConvertUnicodeToAscii(StreamReaderHelper.ReadString(memoryStream, this.Length));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeAnsiString : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        public EvtxValueTypeAnsiString()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            this.Data = StreamReaderHelper.ReadString(memoryStream, this.Length);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeInt8 : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        public EvtxValueTypeInt8()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            Data = StreamReaderHelper.ReadByte(memoryStream).ToString();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeUInt8 : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeUInt8()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            Data = StreamReaderHelper.ReadSByte(memoryStream).ToString();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeInt16 : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeInt16()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            Data = StreamReaderHelper.ReadInt16(memoryStream).ToString();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeUInt16 : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeUInt16()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            Data = StreamReaderHelper.ReadUInt16(memoryStream).ToString();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeInt32 : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeInt32()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            Data = StreamReaderHelper.ReadInt32(memoryStream).ToString();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeUInt32 : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeUInt32()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            Data = StreamReaderHelper.ReadUInt32(memoryStream).ToString();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeInt64 : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeInt64()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            Data = StreamReaderHelper.ReadInt64(memoryStream).ToString();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeUInt64 : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeUInt64()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            Data = StreamReaderHelper.ReadUInt64(memoryStream).ToString();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeReal32 : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeReal32()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            Data = StreamReaderHelper.ReadFloat(memoryStream).ToString();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeReal64 : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeReal64()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            Data = StreamReaderHelper.ReadDouble(memoryStream).ToString();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeBool : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeBool()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            int temp = StreamReaderHelper.ReadInt32(memoryStream);
            if (temp == 0)
            {
                Data = "False";
            }
            else
            {
                Data = "True";
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeBinary : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeBinary()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            string temp = string.Empty;

            byte[] data = StreamReaderHelper.ReadByteArray(memoryStream, this.Length);
            foreach (byte b in data)
            {
                temp += Convert.ToString(b, 2);
            }

            this.Data = temp;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeGuid : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeGuid()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            this.Data = "{" + StreamReaderHelper.ReadInt32(memoryStream).ToString("X2") + "-" +
                              StreamReaderHelper.ReadInt16(memoryStream).ToString("X2") + "-" +
                              StreamReaderHelper.ReadInt16(memoryStream).ToString("X2") + "-" +
                              StreamReaderHelper.ReadByte(memoryStream).ToString("X2") +
                              StreamReaderHelper.ReadByte(memoryStream).ToString("X2") + "-" +
                              StreamReaderHelper.ReadByte(memoryStream).ToString("X2") +
                              StreamReaderHelper.ReadByte(memoryStream).ToString("X2") +
                              StreamReaderHelper.ReadByte(memoryStream).ToString("X2") +
                              StreamReaderHelper.ReadByte(memoryStream).ToString("X2") +
                              StreamReaderHelper.ReadByte(memoryStream).ToString("X2") +
                              StreamReaderHelper.ReadByte(memoryStream).ToString("X2") + "}";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeSizeT : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeSizeT()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            if (this.Length == 4)
            {
                this.Data = StreamReaderHelper.ReadInt32(memoryStream).ToString("X");
            }
            else
            {
                this.Data = StreamReaderHelper.ReadInt64(memoryStream).ToString("X");
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeFileTime : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeFileTime()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            long temp = StreamReaderHelper.ReadInt64(memoryStream);

            DateTime fileTime = DateTime.FromFileTimeUtc(temp);
            this.Data = fileTime.ToLongDateString() + " " + fileTime.ToLongTimeString();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeSysTime : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeSysTime()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        { 
            ushort year = StreamReaderHelper.ReadUInt16(memoryStream);
            ushort month = StreamReaderHelper.ReadUInt16(memoryStream);
            ushort dayOfWeek = StreamReaderHelper.ReadUInt16(memoryStream);
            ushort day = StreamReaderHelper.ReadUInt16(memoryStream);
            ushort hour = StreamReaderHelper.ReadUInt16(memoryStream);
            ushort minutes = StreamReaderHelper.ReadUInt16(memoryStream);
            ushort seconds = StreamReaderHelper.ReadUInt16(memoryStream);
            ushort milliseconds = StreamReaderHelper.ReadUInt16(memoryStream);

            try
            {
                DateTime temp = new DateTime(year, month, day, hour, minutes, seconds, milliseconds);
                this.Data = temp.ToLongDateString() + " " + temp.ToLongTimeString();
            }
            catch (Exception)
            {
                this.Data = string.Empty;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeSid : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeSid()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            byte revision = StreamReaderHelper.ReadByte(memoryStream);
            byte numSubAuthorities = StreamReaderHelper.ReadByte(memoryStream);

            string temp = "S-" + revision + "-";
            for (int index = 0; index < 6; index++) 
            {
                byte auth = StreamReaderHelper.ReadByte(memoryStream);
                if (auth > 0)
                {
                    temp += auth;
                }
            }

            for (int index = 0; index < numSubAuthorities; index++) 
            {
                temp += "-" + StreamReaderHelper.ReadInt32(memoryStream);
            }

            this.Data = temp;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeHexInt32 : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeHexInt32()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            int temp = StreamReaderHelper.ReadInt32(memoryStream);

            Data = String.Format("{0:X}", temp);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeHexInt64 : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeHexInt64()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            long temp = StreamReaderHelper.ReadInt64(memoryStream);

            Data = String.Format("{0:X}", temp);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeBinXml : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        public List<EvtxToken> Tokens = null;

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeBinXml()
        {
            Data = string.Empty;
            Tokens = new List<EvtxToken>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(EvtxChunk evtxChunk, MemoryStream memoryStream)
        {
            byte[] temp = StreamReaderHelper.ReadByteArray(memoryStream, this.Length);

            BXmlParser bXmlParser = new BXmlParser(evtxChunk);
            bXmlParser.NewTokenEvent += new BXmlParser.NewTokenEventHandler(OnBXmlParser_NewTokenEvent);
            bXmlParser.Parse(temp);

            // Parse out the values for each template instance
            foreach (EvtxToken evtxToken in this.Tokens)
            {
                if (evtxToken.GetType() != typeof(EvtxTokenTemplateInstance))
                {
                    continue;
                }

                EvtxTokenTemplateInstance evtxTokenTemplateInstance = (EvtxTokenTemplateInstance)evtxToken;
                evtxTokenTemplateInstance.GetValues(temp, evtxChunk);
            }

            bXmlParser.NewTokenEvent -= new BXmlParser.NewTokenEventHandler(OnBXmlParser_NewTokenEvent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evtxToken"></param>
        private void OnBXmlParser_NewTokenEvent(EvtxToken evtxToken)
        {
            this.Tokens.Add(evtxToken);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeStringArray : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeStringArray()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            string temp = Text.ConvertUnicodeToAscii(StreamReaderHelper.ReadString(memoryStream, this.Length));

            string[] parts = temp.Split('\0');
            foreach (string part in parts)
            {
                this.Data += part + Environment.NewLine;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeAnsiStringArray : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeAnsiStringArray()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            string temp = StreamReaderHelper.ReadString(memoryStream, this.Length);

            string[] parts = temp.Split('\0');
            foreach (string part in parts)
            {
                this.Data += part + Environment.NewLine;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeInt8Array : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeInt8Array()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            int arrayItems = this.Length;
            for (int index = 0; index < arrayItems; index++)
            {
                this.Data += StreamReaderHelper.ReadByte(memoryStream) + Environment.NewLine;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeUInt8Array : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeUInt8Array()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            int arrayItems = this.Length;
            for (int index = 0; index < arrayItems; index++)
            {
                this.Data += StreamReaderHelper.ReadSByte(memoryStream) + Environment.NewLine;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeInt16Array : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeInt16Array()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            int arrayItems = this.Length / 2;
            for (int index = 0; index < arrayItems; index++)
            {
                this.Data += StreamReaderHelper.ReadInt16(memoryStream) + Environment.NewLine;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeUInt16Array : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeUInt16Array()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            int arrayItems = this.Length / 2;
            for (int index = 0; index < arrayItems; index++)
            {
                this.Data += StreamReaderHelper.ReadUInt16(memoryStream) + Environment.NewLine;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeInt32Array : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeInt32Array()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            int arrayItems = this.Length / 4;
            for (int index = 0; index < arrayItems; index++)
            {
                this.Data += StreamReaderHelper.ReadInt32(memoryStream) + Environment.NewLine;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeUInt32Array : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeUInt32Array()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            int arrayItems = this.Length / 4;
            for (int index = 0; index < arrayItems; index++)
            {
                this.Data += StreamReaderHelper.ReadUInt32(memoryStream) + Environment.NewLine;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeInt64Array : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeInt64Array()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            int arrayItems = this.Length / 8;
            for (int index = 0; index < arrayItems; index++)
            {
                this.Data += StreamReaderHelper.ReadInt64(memoryStream) + Environment.NewLine;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeUInt64Array : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeUInt64Array()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            int arrayItems = this.Length / 8;
            for (int index = 0; index < arrayItems; index++)
            {
                this.Data += StreamReaderHelper.ReadUInt64(memoryStream) + Environment.NewLine;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeReal32Array : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeReal32Array()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            int arrayItems = this.Length / 4;
            for (int index = 0; index < arrayItems; index++)
            {
                this.Data += StreamReaderHelper.ReadFloat(memoryStream) + Environment.NewLine;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeReal64Array : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeReal64Array()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            int arrayItems = this.Length / 8;
            for (int index = 0; index < arrayItems; index++)
            {
                this.Data += StreamReaderHelper.ReadDouble(memoryStream) + Environment.NewLine;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeBoolArray : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeBoolArray()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            int arrayItems = this.Length;
            for (int index = 0; index < arrayItems; index++)
            {
                byte temp = StreamReaderHelper.ReadByte(memoryStream);
                if (temp == 0)
                {
                    this.Data += "False" +Environment.NewLine;
                }
                else
                {
                    this.Data += "True" + Environment.NewLine;
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeGuidArray : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeGuidArray()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            int arrayItems = this.Length / 16;
            for (int index = 0; index < arrayItems; index++)
            {
                this.Data = "{" + StreamReaderHelper.ReadInt32(memoryStream).ToString("X") + "-" +
                              StreamReaderHelper.ReadInt16(memoryStream).ToString("X") + "-" +
                              StreamReaderHelper.ReadInt16(memoryStream).ToString("X") + "-" +
                              StreamReaderHelper.ReadByte(memoryStream).ToString("X") +
                              StreamReaderHelper.ReadByte(memoryStream).ToString("X") + "-" +
                              StreamReaderHelper.ReadByte(memoryStream).ToString("X") +
                              StreamReaderHelper.ReadByte(memoryStream).ToString("X") +
                              StreamReaderHelper.ReadByte(memoryStream).ToString("X") +
                              StreamReaderHelper.ReadByte(memoryStream).ToString("X") +
                              StreamReaderHelper.ReadByte(memoryStream).ToString("X") +
                              StreamReaderHelper.ReadByte(memoryStream).ToString("X") + "}" + Environment.NewLine;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeSizeTArray : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeSizeTArray()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {

        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeFileTimeArray : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeFileTimeArray()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            int arrayItems = this.Length / 8;
            for (int index = 0; index < arrayItems; index++)
            {
                long temp = StreamReaderHelper.ReadInt64(memoryStream);

                DateTime fileTime = DateTime.FromFileTimeUtc(temp);
                this.Data = fileTime.ToLongDateString() + " " + fileTime.ToLongTimeString() + Environment.NewLine;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeSysTimeArray : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeSysTimeArray()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            int arrayItems = this.Length / 16;
            for (int index = 0; index < arrayItems; index++)
            {
                short year = StreamReaderHelper.ReadInt16(memoryStream);
                short month = StreamReaderHelper.ReadInt16(memoryStream);
                short dayOfWeek = StreamReaderHelper.ReadInt16(memoryStream);
                short day = StreamReaderHelper.ReadInt16(memoryStream);
                short hour = StreamReaderHelper.ReadInt16(memoryStream);
                short minutes = StreamReaderHelper.ReadInt16(memoryStream);
                short seconds = StreamReaderHelper.ReadInt16(memoryStream);
                short milliseconds = StreamReaderHelper.ReadInt16(memoryStream);

                DateTime temp = new DateTime(year, month, day, hour, minutes, seconds, milliseconds);

                this.Data = temp.ToLongDateString() + " " + temp.ToLongTimeString() + Environment.NewLine;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeSidArray : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeSidArray()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            int counter = 0;
            do
            {
                byte revision = StreamReaderHelper.ReadByte(memoryStream);
                byte numSubAuthorities = StreamReaderHelper.ReadByte(memoryStream);

                counter += 2;

                string temp = "S-" + revision + "-";
                for (int index = 0; index < 6; index++)
                {
                    byte auth = StreamReaderHelper.ReadByte(memoryStream);
                    if (auth > 0)
                    {
                        temp += auth;
                    }

                    counter++;
                }

                for (int index = 0; index < numSubAuthorities; index++)
                {
                    temp += "-" + StreamReaderHelper.ReadInt32(memoryStream);

                    counter += 4;
                }

                this.Data = temp;
            }
            while (counter < this.Length);
            
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeHexInt32Array : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeHexInt32Array()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            int arrayItems = this.Length / 4;
            for (int index = 0; index < arrayItems; index++)
            {
                int temp = StreamReaderHelper.ReadInt32(memoryStream);

                Data += String.Format("{0:X}", temp) + Environment.NewLine;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EvtxValueTypeHexInt64Array : EvtxValueType
    {
        public string Data { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EvtxValueTypeHexInt64Array()
        {
            Data = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStream"></param>
        public void Parse(MemoryStream memoryStream)
        {
            int arrayItems = this.Length / 8;
            for (int index = 0; index < arrayItems; index++)
            {
                long temp = StreamReaderHelper.ReadInt64(memoryStream);

                Data += String.Format("{0:X}", temp) + Environment.NewLine;
            }
        }
    }
}
