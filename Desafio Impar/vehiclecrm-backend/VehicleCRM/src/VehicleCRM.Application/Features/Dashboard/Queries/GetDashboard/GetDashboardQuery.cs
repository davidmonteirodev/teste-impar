using MediatR;

namespace VehicleCRM.Application.Features.Dashboard.Queries
{
    public sealed record GetDashboardQuery : IRequest<DashboardResponse>
    {
    }
}
