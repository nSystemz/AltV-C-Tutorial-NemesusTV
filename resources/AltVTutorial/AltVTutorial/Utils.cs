using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltVTutorial
{
    class Utils
    {
        public static void adminLog(string text, string username)
        {
            HTTP.Post("https://discord.com/api/webhooks/878302777997152286/oy162sw3YOZhQkrE3agrTm_Jp5qPQx_t7iwAimVt9xSKZ-19kgxNta8NGhlreL73AkIk", new System.Collections.Specialized.NameValueCollection()
            {
                {
                    "username",
                    username
                },
                {
                    "content",
                    text
                }
            });
        }
    }
}
