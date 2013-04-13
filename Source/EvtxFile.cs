using System;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Security;

namespace woanware
{
    /// <summary>
    /// 
    /// </summary>
    internal class EvtxFile
    {
        #region Member Variables
        public string Path { get; set; }
        public long FilePk { get; set; }
        public string RegistryFile { get; set; }
        public string Drive { get; set; }
        public EvtxParser.ResourceMode ResourceMode { get; set; }
        public string SystemRoot { get; private set; }
        public string Signature { get; set; }
        public Int64 CurrentChunkNum { get; set; }
        public Int64 NextRecordNum { get; set; }
        public UInt32 HeaderPart1Len { get; set; }
        public UInt16 MinorVersion { get; set; }
        public UInt16 MajorVersion { get; set; }
        public UInt16 HeaderSize { get; set; }
        public UInt16 ChunkCount { get; set; }
        public UInt32 Flags { get; set; }
        public UInt32 CheckSum { get; set; }
        public UInt32 CalculatedCheckSum { get; set; }
        public bool IsDirty { get; set; }
        public bool IsFull { get; set; }
        public List<EvtxChunk> Chunks { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public EvtxFile()
        {
            Chunks = new List<EvtxChunk>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string LoadSystemRoot()
        {
            if (ResourceMode == EvtxParser.ResourceMode.LocalSystem)
            {
                try
                {
                    RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion");
                    if (registryKey == null)
                    {
                        return "Unable to locate registry key: Microsoft\\Windows NT\\CurrentVersion";
                    }

                    string ret = registryKey.GetValue("SystemRoot").ToString();
                    if (ret == null)
                    {
                        return "Unable to locate registry value: SystemRoot";
                    }
                    else
                    {
                        SystemRoot = ret.ToString();
                    }
                }
                catch (SecurityException)
                {
                    return "Unable to retrieve registry key: Microsoft\\Windows NT\\CurrentVersion\\SystemRoot due to security permissions";
                }
                catch (Exception)
                {
                    return "Unable to retrieve registry key: Microsoft\\Windows NT\\CurrentVersion\\SystemRoot";
                }
            }
            else
            {
                RegParser regParser = new RegParser(RegistryFile);

                RegKey rootKey = regParser.RootKey;

                RegKey regKey = rootKey.Key("Microsoft\\Windows NT\\CurrentVersion");

                if (regKey == null)
                {
                    return "Unable to locate registry key: Microsoft\\Windows NT\\CurrentVersion";
                }

                RegValue regValue = regKey.Value("SystemRoot");
                if (regValue == null)
                {
                    return "Unable to locate registry value: SystemRoot";
                }

                SystemRoot = Helper.ReplaceNulls(regValue.Data.ToString());

                return string.Empty;
            }

            return string.Empty;
        }
        #endregion
    }
}
