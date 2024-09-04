using System.Net.Sockets;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AirlineAPI.Models
{
   
        public class HappyWorldTicketInfoDto
        {
            public string TicketTime { get; set; }
            public TicketInfo TicketInfo { get; set; }
        }

        public class TicketInfo
        {
            public string BusinessOrderNo { get; set; }
            public string OrderNo { get; set; }
            public List<TicketPassenger> TicketPassengers { get; set; }
        }

        public class TicketPassenger
        {
            public int AgeType { get; set; }
            public string CardNum { get; set; }
            public string CardType { get; set; }
            public string EncryptCardNum { get; set; }
            public string Name { get; set; }
            public List<Ticket> Tickets { get; set; }
            public int UniqueKey { get; set; }
        }

        public class Ticket
        {
            public SegmentIndex SegmentIndex { get; set; }
            public string TicketNo { get; set; }
        }

        public class SegmentIndex
        {
            public string FlightNum { get; set; }
            public int SegmentType { get; set; }
            public int SequenceNum { get; set; }
        }


    }
