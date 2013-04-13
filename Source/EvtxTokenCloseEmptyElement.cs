using System.IO;

namespace woanware
{
    /// <summary>
    /// 
    /// </summary>
    internal class EvtxTokenCloseEmptyElement : EvtxToken
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="evtxChunk"></param>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        public int Parse(EvtxChunk evtxChunk, MemoryStream memoryStream)
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
            return "/>";
        }
    }
}
