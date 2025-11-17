using PScheduler.Domain.IRepository;
using PScheduler.Domain.IService;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PScheduler.Service.Services
{

    public class ScheduleJobService : IScheduleJobService
    {
        public IHelperService _helperService;
        public ScheduleJobService(IHelperService helperService)
        {
            _helperService = helperService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await _helperService.AddPScheduleService("Add Data in Table");
        }
    }
}
