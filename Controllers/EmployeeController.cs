using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using test.Model;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public EmployeeController(IConfiguration config)
        {
            _configuration = config;
        }

        [HttpGet]
        [Route("GetAll")]
        public List<Employee> GetAll()
        {
            List<Employee> Lst = new List<Employee>();
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MyDB"));
            SqlCommand cmd = new SqlCommand("Select * from tblEmployee", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Employee obj = new Employee();
                obj.ID = int.Parse(dt.Rows[i]["ID"].ToString());
                obj.Name = dt.Rows[i]["EmName"].ToString();
                obj.Address = dt.Rows[i]["EmAddress"].ToString();
                obj.PhoneNumber = dt.Rows[i]["EmPhoneNumber"].ToString();
                obj.EmRole = dt.Rows[i]["EmRole"].ToString();
                obj.EmStatus = dt.Rows[i]["EmStatus"].ToString();
                obj.EmPassword = dt.Rows[i]["EmPassword"].ToString();
                Lst.Add(obj);
            }
            return Lst;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Employee obj)
        {
            try
            {
                string query = "INSERT INTO tblEmployee (EmName, EmAddress, EmPhoneNumber,EmRole,EmStatus,EmPassword) " +
                               "VALUES (@EmName, @EmAddress, @EmPhoneNumber,@EmRole,@EmStatus,@EmPassword)";

                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MyDB")))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        // Add parameters to the query
                        cmd.Parameters.AddWithValue("@EmName", obj.Name);
                        cmd.Parameters.AddWithValue("@EmAddress", obj.Address);
                        cmd.Parameters.AddWithValue("@EmPhoneNumber", obj.PhoneNumber);
                        cmd.Parameters.AddWithValue("@EmRole", obj.EmRole);
                        cmd.Parameters.AddWithValue("@EmStatus", obj.EmStatus);
                        cmd.Parameters.AddWithValue("@EmPassword", obj.EmPassword);



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
        public async Task<IActionResult> Update(Employee obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MyDB"));
                SqlCommand cmd = new SqlCommand("update tblEmployee set EmName='" + obj.Name + "',EmAddess='" + obj.Address + "',PhoneNumber='"+obj.PhoneNumber+"',EmRole='" + obj.EmRole + "',EmStatus='" + obj.EmStatus +"',EmPassword='"+obj.EmPassword+
                 "'where ID='" + obj.ID + "'", con);
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
        [Route("Employee/UpgradeRole/{iD}")]
        [HttpPost]
        public async Task<IActionResult> UpgradeRole([FromRoute] int iD, [FromQuery] int emRole)
        {
            try
            {
                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MyDB"));
                SqlCommand cmd = new SqlCommand("Update tblEmployee set EmRole='" + emRole + "' where ID='" + iD + "'", con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return Ok();
            }
            catch(Exception ex) {
                return StatusCode(500, $"Internal server error: {ex.Message}");

            }
        }
    }
}

