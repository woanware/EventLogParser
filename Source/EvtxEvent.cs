using System;
using System.Runtime.InteropServices;
using System.Text;

namespace woanware
{
    /// <summary>
    /// 
    /// </summary>
    internal class EvtxEvent
    {
        #region Member Variables
        public EvtxChunk Chunk { get; set; }
        public long Offset { get; private set; }
        public uint Length1 { get; set;}
        public long NumLogRecord { get; set;}
        public DateTime TimeCreated { get; set;}
        public uint Length2 { get; set;}
        public EvtxBXml BXml { get; set; }
        public long Start { get; set; }
        public string Xml { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chunk"></param>
        /// <param name="offset"></param>
        /// <param name="start"></param>
        public EvtxEvent(EvtxChunk chunk, 
                         long offset, 
                         long start)
        {
            Chunk = chunk;
            Offset = offset;
            Start = start;
            Xml = string.Empty;

            BXml = new EvtxBXml(this, chunk);
        }
        #endregion
    }
}
