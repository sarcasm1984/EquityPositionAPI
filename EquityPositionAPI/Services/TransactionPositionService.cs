using System;
using EquityPositionAPI.Models;
using EquityPositionAPI.Objects;
using MongoDB.Driver;

namespace EquityPositionAPI.Services
{
	public class TransactionPositionService: ITransactionPosition
	{
        private IMongoCollection<Position> _position;
        private IMongoCollection<Transaction> _transaction;

        public TransactionPositionService(ITrafTestDatabaseSettings trafTestDatabaseSettings,
            IMongoClient mongoClient)
		{
            var database = mongoClient.GetDatabase(trafTestDatabaseSettings.DatabaseName);
            _position = database.GetCollection<Position>(trafTestDatabaseSettings.PositionCollectionName);
            _transaction = database.GetCollection<Transaction>(trafTestDatabaseSettings.TransactionCollectionName);
		}

        public Position Create(TransactionObj transaction)
        {
            var tempPos = _position.Find(pos => pos.SecurityCode == transaction.SecurityCode).FirstOrDefault();
            var trans = new Transaction();
            if (tempPos != null)
            {
                if (transaction.Buy_Sell == "BUY")
                    tempPos.Value += transaction.Qty;
                else if (transaction.Buy_Sell == "SELL")
                    tempPos.Value -= transaction.Qty;
                tempPos.TradeVersion = 1;
                tempPos.TradeId = tempPos.TradeId + 1;
                trans.TradeId = tempPos.TradeId;
                trans.TradeVersion = tempPos.TradeVersion;
                trans.Action = transaction.Action;
                trans.Buy_Sell = transaction.Buy_Sell;
                trans.SecurityCode = transaction.SecurityCode;
                trans.Qty = transaction.Qty;
                _position.ReplaceOne(p => p.SecurityCode == transaction.SecurityCode, tempPos);
                _transaction.InsertOne(trans);
                return tempPos;
            }
            else
            {
                var pos = new Position();
                pos.SecurityCode = transaction.SecurityCode;
                pos.Value = (transaction.Buy_Sell == "BUY") ? transaction.Qty : -transaction.Qty;
                pos.TradeId = 1;
                pos.TradeVersion = 1;
                trans.TradeId = pos.TradeId;
                trans.TradeVersion = pos.TradeVersion;
                trans.Action = transaction.Action;
                trans.Buy_Sell = transaction.Buy_Sell;
                trans.SecurityCode = transaction.SecurityCode;
                trans.Qty = transaction.Qty;
                _position.InsertOne(pos);
                _transaction.InsertOne(trans);
                return pos;
            }
        }

        public List<Position> Get()
        {
            return _position.Find(pos => true).ToList();
        }

        public Position Get(string secCode)
        {
            return _position.Find(pos => pos.SecurityCode == secCode).FirstOrDefault();
        }

        public List<Transaction> GetHistory()
        {
            return _transaction.Find(t => true).ToList();
        }

        public List<Transaction> GetHistory(string secCode)
        {
            return _transaction.Find(t => t.SecurityCode == secCode).ToList();
        }

        public void Remove(TransactionObj transaction)
        {
            var tempPos = _position.Find(p => p.SecurityCode == transaction.SecurityCode).FirstOrDefault();
            var pos = new Position();
            var trans = new Transaction();
            pos.SecurityCode = transaction.SecurityCode;
            pos.Value = 0;
            pos.TradeVersion = tempPos.TradeVersion + 1;
            pos.TradeId = tempPos.TradeId;
            trans.TradeId = pos.TradeId;
            trans.TradeVersion = pos.TradeVersion;
            trans.Action = transaction.Action;
            trans.Buy_Sell = transaction.Buy_Sell;
            trans.SecurityCode = transaction.SecurityCode;
            trans.Qty = transaction.Qty;
            _position.ReplaceOne(p => p.SecurityCode == transaction.SecurityCode, pos);
            _transaction.InsertOne(trans);
        }

        public void Update(TransactionObj transaction)
        {
            var trans = new Transaction();
            var tempPos = _position.Find(pos => pos.SecurityCode == transaction.SecurityCode).FirstOrDefault();
            if (tempPos == null)
            {
                trans.Action = "INSERT";
                var pos = new Position();
                pos.SecurityCode = transaction.SecurityCode;
                pos.Value = (transaction.Buy_Sell == "BUY") ? transaction.Qty : -transaction.Qty;
                pos.TradeId = 1;
                pos.TradeVersion = 1;
                trans.TradeId = pos.TradeId;
                trans.TradeVersion = pos.TradeVersion;
                trans.Action = transaction.Action;
                trans.Buy_Sell = transaction.Buy_Sell;
                trans.SecurityCode = transaction.SecurityCode;
                trans.Qty = transaction.Qty;
                _position.InsertOne(pos);
            }
            else
            {
                var lastTrans = _transaction.Find(t => t.TradeId == tempPos.TradeId && t.TradeVersion == tempPos.TradeVersion
                    && t.SecurityCode == tempPos.SecurityCode).FirstOrDefault();
                if(lastTrans.Action == "CANCEL")
                {
                    var pos = new Position();
                    pos.SecurityCode = transaction.SecurityCode;
                    pos.Value = (transaction.Buy_Sell == "BUY") ? transaction.Qty : -transaction.Qty;
                    pos.TradeId = tempPos.TradeId + 1;
                    pos.TradeVersion = 1;
                    trans.TradeId = pos.TradeId;
                    trans.TradeVersion = pos.TradeVersion;
                    trans.Action = "INSERT";
                    trans.Buy_Sell = transaction.Buy_Sell;
                    trans.SecurityCode = transaction.SecurityCode;
                    trans.Qty = transaction.Qty;
                    _position.ReplaceOne(p => p.SecurityCode == transaction.SecurityCode, pos);
                }
                else
                {
                    var pos = new Position();
                    pos.SecurityCode = transaction.SecurityCode;
                    if (transaction.Buy_Sell == "BUY")
                        pos.Value = tempPos.Value + transaction.Qty;
                    else if (transaction.Buy_Sell == "SELL")
                        pos.Value = tempPos.Value - transaction.Qty;
                    pos.TradeId = tempPos.TradeId;
                    pos.TradeVersion = tempPos.TradeVersion + 1;
                    trans.TradeId = pos.TradeId;
                    trans.TradeVersion = pos.TradeVersion;
                    trans.Action = transaction.Action;
                    trans.Buy_Sell = transaction.Buy_Sell;
                    trans.SecurityCode = transaction.SecurityCode;
                    trans.Qty = transaction.Qty;
                    _position.ReplaceOne(p => p.SecurityCode == transaction.SecurityCode, pos);
                }
            }
            _transaction.InsertOne(trans);
        }
    }
}

