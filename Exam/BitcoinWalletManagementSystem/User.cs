using System.Collections.Generic;

namespace BitcoinWalletManagementSystem
{
    public class User
    {
        public User()
        {
            this.Wallets = new HashSet<Wallet>();
            this.Transactions = new List<Transaction>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public ICollection<Wallet> Wallets { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}