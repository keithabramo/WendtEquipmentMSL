using System.Collections.Generic;
using WendtEquipmentTracking.DataAccess.SQL.Specifications.Vendors;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications
{

    public static class VendorSpecs
    {
        public static Specification<Vendor> Id(int id)
        {
            return new IdSpecification(id);
        }

        public static Specification<Vendor> Ids(IEnumerable<int> ids)
        {
            return new IdsSpecification(ids);
        }

        public static Specification<Vendor> IsDeleted()
        {
            return new IsDeletedSpecification();
        }

        public static Specification<Vendor> ProjectId(int projectId)
        {
            return new ProjectIdSpecification(projectId);
        }
    }
}
