using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;



namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        public readonly IConfiguration _configuration;
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        [HttpGet]

        public JsonResult Get()
        {
            string query = @"
                        select Id,name,email,password,role from 
                       user
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("energyp");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(User user)
        {
            string query = @"
                        insert into user (name,email,password,role) values
                                                    (@name,@email,@password,@role);
                        
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("energyp");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@name", user.name);
                    myCommand.Parameters.AddWithValue("@email", user.email);
                    myCommand.Parameters.AddWithValue("@password", user.password);
                    myCommand.Parameters.AddWithValue("@role", user.role);


                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }


        [HttpPut("{id}")]
        public JsonResult Put(User user)
        {
            string query = @"
                        update user set 
                        name =@name,
                        email =@email,
                        password =@password,
                        role =@role
                        where Id=@Id;
                        
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("energyp");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@Id", user.Id);
                    myCommand.Parameters.AddWithValue("@name", user.name);
                    myCommand.Parameters.AddWithValue("@email", user.email);
                    myCommand.Parameters.AddWithValue("@password", user.password);
                    myCommand.Parameters.AddWithValue("@role", user.role);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Updated Successfully");
        }



        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                        delete from user 
                        where Id=@Id;
                        
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("energyp");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@Id", id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Deleted Successfully");
        }



        /*   [HttpPost]
           [Route("login")]
           public JsonResult login(Login log)
           {
               string query = @"
                           select * from user
                           where email =@email and password=@password;
                ";
               DataTable table = new DataTable();
               string sqlDataSource = _configuration.GetConnectionString("energyp");
               MySqlDataReader myReader;
               using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
               {
                   mycon.Open();
                   using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                   {
                       myCommand.Parameters.AddWithValue("@email", log.email);
                       myCommand.Parameters.AddWithValue("@password", log.password);
                       myReader = myCommand.ExecuteReader();
                       table.Load(myReader);
                       myReader.Close();
                       mycon.Close();
                   }
               }
               return new JsonResult(table);
           }
   */

        [HttpPost]
        [Route("login")]
        public string login(User user)
        {
            string query = @"
                        select * from user
                        where email =@email and role= @role and password=@password;
             ";

            string msg = string.Empty;
            try
            {
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("energyp");
                MySqlDataReader myReader;
                using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
                {
                    mycon.Open();
                    using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                    {
                        myCommand.Parameters.AddWithValue("@email", user.email);
                        myCommand.Parameters.AddWithValue("@password", user.password);
                        myCommand.Parameters.AddWithValue("@role", user.role);

                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);

                        if (table.Rows.Count > 0)
                        {
                            msg ="User valid";
                        }
                        else
                        {
                            msg ="invalid user";
                        }

                        myReader.Close();
                        mycon.Close();
                    }

                }

            }
            catch (Exception e)
            {
                msg = e.Message;
            }
            return msg;
        }
    }
}