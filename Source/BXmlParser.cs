using System;
using System.Collections.Generic;
using System.IO;
using NLog;

namespace woanware
{
    /// <summary>
    /// 
    /// </summary>
    internal class BXmlParser
    {
        public delegate void NewTokenEventHandler(EvtxToken evtxToken);
        public event NewTokenEventHandler NewTokenEvent;

        private EvtxChunk _evtxChunk = null;
        private long _length = 0;
        private bool _parsingTemplate = false;
        private EvtxTemplate _evtxTemplate = null;
        private long _eofStream = 0;
        private Stack<EvtxToken> _tokens = null;
        private static Logger _logger = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evtxChunk"></param>
        public BXmlParser(EvtxChunk evtxChunk)
        {
            _logger = LogManager.GetLogger("BXmlParser");

            _evtxChunk = evtxChunk;

            _tokens = new Stack<EvtxToken>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Parse(byte[] data)
        {
            MemoryStream memoryStream = new MemoryStream(data);

            bool process = true;
            do
            {
                int type = StreamReaderHelper.ReadByte(memoryStream);

                _logger.Info("Type No.: " + type.ToString("X2") + "#" + type);

                switch (type)
                {
                    // EOFToken
                    case 0x00:
                        _logger.Info("Identified token: EvtxTokenEOF");

                        EvtxTokenEOF evtxTokenEOF = new EvtxTokenEOF();
                        _length -= evtxTokenEOF.Parse(_evtxChunk, memoryStream);

                        this.ProcessNewToken(evtxTokenEOF);
                        if (_parsingTemplate == true)
                        {
                            _evtxChunk.Templates.Add(_evtxTemplate);
                            _parsingTemplate = false;
                        }
                        break;
                    // OpenStartElementToken
                    case 0x01:
                    case 0x41:
                        _logger.Info("Identified token: EvtxTokenOpenStartElement");

                        EvtxTokenOpenStartElement evtxTokenOpenStartElement = new EvtxTokenOpenStartElement(_evtxChunk);
                        evtxTokenOpenStartElement.HasElements = type == 0x41 ? true : false;
                        _length -= evtxTokenOpenStartElement.Parse(_evtxChunk, memoryStream);

                        _tokens.Push(evtxTokenOpenStartElement);

                        this.ProcessNewToken(evtxTokenOpenStartElement);
                        break;
                    // CloseStartElementToken
                    case 0x02:
                        _logger.Info("Identified token: EvtxTokenCloseStartElement");

                        EvtxTokenCloseStartElement evtxTokenCloseStartElement = new EvtxTokenCloseStartElement();
                        _length -= evtxTokenCloseStartElement.Parse(_evtxChunk, memoryStream);

                        this.ProcessNewToken(evtxTokenCloseStartElement);
                        break;
                    // CloseEmptyElementToken
                    case 0x03:
                        _logger.Info("Identified token: EvtxTokenCloseEmptyElement");

                        EvtxTokenCloseEmptyElement evtxTokenCloseEmptyElement = new EvtxTokenCloseEmptyElement();
                        _length -= evtxTokenCloseEmptyElement.Parse(_evtxChunk, memoryStream);

                        _tokens.Pop();

                        this.ProcessNewToken(evtxTokenCloseEmptyElement);
                        break;
                    // EndElementToken
                    case 0x04:
                        _logger.Info("Identified token: EvtxTokenEndElement");

                        EvtxTokenEndElement evtxTokenEndElement = new EvtxTokenEndElement(_evtxChunk);
                        _length -= evtxTokenEndElement.Parse(_evtxChunk, memoryStream);

                        EvtxTokenOpenStartElement evtxTempEndElement = (EvtxTokenOpenStartElement)_tokens.Pop();
                        evtxTokenEndElement.Pointer = evtxTempEndElement.Pointer;

                        this.ProcessNewToken(evtxTokenEndElement);
                        break;
                    // ValueTextToken
                    case 0x05:
                    case 0x45:
                        _logger.Info("Identified token: EvtxTokenValueText");

                        EvtxTokenValueText evtxTokenValueText = new EvtxTokenValueText();
                        evtxTokenValueText.HasMore = type == 0x45 ? true : false;
                        _length -= evtxTokenValueText.Parse(_evtxChunk, memoryStream);

                        this.ProcessNewToken(evtxTokenValueText);
                        break;
                    // AttributeToken
                    case 0x06:
                    case 0x46:
                        _logger.Info("Identified token: EvtxTokenAttribute");

                        EvtxTokenAttribute evtxTokenAttribute = new EvtxTokenAttribute(_evtxChunk);
                        evtxTokenAttribute.HasMore = type == 0x46 ? true : false;
                        _length -= evtxTokenAttribute.Parse(_evtxChunk, memoryStream);

                        this.ProcessNewToken(evtxTokenAttribute);
                        break;
                    // CDATASectionToken
                    case 0x07:
                    case 0x47:
                        _logger.Info("Identified token: EvtxTokenCDataSection");

                        EvtxTokenCDataSection evtxTokenCDataSection = new EvtxTokenCDataSection();
                        evtxTokenCDataSection.HasMore = type == 0x47 ? true : false;
                        _length -= evtxTokenCDataSection.Parse(_evtxChunk, memoryStream);

                        this.ProcessNewToken(evtxTokenCDataSection);
                        break;
                    // CharRefToken
                    case 0x08:
                    case 0x48:
                        _logger.Info("Identified token: EvtxTokenCharRef");

                        EvtxTokenCharRef evtxTokenCharRef = new EvtxTokenCharRef();
                        evtxTokenCharRef.HasMore = type == 0x48 ? true : false;
                        _length -= evtxTokenCharRef.Parse(_evtxChunk, memoryStream);

                        this.ProcessNewToken(evtxTokenCharRef);
                        break;
                    // EntityRefToken
                    case 0x09:
                    case 0x49:
                        _logger.Info("Identified token: EvtxTokenEntityRef");

                        EvtxTokenEntityRef evtxTokenEntityRef = new EvtxTokenEntityRef();
                        evtxTokenEntityRef.HasMore = type == 0x49 ? true : false;
                        _length -= evtxTokenEntityRef.Parse(_evtxChunk, memoryStream);

                        this.ProcessNewToken(evtxTokenEntityRef);
                        break;
                    // PITargetToken
                    case 0x0A:
                        _logger.Info("Identified token: EvtxTokenPITarget");

                        EvtxTokenPITarget evtxTokenPITarget = new EvtxTokenPITarget();
                        _length -= evtxTokenPITarget.Parse(_evtxChunk, memoryStream);

                        this.ProcessNewToken(evtxTokenPITarget);
                        break;
                    // PIDataToken
                    case 0x0B:
                        _logger.Info("Identified token: EvtxTokenPIData");

                        EvtxTokenPIData evtxTokenPIData = new EvtxTokenPIData();
                        _length -= evtxTokenPIData.Parse(_evtxChunk, memoryStream);

                        this.ProcessNewToken(evtxTokenPIData);
                        break;
                    // TemplateInstanceToken
                    case 0x0C:
                        _logger.Info("Identified token: EvtxTokenTemplateInstance");

                        EvtxTokenTemplateInstance evtxTokenTemplateInstance = new EvtxTokenTemplateInstance();
                        _length -= evtxTokenTemplateInstance.Parse(_evtxChunk, memoryStream);

                        if (_evtxChunk.DoesTemplateExist(evtxTokenTemplateInstance.TemplateId.ToString()) == false)
                        {
                            _evtxTemplate = new EvtxTemplate(evtxTokenTemplateInstance.TemplateId.ToString(), evtxTokenTemplateInstance.ValuesOffset);
                            
                            this.ProcessNewToken(evtxTokenTemplateInstance);

                            // Set this after so that the template instance still gets added to the original item
                            _parsingTemplate = true;
                        }
                        else
                        {
                            this.ProcessNewToken(evtxTokenTemplateInstance);
                            //_parsingTemplate = false;
                        }

                        break;
                    // NormalSubstitutionToken
                    case 0x0D:
                        _logger.Info("Identified token: EvtxTokenNormalSubstitution");

                        EvtxTokenNormalSubstitution evtxTokenNormalSubstitution = new EvtxTokenNormalSubstitution();
                        _length -= evtxTokenNormalSubstitution.Parse(_evtxChunk, memoryStream);

                        this.ProcessNewToken(evtxTokenNormalSubstitution);
                        break;
                    // OptionalSubstitutionToken
                    case 0x0E:
                        _logger.Info("Identified token: EvtxTokenOptionalSubstitution");

                        EvtxTokenOptionalSubstitution evtxTokenOptionalSubstitution = new EvtxTokenOptionalSubstitution();
                        _length -= evtxTokenOptionalSubstitution.Parse(_evtxChunk, memoryStream);

                        this.ProcessNewToken(evtxTokenOptionalSubstitution);
                        break;
                    // FragmentHeaderToken
                    case 0x0F:
                        _logger.Info("Identified token: EvtxTokenFragmentHeader");

                        EvtxTokenFragmentHeader evtxTokenFragmentHeader = new EvtxTokenFragmentHeader();
                        _length -= evtxTokenFragmentHeader.Parse(_evtxChunk, memoryStream);

                        this.ProcessNewToken(evtxTokenFragmentHeader);
                        break;
                    default:
                        Console.WriteLine("UNKNOWN TYPE:" + type);
                        break;
                }

               if (memoryStream.Position >= this._eofStream & this._eofStream > 0)
               {
                   process = false;
               }
            }
            while (process == true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evtxToken"></param>
        private void ProcessNewToken(EvtxToken evtxToken)
        {
            _logger.Info("Processing new token: " + evtxToken.GetType());

            if (evtxToken.GetType() == typeof(EvtxTokenTemplateInstance))
            {
                EvtxTokenTemplateInstance evtxTokenTemplateInstance = (EvtxTokenTemplateInstance)evtxToken;

                if (evtxTokenTemplateInstance.EofStream > _eofStream)
                {
                    _logger.Info("Updating EOF (EvtxTokenTemplateInstance): " + evtxTokenTemplateInstance.EofStream);
                    _eofStream = evtxTokenTemplateInstance.EofStream;
                }
            }

            if (evtxToken.GetType() == typeof(EvtxTokenOpenStartElement))
            {
                EvtxTokenOpenStartElement evtxTokenOpenStartElement = (EvtxTokenOpenStartElement)evtxToken;

                if (evtxTokenOpenStartElement.EofStream > _eofStream)
                {
                    _logger.Info("Updating EOF (EvtxTokenOpenStartElement): " + evtxTokenOpenStartElement.EofStream);
                    _eofStream = evtxTokenOpenStartElement.EofStream;
                }
            }

            if (_parsingTemplate == true)
            {
                _logger.Info("Adding new token to template");
                _evtxTemplate.Tokens.Add(evtxToken);
            }
            else
            {
                this.OnNewToken(evtxToken);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evtxToken"></param>
        private void OnNewToken(EvtxToken evtxToken)
        {
            if (NewTokenEvent != null)
            {
                NewTokenEvent(evtxToken);
            }
        }
    }
}
