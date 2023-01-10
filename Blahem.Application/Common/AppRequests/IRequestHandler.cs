namespace Blahem.Application.Common.AppRequests;

public interface IRequestHandler<TRequest, TResult>
{
    Task<AppResponse<TResult>> Handle(TRequest request, CancellationToken cancellationToken);
}

public interface IRequestHandler<TRequest>
{
    Task<AppResponse> Handle(TRequest request, CancellationToken cancellationToken);
}
