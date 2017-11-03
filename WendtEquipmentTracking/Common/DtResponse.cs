// <copyright>Copyright (c) 2014 SpryMedia Ltd - All Rights Reserved</copyright>
//
// <summary>
// Attributes that can be used for properties in Editor models
// </summary>
using System.Collections.Generic;

namespace WendtEquipmentTracking.App.Common
{
    /// <summary>
    /// DataTables and Editor response object. This object can be used to
    /// construct and contain the data in response to a DataTables or Editor
    /// request before JSON encoding it and sending to the client-side.
    ///
    /// Note that this object uses lowercase property names as this it output
    /// directly to JSON, so the format and parameter names that DataTables and
    /// Editor expect must be used.
    /// </summary>
    public class DtResponse
    {
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
         * Public properties
         */

        public IEnumerable<object> data { get; set; }

        /* Editor parameters */

        /// <summary>
        /// General error message if there is one
        /// </summary>
        public string error { get; set; }

        /// <summary>
        /// List of field errors if one or more fields are in an error state
        /// when validated
        /// </summary>
        public List<FieldError> fieldErrors { get; set; }

        /// <summary>
        /// Id of the newly created row for the create action
        /// </summary>
        public int? id { get; set; }




        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
         * Nested class
         */

        /// <summary>
        /// Editor field error nested class. Describes an error message for a
        /// field if it is in an error state.
        /// </summary>
        public class FieldError
        {
            /// <summary>
            /// Name of the field in error state
            /// </summary>
            public string name { get; set; }

            /// <summary>
            /// Error message
            /// </summary>
            public string status { get; set; }
        }

    }
}
