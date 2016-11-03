﻿using AASC.Partner.API.Configuration.Intel;
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


        // Driver broadcasts a message to all other riders and drivers with its location every x second
        public void driverBroadcastLocation(string driverUsername, string geocoords)
        {
            this.Clients.Others.receiveLocation(driverUsername, geocoords);
        }

        // Rider broadcast to all drivers that it wants to be picked up
        public void pickMeUpSignal(string riderUsername, string geocoords)
        {
            this.Clients.Others.receivePickMeUpSignal(riderUsername, geocoords);
        }








        // Driver broadcasts its location to rider every 3 second
        public void sendLocation(string driverUsername, string geocoords, string riderUsername)
        {
            this.Clients.Others.currentLocation(driverUsername, geocoords, riderUsername);
        }


        // Rider receives all driver location displays all drivers on the map
        public void receiveLocation(string driverUsername, string geocoords)
        {
            this.Clients.Others.receiveLocation(driverUsername, geocoords);
        }


        // Rider displays driver on map
        public void receiveLocation(string driverUsername, string geocoords, string riderUsername)
        {
            this.Clients.Others.receiveLocation(driverUsername, geocoords, riderUsername);
        }





        // Driver signals to rider that it will pick the rider up
        public void driverAgreementSignal(string driverUsername, string riderUsername)
        {
            this.Clients.Others.driverAgreementSignal(driverUsername, riderUsername);
        }


        // Rider handshakes back to driver 
        public void riderAgreementSignal(string riderUsername, string driverUsername)
        {
            this.Clients.Others.riderAgreementSignal(riderUsername, driverUsername);
        }

        // Driver signals to rider that it will pick the rider up
        public void riderIAmHere(string driverUsername, string riderUsername)
        {
            this.Clients.Others.riderIAmHere(driverUsername, riderUsername);
        }

    }

}
