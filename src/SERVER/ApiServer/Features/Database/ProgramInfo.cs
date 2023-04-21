using MediatR;
using Smart.Kh2Ems.EF.Core.Infrastructure.Reverse;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Database;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;

namespace ApiServer.Features.Database;


public class ProgramInfoModel
{
    public class Command : IRequest<Response>
    {
        public int ComputerId { get; set; }
    }

    public class Response : BaseResponse
    {
        public List<ProgramModel>? Datas { get; set; }
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
                var programs = _context.ProgramInfoViews.Select(x => new ProgramModel
                {
                    ProgramId = x.ProgramId,
                    Name = x.Name,
                    ComputerId = x.ComputerFk,
                    ComputerGroupId = x.ComputerGroupFk,
                    ProgramTypeId = x.ProgramTypeFk,
                    ProgramNo = x.ProgramNo,
                    ExecuteType = x.ExecuteType,
                    AlarmPriorityId = x.AlarmPriorityFk,
                    StateGroupId = x.StateGroupFk,
                    StartCmd = x.StartCmd,
                    StopCmd = x.StopCmd,
                    UpdatePeriod = x.UpdatePeriod,
                    UseFlag = x.UseFlag,
                    ProcFullName = x.ProcFullName,
                    ProgramDesc = x.ProgramDesc,
                    DpName = x.DpName,
                    DpType = x.DpType
                });

                if(request.ComputerId == 0)
                {
                    response.Datas = programs.ToList();
                }
                else
                {
                    response.Datas = programs.Where(x => x.ComputerId == request.ComputerId).ToList();
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