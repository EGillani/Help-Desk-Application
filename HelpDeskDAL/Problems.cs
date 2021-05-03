using System;
using System.Collections.Generic;

namespace HelpDeskDAL
{
    public partial class Problems : HelpdeskEntity
    {
        public Problems()
        {
            Calls = new HashSet<Calls>();
        }

        public string Description { get; set; }

        public virtual ICollection<Calls> Calls { get; set; }
    }
}

//using System;
//using System.Collections.Generic;

//namespace HelpDeskDAL
//{
//    public partial class Problems : HelpdeskEntity
//    {
//        public string Description { get; set; }

//    }
//}
