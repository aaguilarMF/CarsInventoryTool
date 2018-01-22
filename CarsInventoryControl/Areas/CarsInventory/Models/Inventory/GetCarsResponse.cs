using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CarsInventoryControl.Areas.CarsInventory.Models.Inventory
{
    [DataContract]
    public class GetCarsResponse
    {
        [DataMember]
        public bool NextPageExists { get; set; }
        [DataMember]
        public List<Cars> Inventory { get; set; }
    }
}