using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CarsInventoryControl.Areas.CarsInventory.Controllers
{
    [System.Web.Http.RoutePrefix("CarsInventoryAPI/Inventory")]
    public class InventoryController : ApiController
    {
        [System.Web.Http.Route("Cars/{pageCount}/{caseSensitive}/{locationNo}/{availabilityControlId}/{requestedBy}/{warehouse}")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetCars(bool caseSensitive, string locationNo = null, string availabilityControlId= null, string requestedBy= null, string warehouse= null, int? pageCount = null, int? perPage = null)
        {
            try
            {

                if (pageCount == null)
                {
                    pageCount = 1;
                }
                if (perPage == null)
                {
                    perPage = 20;
                }
                var lnNotNull = locationNo == null? false: ((locationNo != "null") ? true : false);
                var aciNotNull = availabilityControlId == null ? false : ((availabilityControlId != "null") ? true : false);
                var rbNotNull = requestedBy == null ? false : ((requestedBy != "null") ? true : false);
                var wNotNull = warehouse == null ? false : ((warehouse != "null") ? true : false);
  
                var context = new EntityModels.Prod8Entities();
                IQueryable<Models.Inventory.Cars> cars;
                cars = context.CARS_AVAILABILTY_CONTROL_TAB.Where(x =>
                    (caseSensitive? (lnNotNull ? x.LOCATION_NO.Contains(locationNo) : true) : (lnNotNull ? x.LOCATION_NO.ToUpper().Contains(locationNo.ToUpper()) : true)) &&
                    (caseSensitive? (aciNotNull ? x.AVAILABILITY_CONTROL_ID.Contains(availabilityControlId) : true): (aciNotNull ? x.AVAILABILITY_CONTROL_ID.ToUpper().Contains(availabilityControlId.ToUpper()) : true)) &&
                    (caseSensitive? (rbNotNull ? x.REQUESTED_BY.Contains(requestedBy) : true):(rbNotNull ? x.REQUESTED_BY.ToUpper().Contains(requestedBy.ToUpper()) : true)) &&
                    (caseSensitive? (wNotNull ? x.WAREHOUSE.Contains(warehouse) : true): (wNotNull ? x.WAREHOUSE.ToUpper().Contains(warehouse.ToUpper()) : true)))
                .OrderBy(x => x.LOCATION_NO).Skip(((int)pageCount - 1) * (int)perPage).Select(x => new Models.Inventory.Cars()
                {
                    LocationNo = x.LOCATION_NO,
                    AvailabilityControlId = x.AVAILABILITY_CONTROL_ID,
                    RequestedBy = x.REQUESTED_BY,
                    Warehouse = x.WAREHOUSE
                }).Take((int)perPage + 1);
                
                return Ok(new Models.Inventory.GetCarsResponse()
                {
                    NextPageExists = cars.Count() == 21 ? true : false,
                    Inventory = cars.Take(20).ToList()
                });

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [Route("Car/{locationNo}")]
        [HttpPost]
        public IHttpActionResult GetCar([FromBody] Models.GridState gridState, string locationNo = null)
        {
            try
            {

                if (locationNo == null)
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
                if (car == null)
                {
                    //throw httpresponseerror
                    throw new Exception();
                }
                return Ok(car);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("Save/{isAdd}")]
        [HttpPost]
        public IHttpActionResult Save([FromBody] Models.Inventory.Cars carsEntry, bool isAdd)
        {

            if (isAdd)
            {
                return Add(carsEntry);
            }
            else
            {
                return Edit(carsEntry);
            }
        }
        [Route("Delete")]
        [HttpPost]
        public IHttpActionResult Delete([FromBody] Models.Inventory.Cars carsEntry)
        {
            try
            {
                var context = new EntityModels.Prod8Entities();
                var exists = context.CARS_AVAILABILTY_CONTROL_TAB.Where(x => x.LOCATION_NO == carsEntry.LocationNo).FirstOrDefault();
                if (exists == null)
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        ReasonPhrase = "No data found. Delete Failed"
                    });
                }
                context.CARS_AVAILABILTY_CONTROL_TAB.Remove(exists);
                context.SaveChanges();

                return Ok(true);
            }
            catch (HttpResponseException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region ADD/EDIT
        private IHttpActionResult Edit([FromBody] Models.Inventory.Cars carsEntry)
        {
            try
            {
                var context = new EntityModels.Prod8Entities();
                var carEntryFromDB = context.CARS_AVAILABILTY_CONTROL_TAB.Where(x => x.LOCATION_NO == carsEntry.LocationNo).FirstOrDefault();
                if (carEntryFromDB == null)
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        ReasonPhrase = "No data for this entry exists."
                    });
                }
                //update entry then save
                carEntryFromDB.LOCATION_NO = carsEntry.LocationNo;
                carEntryFromDB.AVAILABILITY_CONTROL_ID = carsEntry.AvailabilityControlId;
                carEntryFromDB.REQUESTED_BY = carsEntry.RequestedBy;
                carEntryFromDB.WAREHOUSE = carsEntry.Warehouse;

                context.SaveChanges();

                var car = new Models.Inventory.Cars()
                {
                    LocationNo = carEntryFromDB.LOCATION_NO,
                    AvailabilityControlId = carEntryFromDB.AVAILABILITY_CONTROL_ID,
                    RequestedBy = carEntryFromDB.REQUESTED_BY,
                    Warehouse = carEntryFromDB.WAREHOUSE
                };

                return Ok(car);
            }
            catch (HttpResponseException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private IHttpActionResult Add([FromBody] Models.Inventory.Cars carsEntry)
        {
            try
            {
                var context = new EntityModels.Prod8Entities();
                //add new entry
                if(String.IsNullOrWhiteSpace(carsEntry.LocationNo) ||
                   String.IsNullOrWhiteSpace(carsEntry.LocationNo) ||
                   String.IsNullOrWhiteSpace(carsEntry.LocationNo) ||
                   String.IsNullOrWhiteSpace(carsEntry.LocationNo))
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        ReasonPhrase = "One or more data fields is empty, null, or just not defined."
                    });
                }
                var newCar = new EntityModels.CARS_AVAILABILTY_CONTROL_TAB()
                {
                    LOCATION_NO = carsEntry.LocationNo,
                    AVAILABILITY_CONTROL_ID = carsEntry.AvailabilityControlId,
                    REQUESTED_BY = carsEntry.RequestedBy,
                    WAREHOUSE = carsEntry.Warehouse,
                    ROWVERSION = DateTime.Today
                };
                context.CARS_AVAILABILTY_CONTROL_TAB.Add(newCar);
                context.SaveChanges();

                var car = new Models.Inventory.Cars()
                {
                    LocationNo = newCar.LOCATION_NO,
                    AvailabilityControlId = newCar.AVAILABILITY_CONTROL_ID,
                    RequestedBy = newCar.REQUESTED_BY,
                    Warehouse = newCar.WAREHOUSE
                };


                return Ok(car);
            }
            catch (HttpResponseException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
