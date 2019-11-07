using iBoxDB.LocalServer;
using Plenteum.CryptoNote;
using Plenteum.CryptoNote.Blockchain;
using Plenteum.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plenteum.Database
{
    public class TransactionRepository : BaseRepository, IRepository<Transaction>
    {

        public TransactionRepository(DB db) : base(db)
        {
            
        }

        public bool Add(Transaction item)
        {
            //TODO: Need to add a Block and it's transactions in a batch (Transaction) to avoid possible data loss
            if (item.PaymentId == Constants.NULL_HASH)
            {
                item.PaymentId = ""; //clear this to save on storage space
            }
            AutoBox auto = _db.Open();
            CommitResult cr;
            using (Box box = auto.Cube())
            {
                //set the Tx Id - this is just a random Value for storage purposes
                item.Id = box.NewId();
                    box["Transaction"].Insert<Transaction>(item);
                cr = box.Commit();
            }
            if (cr == CommitResult.OK) {
                return true;
            }
            return false;
        }

        public int Count(ValueList Conditions)
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public bool Delete(Transaction item)
        {
            throw new NotImplementedException();
        }

        public Transaction Find(ValueList Conditions)
        {
            AutoBox auto = _db.Open();
            StringBuilder ql = new StringBuilder();
            ql.Append($"from Transaction");
            List<object> args = new List<object>();
            if (Conditions.Count > 0)
            {
                ql.Append($" where ");
                for (var i = 0; i < Conditions.Count; i++)
                {
                    ql.Append($" {Conditions[i].Name} == ? &");
                    args.Add(Conditions[i].Value);
                }
            }
            IEnumerable<Transaction> result;
            using (Box box = auto.Cube())
            {
                
                try
                {
                    return box.Select<Transaction>(ql.ToString().TrimEnd('&'), args.ToArray()).First();
                }
                catch (Exception ex)
                {
                    //TODO: Log Exception
                }
            }
            return null; //TODO: find a better way to handle this

        }

        public IEnumerable<Transaction> FindAll(ValueList Conditions)
        {
            AutoBox auto = _db.Open();
            StringBuilder ql = new StringBuilder();
            ql.Append($"from Transaction");
            List<object> args = new List<object>();
            if (Conditions.Count > 0)
            {
                ql.Append($" where ");
                for (var i = 0; i < Conditions.Count; i++)
                {
                    ql.Append($" {Conditions[i].Name} == ? &");
                    args.Add(Conditions[i].Value);
                }
            }
            IEnumerable<Transaction> result = null;
            using (Box box = auto.Cube())
            {
                try
                {
                    result = box.Select<Transaction>(ql.ToString().TrimEnd('&'), args.ToArray());
                    return result.ToList();
                }
                catch (Exception ex)
                {
                    //TODO: Log Exeption
                }
            }
            return result;
        }

        public Transaction FindMax(string fieldName)
        {
            throw new NotImplementedException();
        }

        public Transaction FindMin(string fieldName)
        {
            throw new NotImplementedException();
        }

        public bool Update(Transaction item)
        {
            throw new NotImplementedException();
        }
    }
}
