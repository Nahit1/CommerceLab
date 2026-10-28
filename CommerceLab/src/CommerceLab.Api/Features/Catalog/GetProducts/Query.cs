using CommerceLab.Api.Common.Results;
using MediatR;

namespace CommerceLab.Api.Features.Catalog.GetProducts;

public sealed record Query
    : IRequest<Result<List<Response>>>;