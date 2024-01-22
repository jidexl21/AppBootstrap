using AppBootstrap.Services;
using MediatR;

namespace AppBootstrap.CQRS
{
    public  static class GetCaptains
    {
        public class GetCaptainsRequest : IRequest<GetCaptainsResponse> { }
        public class GetCaptainsHandler : IRequestHandler<GetCaptainsRequest, GetCaptainsResponse>
        {
            public Task<GetCaptainsResponse> Handle(GetCaptainsRequest request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
        public class GetCaptainsResponse : ApiResponse<CaptainInfo>{ }

        public class CaptainInfo
        {
            public string Name { get; set; }
        }
    }
}