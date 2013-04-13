using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using NLog;

namespace woanware
{
    /// <summary>
    /// 
    /// </summary>
    public class EvtxParser
    {
        /// <summary>
        /// 
        /// </summary>
        public enum ResourceMode
        {
            None = 0,
            LocalSystem = 1,
            TargetSystem = 2
        }

        #region Public Events
        public delegate void NewEventHandler(long filePk, EvtxEventInfo evtxEventInfo);
        public event NewEventHandler NewEvent;

        public delegate void ErrorHandler(string text);
        public event ErrorHandler Error;

        public delegate void CompleteEventHandler();
        public event CompleteEventHandler Complete;
        #endregion

        private FileStream _fileStream = null;
        private EvtxFile EvtxFile { get; set; }
        private static Logger _logger = null;

        /// <summary>
        /// 
        /// </summary>
        public EvtxParser()
        {
            _logger = LogManager.GetLogger("EvtxParser");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePk"></param>
        /// <param name="filePath"></param>
        /// <param name="registryFile"></param>
        /// <param name="drive"></param>
        /// <param name="resourceMode"></param>
        public void Parse(long filePk,
                          string filePath, 
                          string registryFile, 
                          string drive, 
                          ResourceMode resourceMode)
        {
            MethodInvoker methodInvoker = delegate
            {
                _logger.Info("Parsing file: " + filePath);

                EvtxFile = new EvtxFile();
                EvtxFile.Path = filePath;
                EvtxFile.FilePk = filePk;
                EvtxFile.RegistryFile = registryFile;
                EvtxFile.Drive = drive;
                EvtxFile.ResourceMode = resourceMode;

                string ret = EvtxFile.LoadSystemRoot();
                if (ret.Length > 0)
                {
                    OnError(ret);
                    return;
                }

                _fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                this.ParseFileHeader();
                this.ParseChunks();

                for (int index = 0; index < EvtxFile.Chunks.Count; index++)
                {
                    _logger.Info("Parsing chunk no.: " + index + 1);

                    this.ReadEvents(EvtxFile.Chunks[index]);

                    for (int indexEvt = 0; indexEvt < EvtxFile.Chunks[index].Events.Count; indexEvt++)
                    {
                        _logger.Info("Parsing event no.: " + indexEvt + 1);

                        EvtxEvent evtxEvent = EvtxFile.Chunks[index].Events[indexEvt];

                        evtxEvent.BXml.Parse();

                        XmlGenerator xmlGenerator = new XmlGenerator(EvtxFile.Chunks[index], EvtxFile.Chunks[index].Events[indexEvt]);
                        evtxEvent.Xml = xmlGenerator.Generate();

                        EvtxEventInfo evtxEventInfo = new EvtxEventInfo(evtxEvent.Xml, EvtxFile.RegistryFile, EvtxFile.Drive, EvtxFile.ResourceMode);
                        OnNewEvent(EvtxFile.FilePk, evtxEventInfo);
                    }
                }

                OnComplete();
            };

            methodInvoker.BeginInvoke(null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        private void ParseFileHeader()
        {
            EvtxFile.Signature = StreamReaderHelper.ReadString(_fileStream, 8).Trim('\0');

            _logger.Info("File signature: " + EvtxFile.Signature);

            _fileStream.Seek(8, SeekOrigin.Current);

            EvtxFile.CurrentChunkNum = StreamReaderHelper.ReadInt64(_fileStream);
            EvtxFile.NextRecordNum = StreamReaderHelper.ReadInt64(_fileStream);
            EvtxFile.HeaderPart1Len = StreamReaderHelper.ReadUInt32(_fileStream);
            EvtxFile.MinorVersion = StreamReaderHelper.ReadUInt16(_fileStream);
            EvtxFile.MajorVersion = StreamReaderHelper.ReadUInt16(_fileStream);
            EvtxFile.HeaderSize = StreamReaderHelper.ReadUInt16(_fileStream);
            EvtxFile.ChunkCount = StreamReaderHelper.ReadUInt16(_fileStream);

            _logger.Info("Chunk count: " + EvtxFile.ChunkCount);

            _fileStream.Seek(76, SeekOrigin.Current);

            EvtxFile.Flags = StreamReaderHelper.ReadUInt32(_fileStream);
            EvtxFile.CheckSum = StreamReaderHelper.ReadUInt32(_fileStream);

            _logger.Info("File checksum: " + EvtxFile.CheckSum);

            byte[] data = new byte[120];

            _fileStream.Seek(0, SeekOrigin.Begin);
            _fileStream.Read(data, 0, 120);

            Crc32 crc32 = new Crc32();
            crc32.ComputeHash(data);

            EvtxFile.CalculatedCheckSum = crc32.CrcValue;

            _logger.Info("Calculated file checksum: " + EvtxFile.CalculatedCheckSum);

            if ((EvtxFile.Flags & 1) == 1)
            {
                EvtxFile.IsDirty = true;
            }

            if ((EvtxFile.Flags & 2) == 2)
            {
                EvtxFile.IsFull = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ParseChunks()
        {
            _fileStream.Seek(4096, SeekOrigin.Begin);

            do
            {
                long currentPosition = _fileStream.Position;

                if (currentPosition + 65536 > _fileStream.Length)
                {
                    break;
                }

                EvtxChunk chunk = new EvtxChunk(currentPosition);
                chunk.Data = StreamReaderHelper.ReadByteArray(_fileStream, 65536);

                _fileStream.Seek(currentPosition, SeekOrigin.Begin);

                byte[] data = new byte[8];

                _fileStream.Read(data, 0, 8);

                string chunkSig = Text.ByteArray2String(data, false, false);
                if (chunkSig != "ElfChnk\0")
                {
                    _logger.Info("Invalid chunk signature: " + chunkSig);
                    break;
                }

                chunk.NumLogRecFirst = StreamReaderHelper.ReadInt64(_fileStream);
                chunk.NumLogRecLast = StreamReaderHelper.ReadInt64(_fileStream);
                chunk.NumFileRecFirst = StreamReaderHelper.ReadInt64(_fileStream);
                chunk.NumFileRecLast = StreamReaderHelper.ReadInt64(_fileStream);

                chunk.OfsTables = StreamReaderHelper.ReadUInt32(_fileStream);
                chunk.OfsRecLast = StreamReaderHelper.ReadUInt32(_fileStream);
                chunk.OfsRecNext = StreamReaderHelper.ReadUInt32(_fileStream);

                _fileStream.Seek(72, SeekOrigin.Current);


                //byte[] temp2 = StreamReaderHelper.ReadByteArray(_fileStream, 4);
                //Array.Reverse(temp2);

                //UInt32 ttt = BitConverter.ToUInt32(temp2, 0);
                chunk.Checksum = StreamReaderHelper.ReadUInt32(_fileStream);

                _logger.Info("Chunk checksum: " + chunkSig);

                //for (int index = 0; index < 64; index++)
                //{
                //    _fileStream.Read(data, 0, 4);
                //    chunk.StringTable[index] = BitConverter.ToUInt32(data, 0);
                //}

                //for (int index = 0; index < 32; index++)
                //{
                //    _fileStream.Read(data, 0, 4);
                //    chunk.TemplateTable[index] = BitConverter.ToUInt32(data, 0);
                //}

                data = new byte[120];

                List<byte> temp = new List<byte>();
                _fileStream.Seek(currentPosition, SeekOrigin.Begin);
                _fileStream.Read(data, 0, 120);

                temp.AddRange(data);

                _fileStream.Seek(8, SeekOrigin.Current);

                data = new byte[384];
                _fileStream.Read(data, 0, 384);

                temp.AddRange(data);

                Crc32 crc32 = new Crc32();
                crc32.ComputeHash(temp.ToArray());

                chunk.CalculatedCheckSum = crc32.CrcValue;

                _logger.Info("Calculated chunk checksum: " + chunk.CalculatedCheckSum);

                if (chunk.Offset + 65536 > _fileStream.Length)
                {
                    break;
                }

                _fileStream.Seek(chunk.Offset + 65536, SeekOrigin.Begin);

                EvtxFile.Chunks.Add(chunk);

                _logger.Info("Added chunk:" + EvtxFile.Chunks.Count);
            }
            while (_fileStream.Position < _fileStream.Length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chunk"></param>
        /// <returns></returns>
        private void ReadEvents(EvtxChunk chunk)
        {
            _fileStream.Seek(chunk.Offset + 0x200, SeekOrigin.Begin);

            do
            {
                long eventStart = _fileStream.Position - chunk.Offset;
                long currentPosition = _fileStream.Position;

                if (eventStart > chunk.OfsRecNext)
                {
                    return;    
                }

                byte[] data = new byte[4];

                _fileStream.Read(data, 0, 4);

                string chunkSig = Text.ByteArray2String(data, false, false);
                if (chunkSig != "**\0\0")
                {
                    _logger.Info("Invalid event signature: " + chunkSig);
                    continue;
                }

                EvtxEvent evtxEvent = new EvtxEvent(chunk, currentPosition, eventStart);

                evtxEvent.Length1 = StreamReaderHelper.ReadUInt32(_fileStream);

                long tempPosition = _fileStream.Position;

                _fileStream.Seek(currentPosition + (evtxEvent.Length1 - 4), SeekOrigin.Begin);

                evtxEvent.Length2 = StreamReaderHelper.ReadUInt32(_fileStream);

                if (evtxEvent.Length1 != evtxEvent.Length2)
                {
                    return;
                }

                _fileStream.Seek(tempPosition, SeekOrigin.Begin);

                evtxEvent.NumLogRecord = StreamReaderHelper.ReadInt64(_fileStream);
                evtxEvent.TimeCreated = StreamReaderHelper.ReadDateTime(_fileStream);

                int length = (int)evtxEvent.Length1 - 28;
                evtxEvent.BXml.Data = StreamReaderHelper.ReadByteArray(_fileStream, length);

                chunk.Events.Add(evtxEvent);

                _logger.Info("Added event:" + chunk.Events.Count);
            }
            while (_fileStream.Position < _fileStream.Length);
        }

        #region Event Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePk"></param>
        /// <param name="evtxEventInfo"></param>
        private void OnNewEvent(long filePk, EvtxEventInfo evtxEventInfo)
        {
            if (NewEvent != null)
            {
                NewEvent(filePk, evtxEventInfo);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        private void OnError(string text)
        {
            if (Error != null)
            {
                Error(text);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnComplete()
        {
            if (Complete != null)
            {
                Complete();
            }
        }
        #endregion
    }
}
