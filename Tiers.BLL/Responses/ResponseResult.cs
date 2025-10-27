using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiers.BLL.Responses
{
    public record ResponseResult<T>(T Result, string? ErrorMessage, bool IsSuccess);
}
