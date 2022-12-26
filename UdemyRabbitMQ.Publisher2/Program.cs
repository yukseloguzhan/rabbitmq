using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace UdemyRabbitMQ.Publisher2
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
            channel.ExchangeDeclare("logs-topic", durable: true, type: ExchangeType.Topic); // 2.parametre uygulamaya restart atınca kaybolmasın diye

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;           // Mesajları kalıcı yapmak için 

            Random rnd = new Random();
            for (int i = 1; i <= 50; i++)
            {

                LogNames log1 = (LogNames)rnd.Next(1, 5);
                LogNames log2 = (LogNames)rnd.Next(1, 5);
                LogNames log3 = (LogNames)rnd.Next(1, 5);

                var routeKey = $"{log1}.{log2}.{log3}";
                string message = $"log-type: {log1} - {log2} - {log3}";
                var messageBody = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("logs-topic", routeKey, properties, messageBody); // ilk parametre exchange ismi   // mesajı kalıcı yapmak için properties ekledim
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
