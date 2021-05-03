
/**
 * Class Name:Departments.cs
 * Purpose: Proxy class part of the Entity framework: Interacts with the underlying database to help us perform CRUD operations. 
 * Coder: Eraj Gillani 0858887
 * Date: November 12th 2020
 **/

using System;
using System.Collections.Generic;

namespace HelpDeskDAL
{
    public partial class Departments : HelpdeskEntity
    {
        public Departments() 
        {
            Employees = new HashSet<Employees>();
        }


        public string DepartmentName { get; set; }


        public virtual ICollection<Employees> Employees { get; set; }
    }
}
