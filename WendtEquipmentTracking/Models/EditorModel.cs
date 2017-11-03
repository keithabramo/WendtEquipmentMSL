using System.Collections.Generic;

namespace WendtEquipmentTracking.App.Models
{
    public class EditorModel
    {
        /// <summary>
        /// Dictionary of data sent by Editor (may contain nested data)
        /// </summary>
        public object Data;

        /// <summary>
        /// Designates which action request was made (add/edit/delete etc)
        /// </summary>
        public string Action { get; set; }

    }
}