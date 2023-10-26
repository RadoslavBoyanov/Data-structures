using System;
using System.Collections.Generic;

namespace BitcoinWalletManagementSystem
{
    public class Transaction
    {
        //public Transaction()
        //{
        //    this.Users = new List<User>();
        //}

        public string Id { get; set; }

        public string SenderWalletId { get; set; }

        public string ReceiverWalletId { get; set; }

        public long Amount { get; set; }

        public DateTime Timestamp { get; set; }

        //public ICollection<User> Users { get; set; }
    }
}

