using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCrawler.Web.Models
{
    public class BaseResponse
    {
        public String Status { get; protected set; }
        public String Message { get; protected set; }
    }
}
