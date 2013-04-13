using System.IO;

namespace woanware
{
    /// <summary>
    /// 
    /// </summary>
    internal class EvtxTokenEndElement : EvtxToken
    {
        private EvtxChunk _evtxChunk = null;
        public int Pointer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evtxChunk"></param>
        public EvtxTokenEndElement(EvtxChunk evtxChunk)
        {
            _evtxChunk = evtxChunk;
        }

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
            EvtxString evtxString = _evtxChunk.GetString(this.Pointer);

            if (evtxString != null)
            {
                return "</" + evtxString.Value + ">";
            }

            return string.Empty;
        }
    }
}
