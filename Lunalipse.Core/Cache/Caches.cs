using Lunalipse.Common.Interfaces.ICache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Lunalipse.Common.Data.Attribute;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Lunalipse.Common.Generic.Cache.CacheInfo;
using System.Collections;

namespace Lunalipse.Core.Cache
{
    public class Caches : ICache
    {
        public string CacheTo<T>(T instance, WinterWrapUp cw) where T : ICachable
        {
            JObject final = new JObject();
            final["name"] = cw.markName;
            final["date"] = cw.createDate;
            final["clean"] = cw.deletable;
            final["ctx"] = WriteNested(instance);
            return final.ToString();
        }

        public T RestoreTo<T>(object ctx) where T : ICachable
        {
            return (T)ReadNested(typeof(T), (JObject)ctx);
        }

        private List<FieldInfo> GetCachableFields(Type type)
        {
            List<FieldInfo> vars = new List<FieldInfo>();
            foreach (FieldInfo fi in type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                if (fi.GetCustomAttribute(typeof(Cachable)) != null)
                {
                    vars.Add(fi);
                }
            }
            return vars;
        }

        private List<object> getListItem(FieldInfo list, object father)
        {
            List<object> items = new List<object>();
            object value = list.GetValue(father);
            int count = (int)list.FieldType.GetProperty("Count").GetValue(value);
            for(int i = 0; i < count; i++)
            {
                object[] index = new object[] { i };
                object item = list.FieldType.GetProperty("Item").GetValue(value, index);
                items.Add(item);
            }
            return items;
        }

        private JProperty WriteToNode(FieldInfo variable,object major)
        {
            return new JProperty(variable.Name, variable.GetValue(major));
        }

        private JObject WriteNested(object obj)
        {
            Type type = obj.GetType();
            JObject level = new JObject();
            if (type.GetInterface("ICachable") == null) return level;
            foreach (FieldInfo fi in GetCachableFields(type))
            {
                if (fi.FieldType.IsGenericType &&
                    fi.FieldType.GetGenericTypeDefinition().Name.Equals("List`1"))
                {
                    JArray ja = new JArray();
                    foreach (object t in getListItem(fi, obj))
                    {
                        Type ty = t.GetType();
                        if (ty.IsClass && !ty.Equals(typeof(String)))
                            ja.Add(WriteNested(t));
                        else
                            ja.Add(t);
                    }
                    level.Add(fi.Name, ja);
                }
                else if (fi.FieldType.IsArray)
                {
                    Array arr = (Array)fi.GetValue(obj);
                    JArray ja = new JArray();
                    for(int i = 0; i < arr.Length; i++)
                    {
                        ja.Add(arr.GetValue(i));
                    }
                    level.Add(fi.Name, ja);
                }
                else if (!fi.FieldType.IsValueType && !fi.FieldType.Equals(typeof(String)))
                    level.Add(new JProperty(fi.Name, WriteNested(fi.FieldType)));
                else
                    level.Add(WriteToNode(fi,obj));
            }
            return level;
        }

        private object ReadNested(Type insType,JObject layer)
        {
            object instance = Activator.CreateInstance(insType);
            foreach(JProperty jp in layer.Properties())
            {
                string name = jp.Name;
                FieldInfo fi = insType.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (jp.Value.Type == JTokenType.Array)
                {
                    Type vartype = fi.FieldType;
                    ArrayList varo = null;
                    IList list = null;
                    bool isGeneric = vartype.IsGenericType && vartype.GetGenericArguments()[0].GetInterface("ICachable") != null;
                    Type elementType = isGeneric ? vartype.GetGenericArguments()[0] : vartype.GetElementType();
                    if (!isGeneric) varo = new ArrayList();
                    else list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));
                    foreach (JToken jo in jp.Value.Children())
                    {
                        if (isGeneric)
                        {
                            list.Add(ReadNested(elementType, (JObject)jo));
                        }
                        else
                        {
                            varo.Add(AppliedValue(jo));
                        }
                    }
                    if (vartype.IsArray)
                        fi.SetValue(instance, varo.ToArray(elementType));
                    else
                        fi.SetValue(instance, list);
                }
                else if (!fi.FieldType.IsValueType && !fi.FieldType.Equals(typeof(String)))
                {
                    fi.SetValue(instance, ReadNested(fi.FieldType, (JObject)jp.Value));
                }
                else
                {
                    fi.SetValue(instance, AppliedValue(jp.Value));
                }
            }
            return instance;
        }

        private object AppliedValue(JToken jp)
        {
            switch (jp.Type)
            {
                case JTokenType.String:
                    return jp.Value<string>();
                case JTokenType.Integer:
                    return jp.Value<int>();
                case JTokenType.Boolean:
                    return jp.Value<bool>();
                case JTokenType.Float:
                    return jp.Value<float>();
                case JTokenType.Null:
                    return null;
                default:
                    return null;
            }
        }
    }
}
