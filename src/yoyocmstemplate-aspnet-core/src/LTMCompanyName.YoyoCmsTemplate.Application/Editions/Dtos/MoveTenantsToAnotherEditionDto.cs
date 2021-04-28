using System;
using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.Editions.Dtos
{
    public class MoveTenantsToAnotherEditionDto
    {
        [Range(1, Int32.MaxValue)]
        public int SourceEditionId { get; set; }

        [Range(1, Int32.MaxValue)]
        public int TargetEditionId { get; set; }
    }
}