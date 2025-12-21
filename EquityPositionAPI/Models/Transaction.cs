using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace EquityPositionAPI.Models
{
    [BsonIgnoreExtraElements]
    public class Transaction
	{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("tradeid")]
        public int TradeId { get; set; }

        [BsonElement("tradever")]
        public int TradeVersion { get; set; }

        [BsonElement("securitycode")]
        public string SecurityCode { get; set; }

        [BsonElement("action")]
        public string Action { get; set; }

        [BsonElement("qty")]
        public int Qty { get; set; }

        [BsonElement("buy_sell")]
        public string Buy_Sell { get; set; }
    }
}

