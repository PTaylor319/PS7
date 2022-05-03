using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWARM.Server.Controllers.Base
{
    interface IBaseController<T>
    {
        Task<IActionResult> Delete(int Value);
        Task<IActionResult> Get();
        Task<IActionResult> Get(int Value);
        Task<IActionResult> Put([FromBody] T _object);
        Task<IActionResult> Post([FromBody] T _object);
    }
}
