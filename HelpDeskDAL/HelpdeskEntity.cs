/**
 * Class Name:HelpdeskEntity.cs
 * Purpose: Domain classes inherit from this entity 
 * Coder: Eraj Gillani 0858887
 * Date: November 12th 2020
 **/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HelpDeskDAL
{
    public class HelpdeskEntity
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] Timer { get; set; }
    }
}
