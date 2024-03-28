namespace test.Model
{
    public class   Employee : People
    {
        public int ID { get; set; }  
        public  string EmEmail { get; set; }    
        public  string EmRole {  get; set; }
        public string  EmStatus { get; set; }
        public string EmPassword { get; set; }

    }
    public class LoginRequest
    {
        public int id { get; set; }
        public string emPassword { get; set; }
    }
}
    