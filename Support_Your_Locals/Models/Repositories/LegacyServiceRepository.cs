using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Support_Your_Locals.Models.Repositories
{
    public class LegacyServiceRepository : ILegacyServiceRepository
    {

        private String connectionString;
        
        public LegacyServiceRepository(IConfiguration configuration)
        {
            this.connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }


        public List<Business> GetBusinesses()
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlConnection connection = new SqlConnection(connectionString);
            DataSet ds = new DataSet();
            try
            {
                connection.Open();
                adapter.SelectCommand = new SqlCommand("Select BusinessId, UserId, Description, Header, PictureData, PhoneNumber, Latitude, Longitude FROM dbo.Business", connection);
                adapter.Fill(ds);
            }
            finally
            {
                connection.Close();
                adapter.Dispose();
            }

            return ds.Tables[0].Select().Select(row => new Business()
            {
                BusinessID = row.Field<long>("BusinessId"),
                Description = row.Field<string>("Description"),
                UserID = row.Field<long>("UserId"),
                Header = row.Field<string>("Header"),
                PhoneNumber = row.Field<string>("PhoneNumber"),
                Latitude = row.Field<string>("Latitude"),
                Longitude = row.Field<string>("Longitude"),
                PictureData = row.Field<byte[]>("PictureData"),
                User = GetUser(row.Field<long>("UserId")),
            }).ToList();
        }

        public User GetUser(long userId)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlConnection connection = new SqlConnection(connectionString);
            DataSet ds = new DataSet();
            try
            {
                connection.Open();
                adapter.SelectCommand = new SqlCommand("Select UserID, Email, Name, Surname, BirthDate FROM dbo.Users WHERE UserID = @id", connection);
                adapter.SelectCommand.Parameters.Add("@Id", System.Data.SqlDbType.BigInt).Value = userId;
                adapter.Fill(ds);
            }
            finally
            {
                connection.Close();
                adapter.Dispose();
            }

            return ds.Tables[0].Select().Select(row => new User()
            {
                UserID = row.Field<long>("UserID"),
                Email = row.Field<string>("Email"),
                Name = row.Field<string>("Name"),
                Surname = row.Field<string>("Surname"),
                BirthDate = row.Field<DateTime>("BirthDate")
            }).First();
        }

        public List<User> GetUsers()
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlConnection connection = new SqlConnection(connectionString);
            DataSet ds = new DataSet();
            try
            {
                connection.Open();
                adapter.SelectCommand = new SqlCommand("Select UserID, Email, Name, Surname, BirthDate FROM dbo.Users", connection);
                adapter.Fill(ds);
            }
            finally
            {
                connection.Close();
                adapter.Dispose();
            }

            return ds.Tables[0].Select().Select(row => new User()
            {
                UserID = row.Field<long>("UserID"),
                Email = row.Field<string>("Email"),
                Name = row.Field<string>("Name"),
                Surname = row.Field<string>("Surname"),
                BirthDate = row.Field<DateTime>("BirthDate")
            }).ToList();
        }

        public void DeleteBusiness(long businessId)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                adapter.DeleteCommand = new SqlCommand("DELETE FROM dbo.Business WHERE BusinessID = @Id", connection);
                adapter.DeleteCommand.Parameters.Add("@Id", System.Data.SqlDbType.BigInt).Value = businessId;
                adapter.DeleteCommand.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
                adapter.Dispose();
            }
        }

        public void DeleteUser(long userId)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                adapter.DeleteCommand = new SqlCommand("DELETE FROM dbo.Users WHERE UserId = @Id", connection);
                adapter.DeleteCommand.Parameters.Add("@Id", System.Data.SqlDbType.BigInt).Value = userId;
                adapter.DeleteCommand.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
                adapter.Dispose();
            }
        }

        public List<Product> GetProducts()
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlConnection connection = new SqlConnection(connectionString);
            DataSet ds = new DataSet();
            try
            {
                connection.Open();
                adapter.SelectCommand = new SqlCommand("Select ProductID, Name, PricePerUnit, Unit, Comment, Picture, BusinessID FROM dbo.Products", connection);
                adapter.Fill(ds);
            }
            finally
            {
                connection.Close();
                adapter.Dispose();
            }

            return ds.Tables[0].Select().Select(row => new Product()
            {
                ProductID = row.Field<long>("ProductID"),
                Name = row.Field<string>("Name"),
                PricePerUnit = row.Field<decimal>("PricePerUnit"),
                Unit = row.Field<string>("Unit"),
                Comment = row.Field<string>("Comment"),
                Picture = row.Field<string>("Picture"),
                BusinessID = row.Field<long>("BusinessID"),
            }).ToList();
        }

        public List<Question> GetQuestions()
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlConnection connection = new SqlConnection(connectionString);
            DataSet ds = new DataSet();
            try
            {
                connection.Open();
                adapter.SelectCommand = new SqlCommand("Select QuestionId, Email, Text, IsAnswered, Response FROM dbo.Questions", connection);
                adapter.Fill(ds);
            }
            finally
            {
                connection.Close();
                adapter.Dispose();
            }

            return ds.Tables[0].Select().Select(row => new Question()
            {
                QuestionId = row.Field<long>("QuestionId"),
                Email = row.Field<string>("Email"),
                Text = row.Field<string>("Text"),
                IsAnswered = row.Field<bool>("IsAnswered"),
                Response = row.Field<string>("Response"),
            }).ToList();
        }

        public void AddQuestion(string email, string text)
        {

            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                adapter.InsertCommand = new SqlCommand("insert into dbo.Questions (Email, Text, IsAnswered) values (@Email, @Text, 0)", connection);
                adapter.InsertCommand.Parameters.Add("@Email", System.Data.SqlDbType.VarChar).Value = email;
                adapter.InsertCommand.Parameters.Add("@Text", System.Data.SqlDbType.VarChar).Value = text;
                adapter.InsertCommand.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
                adapter.Dispose();
            }
        }

        public void AnswerQuestion(long questionId, string response)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                adapter.UpdateCommand = new SqlCommand("update dbo.Questions set IsAnswered = 1, Response = @Response where QuestionId = @Id", connection);
                adapter.UpdateCommand.Parameters.Add("@Id", System.Data.SqlDbType.BigInt).Value = questionId;
                adapter.UpdateCommand.Parameters.Add("@Response", System.Data.SqlDbType.VarChar).Value = response;
                adapter.UpdateCommand.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
                adapter.Dispose();
            }
        }
    }
}
