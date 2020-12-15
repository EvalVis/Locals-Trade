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
                adapter.SelectCommand = new SqlCommand("Select BusinessId, UserId, Description, Header, Picture, PhoneNumber, Latitude, Longitude FROM dbo.Business", connection);
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
                Picture = row.Field<string>("Longitude"),
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
    }
}
