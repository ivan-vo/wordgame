using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PrimeService
{
    public class Player
    {
        private string name;         
        public NetworkStream client;
        public Player(string name,NetworkStream client)
        {
            this.name = name;
            this.client = client;
        }
        public string GetName()
        {
            return name;
        }
    }
}
