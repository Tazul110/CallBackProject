using System.Net.Sockets;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AirlineAPI.Models
{
    public partial class HappyWorldTicketInfoDto
    {
        [JsonProperty("TicketTime")]
        public string TicketTime { get; set; }

        [JsonProperty("ticketInfo")]
        public TicketInfo TicketInfo { get; set; }
    }

    public partial class TicketInfo
    {
        [JsonProperty("businessOrderNo")]
        public string BusinessOrderNo { get; set; }

        [JsonProperty("orderNo")]
        public string OrderNo { get; set; }

        [JsonProperty("ticketPassengers")]
        public List<TicketPassenger> TicketPassengers { get; set; }
    }

    public partial class TicketPassenger
    {
        [JsonProperty("ageType")]
        public long AgeType { get; set; }

        [JsonProperty("cardNum")]
        public string CardNum { get; set; }

        [JsonProperty("cardType")]
        public long CardType { get; set; }

        [JsonProperty("encryptCardNum")]
        public string EncryptCardNum { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("tickets")]
        public List<Ticket> Tickets { get; set; }

        [JsonProperty("uniqueKey")]
        public long UniqueKey { get; set; }
    }

    public partial class Ticket
    {
        [JsonProperty("segmentIndex")]
        public SegmentIndex SegmentIndex { get; set; }

        [JsonProperty("ticketNo")]
        public string TicketNo { get; set; }
    }

    public partial class SegmentIndex
    {
        [JsonProperty("flightNum")]
        public string FlightNum { get; set; }

        [JsonProperty("segmentType")]
        public long SegmentType { get; set; }

        [JsonProperty("sequenceNum")]
        public long SequenceNum { get; set; }
    }


}
