// <copyright>Copyright (c) 2014 SpryMedia Ltd - All Rights Reserved</copyright>
//
// <summary>
// DataTables and Editor request model
// </summary>
using System;
using System.Collections.Generic;
using System.Linq;

namespace WendtEquipmentTracking.App.Common
{
    /// <summary>
    /// Representation of a DataTables or Editor request. This can be any form
    /// of request from the two libraries, including a standard DataTables get,
    /// a server-side processing request, or an Editor create, edit or delete
    /// command.
    /// </summary>
    public class DtRequest
    {
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
         * Static methods
         */

        /// <summary>
        /// Convert HTTP request data, in the standard HTTP parameter form
        /// submitted by jQuery into a generic dictionary of string / object
        /// pairs so the data can easily be accessed in .NET.
        ///
        /// This static method is generic and not specific to the DtRequest. It
        /// may be used for other data formats as well.
        /// 
        /// Note that currently this does not support nested arrays or objects in arrays
        /// </summary>
        /// <param name="dataIn">Collection of HTTP parameters sent by the client-side</param>
        /// <returns>Dictionary with the data and values contained. These may contain nested lists and dictionaries.</returns>
        public static Dictionary<string, object> HttpData(IEnumerable<KeyValuePair<string, string>> dataIn)
        {
            var dataOut = new Dictionary<string, object>();

            if (dataIn != null)
            {
                foreach (var pair in dataIn)
                {
                    var value = _HttpConv(pair.Value);

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



        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
         * Public parameters
         */

        /// <summary>
        /// Type of request this instance contains the data for
        /// </summary>
        public RequestTypes RequestType;

        /// <summary>
        /// Request type values
        /// </summary>
        public enum RequestTypes
        {
            /// <summary>
            /// Editor create request
            /// </summary>
            EditorCreate,

            /// <summary>
            /// Editor edit request
            /// </summary>
            EditorEdit,

            /// <summary>
            /// Editor remove request
            /// </summary>
            EditorRemove,


        };



        /* Editor parameters */

        /// <summary>
        /// Editor action request
        /// </summary>
        public string Action;

        /// <summary>
        /// Dictionary of data sent by Editor (may contain nested data)
        /// </summary>
        public Dictionary<string, object> Data;

        /// <summary>
        /// List of ids for Editor to operate on
        /// </summary>
        public List<string> Ids = new List<string>();




        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
         * Constructor
         */

        /// <summary>
        /// Convert an HTTP request submitted by the client-side into a
        /// DtRequest object
        /// </summary>
        /// <param name="rawHttp">Data from the client-side</param>
        public DtRequest(IEnumerable<KeyValuePair<string, string>> rawHttp)
        {
            var http = HttpData(rawHttp);

            if (http.ContainsKey("action"))
            {
                // Editor request
                Action = http["action"] as string;

                if (Action == "create")
                {
                    RequestType = RequestTypes.EditorCreate;
                    Data = http["data"] as Dictionary<string, object>;
                }
                else if (Action == "edit")
                {
                    RequestType = RequestTypes.EditorEdit;
                    Data = http["data"] as Dictionary<string, object>;
                }
                else if (Action == "remove")
                {
                    RequestType = RequestTypes.EditorRemove;
                    Data = http["data"] as Dictionary<string, object>;
                }
            }
        }



        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
         * Private functions
         */

        private static object _HttpConv(string dataIn)
        {
            // Boolean
            if (dataIn == "true")
            {
                return true;
            }
            if (dataIn == "false")
            {
                return false;
            }

            // Numeric looking data, but with leading zero
            if (dataIn.IndexOf('0') == 0 && dataIn.Length > 1)
            {
                return dataIn;
            }

            try
            {
                return Convert.ToInt32(dataIn);
            }
            catch (Exception) { }

            try
            {
                return Convert.ToDecimal(dataIn);
            }
            catch (Exception) { }

            return dataIn;
        }



    }
}
