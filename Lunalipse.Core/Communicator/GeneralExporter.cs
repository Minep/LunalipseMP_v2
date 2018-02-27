using Lunalipse.Common.Interfaces.ICommunicator;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace Lunalipse.Core.Communicator
{
    public class GeneralExporter<T> : IGeneralExporter<T>
    {
        string Target;
        XmlDocument xdoc;
        XmlElement root;
        XmlElement xec;

        public GeneralExporter(string target,params string[] ExtraInfo)
        {
            Target = target;
            xdoc = new XmlDocument();
            XmlDeclaration dec = xdoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xdoc.AppendChild(dec);
            root = xdoc.CreateElement("LExport");
            xec = xdoc.CreateElement("Class");
            if (ExtraInfo.Length % 2 == 0)
                for (int i = 0; i < ExtraInfo.Length - 1; i += 2)
                    root.SetAttribute(ExtraInfo[i], ExtraInfo[i + 1]);
        }

        public bool Export(T instance, string selector = null)
        {
            try
            {
                xec.SetAttribute("Type", instance.GetType().FullName);
                if (selector != null) xec.SetAttribute("Selector", selector);
                foreach (FieldInfo fi in instance.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
                {
                    if(selector!=null)
                        if (!fi.Name.Equals(selector)) continue;
                    xec.AppendChild(ParseObj(instance, fi.FieldType, fi));
                }
                root.AppendChild(xec);
                xdoc.AppendChild(root);
                xdoc.Save(Target);
            }
            catch { return false; }
            return true;
        }

        XmlNode ParseObj(object fins, Type t, FieldInfo finf)
        {
            XmlElement xe = xdoc.CreateElement("Field");
            xe.SetAttribute("Name", finf.Name);
            xe.SetAttribute("Type", t.FullName);
            object ins = finf != null ? finf.GetValue(fins) : fins;
            if (t.IsGenericType &&
                t.GetGenericTypeDefinition().Name == "List`1")
            {
                int count = (int)t.GetProperty("Count").GetValue(ins);
                for (int i = 0; i < count; i++)
                {
                    object ListItem = t.GetProperty("Item").GetValue(ins, new object[] { i });
                    if (ListItem.GetType().IsClass)
                    {
                        XmlElement xec = xdoc.CreateElement("Class");
                        xec.SetAttribute("Type", ListItem.GetType().AssemblyQualifiedName);
                        foreach (FieldInfo fi in ListItem.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
                            xec.AppendChild(ParseObj(ListItem, fi.FieldType, fi));
                        xe.AppendChild(xec);
                    }
                }
            }
            //TODO Add support of IDictionary export
            /*else if (t.IsGenericType &&
                t.GetInterface("IDictionary`2") != null)
            {}*/
            else if (t.IsArray)
            {
                Array arr = (Array)ins;
                for (int i = 0; i < arr.Length; i++)
                {
                    object obj = arr.GetValue(i);
                    XmlElement xeu = xdoc.CreateElement("Unit");
                    xeu.SetAttribute("Index", i.ToString());
                    xeu.SetAttribute("Type", obj.GetType().FullName);
                    xeu.InnerText = obj.ToString();
                    xe.AppendChild(xeu);
                }
            }
            else
                xe.InnerText = ins != null ? ins.ToString() : "";
            return xe;
        }

        /*void _getVal<TKey, TValue>(IDictionary<TKey, TValue> idat, XmlElement xe)
        {
            foreach(var p in idat)
            {
                if (p.Value.GetType().IsClass)
                {
                    XmlElement xec = xdoc.CreateElement("Class");
                    xec.SetAttribute("Type", p.Value.GetType().AssemblyQualifiedName);
                    foreach (FieldInfo fi in p.Value.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
                        xec.AppendChild(ParseObj(p.Value, fi.FieldType, fi));
                    xe.AppendChild(xec);
                }
            }
        }*/
    }
}
