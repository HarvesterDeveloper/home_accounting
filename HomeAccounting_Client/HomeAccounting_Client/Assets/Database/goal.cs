//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HomeAccounting_Client.Assets.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class goal
    {
        public int id { get; set; }
        public Nullable<int> owner_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public Nullable<decimal> goal_money { get; set; }
    
        public virtual member member { get; set; }
    }
}
