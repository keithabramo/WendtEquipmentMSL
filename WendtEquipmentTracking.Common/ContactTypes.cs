using System;
using System.Collections.Generic;

namespace WendtEquipmentTracking.Common
{
    public sealed class ContactTypes
    {

        private readonly string name;
        private readonly string value;

        private static readonly Dictionary<ContactTypes, string> instanceToString = new Dictionary<ContactTypes, string>();
        private static readonly Dictionary<string, ContactTypes> stringToInstance = new Dictionary<string, ContactTypes>();

        public static readonly ContactTypes Contractor = new ContactTypes("Contractor", "Contractor");
        public static readonly ContactTypes DEPDistrictContact = new ContactTypes("DEPDistrictContact", "DEP District Contact");
        public static readonly ContactTypes Other1 = new ContactTypes("Other1", "Other 1");
        public static readonly ContactTypes Other2 = new ContactTypes("Other2", "Other 2");
        public static readonly ContactTypes ResponsibleParty = new ContactTypes("ResponsibleParty", "Responsible Party");
        public static readonly ContactTypes SiteOwner = new ContactTypes("SiteOwner", "Site Owner");

        private ContactTypes(string value, string name)
        {
            this.name = name;
            this.value = value;

            instanceToString[this] = value;
            stringToInstance[value] = this;
            stringToInstance[name] = this;
        }

        public override string ToString()
        {
            return name;
        }

        public static IEnumerable<ContactTypes> ToList()
        {
            return instanceToString.Keys;
        }

        public static implicit operator string(ContactTypes step)
        {
            string result = null;
            if (step == null)
            {
                return result;
            }
            else if (instanceToString.ContainsKey(step))
            {
                return instanceToString[step];
            }
            else
            {
                throw new InvalidCastException();
            }
        }

        public static implicit operator ContactTypes(string str)
        {
            ContactTypes result;
            try
            {
                if (stringToInstance.TryGetValue(str, out result))
                    return result;
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
