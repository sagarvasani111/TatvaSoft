using Helperland.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.viewmodel
{
    public class ServiceRequestViewModel
    {
        public string servicestartdate { get; set; }
        public string servicestrarttime { get; set; }
        public float servicehours { get; set; }
        public float subtotal { get; set; }
        public float totalcost { get; set; }
        public string comments { get; set; }
        public bool haspets { get; set; }
        public bool extraSer1 { get; set; }
        public bool extraSer2 { get; set; }
        public bool extraSer3 { get; set; }
        public bool extraSer4 { get; set; }
        public bool extraSer5 { get; set; }
        //public int ServiceId { get; set; }
        //public DateTime ServiceStartDate { get; set; }
        //public decimal TotalCost { get; set; }
        //public List<ServiceRequest> GetServiceRequestData()
        //{
        //    String connectionString = "Data Source-DESKTOP-FMVDTUQ;Initial Catalog-Helperland;Integrated Security=True";
        //    SqlConnection con = new SqlConnection(connectionString);
        //    con.Open();
        //    String sqlQuery = "select ServiceId, ServiceStartDate, TotalCost from [dbo].[ServiceRequest]";
        //    SqlCommand cmd = new SqlCommand(sqlQuery, con);
        //    SqlDataReader dr = cmd.ExecuteReader();

        //    List<ServiceRequest> serviceRequestsList = new List<ServiceRequest>();


        //    while (dr.Read())
        //    {
        //        ServiceRequest serviceRequest = new ServiceRequest();
        //        serviceRequest.ServiceId = Convert.ToInt32(dr[ServiceId].ToString());
        //        serviceRequest.ServiceStartDate = Convert.ToDateTime(dr[ServiceId].ToString());
        //        //serviceRequest.TotalCost = Convert.ToDecimal(dr[TotalCost].ToString());
        //        serviceRequestsList.Add(serviceRequest);
        //    }
        //    con.Close();
        //    return serviceRequestsList;
        //}
    }
}
