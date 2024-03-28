using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using test.Model;
using Microsoft.AspNetCore.Authorization;

namespace test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class BiLLInfoController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public BiLLInfoController(IConfiguration config)
        {
            _configuration = config;
        }
        [HttpGet]
        [Route("GetAll")]
        public List<BiLLInfo> GetAll()
        {
            List<BiLLInfo> Lst = new List<BiLLInfo>();
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MyDB"));
            SqlCommand cmd = new SqlCommand("Select * from tblBillInfo", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                BiLLInfo obj = new BiLLInfo();
                obj.BillInfoID = int.Parse(dt.Rows[i]["BillInfoID"].ToString());
                obj.ProductID = int.Parse(dt.Rows[i]["ProductID"].ToString());
                obj.Price = float.Parse(dt.Rows[i]["Price"].ToString());
                obj.Quantity = int.Parse(dt.Rows[i]["Quantity"].ToString());


                Lst.Add(obj);
            }
            return Lst;

        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create(BiLLInfo obj)
        {
            try
            {
                string query = "INSERT INTO tblBiLLInfo (ProductID, Quantity, Price) " +
                               "VALUES (@ProductID, @Quantity, @Price)";

                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MyDB")))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", obj.ProductID);
                        cmd.Parameters.AddWithValue("@Quantity", obj.Quantity);
                        cmd.Parameters.AddWithValue("@Price", obj.Price);

                        await con.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                return Ok(obj);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> Update(BiLLInfo obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MyDB"));
                SqlCommand cmd = new SqlCommand("update tblBillInfo set ProductID='" + obj.ProductID + "',Quantity='" + obj.Quantity + "',Price='" + obj.Price + "', where BillInfoID='" + obj.BillInfoID + "'", con);
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


    
