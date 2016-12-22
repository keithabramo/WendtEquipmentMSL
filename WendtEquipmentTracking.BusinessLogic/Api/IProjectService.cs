﻿using System.Collections.Generic;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.BusinessLogic.Api
{
    public interface IProjectService
    {
        void Save(ProjectBO projectBO);

        void Update(ProjectBO projectBO);

        void Delete(int id);

        IEnumerable<ProjectBO> GetAll();

        ProjectBO GetById(int id);

    }
}