using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AppBootstrap.Services
{
    
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; } = "Ok";
        /// <summary>
        /// 
        /// </summary>
        public int Status { get; set; } = 200;
        /// <summary>
        /// 
        /// </summary>
        public T Data { get; set; }

        public ApiResponse()
        {

        }
        public ApiResponse(string error)
        {
            Message = error;
            Status = 400;
        }
    }

    public class FileResponse
    {
        public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
        public string title { get; set; }
        public string FileName { get; set; }
        public byte[] Buffer { get; set; }
        public MemoryStream stream { get; set; }
        public string MimeType { get; set; } = "application/octet-stream";
    }
}
