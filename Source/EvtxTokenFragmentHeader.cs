using System.IO;

namespace woanware
{
    internal class EvtxTokenFragmentHeader : EvtxToken
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="evtxChunk"></param>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        public int Parse(EvtxChunk evtxChunk, MemoryStream memoryStream)
        {
            // Move on three bytes which is Major Version (1), Minor Version (1) and Flags (0)
            memoryStream.Seek(3, SeekOrigin.Current);

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
