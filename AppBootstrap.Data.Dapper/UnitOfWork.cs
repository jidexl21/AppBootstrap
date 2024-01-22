using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBootstrap.Data.Dapper
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork()
        {
                
        }
        public ICaptainsDAO CaptainsDAO { get; }

        public void Commit()
        {
            
        }
    }
}
