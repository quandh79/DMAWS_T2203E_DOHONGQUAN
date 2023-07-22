using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMAWS_T2203E_DOHONGQUAN.Models
{
    [Table("ProjectEmployee")]
    public class ProjectEmployee
    {
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }
        [Required]
        public string Tasks { get; set; }
        
        public virtual Employee Employees { get; set; }
        
        public virtual Project Project { get; set; }
    }
}
