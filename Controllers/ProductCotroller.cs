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

    public class ProductCotroller : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ProductCotroller(IConfiguration config)
        {
            _configuration = config;
        }

        [HttpGet]
        [Route("GetAll")]
        public List<Product> GetAll()
        {
            List<Product> Lst = new List<Product>();
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MyDB"));
            SqlCommand cmd = new SqlCommand("Select * from tblProduct", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Product obj = new Product();
                obj.ID = int.Parse(dt.Rows[i]["ID"].ToString());
                obj.ProcName = dt.Rows[i]["ProcName"].ToString();
                obj.Quantity = int.Parse(dt.Rows[i]["Quantity"].ToString());
                obj.ImportUnitPrice = float.Parse(dt.Rows[i]["ImportUnitPrice"].ToString());
                obj.UnitPrice= float.Parse(dt.Rows[i]["UnitPrice"].ToString());
                obj.Note= dt.Rows[i]["Note"].ToString();

                Lst.Add(obj);
            }
            return Lst;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Product obj)
        {
            try
            {
                string query = "INSERT INTO tblProduct (ProcName, Quantity, ImportUnitPrice,UnitPrice,Note) " +
                               "VALUES (@ProcName, @Quantity, @ImportUnitPrice,@UnitPrice,@Note)";

                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MyDB")))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        // Add parameters to the query
                        cmd.Parameters.AddWithValue("@ProcName", obj.ProcName);
                        cmd.Parameters.AddWithValue("@Quantity", obj.Quantity);
                        cmd.Parameters.AddWithValue("@ImportUnitPrice", obj.ImportUnitPrice);
                        cmd.Parameters.AddWithValue("@UnitPrice", obj.UnitPrice);
                        cmd.Parameters.AddWithValue("@Note", obj.Note);


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
        public async Task<IActionResult> Update(Product obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MyDB"));
                SqlCommand cmd = new SqlCommand("update tblProduct set ProcName='" + obj.ProcName + "',Quantity='" + obj.Quantity + "',ImportUnitPrice='" + obj.ImportUnitPrice + "',UnitPrice='"+obj.UnitPrice+"',Note='"+obj.Note+ 
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

    }
}

