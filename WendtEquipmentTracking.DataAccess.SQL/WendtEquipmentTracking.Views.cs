//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Data.Entity.Infrastructure.MappingViews;

[assembly: DbMappingViewCacheTypeAttribute(
    typeof(WendtEquipmentTracking.DataAccess.SQL.WendtEquipmentTrackingEntities),
    typeof(Edm_EntityMappingGeneratedViews.ViewsForBaseEntitySetsda7e9ad7d998b7e291da2d5afe6cefcbed480398d1847b906d669998b2a6afd7))]

namespace Edm_EntityMappingGeneratedViews
{
    using System;
    using System.CodeDom.Compiler;
    using System.Data.Entity.Core.Metadata.Edm;

    /// <summary>
    /// Implements a mapping view cache.
    /// </summary>
    [GeneratedCode("Entity Framework Power Tools", "0.9.0.0")]
    internal sealed class ViewsForBaseEntitySetsda7e9ad7d998b7e291da2d5afe6cefcbed480398d1847b906d669998b2a6afd7 : DbMappingViewCache
    {
        /// <summary>
        /// Gets a hash value computed over the mapping closure.
        /// </summary>
        public override string MappingHashValue
        {
            get { return "da7e9ad7d998b7e291da2d5afe6cefcbed480398d1847b906d669998b2a6afd7"; }
        }

        /// <summary>
        /// Gets a view corresponding to the specified extent.
        /// </summary>
        /// <param name="extent">The extent.</param>
        /// <returns>The mapping view, or null if the extent is not associated with a mapping view.</returns>
        public override DbMappingView GetView(EntitySetBase extent)
        {
            if (extent == null)
            {
                throw new ArgumentNullException("extent");
            }

            var extentName = extent.EntityContainer.Name + "." + extent.Name;

            if (extentName == "WendtEquipmentTrackingDataAccessSQLModelStoreContainer.BillOfLading")
            {
                return GetView0();
            }

            if (extentName == "WendtEquipmentTrackingDataAccessSQLModelStoreContainer.BillOfLadingEquipment")
            {
                return GetView1();
            }

            if (extentName == "WendtEquipmentTrackingDataAccessSQLModelStoreContainer.HardwareKit")
            {
                return GetView2();
            }

            if (extentName == "WendtEquipmentTrackingDataAccessSQLModelStoreContainer.HardwareKitEquipment")
            {
                return GetView3();
            }

            if (extentName == "WendtEquipmentTrackingDataAccessSQLModelStoreContainer.Project")
            {
                return GetView4();
            }

            if (extentName == "WendtEquipmentTrackingDataAccessSQLModelStoreContainer.WorkOrderPrice")
            {
                return GetView5();
            }

            if (extentName == "WendtEquipmentTrackingDataAccessSQLModelStoreContainer.Priority")
            {
                return GetView6();
            }

            if (extentName == "WendtEquipmentTrackingDataAccessSQLModelStoreContainer.Equipment")
            {
                return GetView7();
            }

            if (extentName == "WendtEquipmentTrackingEntities.BillOfLadings")
            {
                return GetView8();
            }

            if (extentName == "WendtEquipmentTrackingEntities.BillOfLadingEquipments")
            {
                return GetView9();
            }

            if (extentName == "WendtEquipmentTrackingEntities.HardwareKits")
            {
                return GetView10();
            }

            if (extentName == "WendtEquipmentTrackingEntities.HardwareKitEquipments")
            {
                return GetView11();
            }

            if (extentName == "WendtEquipmentTrackingEntities.Projects")
            {
                return GetView12();
            }

            if (extentName == "WendtEquipmentTrackingEntities.WorkOrderPrices")
            {
                return GetView13();
            }

            if (extentName == "WendtEquipmentTrackingEntities.Priorities")
            {
                return GetView14();
            }

            if (extentName == "WendtEquipmentTrackingEntities.Equipments")
            {
                return GetView15();
            }

            if (extentName == "WendtEquipmentTrackingDataAccessSQLModelStoreContainer.User")
            {
                return GetView16();
            }

            if (extentName == "WendtEquipmentTrackingEntities.Users")
            {
                return GetView17();
            }

            if (extentName == "WendtEquipmentTrackingDataAccessSQLModelStoreContainer.HardwareCommercialCode")
            {
                return GetView18();
            }

            if (extentName == "WendtEquipmentTrackingEntities.HardwareCommercialCodes")
            {
                return GetView19();
            }

            return null;
        }

        /// <summary>
        /// Gets the view for WendtEquipmentTrackingDataAccessSQLModelStoreContainer.BillOfLading.
        /// </summary>
        /// <returns>The mapping view.</returns>
        private static DbMappingView GetView0()
        {
            return new DbMappingView(@"
    SELECT VALUE -- Constructing BillOfLading
        [WendtEquipmentTracking.DataAccess.SQL.Model.Store.BillOfLading](T1.BillOfLading_BillOfLadingId, T1.BillOfLading_ProjectId, T1.BillOfLading_Revision, T1.BillOfLading_IsCurrentRevision, T1.BillOfLading_CreatedBy, T1.BillOfLading_CreatedDate, T1.BillOfLading_ModifiedBy, T1.BillOfLading_ModifiedDate, T1.BillOfLading_BillOfLadingNumber, T1.BillOfLading_DateShipped, T1.BillOfLading_ToStorage, T1.BillOfLading_TrailerNumber, T1.BillOfLading_Carrier, T1.BillOfLading_FreightTerms, T1.BillOfLading_IsDeleted)
    FROM (
        SELECT 
            T.BillOfLadingId AS BillOfLading_BillOfLadingId, 
            T.ProjectId AS BillOfLading_ProjectId, 
            T.Revision AS BillOfLading_Revision, 
            T.IsCurrentRevision AS BillOfLading_IsCurrentRevision, 
            T.CreatedBy AS BillOfLading_CreatedBy, 
            T.CreatedDate AS BillOfLading_CreatedDate, 
            T.ModifiedBy AS BillOfLading_ModifiedBy, 
            T.ModifiedDate AS BillOfLading_ModifiedDate, 
            T.BillOfLadingNumber AS BillOfLading_BillOfLadingNumber, 
            T.DateShipped AS BillOfLading_DateShipped, 
            T.ToStorage AS BillOfLading_ToStorage, 
            T.TrailerNumber AS BillOfLading_TrailerNumber, 
            T.Carrier AS BillOfLading_Carrier, 
            T.FreightTerms AS BillOfLading_FreightTerms, 
            T.IsDeleted AS BillOfLading_IsDeleted, 
            True AS _from0
        FROM WendtEquipmentTrackingEntities.BillOfLadings AS T
    ) AS T1");
        }

        /// <summary>
        /// Gets the view for WendtEquipmentTrackingDataAccessSQLModelStoreContainer.BillOfLadingEquipment.
        /// </summary>
        /// <returns>The mapping view.</returns>
        private static DbMappingView GetView1()
        {
            return new DbMappingView(@"
    SELECT VALUE -- Constructing BillOfLadingEquipment
        [WendtEquipmentTracking.DataAccess.SQL.Model.Store.BillOfLadingEquipment](T1.BillOfLadingEquipment_BillOfLadingEquipmentId, T1.BillOfLadingEquipment_BillOfLadingId, T1.BillOfLadingEquipment_EquipmentId, T1.BillOfLadingEquipment_Quantity, T1.BillOfLadingEquipment_ShippedFrom, T1.BillOfLadingEquipment_HTSCode, T1.BillOfLadingEquipment_CountryOfOrigin, T1.BillOfLadingEquipment_IsDeleted)
    FROM (
        SELECT 
            T.BillOfLadingEquipmentId AS BillOfLadingEquipment_BillOfLadingEquipmentId, 
            T.BillOfLadingId AS BillOfLadingEquipment_BillOfLadingId, 
            T.EquipmentId AS BillOfLadingEquipment_EquipmentId, 
            T.Quantity AS BillOfLadingEquipment_Quantity, 
            T.ShippedFrom AS BillOfLadingEquipment_ShippedFrom, 
            T.HTSCode AS BillOfLadingEquipment_HTSCode, 
            T.CountryOfOrigin AS BillOfLadingEquipment_CountryOfOrigin, 
            T.IsDeleted AS BillOfLadingEquipment_IsDeleted, 
            True AS _from0
        FROM WendtEquipmentTrackingEntities.BillOfLadingEquipments AS T
    ) AS T1");
        }

        /// <summary>
        /// Gets the view for WendtEquipmentTrackingDataAccessSQLModelStoreContainer.HardwareKit.
        /// </summary>
        /// <returns>The mapping view.</returns>
        private static DbMappingView GetView2()
        {
            return new DbMappingView(@"
    SELECT VALUE -- Constructing HardwareKit
        [WendtEquipmentTracking.DataAccess.SQL.Model.Store.HardwareKit](T1.HardwareKit_HardwareKitId, T1.HardwareKit_ProjectId, T1.HardwareKit_Revision, T1.HardwareKit_IsCurrentRevision, T1.HardwareKit_HardwareKitNumber, T1.HardwareKit_ExtraQuantityPercentage, T1.HardwareKit_CreatedBy, T1.HardwareKit_CreatedDate, T1.HardwareKit_ModifiedBy, T1.HardwareKit_ModifiedDate, T1.HardwareKit_IsDeleted)
    FROM (
        SELECT 
            T.HardwareKitId AS HardwareKit_HardwareKitId, 
            T.ProjectId AS HardwareKit_ProjectId, 
            T.Revision AS HardwareKit_Revision, 
            T.IsCurrentRevision AS HardwareKit_IsCurrentRevision, 
            T.HardwareKitNumber AS HardwareKit_HardwareKitNumber, 
            T.ExtraQuantityPercentage AS HardwareKit_ExtraQuantityPercentage, 
            T.CreatedBy AS HardwareKit_CreatedBy, 
            T.CreatedDate AS HardwareKit_CreatedDate, 
            T.ModifiedBy AS HardwareKit_ModifiedBy, 
            T.ModifiedDate AS HardwareKit_ModifiedDate, 
            T.IsDeleted AS HardwareKit_IsDeleted, 
            True AS _from0
        FROM WendtEquipmentTrackingEntities.HardwareKits AS T
    ) AS T1");
        }

        /// <summary>
        /// Gets the view for WendtEquipmentTrackingDataAccessSQLModelStoreContainer.HardwareKitEquipment.
        /// </summary>
        /// <returns>The mapping view.</returns>
        private static DbMappingView GetView3()
        {
            return new DbMappingView(@"
    SELECT VALUE -- Constructing HardwareKitEquipment
        [WendtEquipmentTracking.DataAccess.SQL.Model.Store.HardwareKitEquipment](T1.HardwareKitEquipment_HardwareKitEquipmentId, T1.HardwareKitEquipment_HardwareKitId, T1.HardwareKitEquipment_EquipmentId, T1.HardwareKitEquipment_QuantityToShip, T1.HardwareKitEquipment_IsDeleted)
    FROM (
        SELECT 
            T.HardwareKitEquipmentId AS HardwareKitEquipment_HardwareKitEquipmentId, 
            T.HardwareKitId AS HardwareKitEquipment_HardwareKitId, 
            T.EquipmentId AS HardwareKitEquipment_EquipmentId, 
            T.QuantityToShip AS HardwareKitEquipment_QuantityToShip, 
            T.IsDeleted AS HardwareKitEquipment_IsDeleted, 
            True AS _from0
        FROM WendtEquipmentTrackingEntities.HardwareKitEquipments AS T
    ) AS T1");
        }

        /// <summary>
        /// Gets the view for WendtEquipmentTrackingDataAccessSQLModelStoreContainer.Project.
        /// </summary>
        /// <returns>The mapping view.</returns>
        private static DbMappingView GetView4()
        {
            return new DbMappingView(@"
    SELECT VALUE -- Constructing Project
        [WendtEquipmentTracking.DataAccess.SQL.Model.Store.Project](T1.Project_ProjectId, T1.Project_ProjectNumber, T1.Project_CreatedBy, T1.Project_CreatedDate, T1.Project_ModifiedBy, T1.Project_ModifiedDate, T1.Project_FreightTerms, T1.Project_ShipToCompany, T1.Project_ShipToAddress, T1.Project_ShipToCSZ, T1.Project_ShipToContact1, T1.Project_ShipToContact1PhoneFax, T1.Project_ShipToContact1Email, T1.Project_ShipToContact2, T1.Project_ShipToContact2PhoneFax, T1.Project_ShipToContact2Email, T1.Project_ShipToBroker, T1.Project_ShipToBrokerPhoneFax, T1.Project_ShipToBrokerEmail, T1.Project_IsCustomsProject, T1.Project_IsDeleted)
    FROM (
        SELECT 
            T.ProjectId AS Project_ProjectId, 
            T.ProjectNumber AS Project_ProjectNumber, 
            T.CreatedBy AS Project_CreatedBy, 
            T.CreatedDate AS Project_CreatedDate, 
            T.ModifiedBy AS Project_ModifiedBy, 
            T.ModifiedDate AS Project_ModifiedDate, 
            T.FreightTerms AS Project_FreightTerms, 
            T.ShipToCompany AS Project_ShipToCompany, 
            T.ShipToAddress AS Project_ShipToAddress, 
            T.ShipToCSZ AS Project_ShipToCSZ, 
            T.ShipToContact1 AS Project_ShipToContact1, 
            T.ShipToContact1PhoneFax AS Project_ShipToContact1PhoneFax, 
            T.ShipToContact1Email AS Project_ShipToContact1Email, 
            T.ShipToContact2 AS Project_ShipToContact2, 
            T.ShipToContact2PhoneFax AS Project_ShipToContact2PhoneFax, 
            T.ShipToContact2Email AS Project_ShipToContact2Email, 
            T.ShipToBroker AS Project_ShipToBroker, 
            T.ShipToBrokerPhoneFax AS Project_ShipToBrokerPhoneFax, 
            T.ShipToBrokerEmail AS Project_ShipToBrokerEmail, 
            T.IsCustomsProject AS Project_IsCustomsProject, 
            T.IsDeleted AS Project_IsDeleted, 
            True AS _from0
        FROM WendtEquipmentTrackingEntities.Projects AS T
    ) AS T1");
        }

        /// <summary>
        /// Gets the view for WendtEquipmentTrackingDataAccessSQLModelStoreContainer.WorkOrderPrice.
        /// </summary>
        /// <returns>The mapping view.</returns>
        private static DbMappingView GetView5()
        {
            return new DbMappingView(@"
    SELECT VALUE -- Constructing WorkOrderPrice
        [WendtEquipmentTracking.DataAccess.SQL.Model.Store.WorkOrderPrice](T1.WorkOrderPrice_WorkOrderPriceId, T1.WorkOrderPrice_ProjectId, T1.WorkOrderPrice_WorkOrderNumber, T1.WorkOrderPrice_SalePrice, T1.WorkOrderPrice_CostPrice, T1.WorkOrderPrice_TotalWeight, T1.WorkOrderPrice_CreatedBy, T1.WorkOrderPrice_CreatedDate, T1.WorkOrderPrice_ModifiedBy, T1.WorkOrderPrice_ModifiedDate, T1.WorkOrderPrice_IsDeleted)
    FROM (
        SELECT 
            T.WorkOrderPriceId AS WorkOrderPrice_WorkOrderPriceId, 
            T.ProjectId AS WorkOrderPrice_ProjectId, 
            T.WorkOrderNumber AS WorkOrderPrice_WorkOrderNumber, 
            T.SalePrice AS WorkOrderPrice_SalePrice, 
            T.CostPrice AS WorkOrderPrice_CostPrice, 
            T.TotalWeight AS WorkOrderPrice_TotalWeight, 
            T.CreatedBy AS WorkOrderPrice_CreatedBy, 
            T.CreatedDate AS WorkOrderPrice_CreatedDate, 
            T.ModifiedBy AS WorkOrderPrice_ModifiedBy, 
            T.ModifiedDate AS WorkOrderPrice_ModifiedDate, 
            T.IsDeleted AS WorkOrderPrice_IsDeleted, 
            True AS _from0
        FROM WendtEquipmentTrackingEntities.WorkOrderPrices AS T
    ) AS T1");
        }

        /// <summary>
        /// Gets the view for WendtEquipmentTrackingDataAccessSQLModelStoreContainer.Priority.
        /// </summary>
        /// <returns>The mapping view.</returns>
        private static DbMappingView GetView6()
        {
            return new DbMappingView(@"
    SELECT VALUE -- Constructing Priority
        [WendtEquipmentTracking.DataAccess.SQL.Model.Store.Priority](T1.Priority_PriorityId, T1.Priority_ProjectId, T1.Priority_PriorityNumber, T1.Priority_DueDate, T1.Priority_EquipmentName, T1.Priority_CreatedBy, T1.Priority_CreatedDate, T1.Priority_ModifiedBy, T1.Priority_ModifiedDate, T1.Priority_IsDeleted)
    FROM (
        SELECT 
            T.PriorityId AS Priority_PriorityId, 
            T.ProjectId AS Priority_ProjectId, 
            T.PriorityNumber AS Priority_PriorityNumber, 
            T.DueDate AS Priority_DueDate, 
            T.EquipmentName AS Priority_EquipmentName, 
            T.CreatedBy AS Priority_CreatedBy, 
            T.CreatedDate AS Priority_CreatedDate, 
            T.ModifiedBy AS Priority_ModifiedBy, 
            T.ModifiedDate AS Priority_ModifiedDate, 
            T.IsDeleted AS Priority_IsDeleted, 
            True AS _from0
        FROM WendtEquipmentTrackingEntities.Priorities AS T
    ) AS T1");
        }

        /// <summary>
        /// Gets the view for WendtEquipmentTrackingDataAccessSQLModelStoreContainer.Equipment.
        /// </summary>
        /// <returns>The mapping view.</returns>
        private static DbMappingView GetView7()
        {
            return new DbMappingView(@"
    SELECT VALUE -- Constructing Equipment
        [WendtEquipmentTracking.DataAccess.SQL.Model.Store.Equipment](T1.Equipment_EquipmentId, T1.Equipment_ProjectId, T1.Equipment_EquipmentName, T1.Equipment_Priority, T1.Equipment_CreatedBy, T1.Equipment_CreatedDate, T1.Equipment_ModifiedBy, T1.Equipment_ModifiedDate, T1.Equipment_ReleaseDate, T1.Equipment_DrawingNumber, T1.Equipment_WorkOrderNumber, T1.Equipment_Quantity, T1.Equipment_ShippingTagNumber, T1.Equipment_Description, T1.Equipment_UnitWeight, T1.Equipment_TotalWeight, T1.Equipment_TotalWeightShipped, T1.Equipment_ReadyToShip, T1.Equipment_ShippedQuantity, T1.Equipment_LeftToShip, T1.Equipment_FullyShipped, T1.Equipment_CustomsValue, T1.Equipment_SalePrice, T1.Equipment_Notes, T1.Equipment_ShippedFrom, T1.Equipment_HTSCode, T1.Equipment_CountryOfOrigin, T1.Equipment_IsHardware, T1.Equipment_IsDeleted, T1.Equipment_HardwareKitId)
    FROM (
        SELECT 
            T.EquipmentId AS Equipment_EquipmentId, 
            T.ProjectId AS Equipment_ProjectId, 
            T.EquipmentName AS Equipment_EquipmentName, 
            T.Priority AS Equipment_Priority, 
            T.CreatedBy AS Equipment_CreatedBy, 
            T.CreatedDate AS Equipment_CreatedDate, 
            T.ModifiedBy AS Equipment_ModifiedBy, 
            T.ModifiedDate AS Equipment_ModifiedDate, 
            T.ReleaseDate AS Equipment_ReleaseDate, 
            T.DrawingNumber AS Equipment_DrawingNumber, 
            T.WorkOrderNumber AS Equipment_WorkOrderNumber, 
            T.Quantity AS Equipment_Quantity, 
            T.ShippingTagNumber AS Equipment_ShippingTagNumber, 
            T.Description AS Equipment_Description, 
            T.UnitWeight AS Equipment_UnitWeight, 
            T.TotalWeight AS Equipment_TotalWeight, 
            T.TotalWeightShipped AS Equipment_TotalWeightShipped, 
            T.ReadyToShip AS Equipment_ReadyToShip, 
            T.ShippedQuantity AS Equipment_ShippedQuantity, 
            T.LeftToShip AS Equipment_LeftToShip, 
            T.FullyShipped AS Equipment_FullyShipped, 
            T.CustomsValue AS Equipment_CustomsValue, 
            T.SalePrice AS Equipment_SalePrice, 
            T.Notes AS Equipment_Notes, 
            T.ShippedFrom AS Equipment_ShippedFrom, 
            T.HTSCode AS Equipment_HTSCode, 
            T.CountryOfOrigin AS Equipment_CountryOfOrigin, 
            T.IsHardware AS Equipment_IsHardware, 
            T.IsDeleted AS Equipment_IsDeleted, 
            T.HardwareKitId AS Equipment_HardwareKitId, 
            True AS _from0
        FROM WendtEquipmentTrackingEntities.Equipments AS T
    ) AS T1");
        }

        /// <summary>
        /// Gets the view for WendtEquipmentTrackingEntities.BillOfLadings.
        /// </summary>
        /// <returns>The mapping view.</returns>
        private static DbMappingView GetView8()
        {
            return new DbMappingView(@"
    SELECT VALUE -- Constructing BillOfLadings
        [WendtEquipmentTracking.DataAccess.SQL.Model.BillOfLading](T1.BillOfLading_BillOfLadingId, T1.BillOfLading_ProjectId, T1.BillOfLading_Revision, T1.BillOfLading_IsCurrentRevision, T1.BillOfLading_CreatedBy, T1.BillOfLading_CreatedDate, T1.BillOfLading_ModifiedBy, T1.BillOfLading_ModifiedDate, T1.BillOfLading_BillOfLadingNumber, T1.BillOfLading_DateShipped, T1.BillOfLading_ToStorage, T1.BillOfLading_TrailerNumber, T1.BillOfLading_Carrier, T1.BillOfLading_FreightTerms, T1.BillOfLading_IsDeleted)
    FROM (
        SELECT 
            T.BillOfLadingId AS BillOfLading_BillOfLadingId, 
            T.ProjectId AS BillOfLading_ProjectId, 
            T.Revision AS BillOfLading_Revision, 
            T.IsCurrentRevision AS BillOfLading_IsCurrentRevision, 
            T.CreatedBy AS BillOfLading_CreatedBy, 
            T.CreatedDate AS BillOfLading_CreatedDate, 
            T.ModifiedBy AS BillOfLading_ModifiedBy, 
            T.ModifiedDate AS BillOfLading_ModifiedDate, 
            T.BillOfLadingNumber AS BillOfLading_BillOfLadingNumber, 
            T.DateShipped AS BillOfLading_DateShipped, 
            T.ToStorage AS BillOfLading_ToStorage, 
            T.TrailerNumber AS BillOfLading_TrailerNumber, 
            T.Carrier AS BillOfLading_Carrier, 
            T.FreightTerms AS BillOfLading_FreightTerms, 
            T.IsDeleted AS BillOfLading_IsDeleted, 
            True AS _from0
        FROM WendtEquipmentTrackingDataAccessSQLModelStoreContainer.BillOfLading AS T
    ) AS T1");
        }

        /// <summary>
        /// Gets the view for WendtEquipmentTrackingEntities.BillOfLadingEquipments.
        /// </summary>
        /// <returns>The mapping view.</returns>
        private static DbMappingView GetView9()
        {
            return new DbMappingView(@"
    SELECT VALUE -- Constructing BillOfLadingEquipments
        [WendtEquipmentTracking.DataAccess.SQL.Model.BillOfLadingEquipment](T1.BillOfLadingEquipment_BillOfLadingEquipmentId, T1.BillOfLadingEquipment_BillOfLadingId, T1.BillOfLadingEquipment_EquipmentId, T1.BillOfLadingEquipment_Quantity, T1.BillOfLadingEquipment_ShippedFrom, T1.BillOfLadingEquipment_HTSCode, T1.BillOfLadingEquipment_CountryOfOrigin, T1.BillOfLadingEquipment_IsDeleted)
    FROM (
        SELECT 
            T.BillOfLadingEquipmentId AS BillOfLadingEquipment_BillOfLadingEquipmentId, 
            T.BillOfLadingId AS BillOfLadingEquipment_BillOfLadingId, 
            T.EquipmentId AS BillOfLadingEquipment_EquipmentId, 
            T.Quantity AS BillOfLadingEquipment_Quantity, 
            T.ShippedFrom AS BillOfLadingEquipment_ShippedFrom, 
            T.HTSCode AS BillOfLadingEquipment_HTSCode, 
            T.CountryOfOrigin AS BillOfLadingEquipment_CountryOfOrigin, 
            T.IsDeleted AS BillOfLadingEquipment_IsDeleted, 
            True AS _from0
        FROM WendtEquipmentTrackingDataAccessSQLModelStoreContainer.BillOfLadingEquipment AS T
    ) AS T1");
        }

        /// <summary>
        /// Gets the view for WendtEquipmentTrackingEntities.HardwareKits.
        /// </summary>
        /// <returns>The mapping view.</returns>
        private static DbMappingView GetView10()
        {
            return new DbMappingView(@"
    SELECT VALUE -- Constructing HardwareKits
        [WendtEquipmentTracking.DataAccess.SQL.Model.HardwareKit](T1.HardwareKit_HardwareKitId, T1.HardwareKit_ProjectId, T1.HardwareKit_Revision, T1.HardwareKit_IsCurrentRevision, T1.HardwareKit_HardwareKitNumber, T1.HardwareKit_ExtraQuantityPercentage, T1.HardwareKit_CreatedBy, T1.HardwareKit_CreatedDate, T1.HardwareKit_ModifiedBy, T1.HardwareKit_ModifiedDate, T1.HardwareKit_IsDeleted)
    FROM (
        SELECT 
            T.HardwareKitId AS HardwareKit_HardwareKitId, 
            T.ProjectId AS HardwareKit_ProjectId, 
            T.Revision AS HardwareKit_Revision, 
            T.IsCurrentRevision AS HardwareKit_IsCurrentRevision, 
            T.HardwareKitNumber AS HardwareKit_HardwareKitNumber, 
            T.ExtraQuantityPercentage AS HardwareKit_ExtraQuantityPercentage, 
            T.CreatedBy AS HardwareKit_CreatedBy, 
            T.CreatedDate AS HardwareKit_CreatedDate, 
            T.ModifiedBy AS HardwareKit_ModifiedBy, 
            T.ModifiedDate AS HardwareKit_ModifiedDate, 
            T.IsDeleted AS HardwareKit_IsDeleted, 
            True AS _from0
        FROM WendtEquipmentTrackingDataAccessSQLModelStoreContainer.HardwareKit AS T
    ) AS T1");
        }

        /// <summary>
        /// Gets the view for WendtEquipmentTrackingEntities.HardwareKitEquipments.
        /// </summary>
        /// <returns>The mapping view.</returns>
        private static DbMappingView GetView11()
        {
            return new DbMappingView(@"
    SELECT VALUE -- Constructing HardwareKitEquipments
        [WendtEquipmentTracking.DataAccess.SQL.Model.HardwareKitEquipment](T1.HardwareKitEquipment_HardwareKitEquipmentId, T1.HardwareKitEquipment_HardwareKitId, T1.HardwareKitEquipment_EquipmentId, T1.HardwareKitEquipment_QuantityToShip, T1.HardwareKitEquipment_IsDeleted)
    FROM (
        SELECT 
            T.HardwareKitEquipmentId AS HardwareKitEquipment_HardwareKitEquipmentId, 
            T.HardwareKitId AS HardwareKitEquipment_HardwareKitId, 
            T.EquipmentId AS HardwareKitEquipment_EquipmentId, 
            T.QuantityToShip AS HardwareKitEquipment_QuantityToShip, 
            T.IsDeleted AS HardwareKitEquipment_IsDeleted, 
            True AS _from0
        FROM WendtEquipmentTrackingDataAccessSQLModelStoreContainer.HardwareKitEquipment AS T
    ) AS T1");
        }

        /// <summary>
        /// Gets the view for WendtEquipmentTrackingEntities.Projects.
        /// </summary>
        /// <returns>The mapping view.</returns>
        private static DbMappingView GetView12()
        {
            return new DbMappingView(@"
    SELECT VALUE -- Constructing Projects
        [WendtEquipmentTracking.DataAccess.SQL.Model.Project](T1.Project_ProjectId, T1.Project_ProjectNumber, T1.Project_CreatedBy, T1.Project_CreatedDate, T1.Project_ModifiedBy, T1.Project_ModifiedDate, T1.Project_FreightTerms, T1.Project_ShipToCompany, T1.Project_ShipToAddress, T1.Project_ShipToCSZ, T1.Project_ShipToContact1, T1.Project_ShipToContact1PhoneFax, T1.Project_ShipToContact1Email, T1.Project_ShipToContact2, T1.Project_ShipToContact2PhoneFax, T1.Project_ShipToContact2Email, T1.Project_ShipToBroker, T1.Project_ShipToBrokerPhoneFax, T1.Project_ShipToBrokerEmail, T1.Project_IsCustomsProject, T1.Project_IsDeleted)
    FROM (
        SELECT 
            T.ProjectId AS Project_ProjectId, 
            T.ProjectNumber AS Project_ProjectNumber, 
            T.CreatedBy AS Project_CreatedBy, 
            T.CreatedDate AS Project_CreatedDate, 
            T.ModifiedBy AS Project_ModifiedBy, 
            T.ModifiedDate AS Project_ModifiedDate, 
            T.FreightTerms AS Project_FreightTerms, 
            T.ShipToCompany AS Project_ShipToCompany, 
            T.ShipToAddress AS Project_ShipToAddress, 
            T.ShipToCSZ AS Project_ShipToCSZ, 
            T.ShipToContact1 AS Project_ShipToContact1, 
            T.ShipToContact1PhoneFax AS Project_ShipToContact1PhoneFax, 
            T.ShipToContact1Email AS Project_ShipToContact1Email, 
            T.ShipToContact2 AS Project_ShipToContact2, 
            T.ShipToContact2PhoneFax AS Project_ShipToContact2PhoneFax, 
            T.ShipToContact2Email AS Project_ShipToContact2Email, 
            T.ShipToBroker AS Project_ShipToBroker, 
            T.ShipToBrokerPhoneFax AS Project_ShipToBrokerPhoneFax, 
            T.ShipToBrokerEmail AS Project_ShipToBrokerEmail, 
            T.IsCustomsProject AS Project_IsCustomsProject, 
            T.IsDeleted AS Project_IsDeleted, 
            True AS _from0
        FROM WendtEquipmentTrackingDataAccessSQLModelStoreContainer.Project AS T
    ) AS T1");
        }

        /// <summary>
        /// Gets the view for WendtEquipmentTrackingEntities.WorkOrderPrices.
        /// </summary>
        /// <returns>The mapping view.</returns>
        private static DbMappingView GetView13()
        {
            return new DbMappingView(@"
    SELECT VALUE -- Constructing WorkOrderPrices
        [WendtEquipmentTracking.DataAccess.SQL.Model.WorkOrderPrice](T1.WorkOrderPrice_WorkOrderPriceId, T1.WorkOrderPrice_ProjectId, T1.WorkOrderPrice_WorkOrderNumber, T1.WorkOrderPrice_SalePrice, T1.WorkOrderPrice_CostPrice, T1.WorkOrderPrice_TotalWeight, T1.WorkOrderPrice_CreatedBy, T1.WorkOrderPrice_CreatedDate, T1.WorkOrderPrice_ModifiedBy, T1.WorkOrderPrice_ModifiedDate, T1.WorkOrderPrice_IsDeleted)
    FROM (
        SELECT 
            T.WorkOrderPriceId AS WorkOrderPrice_WorkOrderPriceId, 
            T.ProjectId AS WorkOrderPrice_ProjectId, 
            T.WorkOrderNumber AS WorkOrderPrice_WorkOrderNumber, 
            T.SalePrice AS WorkOrderPrice_SalePrice, 
            T.CostPrice AS WorkOrderPrice_CostPrice, 
            T.TotalWeight AS WorkOrderPrice_TotalWeight, 
            T.CreatedBy AS WorkOrderPrice_CreatedBy, 
            T.CreatedDate AS WorkOrderPrice_CreatedDate, 
            T.ModifiedBy AS WorkOrderPrice_ModifiedBy, 
            T.ModifiedDate AS WorkOrderPrice_ModifiedDate, 
            T.IsDeleted AS WorkOrderPrice_IsDeleted, 
            True AS _from0
        FROM WendtEquipmentTrackingDataAccessSQLModelStoreContainer.WorkOrderPrice AS T
    ) AS T1");
        }

        /// <summary>
        /// Gets the view for WendtEquipmentTrackingEntities.Priorities.
        /// </summary>
        /// <returns>The mapping view.</returns>
        private static DbMappingView GetView14()
        {
            return new DbMappingView(@"
    SELECT VALUE -- Constructing Priorities
        [WendtEquipmentTracking.DataAccess.SQL.Model.Priority](T1.Priority_PriorityId, T1.Priority_ProjectId, T1.Priority_PriorityNumber, T1.Priority_DueDate, T1.Priority_EquipmentName, T1.Priority_CreatedBy, T1.Priority_CreatedDate, T1.Priority_ModifiedBy, T1.Priority_ModifiedDate, T1.Priority_IsDeleted)
    FROM (
        SELECT 
            T.PriorityId AS Priority_PriorityId, 
            T.ProjectId AS Priority_ProjectId, 
            T.PriorityNumber AS Priority_PriorityNumber, 
            T.DueDate AS Priority_DueDate, 
            T.EquipmentName AS Priority_EquipmentName, 
            T.CreatedBy AS Priority_CreatedBy, 
            T.CreatedDate AS Priority_CreatedDate, 
            T.ModifiedBy AS Priority_ModifiedBy, 
            T.ModifiedDate AS Priority_ModifiedDate, 
            T.IsDeleted AS Priority_IsDeleted, 
            True AS _from0
        FROM WendtEquipmentTrackingDataAccessSQLModelStoreContainer.Priority AS T
    ) AS T1");
        }

        /// <summary>
        /// Gets the view for WendtEquipmentTrackingEntities.Equipments.
        /// </summary>
        /// <returns>The mapping view.</returns>
        private static DbMappingView GetView15()
        {
            return new DbMappingView(@"
    SELECT VALUE -- Constructing Equipments
        [WendtEquipmentTracking.DataAccess.SQL.Model.Equipment](T1.Equipment_EquipmentId, T1.Equipment_ProjectId, T1.Equipment_EquipmentName, T1.Equipment_Priority, T1.Equipment_CreatedBy, T1.Equipment_CreatedDate, T1.Equipment_ModifiedBy, T1.Equipment_ModifiedDate, T1.Equipment_ReleaseDate, T1.Equipment_DrawingNumber, T1.Equipment_WorkOrderNumber, T1.Equipment_Quantity, T1.Equipment_ShippingTagNumber, T1.Equipment_Description, T1.Equipment_UnitWeight, T1.Equipment_TotalWeight, T1.Equipment_TotalWeightShipped, T1.Equipment_ReadyToShip, T1.Equipment_ShippedQuantity, T1.Equipment_LeftToShip, T1.Equipment_FullyShipped, T1.Equipment_CustomsValue, T1.Equipment_SalePrice, T1.Equipment_Notes, T1.Equipment_ShippedFrom, T1.Equipment_HTSCode, T1.Equipment_CountryOfOrigin, T1.Equipment_IsHardware, T1.Equipment_IsDeleted, T1.Equipment_HardwareKitId)
    FROM (
        SELECT 
            T.EquipmentId AS Equipment_EquipmentId, 
            T.ProjectId AS Equipment_ProjectId, 
            T.EquipmentName AS Equipment_EquipmentName, 
            T.Priority AS Equipment_Priority, 
            T.CreatedBy AS Equipment_CreatedBy, 
            T.CreatedDate AS Equipment_CreatedDate, 
            T.ModifiedBy AS Equipment_ModifiedBy, 
            T.ModifiedDate AS Equipment_ModifiedDate, 
            T.ReleaseDate AS Equipment_ReleaseDate, 
            T.DrawingNumber AS Equipment_DrawingNumber, 
            T.WorkOrderNumber AS Equipment_WorkOrderNumber, 
            T.Quantity AS Equipment_Quantity, 
            T.ShippingTagNumber AS Equipment_ShippingTagNumber, 
            T.Description AS Equipment_Description, 
            T.UnitWeight AS Equipment_UnitWeight, 
            T.TotalWeight AS Equipment_TotalWeight, 
            T.TotalWeightShipped AS Equipment_TotalWeightShipped, 
            T.ReadyToShip AS Equipment_ReadyToShip, 
            T.ShippedQuantity AS Equipment_ShippedQuantity, 
            T.LeftToShip AS Equipment_LeftToShip, 
            T.FullyShipped AS Equipment_FullyShipped, 
            T.CustomsValue AS Equipment_CustomsValue, 
            T.SalePrice AS Equipment_SalePrice, 
            T.Notes AS Equipment_Notes, 
            T.ShippedFrom AS Equipment_ShippedFrom, 
            T.HTSCode AS Equipment_HTSCode, 
            T.CountryOfOrigin AS Equipment_CountryOfOrigin, 
            T.IsHardware AS Equipment_IsHardware, 
            T.IsDeleted AS Equipment_IsDeleted, 
            T.HardwareKitId AS Equipment_HardwareKitId, 
            True AS _from0
        FROM WendtEquipmentTrackingDataAccessSQLModelStoreContainer.Equipment AS T
    ) AS T1");
        }

        /// <summary>
        /// Gets the view for WendtEquipmentTrackingDataAccessSQLModelStoreContainer.User.
        /// </summary>
        /// <returns>The mapping view.</returns>
        private static DbMappingView GetView16()
        {
            return new DbMappingView(@"
    SELECT VALUE -- Constructing User
        [WendtEquipmentTracking.DataAccess.SQL.Model.Store.User](T1.User_UserId, T1.User_UserName, T1.User_ProjectId)
    FROM (
        SELECT 
            T.UserId AS User_UserId, 
            T.UserName AS User_UserName, 
            T.ProjectId AS User_ProjectId, 
            True AS _from0
        FROM WendtEquipmentTrackingEntities.Users AS T
    ) AS T1");
        }

        /// <summary>
        /// Gets the view for WendtEquipmentTrackingEntities.Users.
        /// </summary>
        /// <returns>The mapping view.</returns>
        private static DbMappingView GetView17()
        {
            return new DbMappingView(@"
    SELECT VALUE -- Constructing Users
        [WendtEquipmentTracking.DataAccess.SQL.Model.User](T1.User_UserId, T1.User_UserName, T1.User_ProjectId)
    FROM (
        SELECT 
            T.UserId AS User_UserId, 
            T.UserName AS User_UserName, 
            T.ProjectId AS User_ProjectId, 
            True AS _from0
        FROM WendtEquipmentTrackingDataAccessSQLModelStoreContainer.User AS T
    ) AS T1");
        }

        /// <summary>
        /// Gets the view for WendtEquipmentTrackingDataAccessSQLModelStoreContainer.HardwareCommercialCode.
        /// </summary>
        /// <returns>The mapping view.</returns>
        private static DbMappingView GetView18()
        {
            return new DbMappingView(@"
    SELECT VALUE -- Constructing HardwareCommercialCode
        [WendtEquipmentTracking.DataAccess.SQL.Model.Store.HardwareCommercialCode](T1.HardwareCommercialCode_HardwareCommercialCodeId, T1.HardwareCommercialCode_PartNumber, T1.HardwareCommercialCode_Description, T1.HardwareCommercialCode_CommodityCode, T1.HardwareCommercialCode_CreatedBy, T1.HardwareCommercialCode_CreatedDate, T1.HardwareCommercialCode_ModifiedBy, T1.HardwareCommercialCode_ModifiedDate, T1.HardwareCommercialCode_IsDeleted)
    FROM (
        SELECT 
            T.HardwareCommercialCodeId AS HardwareCommercialCode_HardwareCommercialCodeId, 
            T.PartNumber AS HardwareCommercialCode_PartNumber, 
            T.Description AS HardwareCommercialCode_Description, 
            T.CommodityCode AS HardwareCommercialCode_CommodityCode, 
            T.CreatedBy AS HardwareCommercialCode_CreatedBy, 
            T.CreatedDate AS HardwareCommercialCode_CreatedDate, 
            T.ModifiedBy AS HardwareCommercialCode_ModifiedBy, 
            T.ModifiedDate AS HardwareCommercialCode_ModifiedDate, 
            T.IsDeleted AS HardwareCommercialCode_IsDeleted, 
            True AS _from0
        FROM WendtEquipmentTrackingEntities.HardwareCommercialCodes AS T
    ) AS T1");
        }

        /// <summary>
        /// Gets the view for WendtEquipmentTrackingEntities.HardwareCommercialCodes.
        /// </summary>
        /// <returns>The mapping view.</returns>
        private static DbMappingView GetView19()
        {
            return new DbMappingView(@"
    SELECT VALUE -- Constructing HardwareCommercialCodes
        [WendtEquipmentTracking.DataAccess.SQL.Model.HardwareCommercialCode](T1.HardwareCommercialCode_HardwareCommercialCodeId, T1.HardwareCommercialCode_PartNumber, T1.HardwareCommercialCode_Description, T1.HardwareCommercialCode_CommodityCode, T1.HardwareCommercialCode_CreatedBy, T1.HardwareCommercialCode_CreatedDate, T1.HardwareCommercialCode_ModifiedBy, T1.HardwareCommercialCode_ModifiedDate, T1.HardwareCommercialCode_IsDeleted)
    FROM (
        SELECT 
            T.HardwareCommercialCodeId AS HardwareCommercialCode_HardwareCommercialCodeId, 
            T.PartNumber AS HardwareCommercialCode_PartNumber, 
            T.Description AS HardwareCommercialCode_Description, 
            T.CommodityCode AS HardwareCommercialCode_CommodityCode, 
            T.CreatedBy AS HardwareCommercialCode_CreatedBy, 
            T.CreatedDate AS HardwareCommercialCode_CreatedDate, 
            T.ModifiedBy AS HardwareCommercialCode_ModifiedBy, 
            T.ModifiedDate AS HardwareCommercialCode_ModifiedDate, 
            T.IsDeleted AS HardwareCommercialCode_IsDeleted, 
            True AS _from0
        FROM WendtEquipmentTrackingDataAccessSQLModelStoreContainer.HardwareCommercialCode AS T
    ) AS T1");
        }
    }
}
