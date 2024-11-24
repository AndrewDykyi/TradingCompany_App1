using System;

namespace DAL.Concrete
{
    public class CategoryManagementDto
    {
        public int ManagementID { get; set; }
        public int UserID { get; set; }
        public int CategoryID { get; set; }
        public int ActionID { get; set; }
        public DateTime ActionDate { get; set; }
    }
}