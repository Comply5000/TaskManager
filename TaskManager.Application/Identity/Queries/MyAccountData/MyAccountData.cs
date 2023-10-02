using MediatR;

namespace TaskManager.Application.Identity.Queries.MyAccountData;

public sealed record MyAccountData : IRequest<MyAccountDataResponse>;
