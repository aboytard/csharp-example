namespace SendEmailExample
{
    partial class Form_EmailDeBase
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Lb_AdresseDeLExpediteur = new Label();
            Tb_AdresseDeLExpediteur = new TextBox();
            Lb_AdresseDuDestinataire = new Label();
            TbAdresseDuDestinataire = new TextBox();
            Lb_Objet = new Label();
            Tb_Objet = new TextBox();
            Lb_CorpsDuTexte = new Label();
            Tb_CorpsDuTexte = new TextBox();
            Btn_Envoyer = new Button();
            Lb_Error = new Label();
            SuspendLayout();
            // 
            // Lb_AdresseDeLExpediteur
            // 
            Lb_AdresseDeLExpediteur.AutoSize = true;
            Lb_AdresseDeLExpediteur.Location = new Point(124, 91);
            Lb_AdresseDeLExpediteur.Name = "Lb_AdresseDeLExpediteur";
            Lb_AdresseDeLExpediteur.Size = new Size(345, 41);
            Lb_AdresseDeLExpediteur.TabIndex = 0;
            Lb_AdresseDeLExpediteur.Text = "Adresse de l expediteur :";
            // 
            // Tb_AdresseDeLExpediteur
            // 
            Tb_AdresseDeLExpediteur.Location = new Point(498, 85);
            Tb_AdresseDeLExpediteur.Name = "Tb_AdresseDeLExpediteur";
            Tb_AdresseDeLExpediteur.Size = new Size(649, 47);
            Tb_AdresseDeLExpediteur.TabIndex = 1;
            // 
            // Lb_AdresseDuDestinataire
            // 
            Lb_AdresseDuDestinataire.AutoSize = true;
            Lb_AdresseDuDestinataire.Location = new Point(126, 177);
            Lb_AdresseDuDestinataire.Name = "Lb_AdresseDuDestinataire";
            Lb_AdresseDuDestinataire.Size = new Size(343, 41);
            Lb_AdresseDuDestinataire.TabIndex = 2;
            Lb_AdresseDuDestinataire.Text = "Adresse du destinataire :";
            // 
            // TbAdresseDuDestinataire
            // 
            TbAdresseDuDestinataire.Location = new Point(499, 171);
            TbAdresseDuDestinataire.Name = "TbAdresseDuDestinataire";
            TbAdresseDuDestinataire.Size = new Size(646, 47);
            TbAdresseDuDestinataire.TabIndex = 3;
            // 
            // Lb_Objet
            // 
            Lb_Objet.AutoSize = true;
            Lb_Objet.Location = new Point(128, 261);
            Lb_Objet.Name = "Lb_Objet";
            Lb_Objet.Size = new Size(115, 41);
            Lb_Objet.TabIndex = 4;
            Lb_Objet.Text = "Objet : ";
            // 
            // Tb_Objet
            // 
            Tb_Objet.Location = new Point(491, 269);
            Tb_Objet.Name = "Tb_Objet";
            Tb_Objet.Size = new Size(656, 47);
            Tb_Objet.TabIndex = 5;
            // 
            // Lb_CorpsDuTexte
            // 
            Lb_CorpsDuTexte.AutoSize = true;
            Lb_CorpsDuTexte.Location = new Point(128, 355);
            Lb_CorpsDuTexte.Name = "Lb_CorpsDuTexte";
            Lb_CorpsDuTexte.Size = new Size(236, 41);
            Lb_CorpsDuTexte.TabIndex = 6;
            Lb_CorpsDuTexte.Text = "Corps du texte : ";
            // 
            // Tb_CorpsDuTexte
            // 
            Tb_CorpsDuTexte.AcceptsReturn = true;
            Tb_CorpsDuTexte.AcceptsTab = true;
            Tb_CorpsDuTexte.AllowDrop = true;
            Tb_CorpsDuTexte.Location = new Point(489, 355);
            Tb_CorpsDuTexte.Multiline = true;
            Tb_CorpsDuTexte.Name = "Tb_CorpsDuTexte";
            Tb_CorpsDuTexte.Size = new Size(656, 301);
            Tb_CorpsDuTexte.TabIndex = 7;
            // 
            // Btn_Envoyer
            // 
            Btn_Envoyer.Location = new Point(820, 675);
            Btn_Envoyer.Name = "Btn_Envoyer";
            Btn_Envoyer.Size = new Size(325, 88);
            Btn_Envoyer.TabIndex = 8;
            Btn_Envoyer.Text = "Envoyer";
            Btn_Envoyer.UseVisualStyleBackColor = true;
            Btn_Envoyer.Click += Btn_Envoyer_Click;
            // 
            // Lb_Error
            // 
            Lb_Error.AutoSize = true;
            Lb_Error.Location = new Point(126, 699);
            Lb_Error.Name = "Lb_Error";
            Lb_Error.Size = new Size(125, 41);
            Lb_Error.TabIndex = 9;
            Lb_Error.Text = "Lb_Error";
            // 
            // Form_EmailDeBase
            // 
            AutoScaleDimensions = new SizeF(17F, 41F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1343, 860);
            Controls.Add(Lb_Error);
            Controls.Add(Btn_Envoyer);
            Controls.Add(Tb_CorpsDuTexte);
            Controls.Add(Lb_CorpsDuTexte);
            Controls.Add(Tb_Objet);
            Controls.Add(Lb_Objet);
            Controls.Add(TbAdresseDuDestinataire);
            Controls.Add(Lb_AdresseDuDestinataire);
            Controls.Add(Tb_AdresseDeLExpediteur);
            Controls.Add(Lb_AdresseDeLExpediteur);
            Name = "Form_EmailDeBase";
            Text = "Email de Base";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label Lb_AdresseDeLExpediteur;
        private TextBox Tb_AdresseDeLExpediteur;
        private Label Lb_AdresseDuDestinataire;
        private TextBox TbAdresseDuDestinataire;
        private Label Lb_Objet;
        private TextBox Tb_Objet;
        private Label Lb_CorpsDuTexte;
        private TextBox Tb_CorpsDuTexte;
        private Button Btn_Envoyer;
        private Label Lb_Error;
    }
}