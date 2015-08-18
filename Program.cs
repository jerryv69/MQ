using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using RabbitMQ.Client;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory connFactory = new ConnectionFactory
            {
                // AppSettings["CLOUDAMQP_URL"] contains the connection string
                // when you've added the CloudAMQP Addon
                //Uri = System.Configuration.ConfigurationManager.AppSettings["CLOUDAMQP_URL"];
                Uri = "amqp://rryotcdm:DPRstPbeaC4SNGsFKzhrJR0MaTfw-rEB@owl.rmq.cloudamqp.com/rryotcdm"
            };

            // Open up a connection and a channel (a connection may have many channels)
            using (var conn = connFactory.CreateConnection())
            using (var channel = conn.CreateModel()) // Note, don't share channels between threads
            {
                // The message we want to put on the queue
                var message = DateTime.Now.ToString("F");

                // the data put on the queue must be a byte array
                var data = Encoding.UTF8.GetBytes(message);

                // ensure that the queue exists before we publish to it
                channel.QueueDeclare("queue1", false, false, false, null);

                // publish to the "default exchange", with the queue name as the routing key
                channel.BasicPublish("", "queue1", null, data);

                System.Console.WriteLine("Message Published Done"); 
            }
        }
    }
}
