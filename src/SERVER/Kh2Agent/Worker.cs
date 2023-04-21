using Kh2Agent.Commands;
using MediatR;
using Smart.Kh2Ems.Infrastructure.Api;
using Smart.Kh2Ems.Infrastructure.resources;
using Smart.Kh2Ems.Infrastructure.Shared.Interfaces;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace Kh2Agent;


public class Worker : BackgroundService
{
    private readonly IApiLogger _logger;
    private readonly IConfiguration _configuration;
    private readonly IMediator _mediator;

    public Worker(IApiLogger logger, IConfiguration configuration, IMediator mediator)
    {
        _logger = logger;
        this._configuration = configuration;
        this._mediator = mediator;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var configValue = _configuration.GetSection("AgentSetting");
        int computerId = configValue.GetValue<int>("ComputerId");
        string? apiURL = configValue.GetValue<string>("apiURL");
        string? apiPath = configValue.GetValue<string>("apiPath");
        string cubeBoxName = $"AGENT_P{computerId.ToString("D3")}";

        RestApiClient restApiClient = new RestApiClient(apiURL!);
        string apiFullPath = $"{apiPath}/{cubeBoxName}";
        while (!stoppingToken.IsCancellationRequested)
        {
            // ������
            var httpMessage = await restApiClient.GetAsyncHttp(apiFullPath);
            var jsonString = await httpMessage.Content.ReadAsStringAsync();
            if(jsonString != null)
            {
                var command = new JsonSerializer().JsonStringToData<AgentCommand.Command>(jsonString);
                if (command?.Result??false)
                {
                    _logger.LogInformation($"�޽��� ���� ���� : {command.Result}");

                }
            }
            await Task.Delay(1000, stoppingToken);
        }
    }



    private void ProcessExited(object sender, EventArgs e)
    {
        Process? p = sender as Process;
        if (p != null)
        {
            _logger.LogInformation($"PID:{p.Id} NM:{p.MachineName} S:{p.StartTime} E:{p.ExitTime} has:{p.HasExited} {p.StartInfo.FileName} ����Ǿ����ϴ�.");
            p.Start();
        }
    }
}