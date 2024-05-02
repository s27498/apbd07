using Microsoft.Data.SqlClient;

namespace WebApplication1.Repositories;

public class Product_WarehouseRepository
{
    private readonly IConfiguration _configuration;
    public Product_WarehouseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> DoesIdOrderExist(int idOrder)
    {
        var query = "Select 1 from Product_Warehouse where idOrder = @idOrder";

        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();
        

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@id", idOrder);

        await connection.OpenAsync();

        var result = await command.ExecuteScalarAsync();
        
        return result is not null;
    }
    public async Task<int> InsertRecordAndGetPrimaryKey(int idWarehouse, int idProd, int idOrder, int amount, decimal price, DateTime createdAt)
    {
        var query = "INSERT INTO Product_Warehouse(IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt)" +
                    " VALUES(@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, @CreatedAt);" +
                    " SELECT SCOPE_IDENTITY();";

        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@IdWarehouse", idWarehouse);
        command.Parameters.AddWithValue("@IdProduct", idProd);
        command.Parameters.AddWithValue("@IdOrder", idOrder);
        command.Parameters.AddWithValue("@Amount", amount);
        command.Parameters.AddWithValue("@Price", price * amount); 
        command.Parameters.AddWithValue("@CreatedAt", createdAt);

        await connection.OpenAsync();

        // Wykonaj zapytanie i pobierz wartość klucza głównego wygenerowanego
        int primaryKey = Convert.ToInt32(await command.ExecuteScalarAsync());

        return primaryKey;
    }

    
    
    
}