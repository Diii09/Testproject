namespace test.Model
{
    public class Customer:People    
    {
        public int ID { get; set; }
        public int CusRank { get; set; }
                          
                
    }

    public class CreateCustomerRequest
    {
    
        public People InfoCustomer { get; set; }
        public int CusRank { get; set; }


    }
}
