using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using VPP.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Hosting;


namespace VPP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

       

        public ProductsController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult get()
        {
            string query = @" select * from Products";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("VanPhongPhamAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult post(Products pro)
        {
            string query = @" INSERT INTO Products VALUES (@IdProduct, @IdCategory, @ProductName, @ProductPrice, @ProductQuantity, @ProductDescribe, @PhotoFileName)";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("VanPhongPhamAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@IdProduct", pro.IdProduct);
                    myCommand.Parameters.AddWithValue("@IdCategory", pro.IdCategory);
                    myCommand.Parameters.AddWithValue("@ProductName", pro.ProductName);
                    myCommand.Parameters.AddWithValue("@ProductPrice", pro.ProductPrice);
                    myCommand.Parameters.AddWithValue("@ProductQuantity", pro.ProductQuantity);
                    myCommand.Parameters.AddWithValue("@ProductDescribe", pro.ProductDescribe);
                    myCommand.Parameters.AddWithValue("@PhotoFileName", pro.PhotoFileName);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPut]
        public JsonResult put(Products pro)
        {
            string query = @" UPDATE Products SET  IdCategory=@IdCategory, ProductName=@ProductName, 
                            ProductPrice=@ProductPrice, ProductQuantity=@ProductQuantity, ProductDescribe=@ProductDescribe, PhotoFileName=@PhotoFileName WHERE IdProduct=@IdProduct";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("VanPhongPhamAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@IdProduct", pro.IdProduct);
                    myCommand.Parameters.AddWithValue("@IdCategory", pro.IdCategory);
                    myCommand.Parameters.AddWithValue("@ProductName", pro.ProductName);
                    myCommand.Parameters.AddWithValue("@ProductPrice", pro.ProductPrice);
                    myCommand.Parameters.AddWithValue("@ProductQuantity", pro.ProductQuantity);
                    myCommand.Parameters.AddWithValue("@ProductDescribe", pro.ProductDescribe);
                    myCommand.Parameters.AddWithValue("@PhotoFileName", pro.PhotoFileName);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpDelete]
        public JsonResult delete(string id)
        {
            string query = @" DELETE FROM Products WHERE IdProduct=@IdProduct";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("VanPhongPhamAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@IdProduct", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0]; 
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(filename);
            }
            catch (Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }
    }
}
