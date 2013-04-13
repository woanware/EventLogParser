using System.IO;

namespace woanware
{
    /// <summary>
    /// 
    /// </summary>
    internal class EvtxTokenNormalSubstitution : EvtxToken
    {
        public short SubstitutionId { get; set; }
        public byte Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evtxChunk"></param>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        public int Parse(EvtxChunk evtxChunk, MemoryStream memoryStream)
        {
            this.SubstitutionId = StreamReaderHelper.ReadInt16(memoryStream);
            this.Type = StreamReaderHelper.ReadByte(memoryStream);

            return 3;
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
