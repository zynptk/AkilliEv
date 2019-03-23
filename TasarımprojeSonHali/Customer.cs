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

namespace OnlineBookStore
{
    public class Customer
    {
        private int m_customerID;
        private string m_Name;
        private string m_Address;
        private string m_Email;
        private string m_Username;
        private string m_Password;
        public Customer()
        {
        
        }
        public Customer(int customerID, string Username, string Password, string Name, string Email, string Address)
        {
            m_customerID = customerID;
            m_Username = Username;
            m_Password = Password;
            m_Name = Name;
            m_Email = Email;
            m_Address = Address;
        }
        //CustomerID
        public int getCustomerID()
        {
            return m_customerID;
        }
        public void setCustomerID(int customerID)
        {
            m_customerID = customerID;
        }

        //Username
        public string getUsername()
        {
            return m_Username;
        }
        public void setUsername(string Username)
        {
            m_Username = Username;
        }

        //Password
        public string getPassword()
        {
            return m_Password;
        }
        public void setPassword(string Password)
        {
            m_Password = Password;
        }

        //Name
        public string getName()
        {
            return m_Name;
        }
        public void setName(string Name)
        {
            m_Name = Name;
        }

        //E-Mail
        public string getEmail()
        {
            return m_Email;
        }
        public void setEmail(string Email)
        {
            m_Email = Email;
        }

        //Address
        public string getAddress()
        {
            return m_Address;
        }
        public void setAddress(string Address)
        {
            m_Address = Address;
        }

        public void saveCustomer()
        {
            StreamWriter customerloginfile = File.AppendText(@"CustomersLoginTimes.txt");
            customerloginfile.WriteLine("Username with " + this.getUsername() + "'s Login Times : ");

            StreamWriter customerfile = File.AppendText(@"CustomersInformations.txt");
            customerfile.WriteLine("Customer ID : " + this.getCustomerID() + "," +
                                   "Username : " + this.getUsername() + "," +
                                   "Password : " + this.getPassword() + "," +
                                   "Name : " + this.getName() + "," +
                                   "E-Mail : " + this.getEmail() + "," +
                                   "Address : " + this.getAddress());
            customerfile.Close();
            customerloginfile.Close();
        }
        public Customer printCustomerDetails(string username)
        {
            Customer myCustomer = new Customer();

            StreamReader readcustomerfile;
            readcustomerfile = File.OpenText(@"CustomersInformations.txt");
            string info = "";
            string line;
            int control = 0;
            int first, last;

            while ((line = readcustomerfile.ReadLine()) != null)
            {
                control = line.IndexOf(username);
                if (control != -1)
                {
                    //Set CustomerID To Customer With Read Text File
                    first = line.IndexOf("Customer ID : ") + "Customer ID : ".Length;
                    last = line.IndexOf(",Username");
                    info = line.Substring(first, last - first);
                    myCustomer.setCustomerID(Convert.ToInt32(info));


                    //Set Username To Customer With Read Text File
                    first = line.IndexOf("Username : ") + "Username : ".Length;
                    last = line.IndexOf(",Password");
                    info = line.Substring(first, last - first);
                    myCustomer.setUsername(info);


                    //Set Password To Customer With Read Text File
                    first = line.IndexOf("Password : ") + "Password : ".Length;
                    last = line.IndexOf(",Name");
                    info = line.Substring(first, last - first);
                    myCustomer.setPassword(info);


                    //Set Name To Customer With Read Text File
                    first = line.IndexOf("Name : ") + "Name : ".Length;
                    last = line.IndexOf(",E-Mail");
                    info = line.Substring(first, last - first);
                    myCustomer.setName(info);


                    //Set E-Mail To Customer With Read Text File
                    first = line.IndexOf("E-Mail : ") + "E-Mail : ".Length;
                    last = line.IndexOf(",Address");
                    info = line.Substring(first, last - first);
                    myCustomer.setEmail(info);


                    //Set Address To Customer With Read Text File
                    first = line.IndexOf("Address : ") + "Address : ".Length;
                    last = line.Length;
                    info = line.Substring(first, last - first);
                    myCustomer.setAddress(info);
                }
            }
            return myCustomer;
        }
        public void AppendDateToCustomer(string filename, string username, string appenddate)
        {
            string line;
            int a;
            string tempfile = Path.GetTempFileName();
            using (var writer = new StreamWriter(tempfile))
            using (var reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    a = line.IndexOf(username);
                    if (a != -1)
                    {
                        line = line + " ||| " + appenddate;
                    }
                    writer.WriteLine(line);
                }
            }
            File.Copy(tempfile, filename, true);
        }
        
    }
}
