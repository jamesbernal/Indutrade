using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.IO;


namespace PedidosOnline.Utilidades
{
    public class MailSender
    {
        private static String Server { get; set; }
        private static String User { get; set; }
        private static String Password { get; set; }
        private static String Domain { get; set; }
        private static String HtmlEnvelope { get; set; }
        private static String Port { get; set; }
        private static bool EnableSSL { get; set; }
        private static String MailFrom { get; set; }
        private static bool SendMailEnabled { get; set; }

    }
}
