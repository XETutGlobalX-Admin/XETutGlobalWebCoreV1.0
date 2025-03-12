using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json;

namespace XETutGlobalX.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class XETutGlobalXMRXChangeController : ControllerBase
    {
        private readonly IConfiguration _config;
        public XETutGlobalXMRXChangeController(IConfiguration config)
        {
            _config = config;
        }
        // GET: api/Configuration
        [Route("api/DBConfigruation/GetDBConnect")]
        [HttpGet]
        public string GetDBConnect()
        {
            var connectionstring = _config.GetValue<string>("XETutGlobalX_DBSetting:DBConnectionString"); // "Information"
            string jsonconnect = JsonSerializer.Serialize(new
            {
                DBConnect = connectionstring,
            });
            return jsonconnect;
        }

        
    }
}
