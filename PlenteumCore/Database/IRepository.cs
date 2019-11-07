using Plenteum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plenteum
{
    public interface IRepository<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        bool Add(T item);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        bool Update(T item);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        bool Delete(T item);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Conditions"></param>
        /// <returns></returns>
        T Find(ValueList Conditions);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Conditions"></param>
        /// <returns></returns>
        IEnumerable<T> FindAll(ValueList Conditions);

        /// <summary>
        /// Counts all rows that match the specified conditions
        /// </summary>
        /// <param name="TableName">The name of the table to count on</param>
        /// <param name="Conditions">A set of values that a row must match to be counted</param>
        /// <returns>An integer representing the number of rows that matched the specified conditions</returns>
        int Count(ValueList Conditions);

        /// <summary>
        /// Counts all rows in a table
        /// </summary>
        /// <param name="TableName">The name of the table to count on</param>
        /// <returns>An integer representing the number of rows in a specified table</returns>
        int Count();
    }
}
