using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using test.Model;
using static test.Model.Constants;

namespace test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class CustomerController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public CustomerController(IConfiguration config)
        {
            _configuration = config;
        }

        [HttpGet]
        [Route("GetAll")]
        public List<Customer> GetAll()
        {
         
                List<Customer> Lst = new List<Customer>();
            
            

                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MyDB"));
                SqlCommand cmd = new SqlCommand("Select * from tblCustomer", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Customer obj = new Customer();
                    obj.ID = int.Parse(dt.Rows[i]["ID"].ToString());
                    obj.Name = dt.Rows[i]["CusName"].ToString();
                    obj.Address = dt.Rows[i]["CusAddress"].ToString();
                    obj.PhoneNumber = dt.Rows[i]["PhoneNumber"].ToString();
                    obj.CusRank = int.Parse(dt.Rows[i]["Ranking"].ToString());
                    Lst.Add(obj);
                }
                return Lst;

            }
          
           

        


        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateCustomerRequest request)
        {
            try
            {
                string query = "INSERT INTO tblCustomer (CusName, CusAddress, PhoneNumber,Ranking) " +
                               "VALUES (@CusName, @CusAddress, @PhoneNumber,@Ranking)";

                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MyDB")))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    { 
                        cmd.Parameters.AddWithValue("@CusName", request.InfoCustomer.Name);
                        cmd.Parameters.AddWithValue("@CusAddress", request.InfoCustomer.Address);
                        cmd.Parameters.AddWithValue("@PhoneNumber", request.InfoCustomer.PhoneNumber);
                        cmd.Parameters.AddWithValue("@Ranking", request.CusRank);

                        await con.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                return Ok(request);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
[Route("Update")]
        [HttpPost]
        public async Task<IActionResult> Update(Customer obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MyDB"));
                SqlCommand cmd = new SqlCommand("update tblCustomer set CusName='" + obj.Name + "',CusAddress='" + obj.Address + "',PhoneNumber='" + obj.PhoneNumber + "',Ranking='"+obj.CusRank+"'where ID='"+obj.ID+"'", con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return Ok(obj);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }

}

