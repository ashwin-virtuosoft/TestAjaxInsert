using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;
using TestAjaxInsert.Models;
using System.Configuration;
using Newtonsoft.Json;
namespace TestAjaxInsert.Controllers
{
    public class HomeController : Controller
    {
        
        private IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
            

        }
        

        public IActionResult Index()
        {
            
            return View();
        }
      
        public async Task<JsonResult> Add_Data([FromBody] TestAjax testAjax)
        {
            string msg = string.Empty;

            try
            {
                string connectionString = _configuration.GetConnectionString("MyDB");
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spInsert", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Name", testAjax.Name);
                        cmd.Parameters.AddWithValue("@Email", testAjax.Email);
                        cmd.Parameters.AddWithValue("@Password", testAjax.Password);
                        cmd.Parameters.AddWithValue("@PhoneNumber", testAjax.PhoneNumber);

                         await cmd.ExecuteNonQueryAsync();
                    }
                }
                msg = "Data Inserted";
            }
            catch (Exception ex)
            {
                msg = "error " + ex.Message;
            }

            return Json(msg);
        }


        public IActionResult Privacy()
        {
            return View();
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
