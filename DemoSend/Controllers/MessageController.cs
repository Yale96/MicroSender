using DemoSend.Models;
using DemoSend.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DemoSend.Controllers
{
    public class MessageController : ApiController
    {
        private MessageRepository messageRepository;
        public MessageController()
        {
            messageRepository = new MessageRepository();
        }
        public Message[] Get()
        {
            return this.messageRepository.GetAllMessages();
        }

        public HttpResponseMessage Post(Message message)
        {
            this.messageRepository.SaveMessage(message);

            var response = Request.CreateResponse<Message>(System.Net.HttpStatusCode.Created, message);

            return response;
        }
    }
}
