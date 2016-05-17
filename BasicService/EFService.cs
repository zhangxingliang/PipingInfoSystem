using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicService
{
    public class EFService
    {
        private static EFService Client;
        private static readonly object  obj = new object();
        public static EFService GetClient() {
            if (Client == null)
            {
                lock (obj)
                {
                    if (Client == null)
                    {
                        Client = new EFService();
                    }
                }
            }
            return Client;
        }

        private EFService() {
            db = new DbModel();
        }

        private DbModel db;

        public ResponseMessage<List<PipingInfo>> Search(SearchRequst r)
        {
            ResponseMessage<List<PipingInfo>> result = new ResponseMessage<List<PipingInfo>>();
            List<PipingDetectionInfo> data = db.PipingDetectionInfoes.ToList().FindAll(p => p.StartWellNo.Contains(r.startNo) && p.LayingYear.Contains(r.year) && p.TubulationType.Contains(r.type));
            result.ext = data.ToPipingInfo();
            result.code = "0";    
            return result;
        }
        public ResponseMessage Delete(string usertoken, string pipingid)
        {

            ResponseMessage result = new ResponseMessage();
            //验证token
            db.PipingDetectionInfoes.Remove(db.PipingDetectionInfoes.ToList().Find(x=>x.PipingID ==pipingid));
            db.PipingPictureInfoes.Remove(db.PipingPictureInfoes.ToList().Find(x => x.PipingID == pipingid));
            if (db.SaveChanges() > 0)
            {
                result.code = "0";
            }
            return result;
        }

        public ResponseMessage<PipingDetailInfo> GetInfo(string usertoken, string pipingid)
        {

            ResponseMessage<PipingDetailInfo> result = new ResponseMessage<PipingDetailInfo>();
            result.ext = new PipingDetailInfo();
            //验证token
            result.ext.PipingDetectionInfo = db.PipingDetectionInfoes.ToList().Find(x => x.PipingID == pipingid);
            result.ext.PipingPictureInfoes = db.PipingPictureInfoes.ToList().FindAll(x => x.PipingID == pipingid);
            if (result.ext.PipingDetectionInfo!=null)
            {
                result.code = "0";
            }
            return result;
        }
    }
}
