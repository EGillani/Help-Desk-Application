/**
 * Class Name:HelpdeskRepository.cs
 * Purpose: Abstracts the data layer and centralize the way of handling the domain classes 
 *          This is accessed from the business layer through the interface 
 * Coder: Eraj Gillani 0858887
 * Date: November 12th 2020
 **/
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace HelpDeskDAL
{
    //inherits from the HelpdeskEntity and IRespository 
    public class HelpdeskRepository<T> : IRepository<T> where T : HelpdeskEntity
    {
        readonly private HelpDeskContext _db = null;

        public HelpdeskRepository(HelpDeskContext context = null)
        {
            _db = context ?? new HelpDeskContext();
        }

        //templated way to get all the objects in the list
        public List<T> GetAll()
        {
            return _db.Set<T>().ToList();
        }

        //templated way to utilize the where expression with the predication function 
        public List<T> GetByExpression(Expression<Func<T, bool>> match)
        {
            return _db.Set<T>().Where(match).ToList();
        }

        //templated way to add an object
        public T Add(T entity)
        {
            _db.Set<T>().Add(entity);
            _db.SaveChanges();
            return entity;
        }

        //templates way to update an object
        public UpdateStatus Update(T updatedEntity)
        {
            UpdateStatus operationStatus = UpdateStatus.Failed;

            try
            {
                HelpdeskEntity currentEntity = GetByExpression(ent => ent.Id == updatedEntity.Id).FirstOrDefault();
                _db.Entry(currentEntity).OriginalValues["Timer"] = updatedEntity.Timer;
                _db.Entry(currentEntity).CurrentValues.SetValues(updatedEntity);

                if (_db.SaveChanges() == 1)
                    operationStatus = UpdateStatus.Ok;
            }
            catch (DbUpdateConcurrencyException dbx)
            {
                operationStatus = UpdateStatus.Stale;
                Console.WriteLine("Problem in " + MethodBase.GetCurrentMethod().Name + dbx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + MethodBase.GetCurrentMethod().Name + ex.Message);
            }
            return operationStatus;
        }

        //templated way to delete an object 
        public int Delete(int id)
        {
            T currentEntity = GetByExpression(ent => ent.Id == id).FirstOrDefault();
            _db.Set<T>().Remove(currentEntity);
            return _db.SaveChanges();
        }
    }
}
