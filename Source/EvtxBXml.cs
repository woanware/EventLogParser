using System.Collections.Generic;
using NLog;

namespace woanware
{
    /// <summary>
    /// 
    /// </summary>
    internal class EvtxBXml
    {
        private byte[] _data;
        private EvtxChunk _evtxChunk = null;
        public List<EvtxToken> Tokens = null;
        private static Logger _logger = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evtxEvent"></param>
        /// <param name="evtxChunk"></param>
        public EvtxBXml(EvtxEvent evtxEvent, EvtxChunk evtxChunk)
        {
            _logger = LogManager.GetLogger("EvtxBXml");

            _evtxChunk = evtxChunk;

            Tokens = new List<EvtxToken>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Parse()
        {
            _logger.Info("Parsing BinXML");

            BXmlParser bXmlParser = new BXmlParser(_evtxChunk);
            bXmlParser.NewTokenEvent += new BXmlParser.NewTokenEventHandler(OnBXmlParser_NewTokenEvent);
            bXmlParser.Parse(_data);

            _logger.Info("Parsing template values");
            foreach (EvtxToken evtxToken in this.Tokens)
            {
                if (evtxToken.GetType() != typeof(EvtxTokenTemplateInstance))
                {
                    continue;
                }

                EvtxTokenTemplateInstance evtxTokenTemplateInstance = (EvtxTokenTemplateInstance)evtxToken;
                evtxTokenTemplateInstance.GetValues(_data, _evtxChunk);
            }

            bXmlParser.NewTokenEvent -= new BXmlParser.NewTokenEventHandler(OnBXmlParser_NewTokenEvent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evtxToken"></param>
        private void OnBXmlParser_NewTokenEvent(EvtxToken evtxToken)
        {
            _logger.Info("Adding new token to EvtxBXml");
            this.Tokens.Add(evtxToken);
        }

        /// <summary>
        /// 
        /// </summary>
        public byte[] Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }
    }
}
