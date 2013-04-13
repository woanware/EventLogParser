using System.IO;

namespace woanware
{
    /// <summary>
    /// 
    /// </summary>
    internal class EvtxTokenValueText : EvtxToken
    {
        public short StringLength { get; set; }
        public string Name { get; set; }
        public bool HasMore { get; set; }
        public short Type { get; set; }
        public EvtxValueType ValueType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evtxChunk"></param>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        public int Parse(EvtxChunk evtxChunk, MemoryStream memoryStream)
        {
            // Move on one byte which is StringType e.g. 1
            int type = StreamReaderHelper.ReadSByte(memoryStream);
            
            this.StringLength = StreamReaderHelper.ReadInt16(memoryStream);
            this.Name = Text.ConvertUnicodeToAscii(StreamReaderHelper.ReadString(memoryStream, StringLength * 2));

            return 3 + ((StringLength + 1) * 2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evtxTemplate"></param>
        /// <returns></returns>
        public string Xml(EvtxTemplate evtxTemplate)
        {
            return this.Name;
        }
    }
}
