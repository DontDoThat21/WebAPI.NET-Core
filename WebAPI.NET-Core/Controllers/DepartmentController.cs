using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult<DataTable> Get()
        {
            string query = @"
                            select DepartmentId, DepartmentName from
                            dbo.Departments
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("AngularNETCoreEmployeesDB");
            using (SqlConnection con = new SqlConnection(sqlDataSource))
            using (SqlCommand cmd = new SqlCommand(query, con))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                da.Fill(table);
            }

            return Ok(table);
        }

        [HttpPost]
        public ActionResult<string> Post(Department department)
        {
            try
            {
                string query = @"
                                insert into dbo.Departments (DepartmentName) values
                                (@DepartmentName)
                                ";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("AngularNETCoreEmployeesDB");
                using (SqlConnection con = new SqlConnection(sqlDataSource))
                using (SqlCommand cmd = new SqlCommand(query, con))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }

                return Ok("Added Successfully!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failure: {ex.Message}");
            }
        }

        [HttpPut]
        public ActionResult<string> Put(Department department)
        {
            try
            {
                string query = @"
                                update dbo.Departments set DepartmentName = @DepartmentName
                                where DepartmentId = @DepartmentId
                                ";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("AngularNETCoreEmployeesDB");
                using (SqlConnection con = new SqlConnection(sqlDataSource))
                using (SqlCommand cmd = new SqlCommand(query, con))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddWithValue("@DepartmentId", department.DepartmentId);
                    cmd.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }

                return Ok("Updated Successfully!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failure updating: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<string> Delete(int id)
        {
            try
            {
                string query = @"
                                delete from dbo.Departments
                                where DepartmentId = @DepartmentId
                                ";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("AngularNETCoreEmployeesDB");
                using (SqlConnection con = new SqlConnection(sqlDataSource))
                using (SqlCommand cmd = new SqlCommand(query, con))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddWithValue("@DepartmentId", id);
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }

                return Ok("Deleted Successfully!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failure deleting: {ex.Message}");
            }
        }
    }
}
