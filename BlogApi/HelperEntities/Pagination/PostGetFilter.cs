using System.ComponentModel.DataAnnotations;

namespace BlogApi.HelperEntities.Pagination;

public class PostGetFilter : PaginationParams
{
    public string? Tag { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? FromDateTime { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? ToDateTime { get; set; }
}