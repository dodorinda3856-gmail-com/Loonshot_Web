using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoonshotTest.Hubs
{
    public class SendSMS
    {
        public static void Run(string phone )
        {
            MessagingLib.Messages messages = new MessagingLib.Messages();

            messages.Add(new MessagingLib.Message()
            {
                to = "01041670372",
                from = phone,
                //imageId = imageId,
                subject = "MMS 제목",
                text = "이미지 아이디가 입력되면 MMS로 발송됩니다."
            });

            // 1만건까지 추가 가능

            MessagingLib.Response response = MessagingLib.SendMessages(messages);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("전송 결과");
                Console.WriteLine("Group ID:" + response.Data.SelectToken("groupId").ToString());
                Console.WriteLine("Status:" + response.Data.SelectToken("status").ToString());
                Console.WriteLine("Count:" + response.Data.SelectToken("count").ToString());
            }
            else
            {
                Console.WriteLine("Error Code:" + response.ErrorCode);
                Console.WriteLine("Error Message:" + response.ErrorMessage);
            }
        }

    }
}
