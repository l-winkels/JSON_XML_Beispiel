using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ValueObjects;

namespace MVCServer.Controllers
{
    [ApiController]
    [Route("adressdata")]
    public class AddressDataContoller : ControllerBase
    {
        private AddressData _addressData = new AddressData("Max", "Musermann", "Musterweg 1", "Musterort", "Germany", "12345");

        private readonly ILogger<AddressDataContoller> _logger;

        public AddressDataContoller(ILogger<AddressDataContoller> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public AddressData Get()
        {
            return _addressData;
        }
    }
}
