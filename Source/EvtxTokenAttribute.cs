using System.IO;

namespace woanware
{
    /// <summary>
    /// 
    /// </summary>
    internal class EvtxTokenAttribute : EvtxToken
    {
        public EvtxChunk Chunk { get; set; }
        public byte[] NameHash { get; set; }
        public short StringLength { get; set; }
        public string Name { get; set; }
        public bool HasMore { get; set; }
        public int Pointer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evtxChunk"></param>
        public EvtxTokenAttribute(EvtxChunk evtxChunk)
        {
            this.Chunk = evtxChunk;   
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evtxChunk"></param>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        public int Parse(EvtxChunk evtxChunk, MemoryStream memoryStream)
        {
            this.Pointer = StreamReaderHelper.ReadInt32(memoryStream);
            if (evtxChunk.Strings.ContainsKey(Pointer) == false)
            {
                memoryStream.Seek(4, SeekOrigin.Current);

                this.NameHash = StreamReaderHelper.ReadByteArray(memoryStream, 2);
                this.StringLength = StreamReaderHelper.ReadInt16(memoryStream);
                this.Name = Text.ConvertUnicodeToAscii(StreamReaderHelper.ReadString(memoryStream, StringLength * 2));

                memoryStream.Seek(2, SeekOrigin.Current);

                EvtxString evtxString = new EvtxString();
                evtxString.Pointer = Pointer;
                evtxString.Value = this.Name;

                evtxChunk.Strings.Add(evtxString.Pointer, evtxString);

                return 8 + 4 + ((StringLength + 1) * 2);
            }

            return 4;
        }

        /// <summary>
        /// NOT USED
        /// </summary>
        /// <param name="evtxTemplate"></param>
        /// <returns></returns>
        public string Xml(EvtxTemplate evtxTemplate)
        {
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evtxTemplate"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Xml(EvtxTemplate evtxTemplate, string value)
        {
            EvtxString evtxString = this.Chunk.GetString(this.Pointer);

            if (evtxString != null)
            {
                return " " + evtxString.Value + "='" + value + "'";
            }

            return string.Empty;
        }
    }
}
