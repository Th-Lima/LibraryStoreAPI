using LibraryStore.Api.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace LibraryStore.Api.Dtos
{
    [ModelBinder(typeof(ProductModelBinder), Name = "product")]
    public class ProductImageDto : ProductBaseDto<IFormFile>
    {
    }
}
