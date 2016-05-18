using BasicService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PipingInfoSystemWeb.Controllers
{
    public class MainController : ApiController
    {
        EFService service = EFService.GetClient();
        [HttpPost]
        public ResponseMessage<List<PipingInfo>> Search(string usertoken, SearchRequst request)
        {
            ResponseMessage<List<PipingInfo>> result = new ResponseMessage<List<PipingInfo>>();
            //验证token
            result = service.Search(usertoken, request);
            return result;
        }

        [HttpGet]
        public ResponseMessage Delete(string usertoken, string pipingid)
        {
            ResponseMessage result = new ResponseMessage();
            //验证token
            result = service.Delete(usertoken, pipingid);
            return result;
        }

        [HttpGet]
        public ResponseMessage<PipingDetailInfo> GetInfo(string usertoken, string pipingid)
        {
            ResponseMessage<PipingDetailInfo> result = new ResponseMessage<PipingDetailInfo>();
            //验证token
            result = service.GetInfo(usertoken, pipingid);
            return result;
        }
    }
}
