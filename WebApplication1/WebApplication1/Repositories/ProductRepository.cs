using Microsoft.Data.SqlClient;

namespace WebApplication1.Repositories;

public class ProductRepository
{
    private readonly IConfiguration _configuration;
    public ProductRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> DoesProductExist(int id)
    {
        var query = "Select 1 from Product where idProduct = @id";

        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();
        

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@id", id);

        await connection.OpenAsync();

        var result = await command.ExecuteScalarAsync();
        
        return result is not null;
    }
    public async Task<decimal> GetProdPrice(int idProduct)
    {
        var query = "SELECT Price FROM Product WHERE IdProduct = @IdProd;";
        
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();
        

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@idProd",idProduct );
    

        await connection.OpenAsync();

        object? result = await command.ExecuteScalarAsync();
        
        decimal price = (decimal) result;
        return price;
    }
}