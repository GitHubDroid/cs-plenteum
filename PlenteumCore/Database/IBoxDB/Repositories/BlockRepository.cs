using Plenteum;
using Plenteum.Database;
using Plenteum.CryptoNote.Blockchain;
using System;
using System.Collections.Generic;
using System.Text;
using iBoxDB.LocalServer;
using System.Linq;

namespace Plenteum.Database
{
    public class BlockRepository : BaseRepository, IRepository<Block>
    {
        public BlockRepository(DB db) : base(db)
        {
        }

        public bool Add(Block item)
        {
            AutoBox auto = _db.Open();
            CommitResult cr;
            using (Box box = auto.Cube())
            {
                box["Block"].Insert<Block>(item, 0);
                cr = box.Commit();
            }
            return cr.Assert();
        }

        public int Count(ValueList Conditions)
        {
            AutoBox auto = _db.Open();
            StringBuilder ql = new StringBuilder();
            ql.Append($"from Block");
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
            IEnumerable<Block> result = null;
            using (Box box = auto.Cube())
            {
                try
                {
                    result = box.Select<Block>(ql.ToString().TrimEnd('&'), args.ToArray());
                    return result.Count();
                }
                catch (Exception ex)
                {
                    //TODO: Log Exeption
                }
            }
            return 0;
        }

        public int Count()
        {
            AutoBox auto = _db.Open();
            StringBuilder ql = new StringBuilder();
            ql.Append($"from Block");
            List<object> args = new List<object>();
            IEnumerable<Block> result = null;
            using (Box box = auto.Cube())
            {
                try
                {
                    result = box.Select<Block>(ql.ToString().TrimEnd('&'), args.ToArray());
                    return result.Count();
                }
                catch (Exception ex)
                {
                    //TODO: Log Exeption
                }
            }
            return 0;
        }

        public bool Delete(Block item)
        {
            throw new NotImplementedException();
        }

        public Block Find(ValueList Conditions)
        {
            AutoBox auto = _db.Open();
            StringBuilder ql = new StringBuilder();
            ql.Append($"from Block");
            List<object> args = new List<object>();
            if (Conditions.Count > 0)
            {
                ql.Append($" where ");
                for (var i=0; i < Conditions.Count; i++){
                    ql.Append($" {Conditions[i].Name} == ? &");
                    args.Add(Conditions[i].Value);
                }
            }
            IEnumerable<Block> result;
            using (Box box = auto.Cube())
            {
                try
                {
                    return box.Select<Block>(ql.ToString().TrimEnd('&'), args.ToArray()).First();
                }
                catch (Exception ex)
                { 
                    //log exception
                }
            }

            return null; //TODO: find a better way to handle this
            
        }

        public IEnumerable<Block> FindAll(ValueList Conditions)
        {
            AutoBox auto = _db.Open();
            StringBuilder ql = new StringBuilder();
            ql.Append($"from Block");
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
            IEnumerable<Block> result = null;
            using (Box box = auto.Cube())
            {
                try
                {
                    result = box.Select<Block>(ql.ToString().TrimEnd('&'), args.ToArray());
                    return result.ToList();
                }
                catch (Exception ex)
                {
                    //TODO: Log Exeption
                }
            }
            return result;
        }

        public Block FindTopBlock()
        {
            AutoBox auto = _db.Open();
            StringBuilder ql = new StringBuilder();
            ql.Append($"from Block");
            List<Block> result;
            using (Box box = auto.Cube())
            {
                result = box.Select<Block>(ql.ToString()).OrderByDescending(x => x.Height).Take(1).ToList();
                //TODO: Test Performance and see if there's a better way to implement this
            }
            try
            {
                return result.First();
            }
            catch (Exception ex)
            {
                //TODO: Log Exception 
                return null; //TODO: find a better way to handle this
            }
        }

        public bool Update(Block item)
        {
            throw new NotImplementedException();
        }
    }
}
