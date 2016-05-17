using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicService
{
    public static class ExtendMethod
    {
        public static List<PipingInfo> ToPipingInfo(this List<PipingDetectionInfo> data)
        {
            List<PipingInfo> result = new List<PipingInfo>();
            int index = 0;
            if (data != null) {
                foreach (var i in data)
                {
                    result.Add(new PipingInfo() {
                        PipingID = i.PipingID,
                        ID = index,
                        AddPerson = i.AddPerson,
                        AddTime = i.AddTime,
                        DetectionAddress = i.DetectionAddress,
                        DetectionDirect = i.DetectionDirect,
                        DetectionFun = i.DetectionFun,
                        DetectionLength = i.DetectionLength,
                        DetectionTime = i.DetectionTime,
                        EndPointDepth = i.EndPointDepth,
                        EndWellNo = i.EndWellNo,
                        IsOverTime = DateTime.Now.Year - int.Parse(i.LayingYear) > 5 ? true:false,
                        LayingYear = i.LayingYear,
                        StartPointDepth = i.StartPointDepth,
                        StartWellNo = i.StartWellNo,
                        TubulationDiameter = i.TubulationDiameter,
                        TubulationLength = i.TubulationLength,
                        TubulationMaterial = i.TubulationMaterial,
                        TubulationType = i.TubulationType,
                        VideoFile = i.VideoFile
                    });
                }
            }
            return result;
        }
    }
}
