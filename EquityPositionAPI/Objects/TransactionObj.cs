using System;
using MongoDB.Bson.Serialization.Attributes;

namespace EquityPositionAPI.Objects
{
	public class TransactionObj
	{
        public string SecurityCode { get; set; } = string.Empty;

        public string Action { get; set; } = string.Empty;

        public int Qty { get; set; }

        public string Buy_Sell { get; set; } = string.Empty;
    }
}

