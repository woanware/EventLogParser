using System.IO;

namespace woanware
{
    /// <summary>
    /// 
    /// </summary>
    internal class EvtxTokenPIData : EvtxToken
    {
        public short StringLength { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evtxChunk"></param>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        public int Parse(EvtxChunk evtxChunk, MemoryStream memoryStream)
        {
            this.StringLength = StreamReaderHelper.ReadInt16(memoryStream);
            this.Name = Text.ConvertUnicodeToAscii(StreamReaderHelper.ReadString(memoryStream, StringLength * 2));

            return 2 + (StringLength * 2) + 2; 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evtxTemplate"></param>
        /// <returns></returns>
        public string Xml(EvtxTemplate evtxTemplate)
        {
            return this.Name + "?>";
        }
    }
}
