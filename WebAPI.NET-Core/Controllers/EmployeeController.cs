using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Get()
        {
            string query = @"
                    select EmployeeId,EmployeeName,Department,
                    convert(varchar(10),DateOfJoining,120) as DateOfJoining,
                    PhotoFileName
                    from
                    dbo.Employee
                    ";
            DataTable table = new DataTable();
            using (var con = new SqlConnection(_configuration.GetConnectionString("AngularNETCoreEmployeesDB")))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                da.Fill(table);
            }

            return Ok(table);
        }

        [HttpPost]
        public IActionResult Post(Employee emp)
        {
            try
            {
                string query = @"
                    insert into dbo.Employee values
                    (
                    @EmployeeName,
                    @Department,
                    @DateOfJoining,
                    @PhotoFileName
                    )
                    ";

                DataTable table = new DataTable();
                using (var con = new SqlConnection(_configuration.GetConnectionString("AngularNETCoreEmployeesDB")))
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
                    cmd.Parameters.AddWithValue("@Department", emp.Department);
                    cmd.Parameters.AddWithValue("@DateOfJoining", emp.DateOfJoining);
                    cmd.Parameters.AddWithValue("@PhotoFileName", emp.PhotoFileName);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                return Ok("Added Successfully!!");
            }
            catch (Exception)
            {
                return BadRequest("Failed to Add!!");
            }
        }

        [HttpPut]
        public IActionResult Put(Employee emp)
        {
            try
            {
                string query = @"
                    update dbo.Employee set 
                    EmployeeName=@EmployeeName,
                    Department=@Department,
                    DateOfJoining=@DateOfJoining,
                    PhotoFileName=@PhotoFileName
                    where EmployeeId=@EmployeeId
                    ";

                DataTable table = new DataTable();
                using (var con = new SqlConnection(_configuration.GetConnectionString("AngularNETCoreEmployeesDB")))
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@EmployeeId", emp.EmployeeId);
                    cmd.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
                    cmd.Parameters.AddWithValue("@Department", emp.Department);
                    cmd.Parameters.AddWithValue("@DateOfJoining", emp.DateOfJoining);
                    cmd.Parameters.AddWithValue("@PhotoFileName", emp.PhotoFileName);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                return Ok("Updated Successfully!!");
            }
            catch (Exception)
            {
                return BadRequest("Failed to Update!!");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                string query = @"
                    delete from dbo.Employee 
                    where EmployeeId=@EmployeeId
                    ";

                DataTable table = new DataTable();
                using (var con = new SqlConnection(_configuration.GetConnectionString("AngularNETCoreEmployeesDB")))
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@EmployeeId", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                return Ok("Deleted Successfully!!");
            }
            catch (Exception)
            {
                return BadRequest("Failed to Delete!!");
            }
        }

        [HttpGet("GetAllDepartmentNames")]
        public IActionResult GetAllDepartmentNames()
        {
            string query = @"
                    select DepartmentName from dbo.Department";

            DataTable table = new DataTable();
            using (var con = new SqlConnection(_configuration.GetConnectionString("AngularNETCoreEmployeesDB")))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                da.Fill(table);
            }

            return Ok(table);
        }

        [HttpPost("SaveFile")]
        public IActionResult SaveFile()
        {
            try
            {
                var httpRequest = HttpContext.Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = Path.Combine(Directory.GetCurrentDirectory(), "Photos", filename);

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return Ok(filename);
            }
            catch (Exception)
            {
                return BadRequest("anonymous.png");
            }
        }

    }
}
