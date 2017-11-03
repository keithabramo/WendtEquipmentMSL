using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WendtEquipmentTracking.App.Common
{
    public static class DatatableHelpers
    {
        public static T ToObject<T>(this Dictionary<string, object> source)
            where T : class, new()
        {
            T someObject = new T();
            Type someObjectType = someObject.GetType();

            foreach (KeyValuePair<string, object> item in source)
            {
                if (someObjectType.GetProperty(item.Key).PropertyType == typeof(string))
                {
                    someObjectType.GetProperty(item.Key).SetValue(someObject, item.Value.ToString(), null);
                }
                else
                {
                    if (!string.IsNullOrEmpty(item.Value.ToString()))
                    {
                        someObjectType.GetProperty(item.Key).SetValue(someObject, Convert.ChangeType(item.Value, someObjectType.GetProperty(item.Key).PropertyType), null);
                    }
                }
            }

            return someObject;
        }

        public static Dictionary<string, object> HttpData()
        {
            var form = HttpContext.Current.Request.Form;

            var dataIn = new List<KeyValuePair<string, string>>();

            if (form != null)
            {
                foreach (var key in form.AllKeys)
                {
                    dataIn.Add(new KeyValuePair<string, string>(key, form[key]));
                }
            }



            var dataOut = new Dictionary<string, object>();

            if (dataIn != null)
            {
                foreach (var pair in dataIn)
                {
                    var value = pair.Value;

                    if (pair.Key.Contains('['))
                    {
                        var keys = pair.Key.Split('[');
                        var innerDic = dataOut;
                        string key;

                        for (int i = 0, ien = keys.Count() - 1; i < ien; i++)
                        {
                            key = keys[i].TrimEnd(']');
                            if (key == "")
                            {
                                // If the key is empty it is an array index value
                                key = innerDic.Count().ToString();
                            }

                            if (!innerDic.ContainsKey(key))
                            {
                                innerDic.Add(key, new Dictionary<string, object>());
                            }
                            innerDic = innerDic[key] as Dictionary<string, object>;
                        }

                        key = keys.Last().TrimEnd(']');
                        if (key == "")
                        {
                            key = innerDic.Count().ToString();
                        }

                        innerDic.Add(key, value);
                    }
                    else
                    {
                        dataOut.Add(pair.Key, value);
                    }
                }
            }

            return dataOut;
        }
    }
}