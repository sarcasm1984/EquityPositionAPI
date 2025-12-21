using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EquityPositionAPI.Objects;
using EquityPositionAPI.Models;
using EquityPositionAPI.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EquityPositionAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private ITransactionPosition _transactionPosition;
        public TradeController(ITransactionPosition transactionPosition)
        {
            _transactionPosition = transactionPosition;
        }

        [HttpGet("getallPositions")]
        public ActionResult<List<Position>> GetAllPositions()
        {
            return _transactionPosition.Get();
        }

        [HttpGet("getPosition/{secCode}")]
        public ActionResult<Position> GetPositionBySecCode(string secCode)
        {
            var position = _transactionPosition.Get(secCode);

            if (position == null)
            {
                return NotFound($"Position with Security Code = {secCode} not found");
            }

            return position;
        }

        [HttpGet("getallTransactions")]
        public ActionResult<List<Transaction>> GetAllTransactionHistory()
        {
            return _transactionPosition.GetHistory();
        }

        [HttpGet("getTransactions/{secCode}")]
        public ActionResult<List<Transaction>> GetTransactionHistoryBySecCode(string secCode)
        {
            return _transactionPosition.GetHistory(secCode);
        }

        [HttpPost]
        public void PostActions([FromBody]TransactionObj transaction)
        {
            if (transaction.Action == "INSERT")
                _transactionPosition.Create(transaction);
            else if (transaction.Action == "UPDATE")
                _transactionPosition.Update(transaction);
            else if (transaction.Action == "CANCEL")
                _transactionPosition.Remove(transaction);
        }
    }
}

