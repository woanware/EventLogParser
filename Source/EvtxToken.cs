using System.IO;

namespace woanware
{
    /// <summary>
    /// 
    /// </summary>
    internal interface EvtxToken
    {
        int Parse(EvtxChunk evtxChunk, MemoryStream memoryStream);

        string Xml(EvtxTemplate evtxTemplate);
    }
}
