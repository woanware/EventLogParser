using System.IO;

namespace woanware
{
    /// <summary>
    /// 
    /// </summary>
    internal class EvtxTokenCloseStartElement : EvtxToken
    {
        public int Pointer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evtxChunk"></param>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        public int Parse(EvtxChunk evtxChunk, 
                         MemoryStream memoryStream)
        {
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evtxTemplate"></param>
        /// <returns></returns>
        public string Xml(EvtxTemplate evtxTemplate)
        {
            return ">";
        }
    }
}
