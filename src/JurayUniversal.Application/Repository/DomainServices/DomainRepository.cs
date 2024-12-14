using JurayUniversal.Application.Dtos;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Application.Repository.DomainServices
{
    public class DomainRepository : IDomainRepository
    {

        private readonly string _connectionString;

        public DomainRepository()
        {
            //_connectionString = "Integrated Security=true;Server=JSS\SQLEXPRESS;Database=DomainBase;";
            _connectionString = "Server=SQL5110.site4now.net;Database=db_a64c98_masterdomain; User Id=db_a64c98_masterdomain_admin;Password=Peter@247;";
        }


        public async Task<List<DomainListDto>> GetAllDomains()
        {
            List<DomainListDto> domains = new List<DomainListDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "SELECT Domain, BaseCompanyUrl FROM DataDomainBase";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    string domainValue = reader.GetString(0);
                                    string baseDomainValue = reader.GetString(1);


                                    DomainListDto domain = new DomainListDto
                                    {
                                        Domain = domainValue,
                                        BaseDomain = baseDomainValue
                                    };
                                    domains.Add(domain);


                                }
                            }
                        }
                    }
                }

                return await Task.FromResult(domains);
            }
            catch (Exception ex)
            {
                // Handle the exception here or log it for debugging purposes
                Console.WriteLine("An error occurred while fetching domains: " + ex.Message);
                return null; // Return null or an empty list as appropriate for your application
            }
        }



        public async Task<bool> AddDomain(DomainDataDto data)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO DataDomainBase (Domain, BaseCompanyUrl, Date) " +
                                   "VALUES (@Domain, @BaseCompanyUrl, @Date)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Domain", data.Domain);
                        command.Parameters.AddWithValue("@BaseCompanyUrl", data.BaseCompanyUrl);
                        command.Parameters.AddWithValue("@Date", data.Date);

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here or log it for debugging purposes
                Console.WriteLine("An error occurred while adding the domain: " + ex.Message);
                return false;
            }
        }

 
        public async Task<DomainDataDto> GetDomain(long id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "SELECT Domain, BaseCompanyUrl, Date FROM DataDomainBase WHERE Id = @Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows && await reader.ReadAsync())
                            {
                                DomainDataDto domain = new DomainDataDto
                                {
                                    Id = id,
                                    Domain = reader.GetString(0),
                                    BaseCompanyUrl = reader.GetString(1),
                                    Date = reader.GetDateTime(2)
                                };
                                return domain;
                            }
                        }
                    }
                }

                return null; // If the domain with the specified id is not found
            }
            catch (Exception ex)
            {
                // Handle the exception here or log it for debugging purposes
                Console.WriteLine("An error occurred while fetching the domain: " + ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateDomain(DomainDataDto data)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "UPDATE DataDomainBase " +
                                   "SET Domain = @Domain, BaseCompanyUrl = @BaseCompanyUrl, Date = @Date " +
                                   "WHERE Id = @Id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", data.Id);
                        command.Parameters.AddWithValue("@Domain", data.Domain);
                        command.Parameters.AddWithValue("@BaseCompanyUrl", data.BaseCompanyUrl);
                        command.Parameters.AddWithValue("@Date", data.Date);

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here or log it for debugging purposes
                Console.WriteLine("An error occurred while updating the domain: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteDomain(long id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "DELETE FROM DataDomainBase WHERE Id = @Id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here or log it for debugging purposes
                Console.WriteLine("An error occurred while deleting the domain: " + ex.Message);
                return false;
            }
        }



    }
}
