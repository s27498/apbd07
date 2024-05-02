using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs;

public class AddProductToWarehouse
{
    [Required]
    public int IdProduct { get; set; }
    [Required]
    public int IdWarehouse { get; set; }
    
    [Range(1,Int32.MaxValue)]
    public int Amount { get; set; }
    
    public DateTime CreatedAt { get; set; }

}