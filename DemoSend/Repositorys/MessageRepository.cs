using DemoSend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using RabbitMQ.Client;
using System.Runtime.InteropServices;

using System.Diagnostics;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;

namespace DemoSend.Repositorys
{
    public class MessageRepository
    {
        private const string CacheKey = "MessageStore";

        public MessageRepository()
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                if (ctx.Cache[CacheKey] == null)
                {
                    var messages = new Message[]
                    {
                        new Message
                        {
                            Id = 1,
                            Application = "Werknemerloket",
                            TimeStamp = "13:30",
                            LogLevel = "Fatal",
                            Location = "/Werknemerloket/login.aspx",
                            Content = "Werknemer kan niet inloggen."

                        },
                        new Message
                        {
                            Id = 2,
                            Application = "Werkgeverloket",
                            TimeStamp = "09:45",
                            LogLevel = "Fatal",
                            Location = "/Werkgeverloket/login.aspx",
                            Content = "Werkgever kan niet inloggen."

                        }
                    };
                    ctx.Cache[CacheKey] = messages;
                }
            }
        }

        public Message[] GetAllMessages()
        {
            return new Message[]
            {
                new Message
                        {
                          Id = 1,
                          Application = "Werknemerloket",
                          TimeStamp = "13:30",
                          LogLevel = "Verbose",
                          Location = "/Werknemerloket/login.aspx",
                          Content = "Werknemer kan niet inloggen."
                        },
                        new Message
                        {
                           Id = 2,
                           Application = "Werkgeverloket",
                           TimeStamp = "09:45",
                           LogLevel = "Debug",
                           Location = "/Werkgeverloket/login.aspx",
                           Content = "Werkgever kan niet inloggen."
                        },
                        new Message
                        {
                           Id = 3,
                           Application = "Werknemerloket",
                           TimeStamp = "15:18",
                           LogLevel = "Information",
                           Location = "/Werknemerloket/login.aspx",
                           Content = "Werknemer kan niet inloggen."
                        },
                        new Message
                        {
                           Id = 4,
                           Application = "Werkgeverloket",
                           TimeStamp = "10:45",
                           LogLevel = "Warning",
                           Location = "/Werkgeverloket/login.aspx",
                           Content = "Werkgever kan niet inloggen."
                        },
                        new Message
                        {
                           Id = 5,
                           Application = "Werknemerloket",
                           TimeStamp = "17:30",
                           LogLevel = "Error",
                           Location = "/Werknemerloket/login.aspx",
                           Content = "Werknemer kan niet inloggen."
                        },
                        new Message
                        {
                           Id = 6,
                           Application = "Werkgeverloket",
                           TimeStamp = "19:35",
                           LogLevel = "Fatal",
                           Location = "/Werkgeverloket/login.aspx",
                           Content = "Werkgever kan niet inloggen."
                        }
            };
        }

        public bool SaveMessage(Message message)
        {
            Message m = new Message
            {
                Id = 6,
                Application = "Werkgeverloket"
            };
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    var currentData = ((Message[])ctx.Cache[CacheKey]).ToList();
                    currentData.Add(message);
                    ctx.Cache[CacheKey] = currentData.ToArray();
                    PushToQueue(message);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            return false;
        }

        public void PushToQueue(Message message)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string messageToSend = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(messageToSend);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: "",
                                     routingKey: "hello",
                                     basicProperties: properties,
                                     body: body);
                Debug.WriteLine(" [x] Verzonden {0}", messageToSend);
            }
        }
    }
}