using Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.DTOs.Project
{
    public class CreateProjectDto
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public int TypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
