using System;
namespace EquityPositionAPI.Models
{
	public interface ITrafTestDatabaseSettings
	{
        string PositionCollectionName { get; set; }
        string TransactionCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}

