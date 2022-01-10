using System;
using System.Threading.Tasks;
using Application.Commands;
using Application.Queries;
using Kibrit.Smpp.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class MessageController : ApiControllerBase
    {
        [HttpPost("send")]
        public async Task<ActionResult<Guid>> Send(SendMessageCommand command)
        {
            return await Mediator.Send(command);
        }
        
        [HttpPost("status")]
        public async Task<ActionResult<int>> Status(GetMessageStateQuery query)
        {
            return await Mediator.Send(query);
        }
    }
}