using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace UdemyRabbitMQ.Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://awrwdywe:FOsptOs50Ted3R2UFtiQIaSgRKt51y_1@moose.rmq.cloudamqp.com/awrwdywe");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.BasicQos(0,1,true); // Herbir subscriber a kaçar kaçar gelsin mesaj

            var consumer = new EventingBasicConsumer(channel);


            var queueName = "direct-queue-Critical";
            channel.BasicConsume(queueName, false,consumer);  // 2.autoAct parametresi, rabbitmq dan mesaj bana gelince kontrol etmeden silsin mi?
                                                                    // false dersek anlamı: sen silme ben mesajı doğru şekilde işlersem sana söylşycem öyle sil 
            Console.WriteLine("Loglar dinleniyor");

            consumer.Received += (object sender, BasicDeliverEventArgs e) =>     //Event ne zaman fırlıyacak
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                Thread.Sleep(1500);
                Console.WriteLine("Gelen message :" + message);

                File.AppendAllText("log-critical.txt",message + "\n");

                channel.BasicAck(e.DeliveryTag,false);  // rabbitmq ya silmesi için mesaj verdim ilgili tag a göre mesajı buluyor ve siliyor
            };
           
            Console.ReadLine();
        }
    }
}
