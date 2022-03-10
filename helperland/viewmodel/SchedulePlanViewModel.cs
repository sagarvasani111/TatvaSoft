using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.viewmodel
{
    public class SchedulePlanViewModel
    {

        [DataType(DataType.Date)]
        public string ServiceStartDate { get; set; }

        public string StartTime { get; set; }

        public string StayTime { get; set; }

        public float TotalTime { get; set; }

        public float TotalCost { get; set; }

        public float EffctiveCost { get; set; }

        public List<bool> Extra { get; set; }

        public string comment { get; set; }
        public bool IsPet { get; set; }
    }
}
