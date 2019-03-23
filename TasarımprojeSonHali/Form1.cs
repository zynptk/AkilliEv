using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Management;
using System.Threading;
using System.Collections;
using System.IO.Ports;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
namespace OnlineBookStore
{
    public partial class Form1 : Form
    {
        string[] ports = SerialPort.GetPortNames();
        public Form1()
        {
            InitializeComponent();
        }


        SqlConnection con;
        SqlDataAdapter da;
        SqlCommand cmd;
        DataSet ds;
        int i = 0;
        int kullaniciId;
        private void pngExit_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void pngMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            
        }
        
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string Date;
            if (Control("Username : " + txtUsername.Text + "," + "Password : " + txtPassword.Text) == true)
            {
                timerLogin.Enabled = true;
                pbLogin.Visible = true;
                lblPleaseWait.Visible = true;
                lblProgBar.Visible = true;

                Customer myCustomer = new Customer();
                myCustomer = myCustomer.printCustomerDetails(txtUsername.Text);

                lblCustomerID.Text = myCustomer.getCustomerID().ToString();
                lblUsername.Text = myCustomer.getUsername();
                lblName.Text = myCustomer.getName();
                lblEmail.Text = myCustomer.getEmail();
                lblAddress.Text = myCustomer.getAddress();
                kullaniciId = myCustomer.getCustomerID();
                
                Date = String.Format("{0:r}", DateTime.Now) + "+";
                myCustomer.AppendDateToCustomer("CustomersLoginTimes.txt", "Username with " + myCustomer.getUsername(), Date);
                lblProgBar.Text += myCustomer.getUsername();

          
               
               
            }
            else
            {
                MessageBox.Show("Login Failed !!");
            }
        }
        private bool Control(string item)
        {
            StreamReader readcustomerfile;
            readcustomerfile = File.OpenText(@"CustomersInformations.txt");
            bool controlresult = false;
            string lines;
            int result;
            while ((lines = readcustomerfile.ReadLine()) != null) 
            {
                result = lines.IndexOf(item);
                if (result != -1)
                {
                    controlresult = true;
                    break;
                }
                else
                {
                    controlresult = false;
                }
            }
            readcustomerfile.Close();
            return controlresult;
        }
       
       
        private void btnRegister2_Click(object sender, EventArgs e)
        {
            Customer myCustomer = new Customer();
            if (Control("Username : " + txtregisterUsername.Text) == false && Control("E-Mail : " + txtregisterEmail.Text) == false)
            {
                timerRegister.Enabled = true;
                pbRegister.Visible = true;
                int customerID = 0;
                StreamReader readcustomerfile;
                readcustomerfile = File.OpenText(@"CustomersLoginTimes.txt");
                while (readcustomerfile.ReadLine() != null)
                {
                    customerID++;
                }
                readcustomerfile.Close();
                customerID += 100000;

                 myCustomer = new Customer(customerID, txtregisterUsername.Text, txtregisterPassword.Text, txtregisterName.Text, txtregisterEmail.Text, txtregisterAddress.Text);
                
                myCustomer.saveCustomer();
            


            }
            con = new SqlConnection("Data Source=LAPTOP-C8RLNA0A\\SQLEXPRESS; Initial Catalog=AkilliEv;Integrated Security=True");
            da = new SqlDataAdapter("Select *From Kullanici", con);
            ds = new DataSet();
            con.Open();
            da.Fill(ds, "Kullanici");
            cmd = new SqlCommand();

            cmd.Connection = con;
          
            cmd.CommandText = "INSERT INTO Kullanici(KullaniciId,KullaniciAdi,Email,Sifre,Adres) VALUES(@param5,@param1,@param2,@param3,@param4)";
            cmd.Parameters.AddWithValue("@param5", myCustomer.getCustomerID());
            cmd.Parameters.AddWithValue("@param1", myCustomer.getUsername());
            cmd.Parameters.AddWithValue("@param2", myCustomer.getEmail());
            cmd.Parameters.AddWithValue("@param3", myCustomer.getPassword());
            cmd.Parameters.AddWithValue("@param4", myCustomer.getAddress());
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            Customer.SelectedIndex = 1;
        }

        private void btnBackLogin_Click(object sender, EventArgs e)
        {
            Customer.SelectedIndex = 0;
        }

       

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            if (Customer.SelectedIndex == 0)
            {
                pbLogin.Value = 0;
                timerLogin.Enabled = false;
                txtUsername.Text = "";
                txtPassword.Text = "";
                lblProgBar.Visible = false;
                lblPleaseWait.Visible = false;
                pbLogin.Visible = false;
            }
            if (Customer.SelectedIndex == 3)
            {
                pbLogin.Value = 0;
                timerLogin.Enabled = false;
            }
            if (Customer.SelectedIndex == 1)
            {
                pbRegister.Value = 0;
                timerRegister.Enabled = false;
                pbRegister.Visible = false;
            }

            
        }

       

        private void btnLogout_Click(object sender, EventArgs e)
        {
     
            lblProgBar.Text = "Loginning With Username : ";
       
            Customer.SelectedIndex = 0;

        }

        private void btnUserLoginTimes_Click_1(object sender, EventArgs e)
        {
            int j = 0;
            int control;
            string line;
            string addedstr = "";
            StreamReader readcustomerfile;
            readcustomerfile = File.OpenText(@"CustomersLoginTimes.txt");
            while ((line = readcustomerfile.ReadLine()) != null)
            {
                control = line.IndexOf(lblUsername.Text);
                if (control != -1)
                {
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (line[i] == '|')
                        {
                            j = i + 4;
                            while (line[j] != '|' && line[j] != line[line.Length - 1])
                            {
                                addedstr += line[j];
                                j++;
                                i++;
                            }

                            KullanıcıGirişZamanı.Items.Add(addedstr);
                            addedstr = "";
                        }
                    }
                }
            }
            KullanıcıGirişZamanı.Visible = true;
        }

        private void timerRegister_Tick(object sender, EventArgs e)
        {

            while (pbRegister.Value != 100)
            {
                if (pbRegister.Value == 0)
                {
                    lblRegisterProgBar.Text = "Please Wait For Control...";
                }
                else if (pbRegister.Value == 50)
                {
                    lblRegisterProgBar.Text = "Register Successfull ! ";
                }
                lblRegisterProgBar.Refresh();
                pbRegister.Value += 1;
                Thread.Sleep(25);
                pbRegister.Refresh();
                
            }
            Customer.SelectedIndex = 0;
        }
        private void timerLogin_Tick(object sender, EventArgs e)
        {
            while (pbLogin.Value != 100)
            {
                pbLogin.Value += 1;
                Thread.Sleep(25);
                pbLogin.Refresh();
            }
            Customer.SelectedIndex = 3;
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pbRegister.Visible = false;
            lblRegisterProgBar.Visible = false;
            foreach (string port in ports)
            {
                comboBox1.Items.Add(port);
                comboBox1.SelectedIndex = 0;
            }
            comboBox2.Items.Add("2400");
            comboBox2.Items.Add("4800");
            comboBox2.Items.Add("9600");
            comboBox2.Items.Add("19200");
            comboBox2.Items.Add("115200");
            comboBox2.SelectedIndex = 2;
            //AÇILIŞTA BAĞLANTININ KAPALI OLDUĞUNU BELİRT.
            label35.Text = "Bağlantı Kapalı";
            label36.Text = "Bağlantı Kapalı";
            label25.Text = "Bağlantı Kapalı";
            label29.Text = "Bağlantı Kapalı";

        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            {
                // Form kapandığında SerialPort1 portu kapat.
                if (serialPort1.IsOpen == true)
                {
                    serialPort1.Close();
                }
            }

        }



        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }
        SmtpClient sc = new SmtpClient();
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            label28.Text = " ";
            label31.Text = " ";
            label39.Text = " ";
            label40.Text = " ";
            con = new SqlConnection("Data Source=LAPTOP-C8RLNA0A\\SQLEXPRESS; Initial Catalog=AkilliEv;Integrated Security=True");
            da = new SqlDataAdapter("Select *From veri", con);
            ds = new DataSet();
            con.Open();
            da.Fill(ds, "veri");
     
            con.Close();
            try
            {

                string sonuc;
                sonuc= serialPort1.ReadExisting();
                String[] veri = sonuc.Split(',');
                cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
               

                cmd.CommandText = "INSERT INTO Veri(KullaniciId,Sicaklik,ToprakNemi,Uzaklik,Gaz,Isik) VALUES(@param6,@param7,@param8,@param9,@param10,@param11)";
                cmd.Parameters.AddWithValue("@param6", kullaniciId);
                cmd.Parameters.AddWithValue("@param7", veri[1]);
                cmd.Parameters.AddWithValue("@param8", veri[2]);
                cmd.Parameters.AddWithValue("@param9",veri[3]);
                cmd.Parameters.AddWithValue("@param10",veri[4]);
                cmd.Parameters.AddWithValue("@param11", veri[5]);
                cmd.ExecuteNonQuery();
                con.Close();
                //textBox2.Text = veri[4];
                //textBox1.Text = veri[2];
                //textBox3.Text = veri[3];
                //textBox4.Text = veri[1];
                //textBox5.Text = veri[5];

                //i = i + 6;
                if (Convert.ToDouble(veri[4]) >= 200)
                {
                    textBox2.Text = veri[4];  //Gaz 200
                    label27.Text = "Gaz Kaçağı Var";

                    sc.Port = 587;
                    sc.Host = "smtp.gmail.com";
                    sc.EnableSsl = true;

                    string kime = lblEmail.Text;
                    string konu = "Akıllı Ev Otomasyon Sistemi Verileriniz";
                    string icerik = "Gaz Kaçağı Var";

                    sc.Credentials = new NetworkCredential("akillievtasarim@gmail.com", "esogutasarim");
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("akillievtasarim@gmail.com", "Güvenlik Durumu");
                    mail.To.Add(kime);
                    mail.Subject = konu;
                    mail.IsBodyHtml = true;
                    mail.Body = icerik;

                    sc.Send(mail);
                }
                else
                {
                    textBox2.Text = veri[4];  //Gaz 200
                    label27.Text = " ";
                }

                if (Convert.ToDouble(veri[2]) >= 500)
                {
                    textBox1.Text = veri[2]; //Toprak Nemi  500
                    label32.Text = "Toprak Nemi Yüksek";

                    sc.Port = 587;
                    sc.Host = "smtp.gmail.com";
                    sc.EnableSsl = true;

                    string kime = lblEmail.Text;
                    string konu = "Akıllı Ev Otomasyon Sistemi Verileriniz";
                    string icerik = "Toprak Nemi Yüksek";

                    sc.Credentials = new NetworkCredential("akillievtasarim@gmail.com", "esogutasarim");
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("akillievtasarim@gmail.com", "Güvenlik Durumu");
                    mail.To.Add(kime);
                    mail.Subject = konu;
                    mail.IsBodyHtml = true;
                    mail.Body = icerik;

                    sc.Send(mail);
                }
                else
                {
                    textBox1.Text = veri[2]; //Toprak Nemi  500
                    label32.Text = " ";
                }
                if (Convert.ToDouble(veri[3]) >= 20)
                {
                    textBox3.Text = veri[3]; //Uzaklık
                    label33.Text = "Kapınız Açıldı";

                    sc.Port = 587;
                    sc.Host = "smtp.gmail.com";
                    sc.EnableSsl = true;

                    string kime = lblEmail.Text;
                    string konu = "Akıllı Ev Otomasyon Sistemi Verileriniz";
                    string icerik = "Kapınız Açıldı";

                    sc.Credentials = new NetworkCredential("akillievtasarim@gmail.com", "esogutasarim");
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("akillievtasarim@gmail.com", "Güvenlik Durumu");
                    mail.To.Add(kime);
                    mail.Subject = konu;
                    mail.IsBodyHtml = true;
                    mail.Body = icerik;

                    sc.Send(mail);
                }
                else
                {
                    textBox3.Text = veri[3]; //Uzaklık
                    label33.Text = " ";
                }
                if (Convert.ToDouble(veri[1]) <= 26)
                {
                    textBox4.Text = veri[1]; //Sıcaklık
                    label34.Text = "Sıcaklık Yüksek";

                    sc.Port = 587;
                    sc.Host = "smtp.gmail.com";
                    sc.EnableSsl = true;

                    string kime = lblEmail.Text;
                    string konu = "Akıllı Ev Otomasyon Sistemi Verileriniz";
                    string icerik = "Sıcaklık Yüksek";
                    sc.Credentials = new NetworkCredential("akillievtasarim@gmail.com", "esogutasarim");
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("akillievtasarim@gmail.com", "Güvenlik Durumu");
                    mail.To.Add(kime);
                    mail.Subject = konu;
                    mail.IsBodyHtml = true;
                    mail.Body = icerik;

                    sc.Send(mail);
                }
                else
                {
                    textBox4.Text = veri[1]; //Sıcaklık
                    label34.Text = " ";
                }
                if (Convert.ToDouble(veri[5]) >= 500)
                {
                    textBox5.Text = veri[5]; //Işık
                    label44.Text = "Işık Şiddeti Yüksek";

                    sc.Port = 587;
                    sc.Host = "smtp.gmail.com";
                    sc.EnableSsl = true;

                    string kime = lblEmail.Text;
                    string konu = "Akıllı Ev Otomasyon Sistemi Verileriniz";
                    string icerik = "Işık Şiddeti Yüksek";

                    sc.Credentials = new NetworkCredential("akillievtasarim@gmail.com", "esogutasarim");
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("akillievtasarim@gmail.com", "Güvenlik Durumu");
                    mail.To.Add(kime);
                    mail.Subject = konu;
                    mail.IsBodyHtml = true;
                    mail.Body = icerik;

                    sc.Send(mail);
                }
                else
                {
                    textBox5.Text = veri[5]; //Işık
                    label44.Text = " ";
                }

                Array.Clear(veri, 0, veri.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                timer1.Stop();
            }
            
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
            if (serialPort1.IsOpen == false)
            {
                if (comboBox1.Text == "")
                    return;
                serialPort1.PortName = comboBox1.Text;
                serialPort1.BaudRate = Convert.ToInt16(comboBox2.Text);
                try
                {
                    serialPort1.Open();
                    label25.ForeColor = Color.Green;
                    label25.Text = "Bağlantı Açık";
                    label29.ForeColor = Color.Green;
                    label29.Text = "Bağlantı Açık";
                    label35.ForeColor = Color.Green;
                    label35.Text = "Bağlantı Açık";
                    label36.ForeColor = Color.Green;
                    label36.Text = "Bağlantı Açık";
                    label43.ForeColor = Color.Green;
                    label43.Text = "Bağlantı Açık";

                }
                catch (Exception hata)
                {
                    MessageBox.Show("Hata:" + hata.Message);
                }
            }
            else
            {
                label25.Text = "Bağlantı kurulu !!!";
                label29.Text = "Bağlantı kurulu !!!";
                label35.Text = "Bağlantı kurulu !!!";
                label36.Text = "Bağlantı kurulu !!!";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            if (serialPort1.IsOpen == true)
            {
                serialPort1.Close();
                label25.ForeColor = Color.Red;
                label25.Text = "Bağlantı Kapalı";
                label29.ForeColor = Color.Red;
                label29.Text = "Bağlantı Kapalı";
                label35.ForeColor = Color.Red;
                label35.Text = "Bağlantı Kapalı";
                label36.ForeColor = Color.Red;
                label36.Text = "Bağlantı Kapalı";
            }
        }

      
    }
}
