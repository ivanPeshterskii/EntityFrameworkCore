using Microsoft.Data.SqlClient;

namespace ORM_Fundamentals;

class Program
{
    static void Main(string[] args)
    {
        string connectionString =
            @"Server=localhost,1433;Database=SoftUni;User Id=sa;Password=YourStrongPassword123!;TrustServerCertificate=True;";

        string sqlQuery = @"
            SELECT 
                CONCAT([FirstName], ' ', [LastName]) AS [FullName],
                [JobTitle],
                [Salary]
            FROM [Employees]
            WHERE [Salary] > 30000";

        using SqlConnection sqlConnection = new SqlConnection(connectionString);

        sqlConnection.Open();

        using SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);

        using SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

        while (sqlDataReader.Read())
        {
            string fullName = sqlDataReader.GetString(0);
            string jobTitle = sqlDataReader.GetString(1);
            decimal salary = sqlDataReader.GetDecimal(2);

            Console.WriteLine($"# {fullName} - {jobTitle} - {salary}");
        }
    }
}