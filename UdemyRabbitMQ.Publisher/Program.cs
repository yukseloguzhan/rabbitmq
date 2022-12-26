using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace UdemyRabbitMQ.Publisher
{

    public enum LogNames
    {
        Critical = 1,
        Error = 2,
        Warning = 3,
        Info = 4
    }


    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://awrwdywe:FOsptOs50Ted3R2UFtiQIaSgRKt51y_1@moose.rmq.cloudamqp.com/awrwdywe");

            using var connection = factory.CreateConnection();
                                            
            var channel = connection.CreateModel();
            channel.ExchangeDeclare("logs-direct", durable:true, type: ExchangeType.Direct); // 2.parametre uygulamaya restart atınca kaybolmasın diye


            Enum.GetNames(typeof(LogNames)).ToList().ForEach(x=>
            {
                var queueName = $"direct-queue-{x}";
                var routeKey = $"route-{x}";
                channel.QueueDeclare(queueName,true,false,false);

                channel.QueueBind(queueName, "logs-direct", routeKey,null);

            });



            for (int i = 1; i <= 50; i++)
            {
                LogNames log = (LogNames) new Random().Next(1,5);
                string message = $"log-type : {log}";  // byte dizisi şeklinde gönderilir
                var messageBody = Encoding.UTF8.GetBytes(message);

                var routeKey = $"route-{log}";

                channel.BasicPublish("logs-direct", routeKey, null, messageBody); // ilk parametre exchange ismi
                Console.WriteLine($"Log gönderildi {message}");
            }


            Console.ReadLine();

            // 1.parametre kuyruk ismi ,
            // RabbitMq ya bir kanal üzerinden bağlanıcam
            // kanal aracılığıyla mesajı göndercem ama mesaj boşa gitmesin diye kuyruk oluşturucam
            // 2.false: kuyruklar rabbitmq restart atınca yok olur, true : fiziksel kuyruklar kaydedilir
            // 3. sadece bu kanal üzerinden mi bağlansın? , consumer da bağlanacağı için false dedim
            // 4. bağlı olan son subscriber da bağlantısını koparırsa kuyruk silinsin mi?




        }
    }
}
