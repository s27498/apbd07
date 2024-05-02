using Microsoft.Data.SqlClient;

namespace WebApplication1.Repositories;

public class WarehouseRepository
{
    private readonly IConfiguration _configuration;
    public WarehouseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> DoesWarehouseExist(int id)
    {
        var query = "Select 1 from Warehouse where idWarehouse = @id";

        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();
        

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@id", id);

        await connection.OpenAsync();

        var result = await command.ExecuteScalarAsync();
        
        return result is not null;
    }
}