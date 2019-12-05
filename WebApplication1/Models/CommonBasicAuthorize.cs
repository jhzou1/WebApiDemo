using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Security;

namespace WebApplication1.Models
{
    /// <summary>
    /// 通用验证类
    /// </summary>
    public class CommonBasicAuthorize: AuthorizeAttribute
    {
        /// <summary>
        /// 重写系统验证事件
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //从当前Http请求信息中获取用户身份数据
            var userAuthorization = actionContext.Request.Headers.Authorization;

            //如果身份验证数据存在(这个信息肯定是加密过的)
            //Ticket基本格式：{"Success":true,"Ticket":"8899DDYIHJ001..."}
            //Ticket里面就是用户名和密码
            if (userAuthorization != null && userAuthorization.Parameter != null)
            {
                if (CheckTicket(userAuthorization.Parameter))
                {
                    //如果验证通过，则让当前请求获得授权( 如果控件已获得授权，则为 true；否则为 false。)
                    base.IsAuthorized(actionContext);
                }
                else
                {
                    HandleUnauthorizedRequest(actionContext);//验证没有被通过授权（拒绝）
                }
            }
            else//如果请求中，没有验证的票据信息
            {
                var attributeList = 
                    actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>()
                    .OfType<AllowAnonymousAttribute>();
                bool allowAnonymous = attributeList.Any(attribute => 
                attribute is AllowAnonymousAttribute);//判断每一个元素是否允许匿名特性

                if (allowAnonymous)//如果确实有匿名特性，则验证通过
                {
                    base.OnAuthorization(actionContext);
                }
                else
                {
                    HandleUnauthorizedRequest(actionContext);//验证没有被通过授权（拒绝）
                }
            }
        }

        /// <summary>
        /// 验证票据信息中用户名和密码是否正确
        /// </summary>
        /// <param name="ticket">加密后的票据信息</param>
        /// <returns></returns>
        private bool CheckTicket(string ticket)
        {
            //传递的ticket是加密过的，必须解密(解密后的格式：10001&111111)
            var userData = FormsAuthentication.Decrypt(ticket).UserData;

            //从解密后的字符串中获取用户名和密码
            int p = userData.IndexOf("&");
            string userName = userData.Substring(0, p);
            string userPwd = userData.Substring(p + 1);

            //验证用户名和密码是否正确（实际开发中可以通过数据库。。。）就是说每次都得验证一下这个票据是否合格
            if (userName.Equals("1001") && userPwd.Equals("123456"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}