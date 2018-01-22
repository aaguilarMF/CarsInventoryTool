using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor;

namespace CarsInventoryControl.Areas.CarsInventory.Controllers
{
    [RouteArea("CarsInventory")]
    [System.Web.Mvc.RoutePrefix("Template")]
    //[System.Web.Mvc.Route("action=index")]
    public class TemplateController : Controller
    {
        [Route("Home")]
        [Route("~/")]
        //[Route("Home/{currentPage}")]
        public ActionResult Home(Models.GridState gridState)
        {
            try
            {
                gridState.PageCount = gridState.PageCount == null ? 1 : gridState.PageCount;
                return View(gridState);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        
        [Route("Edit/{locationNo}")]
        public ActionResult Edit(Models.GridState state, string locationNo = null)
        {
            try
            {
                if(locationNo == null)
                {
                    throw new Exception("No LocationNo provided.");
                }
                var context = new EntityModels.Prod8Entities();
                var car = context.CARS_AVAILABILTY_CONTROL_TAB.Where(x => x.LOCATION_NO == locationNo).Select(x => new Models.Inventory.Cars()
                {
                    LocationNo = x.LOCATION_NO,
                    AvailabilityControlId = x.AVAILABILITY_CONTROL_ID,
                    RequestedBy = x.REQUESTED_BY,
                    Warehouse = x.WAREHOUSE

                }).FirstOrDefault();
                state.CurrentObjectView = car;
                return View(state);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [Route("Add")]
        public ActionResult Add()
        {
            return View();

        }
        
    }
}