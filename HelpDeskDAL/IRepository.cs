/**
 * Class Name:IRepository.cs
 * Purpose: Acts as an interface to the DAL project 
 * Coder: Eraj Gillani 0858887
 * Date: November 12th 2020
 **/

using System;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace HelpDeskDAL
{
    public interface IRepository<T>
    {
        List<T> GetAll();
        List<T> GetByExpression(Expression<Func<T, bool>> match);
        T Add(T entity);
        UpdateStatus Update(T entity);
        int Delete(int i);
    }
}
