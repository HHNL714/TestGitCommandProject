using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PScheduler.Domain.IRepository
{
    public interface IHelperService
    {
        Task AddPScheduleService(string schedule);
    }
}
