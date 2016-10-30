using AASC.Partner.API.Configuration.Intel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace AASC.Partner.API.Hubs
{
	[HubName("dataHub")]
    public class DataHub : Hub
    {
        private static int _count = 0;
        public void hit() {
            _count += 1;
            this.Clients.All.onHit(_count);
        }
        public void sendMessage(string message) {
            this.Clients.All.currentMessage(message);
        }

        public void sendLocation(string driverUsername, double x, double y)
        {
            this.Clients.Others.currentLocation(driverUsername, x, y);
        }

    }


    }
}