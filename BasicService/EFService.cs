using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;
using System.Windows.Forms;
using System.Threading;

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

        public ResponseMessage<List<PipingInfo>> Search(string usertoken, SearchRequst r)
        {
            ResponseMessage<List<PipingInfo>> result = new ResponseMessage<List<PipingInfo>>();
            //验证token
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
        public ResponseMessage Report(string usertoken, string pipingid, string path)
        {
            ResponseMessage result = new ResponseMessage();
            List<PipingPictureInfo> pictures = db.PipingPictureInfoes.ToList().FindAll(p => p.PipingID == pipingid);
            PipingDetectionInfo pipingInfo = db.PipingDetectionInfoes.ToList().Find(p => p.PipingID == pipingid);
            string mbWord = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\" + "PipingInfo.doc";
            Report report = new Report();
            report.CreateNewDocument(mbWord);

            // 检测信息
            report.InsertValue("t1", pipingInfo.VideoFile);
            report.InsertValue("t2", pipingInfo.StartWellNo);
            report.InsertValue("t3", pipingInfo.EndWellNo);
            report.InsertValue("t4", pipingInfo.LayingYear);
            report.InsertValue("t5", pipingInfo.StartPointDepth);
            report.InsertValue("t6", pipingInfo.EndPointDepth);
            report.InsertValue("t7", pipingInfo.TubulationType);
            report.InsertValue("t8", pipingInfo.TubulationMaterial);
            report.InsertValue("t9", pipingInfo.TubulationDiameter);
            report.InsertValue("t10", pipingInfo.DetectionDirect);
            report.InsertValue("t11", pipingInfo.TubulationLength);
            report.InsertValue("t12", pipingInfo.DetectionLength);
            report.InsertValue("t13", pipingInfo.DetectionAddress);
            report.InsertValue("t14", pipingInfo.DetectionTime);
            report.InsertValue("XH", string.Format("{0}→{1}", pipingInfo.StartWellNo, pipingInfo.EndWellNo));
            report.InsertValue("JCFF", pipingInfo.DetectionFun);
            report.InsertValue("remark", pictures.Count > 0 ? pictures[0].Remark : string.Empty);

            pictures.OrderBy(p => p.PictureID);

            // 工程图片信息
            PipingPictureInfo picInfo = pictures.Find(p => p.PictureType == 0);
            if (picInfo != null)
            {
                report.InsertPicture("gcTP", picInfo.PictureFilePath, 220, 220);
                pictures.RemoveAll(p => p.PictureType == 0);
            }

            Table picRemarkTable = report.Document.Content.Tables[2];
            Table remarkTable = report.Document.Content.Tables[3];
            Table picTable = report.Document.Content.Tables[4];

            // 添加图片信息描述 + 添加图片
            for (int i = 0; i < pictures.Count; i++)
            {
                if (i > 0)
                {
                    report.AddRow(2, 1);
                }

                report.InsertCell(picRemarkTable, i + 2, 1, pictures[i].Distance);
                report.InsertCell(picRemarkTable, i + 2, 2, pictures[i].DefectCode);
                report.InsertCell(picRemarkTable, i + 2, 3, pictures[i].Score);
                report.InsertCell(picRemarkTable, i + 2, 4, pictures[i].Grade);
                report.InsertCell(picRemarkTable, i + 2, 5, pictures[i].PipingInternalState);
                report.InsertCell(picRemarkTable, i + 2, 6, pictures[i].PictureID);

                if (i == 0)
                {
                    report.InsertPicture("TP1", pictures[i].PictureFilePath, 220, 220);
                    report.InsertValue("TP1MS", pictures[i].PictureRemark);
                }
                else
                {
                    report.InsertPicture(string.Format("TP{0}", i + 1), pictures[i].PictureFilePath, 220, 220);
                    report.InsertValue(string.Format("TP{0}MS", i + 1), pictures[i].PictureRemark);
                }
            }

            // 图片的数量
            int picCount = pictures.Count;

            // 删除多余的行
            for (int i = 0; i < (5 - (picCount) / 2) * 2; i++)
            {
                report.DeleteRow(4, 12 - i);
            }

            report.Application.Visible = true;
            report.Application.Activate();
            report.Application.ShowMe();

            object what = WdGoToItem.wdGoToBookmark;
            object name = "KG1";
            object missing = System.Reflection.Missing.Value;
            report.Application.Selection.GoTo(what, missing, missing, name);
            SendKeys.SendWait("{DEL}");

            Thread.Sleep(10);

            name = "KG2";
            report.Application.Selection.GoTo(what, missing, missing, name);
            SendKeys.SendWait("{DEL}");

            Thread.Sleep(10);

            report.SaveDocument(path);
            result.code = "0";
            result.msg=string.Format("生成成功！文件地址：{0}", path);
            return result;
        }
        public ResponseMessage AddOrModify(PipingDetailInfo detailInfo)
        {
            var pipingId = detailInfo.PipingDetectionInfo.PipingID;
            ResponseMessage result = new ResponseMessage();
            var temp = db.PipingDetectionInfoes.ToList().Find(p => p.PipingID == pipingId);
            if (temp != null)
            {
                db.PipingDetectionInfoes.Remove(temp);
                db.PipingDetectionInfoes.Add(detailInfo.PipingDetectionInfo);
                db.PipingPictureInfoes.RemoveRange(db.PipingPictureInfoes.ToList().FindAll(p => p.PipingID == pipingId));
                db.PipingPictureInfoes.AddRange(detailInfo.PipingPictureInfoes);
            }
            else
            {
                db.PipingDetectionInfoes.Add(detailInfo.PipingDetectionInfo);
                db.PipingPictureInfoes.AddRange(detailInfo.PipingPictureInfoes);
            }
            try
            {
                if (db.SaveChanges() > 0)
                {
                    result.code = "0";
                }
            }
            catch(Exception e)
            {

            }
            return result;
        }
        public ResponseMessage Regist(Regist user)
        {
            ResponseMessage result = new ResponseMessage();

            var u = db.UserInfoes.ToList().Find(p => p.UserName == user.username);

            if (u != null)
            {
                result.code = "1";
            }
            else
            {
                db.UserInfoes.Add(new UserInfo()
                {
                    UserName = user.username,
                    Password = MD5Helper.MD5Encrypt32(user.password),
                    PhoneNumber = user.number,
                    TrueName = user.truename,
                    ID = user.id,
                    KeID = Guid.NewGuid().ToString("N"),
                    ApplyDate = System.DateTime.Now,
                    IsDelete = 0,
                    UserType = 0,
                    UserID = Guid.NewGuid().ToString("N")
                });
                if (db.SaveChanges() > 0)
                {
                    result.code = "0";
                }
            }
            return result;
        }
        public ResponseMessage<User> Login(string username, string password)
        {
            ResponseMessage<User> result = new ResponseMessage<User>();
            var a = db.UserInfoes.ToList().Find(p => p.UserName == username && p.Password == MD5Helper.MD5Encrypt32(password));
            if (a != null)
            {
                result.code = "0";
                result.ext = new User() {
                    UserName = a.UserName,
                    UserToken = a.UserName,
                    UserType = a.UserType
                };

            }
            return result;
        }
    }
}
