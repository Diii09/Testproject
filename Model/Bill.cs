namespace test.Model
{
    public class Bill
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public int CustomerID { get; set; } 
        public string DayOfPayment { get; set; }
        public float TotalPrice { get; set; }   
        public int Status { get; set; } 
    }
}
