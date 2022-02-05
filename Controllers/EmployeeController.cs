using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using WebAPI_ver2.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace WebAPI_ver2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
 
            private readonly IConfiguration _configuration;
            private readonly IWebHostEnvironment _env;
            public EmployeeController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
            {
                _configuration = configuration;
                _env = webHostEnvironment;
            }

            [HttpGet]
            public JsonResult Get()
            {
                string query = @"SELECT EmployeeId, EmployeeName, Department,
                                CONVERT(VARCHAR(10), DateOfJoining, 120) as DateOfJoining,
                                PhotoFileName 
                                FROM Employee";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
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
            public JsonResult Post(Employee emp)
            {
                string query = @"INSERT INTO Employee 
                            (EmployeeName, Department, DateOfJoining, PhotoFileName)
                            VALUES
                            (
                              '" + emp.EmployeeName + @"',
                              '" + emp.Department + @"',
                              '" + emp.DateOfJoining + @"', 
                              '" + emp.PhotoFileName + @"'
                            )";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
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

                return new JsonResult("Added Successfully");
            }
            [HttpPut]
            public JsonResult Put(Employee emp)
            {
                string query =  @"UPDATE EMPLOYEE SET 
                                 EmployeeName = '" + emp.EmployeeName + @"' 
                                ,Department = '" + emp.Department + @"' 
                                ,DateOfJoining = '" + emp.DateOfJoining + @"' 
                                ,PhotoFileName = '" + emp.PhotoFileName+ @"'                         
                                WHERE EmployeeId = " + emp.EmployeeId + @"";
                
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
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

                return new JsonResult("Update Successfully");
            }

            [HttpDelete("{id}")]
            public JsonResult Delete(int id)
            {
                string query = @"DELETE FROM EMPLOYEE WHERE EmployeeId = " + id + @"";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
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

                return new JsonResult("Deleted Successfully");
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
                    var physicalPath = _env.ContentRootPath + "/Photos" + filename;
                    using(var stream = new FileStream(physicalPath, FileMode.Create))
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

        [Route("GetAllDepartmentNames")]
        public JsonResult GetAllDepartments()
        {
            string query = @"SELECT DepartmentName FROM Department";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
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

    }


}
