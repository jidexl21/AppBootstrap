using AppBootstrap.Core;
using AppBootstrap.Data;
using AppBootstrap.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBootstrap.Services.Implementation
{
    public class CaptainsService : ICaptainsService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CaptainsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<Captain> GetMyFaveCaptain(long location)
        {
            throw new NotImplementedException();
        }
    }
}
