using System.Net.Mail;

namespace SendEmailExample
{
    public partial class Form_EmailDeBase : Form
    {
        public Form_EmailDeBase()
        {
            InitializeComponent();
        }

        private void Btn_Envoyer_Click(object sender, EventArgs e)
        {
            // creation de l email
            MailMessage email = new MailMessage(
                Tb_AdresseDeLExpediteur.Text,
                TbAdresseDuDestinataire.Text,
                Tb_Objet.Text,
                Tb_CorpsDuTexte.Text
                );
            email.Priority = MailPriority.High;
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");

            smtpClient.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "alban.boytard@gmail.com",
                Password = "hdsf bpof fsza vdbb",
            };
            smtpClient.EnableSsl = true;

            try
            {
                smtpClient.Send(email);
            }
            catch (Exception ex)
            {
                Lb_Error.Text = ex.Message;
            }
        }
    }
}