using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CarsInventoryControl.Areas.CarsInventory.Models.Inventory
{
    [DataContract]
    public class Cars
    {
        [DataMember]
        public String LocationNo { get; set; }
        [DataMember]
        public String AvailabilityControlId { get; set; }
        [DataMember]
        public String RequestedBy { get; set; }
        [DataMember]
        public String Warehouse { get; set; }

    }
}