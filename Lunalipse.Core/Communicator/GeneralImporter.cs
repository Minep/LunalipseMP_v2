using Lunalipse.Common.Interfaces.ICommunicator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace Lunalipse.Core.Communicator
{
    public class GeneralImporter<T> : IGeneralImporter<T>
    {
        string Target;
        XmlDocument xdoc;

        public GeneralImporter(string target)
        {
            Target = target;
            xdoc = new XmlDocument();
            XmlReaderSettings xrs = new XmlReaderSettings();
            xrs.IgnoreComments = true;
            XmlReader xr = XmlReader.Create(target, xrs);
            xdoc.Load(xr);
        }

        public T Import()
        {
            try
            {
                XmlNode root = xdoc.ChildNodes[1].FirstChild;
                return (T)ParseXml(root, null);
            }
            catch (Exception e)
            {
                return default(T);
            }
        }

        public string[] GetExtra()
        {
            List<string> info = new List<string>();
            foreach(XmlAttribute xa in xdoc.ChildNodes[1].Attributes)
            {
                info.Add(xa.Name);
                info.Add(xa.Value);
            }
            return info.ToArray();
        }

        public bool Import(T instance, string selector)
        {
            XmlNode root = xdoc.ChildNodes[1].FirstChild;
            object obj = ParseXml(root, null);
            if (obj == null) return false;
            instance.GetType()
                .GetField(selector, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .SetValue(instance, obj);
            return true;
        }

        object ParseXml(XmlNode xn, object father)
        {
            object obj;
            Type t = Type.GetType(xn.Attributes["Type"].Value);
            if (t.IsArray)
                obj = construct(t, xn.ChildNodes.Count);
            else
                obj = construct(t, xn.InnerText);
            if (father == null) father = obj;
            foreach(XmlNode _xn in xn.ChildNodes)
            {
                if (_xn.Attributes == null) continue;
                object _obj = ParseXml(_xn, obj);

                if (t.IsGenericType)
                    (obj as IList).Add(_obj);
                else if (t.IsArray)
                    (obj as Array).SetValue(_obj, int.Parse(_xn.Attributes["Index"].Value));
                else
                    t.GetField(_xn.Attributes["Name"].Value, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                        .SetValue(obj, _obj);
            }
            return obj;
        }

        private object construct(Type type, params object[] appendix)
        {
            if (type.IsValueType) return Convert.ChangeType(appendix[0], type);
            if(type.GetConstructor(Type.EmptyTypes)==null)
            {
                if (type.IsArray)
                    return Activator.CreateInstance(type, appendix);
                return Convert.ChangeType(appendix[0], type);
            }
            return Activator.CreateInstance(type);
        }
    }
}
