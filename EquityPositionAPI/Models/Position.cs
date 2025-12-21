using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace EquityPositionAPI.Models
{
    [BsonIgnoreExtraElements]
	public class Position
	{
        [BsonElement("securitycode")]
        public string SecurityCode { get; set; }

        [BsonElement("value")]
        public int Value { get; set; }

        [BsonElement("tradeid")]
        public int TradeId { get; set; }

        [BsonElement("tradever")]
        public int TradeVersion { get; set; }
    }
}

