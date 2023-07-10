using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace WedSiteBanHang
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Application["SoNguoiTruyCap"] = 0;
            Application["SoNguoiDangOnline"] = 0;
            
        }
        protected void Session_Start()
        {
            Application.Lock(); //Dùng để đồng bộ hóa
            Application["SoNguoiTruyCap"] = (int)Application["SoNguoiTruyCap"] + 1;
            Application["SoNguoiDangOnline"] = (int)Application["SoNguoiDangOnline"] + 1;

            // Application["Online"] = (int)Application["Online"] + 1;
            Application.UnLock();
        }
        protected void Session_End()
        {
            Application.Lock(); //Dùng để đòng bộ hóa
            Application["SoNguoiDangOnline"] = (int)Application["SoNguoiDangOnline"] - 1;
            Application.UnLock();

        }
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            var TaiKhoanCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
               if(TaiKhoanCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(TaiKhoanCookie.Value);
                var Quyen = authTicket.UserData.Split(new Char[] {','});
                var userPrincipal = new GenericPrincipal(new GenericIdentity(authTicket.Name), Quyen);
                Context.User = userPrincipal;
            }

                }
    }
}
