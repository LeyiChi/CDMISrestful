using CDMISrestful.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDMISrestful.DataViewModels
{
    public class UsersViewModel
    {
        
    }
    public class PatientsDataSet
    {
        public List<PatientListTable> DT_PatientList { get; set; }
        public RateTable DT_Rates  { get; set; }
        
        public PatientsDataSet()
        {
            DT_PatientList = new List<PatientListTable>();
            DT_Rates = new RateTable();
        }
    }
}