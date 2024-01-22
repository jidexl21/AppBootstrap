using AppBootstrap.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AppBootstrap
{
    public static class Extensions
    {

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public static ActionResult Respond<T>(this ControllerBase controller, ApiResponse<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode = response.Status,
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public static ActionResult Respond(this ControllerBase controller, FileResponse response)
        {
            if (response.Status == HttpStatusCode.OK)
            {
                return controller.File(response.stream.ToArray(), response.MimeType, response.FileName);
            }
            return controller.BadRequest(response.title);
        }

    }
}
