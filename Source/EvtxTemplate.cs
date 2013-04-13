using System.Collections.Generic;

namespace woanware
{
    /// <summary>
    /// 
    /// </summary>
    internal class EvtxTemplate
    {
        public string TemplateId { get; private set; }
        public long ValuesOffset { get; private set; }
        public List<EvtxToken> Tokens = null;
        public List<EvtxValueType> Values { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="valuesOffset"></param>
        public EvtxTemplate(string templateId, long valuesOffset)
        {
            this.Tokens = new List<EvtxToken>();
            this.Values = new List<EvtxValueType>();

            this.TemplateId = templateId;
            this.ValuesOffset = valuesOffset;
        }
    }
}
