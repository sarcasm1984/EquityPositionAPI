using System;
using EquityPositionAPI.Models;
using EquityPositionAPI.Objects;

namespace EquityPositionAPI.Services
{
	public interface ITransactionPosition
	{
		List<Position> Get();
		Position Get(string secCode);
        List<Transaction> GetHistory();
        List<Transaction> GetHistory(string secCode);
        Position Create(TransactionObj transaction);
		void Update(TransactionObj transaction);
		void Remove(TransactionObj transaction);
	}
}

