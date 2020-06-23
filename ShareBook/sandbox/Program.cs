using System;
using MailKit.Net.Smtp;
using MimeKit;

namespace sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Bora enviar um email de teste?");
            Console.ReadLine();

            Console.WriteLine("Aguarde. Enviando...");

            emailTest();

            Console.WriteLine("Email enviado com sucesso.");

        }

        static void emailTest()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Sharebook", "contato@sharebook.com.br"));
            message.To.Add(new MailboxAddress("Raffaello", "raffacabofrio@gmail.com"));
            message.Subject = "Teste email .net core";
            message.Body = new TextPart("plain")
            {
                Text = @"Testando..."
            };

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect("mail.sharebook.com.br", 465, true);
                client.Authenticate("contato_dev_stg@sharebook.com.br", "W9_m_A__Df5_v:_");
                client.Send(message);
                client.Disconnect(true);
            }

        }

        static void emailTest2()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Sharebook", "contato_dev_stg@sharebook.com.br"));
            message.To.Add(new MailboxAddress("Raffaello", "raffacabofrio@gmail.com"));
            message.Cc.Add(new MailboxAddress("Sharebook", "contato_dev_stg@sharebook.com.br"));
            message.Subject = "Teste email sem SSH";
            message.Body = new TextPart("plain")
            {
                Text = @"Testando... " + System.DateTime.Now.ToString() +
                "(sem SSL)"
            };

            using (var client = new SmtpClient())
            {

                var useSSL = false;
                client.Connect("mail.sharebook.com.br", 8889, useSSL);
                client.Authenticate("contato_dev_stg@sharebook.com.br", "W9_m_A__Df5_v:_");
                client.Send(message);
                client.Disconnect(true);
            }

        }
    }
}
