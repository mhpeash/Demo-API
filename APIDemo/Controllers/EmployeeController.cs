using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using EmployeeDataAccess;


namespace APIDemo.Controllers
{
    public class EmployeeController : ApiController
    {
        public IEnumerable<tbl_Employee> Get()
        {
            using (API_DatabaseEntities  db=new API_DatabaseEntities())
            {
               
                
                    return db.tbl_Employee.ToList();
                
                
            }
        }

        public HttpResponseMessage Get( int id)
        {
            using (API_DatabaseEntities db = new API_DatabaseEntities())
            {
                var dbvalue = db.tbl_Employee.FirstOrDefault(a=>a.ID==id);
                if (dbvalue != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, dbvalue);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with"+ id.ToString()+"not found");
                }
            }
        }


        public HttpResponseMessage Post( [FromBody] tbl_Employee employee )
        {
            using (API_DatabaseEntities db=new API_DatabaseEntities())
            {
                try
                {
                    db.tbl_Employee.Add(employee); //tbl_Employee is an object of DbSet<tbl_Employee> tbl_Employee
                    db.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + employee.ID.ToString());
                    return message;
                }
                catch (Exception ex)
                {

                   return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
                
                
            }
        }

        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (API_DatabaseEntities db = new API_DatabaseEntities())
                {

                    var checkeidexistance = db.tbl_Employee.FirstOrDefault(v => v.ID == id);
                    if (checkeidexistance == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id" + id.ToString() +" " +"Not found");
                    }

                    else
                    {
                        db.tbl_Employee.Remove(checkeidexistance);
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }

                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
