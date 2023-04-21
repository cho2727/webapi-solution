using MediatR;
using Smart.Kh2Ems.EF.Core.Infrastructure.Reverse;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Database;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;

namespace ApiServer.Features.Database;


public class ComputerInfoModel
{
    public class Command : IRequest<Response>
    {
        public int ComputerId { get; set; }
    }

    public class Response : BaseResponse
    {
        public List<CompueterModel>? Datas { get; set; }
    }



    public class CommandHandler : IRequestHandler<Command, Response>
    {
        private readonly KH2emsServerContext _context;

        public CommandHandler(KH2emsServerContext context)
        {
            this._context = context;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var response = new Response { Result = false };
            try
            {
                var computers = _context.ComputerInfoViews.Select(x => new CompueterModel
                {
                    ComputerId = x.ComputerId,
                    Name = x.Name,
                    ComputerGroupId = x.ComputerGroupFk,
                    GroupName = x.GroupName,
                    DupType = x.IsDup,
                    AlarmPriorityFk = x.AlarmPriorityFk,
                    StateGroupFk = x.StateGroupFk,
                    MemberOfficeFk = x.MemberOfficeFk,
                    UseFlag = x.UseFlag,
                    DpName = x.DpName, DpType = x.DpType,
                });

                if(request.ComputerId == 0)
                {
                    response.Datas = computers.ToList();
                }
                else
                {
                    response.Datas = computers.Where(x => x.ComputerId == request.ComputerId).ToList();
                }
                response.Result = true;
            }
            catch (Exception ex)
            {
                response.Error = new Error
                {
                    Code = "02",
                    Message = ex.Message
                };
            }

            return await Task.FromResult(response);
        }
    }
}