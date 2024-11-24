using System;

namespace DAL.Concrete
{
    public class ProductManagementDto
    {
        public int ManagementID { get; set; }
        public int UserID { get; set; }
        public int ProductID { get; set; }
        public int ActionID { get; set; }
        public DateTime ActionDate { get; set; }
    }
}