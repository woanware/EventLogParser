using System;
using System.Collections.Generic;
using System.Text;
using NLog;

namespace woanware
{
    /// <summary>
    /// 
    /// </summary>
    internal class XmlGenerator
    {
        private EvtxChunk _evtxChunk = null;
        private EvtxEvent _evtxEvent = null;
        private StringBuilder _xml = null;
        private static Logger _logger = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evtxChunk"></param>
        /// <param name="evtxEvent"></param>
        public XmlGenerator(EvtxChunk evtxChunk, EvtxEvent evtxEvent)
        {
            _logger = LogManager.GetLogger("XmlGenerator");

            _evtxChunk = evtxChunk;
            _evtxEvent = evtxEvent;

            _xml = new StringBuilder();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Generate()
        {
            _logger.Info("Generating XML");

            this.PerformGeneration(_evtxEvent.BXml.Tokens, null);

            return _xml.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="evtxTemplate"></param>
        private void PerformGeneration(List<EvtxToken> tokens, EvtxTemplate evtxTemplate)
        {
            for (int index = 0; index < tokens.Count; index++)
            {
                _logger.Info("XML token: " + tokens[index].GetType());

                if (tokens[index].GetType() == typeof(EvtxTokenTemplateInstance))
                {
                    EvtxTokenTemplateInstance evtxTokenTemplateInstance = (EvtxTokenTemplateInstance)tokens[index];
                    EvtxTemplate evtxTemplateTemp = _evtxChunk.GetTemplate(evtxTokenTemplateInstance.TemplateId.ToString());
                    if (evtxTemplateTemp != null)
                    {
                        evtxTemplateTemp.Values = evtxTokenTemplateInstance.Values;
                        this.PerformGeneration(evtxTemplateTemp.Tokens, evtxTemplateTemp);
                    }
                }
                else
                {
                    if (tokens[index].GetType() == typeof(EvtxTokenAttribute))
                    {
                        if (tokens[index + 1].GetType() == typeof(EvtxTokenValueText))
                        {
                            // Get the the value from the next token
                            string temp = ((EvtxTokenValueText)tokens[index + 1]).Name;
                            EvtxTokenAttribute evtxTokenAttribute = (EvtxTokenAttribute)tokens[index];

                            _xml.Append(evtxTokenAttribute.Xml(evtxTemplate, temp));

                            index++;
                        }
                        else if (tokens[index + 1].GetType() == typeof(EvtxTokenOptionalSubstitution))
                        {
                            EvtxTokenAttribute evtxTokenAttribute = (EvtxTokenAttribute)tokens[index];
                            EvtxTokenOptionalSubstitution evtxTokenOptionalSubstitution = (EvtxTokenOptionalSubstitution)tokens[index + 1];
                            EvtxValueType evtxValueType = evtxTemplate.Values[evtxTokenOptionalSubstitution.Index];

                            if (evtxValueType.GetType() == typeof(EvtxValueTypeBinXml))
                            {
                                EvtxValueTypeBinXml evtxValueTypeBinXml = (EvtxValueTypeBinXml)evtxValueType;
                                this.PerformGeneration(evtxValueTypeBinXml.Tokens, null);
                            }
                            else
                            {
                                _xml.Append(evtxTokenAttribute.Xml(evtxTemplate, evtxValueType.Data));

                                index++;
                            }
                        }
                        else
                        {
                            _xml.Append(tokens[index].Xml(evtxTemplate));
                        }
                    }
                    else
                    {
                        if (tokens[index].GetType() == typeof(EvtxTokenOptionalSubstitution))
                        {
                            EvtxTokenOptionalSubstitution evtxTokenOptionalSubstitution = (EvtxTokenOptionalSubstitution)tokens[index];
                            EvtxValueType evtxValueType = evtxTemplate.Values[evtxTokenOptionalSubstitution.Index];

                            if (evtxValueType.GetType() == typeof(EvtxValueTypeBinXml))
                            {
                                EvtxValueTypeBinXml evtxValueTypeBinXml = (EvtxValueTypeBinXml)evtxValueType;
                                this.PerformGeneration(evtxValueTypeBinXml.Tokens, null);
                            }
                            else
                            {
                                _xml.Append(evtxValueType.Data);
                            }
                        }
                        else if (tokens[index].GetType() == typeof(EvtxTokenNormalSubstitution))
                        {
                            EvtxTokenNormalSubstitution evtxTokenNormalSubstitution = (EvtxTokenNormalSubstitution)tokens[index];
                            EvtxValueType evtxValueType = evtxTemplate.Values[evtxTokenNormalSubstitution.SubstitutionId];

                            if (evtxValueType.GetType() == typeof(EvtxValueTypeBinXml))
                            {
                                EvtxValueTypeBinXml evtxValueTypeBinXml = (EvtxValueTypeBinXml)evtxValueType;
                                this.PerformGeneration(evtxValueTypeBinXml.Tokens, null);
                            }
                            else
                            {
                                _xml.Append(evtxValueType.Data);
                            }
                        }
                        else
                        {
                            _xml.Append(tokens[index].Xml(evtxTemplate));
                        }
                    }
                }
            }
        }
    }
}
