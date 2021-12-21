using LoonshotTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using LoonshotTest.Models.Login;
using Microsoft.AspNetCore.Http;
using System.Net.WebSockets;
using System.Threading;

namespace LoonshotTest.Controllers
{
    public class WebController : Controller
    {
        private readonly ILogger<WebController> _logger;

        public WebController(ILogger<WebController> logger)
        {
            _logger = logger;
        }
        public class WebSocketController : ControllerBase
        {
            [HttpGet("/ws")]
            public async Task Get()
            {
                if (HttpContext.WebSockets.IsWebSocketRequest)
                {
                    using System.Net.WebSockets.WebSocket webSocket = await
                                       HttpContext.WebSockets.AcceptWebSocketAsync();
                    await Echo(HttpContext, webSocket);
                }
                else
                {
                    HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                }
            }

            private async Task Echo(HttpContext httpContext, WebSocket webSocket)
            {
                var buffer = new byte[1024 * 4];
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                while (!result.CloseStatus.HasValue)
                {
                    await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                }
                await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            }
        }
    }
}
