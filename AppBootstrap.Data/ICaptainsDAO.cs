using AppBootstrap.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBootstrap.Data
{
    public interface ICaptainsDAO : IBaseDAO<Captain>
    {
        Task<Captain> GetLastCaptain();
    }
}
