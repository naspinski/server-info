using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerInfo.WebUI.Ajax
{
    /// <summary>
    /// Summary description for FlipDisplaySetting
    /// </summary>
    public class FlipDisplaySetting : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                context.Response.Write(Settings.Display.Flip(context.Request.Form["key"]).ToString());
            }
            catch (Exception ex) { context.Response.Write("error: " + ex.Message); }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}