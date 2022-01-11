using System;

static class SendLMS
{
    public static void Run(string phone)
    {
        // TextingLib.GetGroupList()
        // Dim group = New TextingLib.Group()
        // Console.WriteLine(group.GetList())

        MessagingLib.Messages messages = new MessagingLib.Messages();
        messages.Add(new MessagingLib.Message()
        {
            to = phone,
            from = "01020933698",
            text = "안녕하세요. 이지피부과입니다. 3분 이후 고객님의 진료차례 입니다. 대략 30분이후 방문을 부탁들리며, 도움이 필요하시면 02-324-4413 로 연락 주세요."
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
