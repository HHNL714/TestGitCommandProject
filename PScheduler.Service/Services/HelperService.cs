using Dapper;
using Npgsql;
using PScheduler.Domain.IRepository;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PScheduler.Service.Services
{
    public class HelperService : IHelperService
    {
        private static ILogger _logger;
        
        public HelperService(ILogger logger) {
            _logger = logger;   
        }

        public static IDbConnection OpenConnection(string connStr)
        {
            var conn = new NpgsqlConnection(connStr);
            conn.Open();
            return conn;
        }


        public async Task AddPScheduleService(string schedule)
        {
            try
            {
                try
                {

                    var _connStr = "Server=dev-db-instance-1.c69vcorhtzom.ap-southeast-1.rds.amazonaws.com;Port=5432;Userid=devadmin;Password=Dev123321;Database=SmsGateway;";
                    //2.update  
                    using (var conn = OpenConnection(_connStr))
                    {
                        var Id = Guid.NewGuid();
                        var insertSQL = string.Format(@"INSERT INTO public." + "\"Testing\"" + "(\"Id\", \"Name\", \"Code\", \"IsDeleted\", \"CreatedBy\", \"CreatedDate\", \"UpdatedBy\", \"UpdatedDate\")"
                                        + "VALUES('" + Id + "', 'TEST3', 'T003', false, '1', '2015-05-29 05:50:06+00', null, null)");
                        var res = conn.Execute(insertSQL);
                        Console.WriteLine(res > 0 ? $"{schedule} successfully!" : "insert failure");
                    }
                }
                catch
                {
                    throw;
                }
                _logger.Information($"{DateTime.Now}: The PerformService() is called with {schedule} schedule");
            }
            catch (Exception ex)
            {
                _logger.Error($"{DateTime.Now}: Exception is occured at PerformService(): {ex.Message}");
                throw new Exception(ex.Message);
            }
        }
    }
}
