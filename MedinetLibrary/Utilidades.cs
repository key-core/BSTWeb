using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MedinetLibrary
{
    public static class Utilidades
    {
        public static void CrearDirectorio(string directorio)
        {
            try
            {
                //si no existe la carpeta temporal la creamos 
                if (!(Directory.Exists(directorio)))
                {
                    Directory.CreateDirectory(directorio);
                }
            }
            catch (Exception e)
            {
                Excepciones.LogException(e, "Utilidades", "CreaDirectorio");
            }
        }
        public static bool enviarMail(string asunto, string cuerpo, string destinatarios)
        {
            string sEmailServer = "mail.medinetla.com";
            string sEmailPort = "587";
            string sEmailUser = "ereporting@medinetla.com";
            string sEmailPassword = "Medinet2014";
            string sEmailSendTo = destinatarios;
            string sEmailSendFrom = "ereporting@medinetla.com";
            string sEmailSendFromName = "Servicio Transmisor MEDINET";


            SmtpClient smtpClient = new SmtpClient();
            MailMessage message = new MailMessage();

            MailAddress fromAddress = new MailAddress(sEmailSendFrom, sEmailSendFromName);

            //You can have multiple emails separated by ;
            string[] sEmailTo = Regex.Split(sEmailSendTo, ";");
            //string[] sEmailCC = Regex.Split(sEmailSendCC, ";");
            int sEmailServerSMTP = int.Parse(sEmailPort);

            smtpClient.Host = sEmailServer;
            //587
            smtpClient.Port = sEmailServerSMTP;
            //EnableSsl Con GMAIL es true 
            //EnableSsl Con MEDINETLA es false
            smtpClient.EnableSsl = false;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Timeout = 5000;

            System.Net.NetworkCredential myCredentials =
            new System.Net.NetworkCredential(sEmailUser, sEmailPassword);
            smtpClient.Credentials = myCredentials;

            message.From = fromAddress;

            if (sEmailTo != null)
            {
                for (int i = 0; i < sEmailTo.Length; ++i)
                {
                    if (sEmailTo[i] != null && sEmailTo[i] != "")
                    {
                        message.To.Add(sEmailTo[i]);
                    }
                }
            }

            //if (sEmailCC != null)
            //{
            //    for (int i = 0; i < sEmailCC.Length; ++i)
            //    {
            //        if (sEmailCC[i] != null && sEmailCC[i] != "")
            //        {
            //            message.To.Add(sEmailCC[i]);
            //        }
            //    }
            //}
            int iPriority = 1;
            switch (iPriority)
            {
                case 1:
                    message.Priority = MailPriority.High;
                    break;
                case 3:
                    message.Priority = MailPriority.Low;
                    break;
                default:
                    message.Priority = MailPriority.Normal;
                    break;
            }

            //You can enable this for Attachments.  
            //SingleFile is a string variable for the file path.
            //foreach (string SingleFile in myFiles)
            //{
            //    Attachment myAttachment = new Attachment(SingleFile);
            //    message.Attachments.Add(myAttachment);
            //}

            //Attachment myAttachment = new Attachment(RutaImagen);
            //message.Attachments.Add(myAttachment);

            //Attachment myAttachmentf = new Attachment(Dts.Variables["FileName"].Value.ToString());
            //message.Attachments.Add(myAttachmentf);

            message.Subject = asunto;
            message.IsBodyHtml = true;
            message.Body = cuerpo;

            try
            {
                smtpClient.Send(message);
                return true;
            }
            catch (SmtpException)
            {
                return false;

            }
            catch (Exception)
            {
                return false;

            }


            //File.Delete(Dts.Variables["FileName"].Value.ToString());



        }
    }
}
