using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CarsInventoryControl.Areas.CarsInventory.Models
{
    [DataContract]
    public class GridState
    {
        [DataMember]
        public String Message { get; set; }
        [DataMember]
        public Models.Inventory.Cars CurrentObjectView { get; set; }
        [DataMember]
        public int? PageCount { get; set; }
    }
}