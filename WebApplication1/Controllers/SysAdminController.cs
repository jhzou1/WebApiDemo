using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    //路由前缀
    [RoutePrefix("api/SysAdmin")]
    //实现验证特性
    [CommonBasicAuthorize]
    public class SysAdminController : ApiController
    {
        //[AllowAnonymous]//可以不加
        [Route("AdminLogin")]
        [AllowAnonymous]//不用授权可以访问
        [HttpPost]
        public string AdminLogin(Models.SysAdmin admin)
        {
            if (CheckLogin(admin))
            {
                //[1]创建身份验证的票据
                FormsAuthenticationTicket userTikect = new FormsAuthenticationTicket
                (
                    0, admin.LoginId,
                    DateTime.Now,
                    DateTime.Now.AddHours(1),
                    true,
                    $"{admin.LoginId}&{admin.LoginPwd}",
                    FormsAuthentication.FormsCookiePath
                );
                //[2]将身份证票据加密后封装
                var encrypetTicket = new
                {
                    Success = true,
                    Ticket = FormsAuthentication.Encrypt(userTikect)
                };
                //[3]序列化身份票据Json格式返回
                return Newtonsoft.Json.JsonConvert.SerializeObject(encrypetTicket);
            }
            else
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(new { Success = false });
            }
        }
        public bool CheckLogin(Models.SysAdmin admin)
        {
            //从数据库中验证用户密码正确性
            if (admin.LoginId == "1001" && admin.LoginPwd == "123456")
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        [Route("QueryAdminNameById")]
        [HttpPost]
        public string QueryAdminNameById(dynamic param)
        {
            return $"欢迎{param.LoginId}管理员:";
        }
    }
}
