using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ConsoleApp_Core_Logging_DI
{
    public class Service
    {
        private readonly ILogger<Service> _log;
        private readonly IConfiguration _config;

        //We pass the type to the logger so he knows what "he is talking about"
        public Service(ILogger<Service> log, IConfiguration config)
        {
            this._log = log;
            _config = config;
        }

        public void MyService()
        {
            for ( int i = 0; i<_config.GetValue<int>("LoopTimes"); i++)
            {
                _log.LogError("Number {a}",i);
            }
        }

    }

}
