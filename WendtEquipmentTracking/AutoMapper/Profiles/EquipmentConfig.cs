﻿using AutoMapper;
using System;
using System.Linq;
using WendtEquipmentTracking.App.AutoMapper.Converters;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.App.AutoMapper.Profiles
{
    public class EquipmentConfig : Profile
    {
        public EquipmentConfig()
        {

            base.CreateMap<DateTime?, string>().ConvertUsing(new NullableDateTimeToStringConverter());
            base.CreateMap<DateTime, string>().ConvertUsing(new DateTimeToStringConverter());
            base.CreateMap<DateTime, DateTime?>().ConvertUsing(new DateTimeToNullableDateTimeConverter());

            base.CreateMap<EquipmentBO, EquipmentModel>();
            base.CreateMap<EquipmentModel, EquipmentBO>()
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore());

            base.CreateMap<EquipmentBO, EquipmentSelectionModel>();
            base.CreateMap<EquipmentSelectionModel, EquipmentBO>();

            base.CreateMap<HardwareKitBO, EquipmentModel>()
                .ForMember(dest => dest.EquipmentName, opt => opt.MapFrom(src => src.HardwareKitNumber))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.HardwareKitEquipments.Sum(hke => hke.Quantity)));

        }
    }
}