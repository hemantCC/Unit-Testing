using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using PracticalAssignment.Models.BusinessEntities;
using PracticalAssignment.Models.DataEntities;

namespace PracticalAssignment.MappingProfiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<tblStudent, StudentVM>();
            CreateMap<StudentVM, tblStudent>();
        }
    }
}