using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public static class ProjectTypeSeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectType>().HasData(
                new ProjectType { Id = 1, Name = "Software Development", Code = (int)ProjectTypeEnum.SoftwareDevelopment },
                new ProjectType { Id = 2, Name = "Marketing Campaign", Code = (int)ProjectTypeEnum.MarketingCampaign },
                new ProjectType { Id = 3, Name = "Research and Development", Code = (int)ProjectTypeEnum.ResearchAndDevelopment },
                new ProjectType { Id = 4, Name = "Event Planning", Code = (int)ProjectTypeEnum.EventPlanning },
                new ProjectType { Id = 5, Name = "Social Job", Code = (int)ProjectTypeEnum.SocialJob }
            );
        }
    }
}
