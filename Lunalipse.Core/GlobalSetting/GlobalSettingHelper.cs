using Lunalipse.Common.Data.Attribute;
using Lunalipse.Common.Data.Errors;
using Lunalipse.Common.Interfaces.ISetting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lunalipse.Core.GlobalSetting
{
    public class GlobalSettingHelper<GS> : ISettingHelper<GS> where GS : IGlobalSetting
    {
        static volatile GlobalSettingHelper<GS> GSH_INSTANCE;
        static readonly object GSH_LOCK = new object();

        public static GlobalSettingHelper<GS> INSTANCE
        {
            get
            {
                if (GSH_INSTANCE == null)
                {
                    lock (GSH_LOCK)
                    {
                        GSH_INSTANCE = GSH_INSTANCE ?? new GlobalSettingHelper<GS>();
                    }
                }
                return GSH_INSTANCE;
            }
        }

        string VERSION;
        public GlobalSettingHelper()
        {
            VERSION = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public bool ReadSetting(string path)
        {
            try
            {
                Type t = typeof(GS);
                XmlDocument xd = new XmlDocument();
                XmlReaderSettings xrs = new XmlReaderSettings();
                xrs.IgnoreComments = true;
                XmlReader xr = XmlReader.Create(path, xrs);
                xd.Load(xr);
                XmlNode xN = xd.SelectSingleNode("Lunalipse");
                if(xN.Attributes["version"] == null)
                {
                    ErrorDelegation.OnErrorRaisedGSH?.Invoke(ErrorGSH.VERSION_NOT_FOUND, "Veriosn Not Found");
                    xr.Close();
                    return false;
                }
                if (!xN.Attributes["version"].Value.Equals(VERSION))
                {
                    ErrorDelegation.OnErrorRaisedGSH?.Invoke(ErrorGSH.VERSION_UNMATCH, xN.Attributes["version"].Value, VERSION);
                }
                foreach (XmlNode xn in xN.ChildNodes)
                {
                    ParseFile(xn, ref t);
                }
                xr.Close();
                return true;
            }
            catch(FileNotFoundException)
            {
                return false;
            }
        }

        public void SaveSetting()
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("Lunalipse");
            root.SetAttribute("version", VERSION);
            foreach(FieldInfo fi in typeof(GS).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (fi.GetCustomAttribute(typeof(ExportedSettingItem)) != null)
                {
                    root.AppendChild(GenerateNode(fi, doc));
                }
            }
            doc.AppendChild(root);
            doc.Save("config.lps");
        }

        private XmlElement GenerateNode(FieldInfo fi,XmlDocument doc)
        {
            XmlElement xe = doc.CreateElement("Field");
            xe.SetAttribute("Name", fi.Name);
            xe.SetAttribute("Type", fi.FieldType.ToString());
            if(fi.FieldType.IsArray)
            {
                Array a = (Array)fi.GetValue(null);
                for(int i = 0; i < a.Length; i++)
                {
                    XmlElement xe_sub = doc.CreateElement("Item");
                    xe_sub.SetAttribute("index", i.ToString());
                    xe_sub.InnerText = a.GetValue(i).ToString();
                    xe.AppendChild(xe_sub);
                }
            }
            else
            {
                xe.InnerText = fi.GetValue(null).ToString();
            }
            return xe;
        }

        private void ParseFile(XmlNode node, ref Type type)
        {
            XmlElement xe = (XmlElement)node;
            string name, dtype;
            if (xe.Attributes["Name"] == null || xe.Attributes["Type"] == null)
            {
                ErrorDelegation.OnErrorRaisedGSH?.Invoke(ErrorGSH.DAMAGED_FIELD, xe.OuterXml);
                return;
            }
            else
            {
                name = xe.Attributes["Name"].Value;
                dtype = xe.Attributes["Type"].Value;
            }
            if (xe.ChildNodes.Count == 0)   // Single Field
            {
                object o;
                if ((o = FromType(xe.InnerText, dtype)) == null)
                {
                    ErrorDelegation.OnErrorRaisedGSH?.Invoke(ErrorGSH.INVALID_VALUE, dtype, xe.InnerText);
                }
                else
                {
                    type.GetField(name,
                        BindingFlags.Static | BindingFlags.Public)
                            .SetValue(null, o);
                }
            }
            else if(xe.ChildNodes.Count > 0)
            {
                Type array = Type.GetType(dtype);
                if (!array.IsArray)
                {
                    ErrorDelegation.OnErrorRaisedGSH?.Invoke(ErrorGSH.INVALID_TYPE, dtype, xe.OuterXml);
                    return;
                }
                Array arr = (Array)Activator.CreateInstance(array, new object[] { xe.ChildNodes.Count });
                foreach(XmlNode xn in xe.ChildNodes)
                {
                    if(xn.Attributes["index"] == null)
                    {
                        ErrorDelegation.OnErrorRaisedGSH?.Invoke(ErrorGSH.INVALID_INDEX, xe.OuterXml);
                        continue;
                    }
                    arr.SetValue(FromType(xn.InnerText, array.GetElementType()), int.Parse(xn.Attributes["index"].Value));
                }
                type.GetField(name, BindingFlags.Static | BindingFlags.Public)
                    .SetValue(null, arr);
            }
        }

        private object FromType(string src, string type)
        {
            try
            {
                return Convert.ChangeType(src, Type.GetType(type, true, false));
            }
            catch(TypeLoadException)
            {
                //GSettingDelegation.OnErrorRaisedGSH?.Invoke(ErrorGSH.INVALID_TYPE, src);
            }
            return null;
        }

        private object FromType(string src, Type type)
        {
            try
            {
                return Convert.ChangeType(src, type);
            }
            catch (InvalidCastException)
            {
                //GSettingDelegation.OnErrorRaisedGSH?.Invoke(ErrorGSH.INVALID_TYPE, src);
            }
            return null;
        }
    }
}
