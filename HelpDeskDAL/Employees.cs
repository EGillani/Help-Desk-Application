
/**
 * Class Name:Employees.cs
 * Purpose: Proxy class part of the Entity framework: Interacts with the underlying database to help us perform CRUD operations. 
 * Coder: Eraj Gillani 0858887
 * Date: November 12th 2020
 **/

using System;
using System.Collections.Generic;

namespace HelpDeskDAL
{
    public partial class Employees : HelpdeskEntity
    {

        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public int DepartmentId { get; set; }
        public bool? IsTech { get; set; }
        public byte[] StaffPicture { get; set; }


        public virtual Departments Department { get; set; }
    }
}
