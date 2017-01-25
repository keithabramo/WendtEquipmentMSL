﻿using System.Collections.Generic;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.BusinessLogic.Api
{
    public interface IEquipmentService
    {
        int Save(EquipmentBO equipmentBO);
        void SaveAll(IEnumerable<EquipmentBO> equipmentBO);

        void Update(EquipmentBO equipmentBO);

        void UpdateAll(IEnumerable<EquipmentBO> equipmentBO);

        void Delete(int id);

        IEnumerable<EquipmentBO> GetAll();
        IEnumerable<EquipmentBO> GetSome(int projectId, int skip, int take);
        IEnumerable<EquipmentBO> GetByBillOfLadingId(int billOfLadingId);
        IEnumerable<EquipmentBO> GetHardwareByShippingTagNumber(int projectId, string shipTagNumber);

        EquipmentBO GetById(int id);

    }
}