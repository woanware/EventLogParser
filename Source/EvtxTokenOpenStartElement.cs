using System;
using System.IO;

namespace woanware
{
    /// <summary>
    /// 
    /// </summary>
    internal class EvtxTokenOpenStartElement : EvtxToken
    {
        private EvtxChunk _evtxChunk = null;
        public bool HasElements { get; set; }
        public int Length { get; set; }
        public byte[] NameHash { get; set; }
        public short StringLength { get; set; }
        public string Name { get; set; }
        public short DependancyId { get; set; }
        public long EofStream { get; set; }
        public int Pointer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evtxChunk"></param>
        public EvtxTokenOpenStartElement(EvtxChunk evtxChunk)
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
            // 2 + 4 + 4 + 4 (Seek)
            this.DependancyId = StreamReaderHelper.ReadInt16(memoryStream);

            int length = StreamReaderHelper.ReadInt32(memoryStream);

            this.EofStream = memoryStream.Position + length;
            this.Pointer = StreamReaderHelper.ReadInt32(memoryStream);

            if (evtxChunk.Strings.ContainsKey(this.Pointer) == false)
            {
                memoryStream.Seek(4, SeekOrigin.Current);

                this.NameHash = StreamReaderHelper.ReadByteArray(memoryStream, 2);
                this.StringLength = StreamReaderHelper.ReadInt16(memoryStream);
                this.Name = Text.ConvertUnicodeToAscii(StreamReaderHelper.ReadString(memoryStream, StringLength * 2));

                EvtxString evtxString = new EvtxString();
                evtxString.Pointer = this.Pointer;
                evtxString.Value = this.Name;

                evtxChunk.Strings.Add(evtxString.Pointer, evtxString);

                if (this.HasElements == true)
                {
                    // 6 == 4 for attribute list length and 2 for null string terminator
                    memoryStream.Seek(6, SeekOrigin.Current);
                    return 0;
                }
                else
                {
                    // 2 == Null string terminator
                    memoryStream.Seek(2, SeekOrigin.Current);
                    return 12 + 4 + (StringLength * 2) + 2;
                }
            }
            else
            {
                if (this.HasElements == true)
                {
                     memoryStream.Seek(4, SeekOrigin.Current);
                    return 0;
                }
                else
                {
                    return 0;
                }
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Xml(EvtxTemplate evtxTemplate)
        {
            EvtxString evtxString = _evtxChunk.GetString(this.Pointer);

            if (evtxString != null)
            {
                return Environment.NewLine + "<" + evtxString.Value;
            }

            return string.Empty;
        }
    }
}
