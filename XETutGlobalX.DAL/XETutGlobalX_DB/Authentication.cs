using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using XEtutGlobalX.Modal.XETutGlobalX_DB.Request;
using XETutGlobalX.DAL.App_Code;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace XETutGlobalX.DAL.XETutGlobalX_DB
{
    public class Authentication
    {
        public DataSet LoginProcess(string username, string password, string EmailID, string parameter, App_Code.DBConnectMode.DBConnect_Mode mode,string fromEmail,string fromPassword)
        {
            SqlConnection conn = new();
            string dbconnectionString = string.Empty;
            DataSet dsResponse = new();
            string otpkey=string.Empty;
            try
            {
                if (username != null && password != null && EmailID != null)
                {

                    XETutGlobalX_DB.DBConnect dBConnect = new();
                    switch (mode)
                    {
                        case DBConnectMode.DBConnect_Mode.Simple:
                            dbconnectionString = dBConnect.GetConnectionString(parameter);
                            break;
                        case DBConnectMode.DBConnect_Mode.Complex:
                            string[] connectionSet = parameter.Split(';');
                            string DataSource = string.Empty; string InitialCatelog = string.Empty;
                            bool Encrypt = true;
                            bool IntegratedSecurity = true;
                            bool TrustServerCertificate = true;
                            //DBConnect = DataSource + ";" + InitialCatalog + ";" + IntegratedSecurity + ";" + Encrypt +";" +  TrustServerCertificate + ";";
                            DataSource = connectionSet[0];
                            InitialCatelog = connectionSet[1];
                            IntegratedSecurity = Convert.ToBoolean(connectionSet[2]);
                            Encrypt = Convert.ToBoolean(connectionSet[3]);
                            TrustServerCertificate = Convert.ToBoolean(connectionSet[4]);


                            dbconnectionString = dBConnect.GetConnectionString(DataSource, InitialCatelog, Encrypt, IntegratedSecurity, TrustServerCertificate);
                            break;

                    }
                    if (dbconnectionString != null && dbconnectionString != string.Empty)
                    {
                        conn.ConnectionString = dbconnectionString;
                        conn = dBConnect.DBConnectOpen(conn);
                        otpkey = GenerateOtp();
                        
                        SqlCommand cmd = new("usp_Login", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@pLoginName", username);
                        cmd.Parameters.AddWithValue("@pEmailID", EmailID);
                        cmd.Parameters.AddWithValue("@pPassword", password);
                        if (otpkey != null)
                        {
                            if (otpkey != string.Empty)
                            {
                                
                                cmd.Parameters.AddWithValue("@otpkey", Convert.ToInt32(otpkey));
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@otpkey", Convert.ToInt32("0"));
                            }
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@otpkey", Convert.ToInt32("0"));
                        }
                        

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dsResponse);
                    }
                }
            }
            catch
            {
                dsResponse = null!;
                throw;
            }
            
            return dsResponse;
        }
        static string GenerateOtp()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString(); // Generates a 6-digit OTP
        }
        public string SendOtpEmail(string from_Email,string from_Password,string toEmail, string otp)
        {
            bool isMailSend = true;
            string SendMessage = string.Empty;
            string fromEmail = from_Email; // Replace with your email address
            string fromPassword = from_Password; // Replace with your email password

           
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(fromEmail);
            mail.To.Add(toEmail);
            mail.Subject = "Your OTP Code";
            mail.Body = $"Your OTP code is {otp}";

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com") // Replace with your SMTP server
            {
                Port = 587,
                Credentials = new NetworkCredential(fromEmail, fromPassword),
                EnableSsl = true,
            };

            try
            {
                smtpClient.Send(mail);
                isMailSend=true;
                SendMessage = $"Send email Successfully";
            }
            catch (Exception ex)
            {
                isMailSend = false;
                SendMessage =  $"Failed to send email: {ex.Message}";
            }
            string jsonResponse = JsonSerializer.Serialize(new { Status=isMailSend, Message = SendMessage });
            return jsonResponse;
            //return "'SendOTPMailResponse': [{'Status':" + isMailSend.ToString() + "," + "'Message': "+ SendMessage + "}]}";
        }
        public DataSet clsRegistrationProcess(clsRegistration_RQ clsRegistration_RQ, string parameter, App_Code.DBConnectMode.DBConnect_Mode mode)
        {
            DataSet ds = new();
            try
            {
                Personal personal = new Personal();

                Contact contact = new Contact();
                LoginCredential loginCredential = new LoginCredential();
                List<Registration_RequestBody> requestBody = new List<Registration_RequestBody>();
                if (clsRegistration_RQ != null)
                {
                    if (clsRegistration_RQ._Request != null)
                    {
                        if (clsRegistration_RQ._Request.RequestBody != null)
                        {
                            requestBody = clsRegistration_RQ._Request.RequestBody;
                            foreach (var rg_Item in requestBody)
                            {
                                if (rg_Item.Personal != null)
                                {
                                    personal = rg_Item.Personal;
                                }
                                if (rg_Item.Contact != null)
                                {
                                    contact = rg_Item.Contact;
                                }
                                if (rg_Item.LoginCredential != null)
                                {
                                    loginCredential = rg_Item.LoginCredential;
                                }

                            }
                        }
                    }
                }
                if(personal != null && contact!=null && loginCredential!=null)
                {
                    SqlConnection conn = new();
                    string dbconnectionString = string.Empty;
                    XETutGlobalX_DB.DBConnect dBConnect = new();
                    switch (mode)
                    {
                        case DBConnectMode.DBConnect_Mode.Simple:
                            dbconnectionString = dBConnect.GetConnectionString(parameter);
                            break;
                        case DBConnectMode.DBConnect_Mode.Complex:
                            string[] connectionSet = parameter.Split(';');
                            string DataSource = string.Empty; string InitialCatelog = string.Empty;
                            bool Encrypt = true;
                            bool IntegratedSecurity = true;
                            bool TrustServerCertificate = true;
                            //DBConnect = DataSource + ";" + InitialCatalog + ";" + IntegratedSecurity + ";" + Encrypt +";" +  TrustServerCertificate + ";";
                            DataSource = connectionSet[0];
                            InitialCatelog = connectionSet[1];
                            IntegratedSecurity = Convert.ToBoolean(connectionSet[2]);
                            Encrypt = Convert.ToBoolean(connectionSet[3]);
                            TrustServerCertificate = Convert.ToBoolean(connectionSet[4]);


                            dbconnectionString = dBConnect.GetConnectionString(DataSource, InitialCatelog, Encrypt, IntegratedSecurity, TrustServerCertificate);
                            break;

                    }
                    if (dbconnectionString != null && dbconnectionString != string.Empty)
                    {
                        conn.ConnectionString = dbconnectionString;
                        conn = dBConnect.DBConnectOpen(conn);
                        SqlCommand cmd = new("SP_Insert_UserLoginDet", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (personal.Item!=null)
                        {
                            cmd.Parameters.AddWithValue("@FName", personal.Item.FirstName);
                            cmd.Parameters.AddWithValue("@MName", personal.Item.MiddleName);
                            cmd.Parameters.AddWithValue("@LName", personal.Item.LastName);
                            cmd.Parameters.AddWithValue("@Gender", personal.Item.Gender);
                            cmd.Parameters.AddWithValue("@DOB", personal.Item.dateOfBirth);
                            cmd.Parameters.AddWithValue("@EmailID", personal.Item.EmailId);
                            cmd.Parameters.AddWithValue("@Designation_Code", personal.Item.desig_Code);
                            cmd.Parameters.AddWithValue("@Designation", personal.Item.desig_Name);
                            if(contact.Item!=null)
                            {
                                cmd.Parameters.AddWithValue("@Country", contact.Item.country);
                                cmd.Parameters.AddWithValue("@City", contact.Item.city);
                                cmd.Parameters.AddWithValue("@State", contact.Item.state);
                                cmd.Parameters.AddWithValue("@Pincode", contact.Item.postalCode);
                                cmd.Parameters.AddWithValue("@Address", contact.Item.currentAddress);
                                cmd.Parameters.AddWithValue("@phone_code", contact.Item.phonecountrycode);
                                cmd.Parameters.AddWithValue("@Contact", contact.Item.phoneNumber);
                            }
                            if(loginCredential.Item != null)
                            {
                                
                                cmd.Parameters.AddWithValue("@UserName", loginCredential.Item.userName);
                                cmd.Parameters.AddWithValue("@Password", loginCredential.Item.password);
                                cmd.Parameters.AddWithValue("@ConfirmPassword", loginCredential.Item.confirm_password);

                            }

                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(ds);
                        }

                    }
                }
                
            }
            catch 
            {
                ds = null!;
            }

            return ds;
        }
    }
}
