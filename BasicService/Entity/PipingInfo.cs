using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicService
{
    public class PipingInfo
    {
        public string PipingID { get; set; }
        public int ID { get; set; }
        public string VideoFile { get; set; }
        public string StartWellNo { get; set; }
        public string EndWellNo { get; set; }
        public string LayingYear { get; set; }
        public string StartPointDepth { get; set; }
        public string EndPointDepth { get; set; }
        public string TubulationType { get; set; }
        public string TubulationMaterial { get; set; }
        public string TubulationDiameter { get; set; }
        public string DetectionDirect { get; set; }
        public string TubulationLength { get; set; }
        public string DetectionAddress { get; set; }
        public string DetectionTime { get; set; }
        public DateTime? AddTime { get; set; }
        public string AddPerson { get; set; }
        public string DetectionLength { get; set; }
        public string DetectionFun { get; set; }
        public bool IsOverTime { get; set; } 
    }
}
