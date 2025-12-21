using System;
namespace EquityPositionAPI.Models
{
	public class TrafTestDatabaseSettings: ITrafTestDatabaseSettings
    {
        public string PositionCollectionName { get; set; } = String.Empty;
        public string TransactionCollectionName { get; set; } = String.Empty;
        public string ConnectionString { get; set; } = String.Empty;
        public string DatabaseName { get; set; } = String.Empty;
    }
}

