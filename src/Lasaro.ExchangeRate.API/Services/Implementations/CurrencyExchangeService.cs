using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lasaro.ExchangeRate.API.Models;
using Lasaro.ExchangeRate.API.Services.Abstractions;
using Lasaro.ExchangeRate.Data.Entities;
using Lasaro.ExchangeRate.Data.Repositories.Abstractions;
using Microsoft.VisualBasic;

namespace Lasaro.ExchangeRate.API.Services.Implementations
{
    public class CurrencyExchangeService : ICurrencyExchangeService
    {
        public ICurrencyExchangeTransactionsRepository TransactionsRepository { get; }
        public IRatesRepository RatesRepository { get; }

        public CurrencyExchangeService(ICurrencyExchangeTransactionsRepository transactionsRepository,
                                       IRatesRepository ratesRepository)
        {
            TransactionsRepository = transactionsRepository;
            RatesRepository = ratesRepository;
        }

        public async Task<double> GetUserRemainingLimitAsync(int userId, string currencyCode, DateTime transactionDate)
        {
            double? currencyTransactionLimitPerMonth = await TransactionsRepository.GetCurrencyTransactionLimitAsync(currencyCode);

            if (!currencyTransactionLimitPerMonth.HasValue)
                throw new InvalidOperationException($"Currency transaction limite not found for currency code {currencyCode}");

            double amountAlreadyExchanged = await TransactionsRepository.GetUserAmountExchangedPerMonthAsync(userId, currencyCode, transactionDate);

            return currencyTransactionLimitPerMonth.Value - amountAlreadyExchanged;
        }

        public double CalculateForeignCurrencyAmount(CurrencyExchangeDirection direction, double localCurrencyAmount, double sellValue, double buyValue)
        {
            double effectiveRate = 0;

            if (direction == CurrencyExchangeDirection.BuyForeign)
            {
                //Client if buying from the market, it will pay the value the market is selling
                effectiveRate = sellValue;
            }
            else if (direction == CurrencyExchangeDirection.SellForeign)
            {
                //Client is selling to the market, it will receive the value the market is buying
                effectiveRate = 1 / buyValue;
            }
            else
            {
                throw new NotImplementedException("Invalid exchange direction!");
            }

            double ForeignCurrencyAmount = localCurrencyAmount / effectiveRate;

            return ForeignCurrencyAmount;
        }

        public async Task<CurrencyExchangeTransactionModel> SubmitTransactionAsync(CurrencyExchangeModel currencyExchangeModel,
                                                      double foreignCurrencyAmount,
                                                      DateTime transactionDate,
                                                      int rateId)
        {
            Rate rate = await RatesRepository.GetRateByIdAsync(rateId);

            CurrencyExchangeTransaction transaction = new CurrencyExchangeTransaction()
            {
                LocalCurrencyAmount = currencyExchangeModel.LocalCurrencyAmount,
                ForeignCurrencyAmount = foreignCurrencyAmount,
                ForeignCurrencyCode = currencyExchangeModel.ForeignCurrencyCode,
                Rate = rate,
                TransactionDate = transactionDate,
                UserId = currencyExchangeModel.UserId
            };

            TransactionsRepository.AddTransaction(transaction);

            await TransactionsRepository.SaveChangesAsync();

            return new CurrencyExchangeTransactionModel()
            {
                Id = transaction.Id,
                UserId = transaction.UserId,
                LocalCurrencyAmount = transaction.LocalCurrencyAmount,
                ForeignCurrencyAmount = transaction.ForeignCurrencyAmount,
                ForeignCurrencyCode = transaction.ForeignCurrencyCode,
                TransactionDate = transaction.TransactionDate,
            };
        }

        public async Task<List<CurrencyExchangeTransactionModel>> GetTransactionsAsync()
        {
            List<CurrencyExchangeTransaction> transactions = await TransactionsRepository.GetTransactionsAsync();

            List<CurrencyExchangeTransactionModel> transactionModels = new List<CurrencyExchangeTransactionModel>();

            foreach (CurrencyExchangeTransaction transaction in transactions)
            {
                transactionModels.Add(new CurrencyExchangeTransactionModel()
                {
                    Id = transaction.Id,
                    UserId = transaction.UserId,
                    LocalCurrencyAmount = transaction.LocalCurrencyAmount,
                    ForeignCurrencyAmount = transaction.ForeignCurrencyAmount,
                    ForeignCurrencyCode = transaction.ForeignCurrencyCode,
                    TransactionDate = transaction.TransactionDate,
                });
            }

            return transactionModels;
        }
    }
}
