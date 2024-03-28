using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using test.Model;
using Microsoft.AspNetCore.Authorization;

namespace test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class BillController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public BillController(IConfiguration config)
        {
            _configuration = config;
        }
        [HttpGet]
        [Route("GetAll")]
        public List<Bill> GetAll()
        {
            List<Bill> Lst = new List<Bill>();
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MyDB"));
            SqlCommand cmd = new SqlCommand("Select * from tblBill", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Bill obj = new Bill();
                obj.ID = int.Parse(dt.Rows[i]["ID"].ToString());
                obj.EmployeeID = int.Parse( dt.Rows[i]["EmployeeID"].ToString());
                obj.CustomerID = int.Parse(dt.Rows[i]["CustomerID"].ToString());
                obj.DayOfPayment = dt.Rows[i]["DayOfPayment"].ToString();

                obj.TotalPrice = float.Parse(dt.Rows[i]["TotalPrice"].ToString());
                obj.Status = int.Parse(dt.Rows[i]["Status"].ToString());
                Lst.Add(obj);
            }
            return Lst;
        
    }
    [HttpPost("Create")]
    public async Task<IActionResult> Create(Bill obj)
    {
        try
        {
            string query = "INSERT INTO tblBill (DayOfPayment, TotalPrice, Status) " +
                           "VALUES (@DayOfPayment, @TotalPrice, @Status)";

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MyDB")))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@DayOfPayment", obj.DayOfPayment);
                    cmd.Parameters.AddWithValue("@TotalPrice", obj.TotalPrice);
                    cmd.Parameters.AddWithValue("@Status", obj.Status);

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
    public async Task<IActionResult> Update(Bill obj)
    {
        try
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MyDB"));
            SqlCommand cmd = new SqlCommand("update tblBill set DayOfPayment='" + obj.DayOfPayment + "',TotalPrice='" + obj.TotalPrice + "',Status='" + obj.Status + "', where ID='" + obj.ID + "'", con);
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

