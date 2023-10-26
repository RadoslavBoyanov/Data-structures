using System;
using System.Collections.Generic;
using System.Linq;

namespace BitcoinWalletManagementSystem
{
    public class BitcoinWalletManager : IBitcoinWalletManager
    {
        private Dictionary<string, Transaction> transactionsById = new Dictionary<string, Transaction>();

        private Dictionary<string, Wallet> walletsById = new Dictionary<string, Wallet>();

        private Dictionary<string, User> usersById = new Dictionary<string, User>();

        public void CreateUser(User user)
        {
            if (!this.usersById.ContainsKey(user.Id))
            {
                this.usersById.Add(user.Id, user);
            }
        }

        public void CreateWallet(Wallet wallet)
        {
            if (!this.walletsById.ContainsKey(wallet.Id))
            {
                this.walletsById.Add(wallet.Id, wallet);
            }
        }

        public bool ContainsUser(User user) => this.usersById.ContainsKey(user.Id);

        public bool ContainsWallet(Wallet wallet) => this.walletsById.ContainsKey(wallet.Id);

        public IEnumerable<Wallet> GetWalletsByUser(string userId)
            => this.walletsById.Values.Where(w => w.UserId == userId);

        public void PerformTransaction(Transaction transaction)
        {
            if (!this.walletsById.ContainsKey(transaction.SenderWalletId) || !this.walletsById.ContainsKey(transaction.ReceiverWalletId) || this.walletsById[transaction.SenderWalletId].Balance <= 0)
            {
                throw new ArgumentException();
            }
            //Create transaction
            if (!this.transactionsById.ContainsKey(transaction.Id))
            {
                this.transactionsById.Add(transaction.Id, transaction);
            }

            //Make transaction between two wallets
            this.walletsById[transaction.ReceiverWalletId].Balance += transaction.Amount;
            this.walletsById[transaction.SenderWalletId].Balance -= transaction.Amount;

            
            //Add transactions for users
            var senderUser = this.walletsById[transaction.SenderWalletId];
            if (this.usersById.ContainsKey(senderUser.UserId))
            {
                this.usersById[senderUser.UserId].Transactions.Add(transaction);
            }
            var receiverUserWallet = this.walletsById[transaction.ReceiverWalletId];
            if (this.usersById.ContainsKey(receiverUserWallet.UserId))
            {
                this.usersById[receiverUserWallet.UserId].Transactions.Add(transaction);
            }

            ////Add ussers in transaction
            //this.transactionsById[transaction.Id].Users.Add(this.usersById[senderUser.UserId]);
            //this.transactionsById[transaction.Id].Users.Add(this.usersById[receiverUserWallet.UserId]);
        }

        public IEnumerable<Transaction> GetTransactionsByUser(string userId)
            => this.usersById[userId].Transactions;

        public IEnumerable<Wallet> GetWalletsSortedByBalanceDescending()
            => this.walletsById.Values.OrderByDescending(x => x.Balance);

        public IEnumerable<User> GetUsersSortedByBalanceDescending()
            => this.usersById.Values.OrderByDescending(u => u.Wallets.Sum(w => w.Balance));

        public IEnumerable<User> GetUsersByTransactionCount()
            => this.usersById.Values.OrderByDescending(u => u.Transactions.Count);
    }
}