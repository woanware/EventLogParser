using System;
using System.Collections.Generic;
using System.IO;
using NLog;

namespace woanware
{
    /// <summary>
    /// 
    /// </summary>
    internal class EvtxTokenTemplateInstance : EvtxToken
    {
        public uint TemplateId { get; set; }
        public string Guid { get; set; }
        public int TemplateDefByteLength { get; set; }
        public long ValuesOffset { get; set; }
        public long EofStream { get; set; }
        public List<EvtxValueType> Values { get; set; }
        private static Logger _logger = null;

        /// <summary>
        /// 
        /// </summary>
        public EvtxTokenTemplateInstance()
        {
            _logger = LogManager.GetLogger("EvtxTokenTemplateInstance");

            this.Values = new List<EvtxValueType>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evtxChunk"></param>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        public int Parse(EvtxChunk evtxChunk, MemoryStream memoryStream)
        {
            long position = memoryStream.Position;

            // Seek one byte e.g. 0
            byte type = StreamReaderHelper.ReadByte(memoryStream);

            this.TemplateId = StreamReaderHelper.ReadUInt32(memoryStream);

            EvtxTemplate evtxTemplate = evtxChunk.GetTemplate(this.TemplateId.ToString());
            if (evtxTemplate == null)
            {
                uint pointer = StreamReaderHelper.ReadUInt32(memoryStream);
                memoryStream.Seek(4, SeekOrigin.Current);

                byte[] temp = StreamReaderHelper.ReadByteArray(memoryStream, 16);

                this.Guid = Text.ConvertByteArrayToHexString(temp);

                this.TemplateDefByteLength = StreamReaderHelper.ReadInt32(memoryStream);

                this.EofStream = memoryStream.Position + this.TemplateDefByteLength;

                this.ValuesOffset = position + 33 + this.TemplateDefByteLength;

                return 33;
            }
            else
            {
                this.EofStream = memoryStream.Position;

                memoryStream.Seek(4, SeekOrigin.Current);
                this.ValuesOffset = memoryStream.Position;
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="evtxChunk"></param>
        public void GetValues(byte[] data, EvtxChunk evtxChunk)
        {
            MemoryStream memoryStream = new MemoryStream(data);
            memoryStream.Seek(this.ValuesOffset, SeekOrigin.Begin);

            _logger.Info("Retrieving template values");

            int numValues = StreamReaderHelper.ReadInt32(memoryStream);

            _logger.Info("Identified no. template values: " + numValues);

            if (numValues == 0)
            {
                return;
            }

            int valueCount = 0;
            do
            {
                short length = StreamReaderHelper.ReadInt16(memoryStream);
                byte type = StreamReaderHelper.ReadByte(memoryStream);
                memoryStream.Seek(1, SeekOrigin.Current);

                switch (type)
                {
                    case 0x20:
                        break;
                    case 0x23:
                        break;
                    case (byte)ValueTypes.AnsiStringArrayType:
                        _logger.Info("Identified value type: EvtxValueTypeAnsiStringArray");
                        EvtxValueTypeAnsiStringArray evtxValueTypeAnsiStringArray = new EvtxValueTypeAnsiStringArray();
                        evtxValueTypeAnsiStringArray.Length = length;
                        this.Values.Add(evtxValueTypeAnsiStringArray);
                        break;
                    case (byte)ValueTypes.AnsiStringType:
                        _logger.Info("Identified value type: EvtxValueTypeAnsiString");
                        EvtxValueTypeAnsiString evtxValueTypeAnsiString = new EvtxValueTypeAnsiString();
                        evtxValueTypeAnsiString.Length = length;
                        this.Values.Add(evtxValueTypeAnsiString);
                        break;
                    case (byte)ValueTypes.BinaryType:
                        _logger.Info("Identified value type: EvtxValueTypeBinary");
                        EvtxValueTypeBinary evtxValueTypeBinary = new EvtxValueTypeBinary();
                        evtxValueTypeBinary.Length = length;
                        this.Values.Add(evtxValueTypeBinary);
                        break;
                    case (byte)ValueTypes.BinXmlType:
                        _logger.Info("Identified value type: EvtxValueTypeBinXml");
                        EvtxValueTypeBinXml evtxValueTypeBinXml = new EvtxValueTypeBinXml();
                        evtxValueTypeBinXml.Length = length;
                        this.Values.Add(evtxValueTypeBinXml);
                        break;
                    case (byte)ValueTypes.BoolArrayType:
                        _logger.Info("Identified value type: EvtxValueTypeBoolArray");
                        EvtxValueTypeBoolArray evtxValueTypeBoolArray = new EvtxValueTypeBoolArray();
                        evtxValueTypeBoolArray.Length = length;
                        this.Values.Add(evtxValueTypeBoolArray);
                        break;
                    case (byte)ValueTypes.BoolType:
                        _logger.Info("Identified value type: EvtxValueTypeBool");
                        EvtxValueTypeBool evtxValueTypeBool = new EvtxValueTypeBool();
                        evtxValueTypeBool.Length = length;
                        this.Values.Add(evtxValueTypeBool);
                        break;
                    case (byte)ValueTypes.FileTimeArrayType:
                        _logger.Info("Identified value type: EvtxValueTypeFileTimeArray");
                        EvtxValueTypeFileTimeArray evtxValueTypeFileTimeArray = new EvtxValueTypeFileTimeArray();
                        evtxValueTypeFileTimeArray.Length = length;
                        this.Values.Add(evtxValueTypeFileTimeArray);
                        break;
                    case (byte)ValueTypes.FileTimeType:
                        _logger.Info("Identified value type: EvtxValueTypeFileTime");
                        EvtxValueTypeFileTime evtxValueTypeFileTime = new EvtxValueTypeFileTime();
                        evtxValueTypeFileTime.Length = length;
                        this.Values.Add(evtxValueTypeFileTime);
                        break;
                    case (byte)ValueTypes.GuidArrayType:
                        _logger.Info("Identified value type: EvtxValueTypeGuidArray");
                        EvtxValueTypeGuidArray evtxValueTypeGuidArray = new EvtxValueTypeGuidArray();
                        evtxValueTypeGuidArray.Length = length;
                        this.Values.Add(evtxValueTypeGuidArray);
                        break;
                    case (byte)ValueTypes.GuidType:
                        _logger.Info("Identified value type: EvtxValueTypeGuid");
                        EvtxValueTypeGuid evtxValueTypeGuid = new EvtxValueTypeGuid();
                        evtxValueTypeGuid.Length = length;
                        this.Values.Add(evtxValueTypeGuid);
                        break;
                    case (byte)ValueTypes.HexInt32ArrayType:
                        _logger.Info("Identified value type: EvtxValueTypeHexInt32Array");
                        EvtxValueTypeHexInt32Array evtxValueTypeHexInt32Array = new EvtxValueTypeHexInt32Array();
                        evtxValueTypeHexInt32Array.Length = length;
                        this.Values.Add(evtxValueTypeHexInt32Array);
                        break;
                    case (byte)ValueTypes.HexInt32Type:
                        _logger.Info("Identified value type: EvtxValueTypeHexInt32");
                        EvtxValueTypeHexInt32 evtxValueTypeHexInt32 = new EvtxValueTypeHexInt32();
                        evtxValueTypeHexInt32.Length = length;
                        this.Values.Add(evtxValueTypeHexInt32);
                        break;
                    case (byte)ValueTypes.HexInt64ArrayType:
                        _logger.Info("Identified value type: EvtxValueTypeHexInt64Array");
                        EvtxValueTypeHexInt64Array evtxValueTypeHexInt64Array = new EvtxValueTypeHexInt64Array();
                        evtxValueTypeHexInt64Array.Length = length;
                        this.Values.Add(evtxValueTypeHexInt64Array);
                        break;
                    case (byte)ValueTypes.HexInt64Type:
                        _logger.Info("Identified value type: EvtxValueTypeHexInt64");
                        EvtxValueTypeHexInt64 evtxValueTypeHexInt64 = new EvtxValueTypeHexInt64();
                        evtxValueTypeHexInt64.Length = length;
                        this.Values.Add(evtxValueTypeHexInt64);
                        break;
                    case (byte)ValueTypes.Int16ArrayType:
                        _logger.Info("Identified value type: EvtxValueTypeInt16Array");
                        EvtxValueTypeInt16Array evtxValueTypeInt16Array = new EvtxValueTypeInt16Array();
                        evtxValueTypeInt16Array.Length = length;
                        this.Values.Add(evtxValueTypeInt16Array);
                        break;
                    case (byte)ValueTypes.Int16Type:
                        _logger.Info("Identified value type: EvtxValueTypeInt16");
                        EvtxValueTypeInt16 evtxValueTypeInt16 = new EvtxValueTypeInt16();
                        evtxValueTypeInt16.Length = length;
                        this.Values.Add(evtxValueTypeInt16);
                        break;
                    case (byte)ValueTypes.Int32ArrayType:
                        _logger.Info("Identified value type: EvtxValueTypeInt32Array");
                        EvtxValueTypeInt32Array evtxValueTypeInt32Array = new EvtxValueTypeInt32Array();
                        evtxValueTypeInt32Array.Length = length;
                        this.Values.Add(evtxValueTypeInt32Array);
                        break;
                    case (byte)ValueTypes.Int32Type:
                        _logger.Info("Identified value type: EvtxValueTypeInt32");
                        EvtxValueTypeInt32 evtxValueTypeInt32 = new EvtxValueTypeInt32();
                        evtxValueTypeInt32.Length = length;
                        this.Values.Add(evtxValueTypeInt32);
                        break;
                    case (byte)ValueTypes.Int64ArrayType:
                        _logger.Info("Identified value type: EvtxValueTypeInt64Array");
                        EvtxValueTypeInt64Array evtxValueTypeInt64Array = new EvtxValueTypeInt64Array();
                        evtxValueTypeInt64Array.Length = length;
                        this.Values.Add(evtxValueTypeInt64Array);
                        break;
                    case (byte)ValueTypes.Int64Type:
                        _logger.Info("Identified value type: EvtxValueTypeInt64");
                        EvtxValueTypeInt64 evtxValueTypeInt64 = new EvtxValueTypeInt64();
                        evtxValueTypeInt64.Length = length;
                        this.Values.Add(evtxValueTypeInt64);
                        break;
                    
                    case (byte)ValueTypes.Int8ArrayType:
                        _logger.Info("Identified value type: EvtxValueTypeInt8Array");
                        EvtxValueTypeInt8Array evtxValueTypeInt8Array = new EvtxValueTypeInt8Array();
                        evtxValueTypeInt8Array.Length = length;
                        this.Values.Add(evtxValueTypeInt8Array);
                        break;
                    case (byte)ValueTypes.Int8Type:
                        _logger.Info("Identified value type: EvtxValueTypeInt8");
                        EvtxValueTypeInt8 evtxValueTypeInt8 = new EvtxValueTypeInt8();
                        evtxValueTypeInt8.Length = length;
                        this.Values.Add(evtxValueTypeInt8);
                        break;
                    case (byte)ValueTypes.NullType:
                        _logger.Info("Identified value type: EvtxValueTypeNull");
                        EvtxValueTypeNull evtxValueTypeNull = new EvtxValueTypeNull();
                        evtxValueTypeNull.Length = length;
                        this.Values.Add(evtxValueTypeNull);
                        break;
                    case (byte)ValueTypes.Real32ArrayType:
                        _logger.Info("Identified value type: EvtxValueTypeReal32Array");
                        EvtxValueTypeReal32Array evtxValueTypeReal32Array = new EvtxValueTypeReal32Array();
                        evtxValueTypeReal32Array.Length = length;
                        this.Values.Add(evtxValueTypeReal32Array);
                        break;
                    case (byte)ValueTypes.Real32Type:
                        _logger.Info("Identified value type: EvtxValueTypeReal32");
                        EvtxValueTypeReal32 evtxValueTypeReal32 = new EvtxValueTypeReal32();
                        evtxValueTypeReal32.Length = length;
                        this.Values.Add(evtxValueTypeReal32);
                        break;
                    case (byte)ValueTypes.Real64ArrayType:
                        _logger.Info("Identified value type: EvtxValueTypeReal64Array");
                        EvtxValueTypeReal64Array evtxValueTypeReal64Array = new EvtxValueTypeReal64Array();
                        evtxValueTypeReal64Array.Length = length;
                        this.Values.Add(evtxValueTypeReal64Array);
                        break;
                    case (byte)ValueTypes.Real64Type:
                        _logger.Info("Identified value type: EvtxValueTypeReal64");
                        EvtxValueTypeReal64 evtxValueTypeReal64 = new EvtxValueTypeReal64();
                        evtxValueTypeReal64.Length = length;
                        this.Values.Add(evtxValueTypeReal64);
                        break;
                    case (byte)ValueTypes.SidArrayType:
                        _logger.Info("Identified value type: EvtxValueTypeSidArray");
                        EvtxValueTypeSidArray evtxValueTypeSidArray = new EvtxValueTypeSidArray();
                        evtxValueTypeSidArray.Length = length;
                        this.Values.Add(evtxValueTypeSidArray);
                        break;
                    case (byte)ValueTypes.SidType:
                        _logger.Info("Identified value type: EvtxValueTypeSid");
                        EvtxValueTypeSid evtxValueTypeSid = new EvtxValueTypeSid();
                        evtxValueTypeSid.Length = length;
                        this.Values.Add(evtxValueTypeSid);
                        break;
                    case (byte)ValueTypes.SizeTArrayType:
                        _logger.Info("Identified value type: EvtxValueTypeSizeTArray");
                        EvtxValueTypeSizeTArray evtxValueTypeSizeTArray = new EvtxValueTypeSizeTArray();
                        evtxValueTypeSizeTArray.Length = length;
                        this.Values.Add(evtxValueTypeSizeTArray);
                        break;
                    case (byte)ValueTypes.SizeTType:
                        _logger.Info("Identified value type: EvtxValueTypeSizeT");
                        EvtxValueTypeSizeT evtxValueTypeSizeT = new EvtxValueTypeSizeT();
                        evtxValueTypeSizeT.Length = length;
                        this.Values.Add(evtxValueTypeSizeT);
                        break;
                    case (byte)ValueTypes.StringArrayType:
                        _logger.Info("Identified value type: EvtxValueTypeStringArray");
                        EvtxValueTypeStringArray evtxValueTypeStringArray = new EvtxValueTypeStringArray();
                        evtxValueTypeStringArray.Length = length;
                        this.Values.Add(evtxValueTypeStringArray);
                        break;
                    case (byte)ValueTypes.StringType:
                        _logger.Info("Identified value type: EvtxValueTypeString");
                        EvtxValueTypeString evtxValueTypeString = new EvtxValueTypeString();
                        evtxValueTypeString.Length = length;
                        this.Values.Add(evtxValueTypeString);
                        break;
                    case (byte)ValueTypes.SysTimeArrayType:
                        _logger.Info("Identified value type: EvtxValueTypeSysTimeArray");
                        EvtxValueTypeSysTimeArray evtxValueTypeSysTimeArray = new EvtxValueTypeSysTimeArray();
                        evtxValueTypeSysTimeArray.Length = length;
                        this.Values.Add(evtxValueTypeSysTimeArray);
                        break;
                    case (byte)ValueTypes.SysTimeType:
                        _logger.Info("Identified value type: EvtxValueTypeSysTime");
                        EvtxValueTypeSysTime evtxValueTypeSysTime = new EvtxValueTypeSysTime();
                        evtxValueTypeSysTime.Length = length;
                        this.Values.Add(evtxValueTypeSysTime);
                        break;
                    case (byte)ValueTypes.UInt16ArrayType:
                        _logger.Info("Identified value type: EvtxValueTypeUInt16Array");
                        EvtxValueTypeUInt16Array evtxValueTypeUInt16Array = new EvtxValueTypeUInt16Array();
                        evtxValueTypeUInt16Array.Length = length;
                        this.Values.Add(evtxValueTypeUInt16Array);
                        break;
                    case (byte)ValueTypes.UInt16Type:
                        _logger.Info("Identified value type: EvtxValueTypeUInt16");
                        EvtxValueTypeUInt16 evtxValueTypeUInt16 = new EvtxValueTypeUInt16();
                        evtxValueTypeUInt16.Length = length;
                        this.Values.Add(evtxValueTypeUInt16);
                        break;
                    case (byte)ValueTypes.UInt32ArrayType:
                        _logger.Info("Identified value type: EvtxValueTypeUInt32Array");
                        EvtxValueTypeUInt32Array evtxValueTypeUInt32Array = new EvtxValueTypeUInt32Array();
                        evtxValueTypeUInt32Array.Length = length;
                        this.Values.Add(evtxValueTypeUInt32Array);
                        break;
                    case (byte)ValueTypes.UInt32Type:
                        _logger.Info("Identified value type: EvtxValueTypeUInt32");
                        EvtxValueTypeUInt32 evtxValueTypeUInt32 = new EvtxValueTypeUInt32();
                        evtxValueTypeUInt32.Length = length;
                        this.Values.Add(evtxValueTypeUInt32);
                        break;
                    case (byte)ValueTypes.UInt64ArrayType:
                        _logger.Info("Identified value type: EvtxValueTypeUInt64Array");
                        EvtxValueTypeUInt64Array evtxValueTypeUInt64Array = new EvtxValueTypeUInt64Array();
                        evtxValueTypeUInt64Array.Length = length;
                        this.Values.Add(evtxValueTypeUInt64Array);
                        break;
                    case (byte)ValueTypes.UInt64Type:
                        _logger.Info("Identified value type: EvtxValueTypeUInt64");
                        EvtxValueTypeUInt64 evtxValueTypeUInt64 = new EvtxValueTypeUInt64();
                        evtxValueTypeUInt64.Length = length;
                        this.Values.Add(evtxValueTypeUInt64);
                        break;
                    case (byte)ValueTypes.UInt8ArrayType:
                        _logger.Info("Identified value type: EvtxValueTypeUInt8Array");
                        EvtxValueTypeUInt8Array evtxValueTypeUInt8Array = new EvtxValueTypeUInt8Array();
                        evtxValueTypeUInt8Array.Length = length;
                        this.Values.Add(evtxValueTypeUInt8Array);
                        break;
                    case (byte)ValueTypes.UInt8Type:
                        _logger.Info("Identified value type: EvtxValueTypeUInt8");
                        EvtxValueTypeUInt8 evtxValueTypeUInt8 = new EvtxValueTypeUInt8();
                        evtxValueTypeUInt8.Length = length;
                        this.Values.Add(evtxValueTypeUInt8);
                        break;
                    default:
                        _logger.Info("Unknown Type: " + type + "(" + type.ToString("X2") + ")");
                        break;
                }

                valueCount++;
            }
            while (numValues != valueCount);

            long valuesOffset = memoryStream.Position;

            // Now extract the values
            foreach (EvtxValueType evtxValueType in this.Values)
            {
                _logger.Info("Identified value type: " + evtxValueType.GetType());

                if (evtxValueType.GetType() != typeof(EvtxValueTypeBinXml))
                {
                    evtxValueType.Parse(memoryStream);
                }
                else
                {
                    EvtxValueTypeBinXml evtxValueTypeBinXml = (EvtxValueTypeBinXml)evtxValueType;
                    evtxValueTypeBinXml.Parse(evtxChunk, memoryStream);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evtxTemplate"></param>
        /// <returns></returns>
        public string Xml(EvtxTemplate evtxTemplate)
        {
            return string.Empty;
        }
    }
}
