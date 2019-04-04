using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCrawler.Web.Models
{
    public class CountResponse : BaseResponse
    {
        public CountResponse(int count, string res = "Success", string mes = null)
        {
            this.Count = count;
            this.Status = res;
            this.Message = mes;
        }
        public int Count { get; }
    }
}
