using Microsoft.Data.SqlClient;

namespace WebApplication1.Repositories;

public class OrderRepository
{
    private readonly IConfiguration _configuration;
    public OrderRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<bool> DoesOrderExist(int idProduct, int amount)
    {
        var query = "SELECT 1 FROM [Order] WHERE IdProduct = @idProduct AND  amount = @amount;";

        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();
        

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@idProduct", idProduct);
        command.Parameters.AddWithValue("@amount", amount);

        await connection.OpenAsync();

        var result = await command.ExecuteScalarAsync();
        
        return result is not null;
    }

    public async Task<bool> DoesCreatedDateIsEarlierThanReqDate(DateTime reqDateTime)
    {
        var query = "SELECT 1 FROM [Order] WHERE CreatedAt < @reqDate;";
        
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();
        

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@reqDate", reqDateTime);
    

        await connection.OpenAsync();

        var result = await command.ExecuteScalarAsync();
        
        return result is not null;
    }

    public async Task<bool> DoesOrderFulfilled(int idProduct)
    {
        var query = "SELECT 1 FROM [Order] WHERE idProduct = @idProd AND FullfilledAt is not null;";
        
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();
        

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@idProd",idProduct );
    

        await connection.OpenAsync();

        var result = await command.ExecuteScalarAsync();
        
        return result is not null;
    }

    public async Task<int> GetOrderID(int idProduct)
    {
        var query = "SELECT IdOrder FROM [Order] WHERE IdProduct = @IdProd;";
        
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();
        

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@idProd",idProduct );
    

        await connection.OpenAsync();

        object? result = await command.ExecuteScalarAsync();
        
        return (int)result;
    }

    public async Task updateFulFillRow(int idOrder)
    {
        DateTime now = DateTime.Now;
        var query = "UPDATE [Order] SET FulfilledAt = @FulfilledAt WHERE IdOrder = @IdOrder;";
        
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();
        

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@idOrder",idOrder );
        command.Parameters.AddWithValue("@FulfilledAt", now);
    

        await connection.OpenAsync();

        await command.ExecuteNonQueryAsync();
    }
}