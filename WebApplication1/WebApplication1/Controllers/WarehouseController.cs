using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarehouseController : ControllerBase
{
    private readonly ProductRepository _productRepository;
    private readonly WarehouseRepository _warehouseRepository;
    private readonly OrderRepository _orderRepository;
    private readonly Product_WarehouseRepository _productWarehouseRepository;

    public WarehouseController(
        ProductRepository productRepository,
        WarehouseRepository warehouseRepository,
        OrderRepository orderRepository,
        Product_WarehouseRepository productWarehouseRepository)
    {
        _productRepository = productRepository;
        _warehouseRepository = warehouseRepository;
        _orderRepository = orderRepository;
        _productWarehouseRepository = productWarehouseRepository;
    }


    [HttpPost]
    public async Task<IActionResult> addProductToWarehouse(AddProductToWarehouse addProductToWarehouse)
    {
        int idProduct = addProductToWarehouse.IdProduct;
        int idWare = addProductToWarehouse.IdWarehouse;
        int amount = addProductToWarehouse.Amount;
        DateTime CreationDate = addProductToWarehouse.CreatedAt;

        int idOrder = await _orderRepository.GetOrderID(idProduct);
        decimal price = await _productRepository.GetProdPrice(idProduct);


        if (!await _productRepository.DoesProductExist(idProduct))
        {
            return NotFound("\"Product with provided id does not exist\"");
        }

        if (!await _warehouseRepository.DoesWarehouseExist(idWare))
        {
            return NotFound("Warehouse with provided id does not exist");
        }

        if (!await _orderRepository.DoesOrderExist(idProduct, amount))
        {
            return NotFound("Provided order does not exist");
        }

        if (!await _orderRepository.DoesCreatedDateIsEarlierThanReqDate(CreationDate))
        {
            return BadRequest("Requested date is earlier than creation date");
        }

        if (!await _orderRepository.DoesOrderFulfilled(idProduct))
        {
            return Conflict("This order has been already fulfilled");
        }

        if (!await _productWarehouseRepository.DoesIdOrderExist(idOrder))
        {
            return NotFound();
        }

        await _orderRepository.updateFulFillRow(idOrder);
        int pk =  await _productWarehouseRepository.InsertRecordAndGetPrimaryKey
            (idWare, idProduct, idOrder, amount, price, CreationDate);
        

        return Ok(pk);
    }
}