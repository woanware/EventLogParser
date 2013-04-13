using System.Collections;
using System.Collections.Generic;

namespace woanware
{
    /// <summary>
    /// 
    /// </summary>
    internal class EvtxChunk
    {
        public string Signature { get; set; }
        public long Offset { get; private set; }
        public long NumLogRecFirst { get; set; }
        public long NumLogRecLast { get; set; }
        public long NumFileRecFirst { get; set; }
        public long NumFileRecLast { get; set; }
        public uint OfsTables { get; set; }
        public uint OfsRecLast { get; set; }
        public uint OfsRecNext { get; set; }
        public uint Checksum { get; set; }
        public uint CalculatedCheckSum { get; set; }
        public Hashtable Strings { get; set; }
        public byte[] Data { get; set; }
        public List<EvtxEvent> Events { get; set; }
        public List<EvtxTemplate> Templates { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        public EvtxChunk(long offset)
        {
            Offset = offset;
            
            Events = new List<EvtxEvent>();
            Templates = new List<EvtxTemplate>();
            Strings = new Hashtable();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public EvtxTemplate GetTemplate(string templateId)
        {
            foreach (EvtxTemplate evtxTemplate in this.Templates)
            {
                if (evtxTemplate.TemplateId == templateId)
                {
                    return evtxTemplate;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public bool DoesTemplateExist(string templateId)
        {
            foreach (EvtxTemplate evtxTemplate in Templates)
            {
                if (evtxTemplate.TemplateId == templateId)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public EvtxString GetString(int pointer)
        {
            if (this.Strings.ContainsKey(pointer) == true)
            {
                return (EvtxString)this.Strings[pointer];
            }

            return null;
        }
    }
}
