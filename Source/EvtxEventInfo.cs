using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using Microsoft.Win32;
using System.Security;
using System.Runtime.InteropServices;

namespace woanware
{
    /// <summary>
    /// 
    /// </summary>
    public class EvtxEventInfo
    {
        #region Member Variables
        public string ProviderName { get; private set; }
        public string ProviderGuid { get; private set; }
        public long? EventId { get; private set; }
        public int? Version { get; private set; }
        public int? Level { get; private set; }
        public int? Task { get; private set; }
        public DateTime? TimeCreated { get; private set; }
        public int? ProcessId { get; private set; }
        public int? ThreadId { get; private set; }
        public string Channel { get; private set; }
        public string Computer { get; private set; }
        public string Sid { get; private set; }
        public string Xml { get; set; }
        public string Description {get; private set;}
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="registryFile"></param>
        /// <param name="drive"></param>
        /// <param name="resourceMode"></param>
        public EvtxEventInfo(string xml,
                             string registryFile, 
                             string drive, 
                             EvtxParser.ResourceMode resourceMode)
        {
            Xml = xml;

            XmlDocument xmlDocument = new XmlDocument();

            byte[] data = System.Text.Encoding.ASCII.GetBytes(xml);

            MemoryStream memoryStream = new MemoryStream(data);
            xmlDocument.Load(memoryStream);

            ProviderName = GetStringNodeAttribute(xmlDocument, "Provider", "Name");
            ProviderGuid = GetStringNodeAttribute(xmlDocument, "Provider", "Guid");
            EventId = GetLongNodeValue(xmlDocument, "EventID");
            Version = GetIntNodeValue(xmlDocument, "Version");
            Level = GetIntNodeValue(xmlDocument, "Level");
            Task = GetIntNodeValue(xmlDocument, "Task");
            TimeCreated = GetDateTimeNodeAttribute(xmlDocument, "TimeCreated", "SystemTime");
            ProcessId = GetIntNodeAttribute(xmlDocument, "Execution", "ProcessID");
            ThreadId = GetIntNodeAttribute(xmlDocument, "Execution", "ThreadID");
            Channel = GetStringNodeValue(xmlDocument, "Channel");
            Computer = GetStringNodeValue(xmlDocument, "Computer");
            Sid = GetStringNodeAttribute(xmlDocument, "Security", "UserID");

            LoadMessageResources(registryFile, drive, resourceMode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="registryFile"></param>
        /// <param name="drive"></param>
        /// <param name="resourceMode"></param>
        private void LoadMessageResources(string registryFile,
                                          string drive,
                                          EvtxParser.ResourceMode resourceMode)
        {
            //string resourceFileName = string.Empty;
            string messageFileName = string.Empty;

            if (resourceMode == EvtxParser.ResourceMode.LocalSystem)
            {
                try
                {
                    RegistryKey registryKey;
                    if (Environment.Is64BitOperatingSystem == true)
                    {
                        registryKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
                    }
                    else
                    {
                        registryKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry32);
                    }

                    if (registryKey == null)
                    {
                        return;
                    }

                    registryKey = registryKey.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\WINEVT\Publishers\" + ProviderGuid);
                    if (registryKey == null)
                    {
                        return;
                    }

                    string ret = registryKey.GetValue("MessageFileName").ToString();
                    if (ret == null)
                    {
                        return;
                    }
                    else
                    {
                        messageFileName = ret.ToString();
                    }
                }
                catch (Exception ex)
                {
                    return;
                }
            }
            else
            {
                RegParser regParser = new RegParser(registryFile);

                RegKey rootKey = regParser.RootKey;

                RegKey regKey = rootKey.Key("Microsoft\\Windows NT\\CurrentVersion\\WINEVT\\Publishers\\" + ProviderGuid);

                if (regKey == null)
                {
                    return;
                }

                RegValue regValue = regKey.Value("MessageFileName");
                if (regValue == null)
                {
                    return;
                }

                messageFileName = Helper.ReplaceNulls(regValue.Data.ToString());
            }

            if (messageFileName.Length == 0)
            {
                return;
            }

            int temp1  = this.HighWord((int)EventId);
            int temp2 = this.LowWord((int)EventId);

            string message = this.GetMessageTableString(messageFileName,(uint)EventId);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, uint dwFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int LoadString(IntPtr hInstance, uint uId, StringBuilder lpBuffer, int nBufferMax);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern int FormatMessageW(uint dwFormatFlags, IntPtr lpSource, uint dwMessageId, int dwLanguageId, out IntPtr MsgBuffer, int nSize, IntPtr Arguments);

        [DllImport("kernel32.dll")]
        private static extern bool FreeLibrary(IntPtr hLibrary);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resFilePath"></param>
        /// <param name="messageIndex"></param>
        /// <returns></returns>
        public  string GetMessageTableString(string resFilePath,
                                                   uint messageIndex)
        {
            try
            {
                const uint FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100;
                const uint FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200;
                const uint FORMAT_MESSAGE_FROM_HMODULE = 0x00000800;
                const uint FORMAT_MESSAGE_ARGUMENT_ARRAY = 0x00002000;

                IntPtr pMessageBuffer;

                uint dwFormatFlags = FORMAT_MESSAGE_FROM_HMODULE | FORMAT_MESSAGE_IGNORE_INSERTS | FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_ARGUMENT_ARRAY;

                IntPtr hInst = LoadLibraryEx(resFilePath, IntPtr.Zero, 0x00000002);

                int dwBufferLength = FormatMessageW(dwFormatFlags, hInst, messageIndex, 0, out pMessageBuffer, 0, IntPtr.Zero);
     
                
                // Get the last error and display it.
                

                string message = string.Empty;
                if (dwBufferLength != 0)
                {
                    message = Marshal.PtrToStringUni(pMessageBuffer);
                    Marshal.FreeHGlobal(pMessageBuffer);
                }
                else
                {
                    string errorMsg = new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error()).Message;
                }

                FreeLibrary(hInst);

                return message;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public  int LowWord(int number)
        { return number & 0x0000FFFF; }

        public  int HighWord( int number)
        { return (number & (0xFFFF << 16)); }

//        public static UIntPtr HKEY_LOCAL_MACHINE = new UIntPtr(0x80000002u);
//        public static UIntPtr HKEY_CURRENT_USER = new UIntPtr(0x80000001u);

//        string example = ReadRegKey(HKEY_LOCAL_MACHINE, @"Software\SomeCompany\OurProduct", "InstalledVersion");

//        [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
//        public static extern int RegOpenKeyEx(
//          UIntPtr hKey,
//          string subKey,
//          int ulOptions,
//          int samDesired,
//          out UIntPtr hkResult);

//        private const int KEY_QUERY_VALUE = &H1;
//Const KEY_SET_VALUE = &H2
//Const KEY_CREATE_SUB_KEY = &H4
//Const KEY_ENUMERATE_SUB_KEYS = &H8
//Const KEY_NOTIFY = &H10
//Const KEY_CREATE_LINK = &H20
//Const KEY_WOW64_32KEY = &H200
//Const KEY_WOW64_64KEY = &H100
//Const KEY_WOW64_RES = &H300

//        private static string ReadRegKey(UIntPtr rootKey, string keyPath, string valueName)
//        {
//            if (RegOpenKeyEx(rootKey, keyPath, 0, KEY_READ, out hKey) == 0)
//            {
//                uint size = 1024;
//                uint type;
//                string keyValue = null;
//                StringBuilder keyBuffer = new StringBuilder();

//                if (RegQueryValueEx(hKey, valueName, IntPtr.Zero, out type, keyBuffer, ref size) == 0)
//                    keyValue = keyBuffer.ToString();

//                RegCloseKey(hKey);

//                return (keyValue);
//            }

//            return (null);  // Return null if the value could not be read
//        }

        #region XML Node Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        private long? GetLongNodeValue(XmlDocument xmlDocument, string tagName)
        {
            try
            {
                XmlNodeList nodes = xmlDocument.GetElementsByTagName(tagName);
                if (nodes.Count == 1)
                {
                    return long.Parse(nodes[0].InnerText);
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        private int? GetIntNodeValue(XmlDocument xmlDocument, string tagName)
        {
            try
            {
                XmlNodeList nodes = xmlDocument.GetElementsByTagName(tagName);
                if (nodes.Count == 1)
                {
                    return int.Parse(nodes[0].InnerText);
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        private string GetStringNodeValue(XmlDocument xmlDocument,
                                          string tagName)
        {
            try
            {
                XmlNodeList nodes = xmlDocument.GetElementsByTagName(tagName);
                if (nodes.Count == 1)
                {
                    return nodes[0].InnerText;
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        private string GetStringNodeAttribute(XmlDocument xmlDocument,
                                              string tagName,
                                              string attributeName)
        {
            try
            {
                XmlNodeList nodes = xmlDocument.GetElementsByTagName(tagName);
                if (nodes.Count == 1)
                {
                    return nodes[0].Attributes[attributeName].InnerText;
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        private int? GetIntNodeAttribute(XmlDocument xmlDocument,
                                         string tagName,
                                         string attributeName)
        {
            try
            {
                XmlNodeList nodes = xmlDocument.GetElementsByTagName(tagName);
                if (nodes.Count == 1)
                {
                    return int.Parse(nodes[0].Attributes[attributeName].InnerText);
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        private DateTime? GetDateTimeNodeAttribute(XmlDocument xmlDocument,
                                                   string tagName,
                                                   string attributeName)
        {
            try
            {
                XmlNodeList nodes = xmlDocument.GetElementsByTagName(tagName);
                if (nodes.Count == 1)
                {
                    return DateTime.Parse(nodes[0].Attributes[attributeName].InnerText);
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
    }
}
