using DemoSend.Models;
using DemoSend.Repositorys;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

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


        public HttpResponseMessage Post( Blaa data)
        {
            Test(data.LogLevels);


            var response = Request.CreateResponse<Message>(System.Net.HttpStatusCode.Created, null);

            return response;
        }

        public class Blaa
        {
            public string[] LogLevels { get; set; }
        }


        private void Test(string[] logLevels)
        {
            var messages = this.messageRepository.GetAllMessages();
            var filteredMessages = messages.Where(x => logLevels.Contains(x.LogLevel));
            foreach (var message in filteredMessages)
                this.messageRepository.SaveMessage(message);
        }
    }
}
