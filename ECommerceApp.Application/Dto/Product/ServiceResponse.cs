using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Application.Dto.Product
{
    public record ServiceResponse(bool Success=false,string Message=null)
    {
    }
}
