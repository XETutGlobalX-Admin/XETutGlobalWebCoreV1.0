using Azure;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XEtutGlobalX.Modal.Material;
using XEtutGlobalX.Modal.XETutGlobalX_DB.Request.UserProfile;
using XETutGlobalX.DAL.App_Code;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace XETutGlobalX.DAL.XETutGlobalX_DB
{
    public class UserProfile
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="parameter"></param>
        /// <param name="DBConnect"></param>
        /// <returns></returns>
        public DataSet GetLoginUserDetails(App_Code.DBConnectMode.DBConnect_Mode mode, string parameter, string DBConnect)
        {
            DataSet dsResponse = new DataSet();
            string[] userInputSet = parameter.Split("||");
            SqlConnection conn = new();
            string dbconnectionString = string.Empty;
            int login_UserKey; string sGuid;
            try
            {
                login_UserKey = Convert.ToInt32(userInputSet[0]);
                sGuid = userInputSet[1];
                if(login_UserKey != 0 && sGuid!=null) 
                {
                    
                    
                    XETutGlobalX_DB.DBConnect dBConnect = new();
                    switch (mode)
                    {
                        case DBConnectMode.DBConnect_Mode.Simple:
                            dbconnectionString = dBConnect.GetConnectionString(DBConnect);
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
                        
                        SqlCommand cmd = new("sp_GetLoginUserDetails", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserKey", login_UserKey);
                        cmd.Parameters.AddWithValue("@Guid", sGuid);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dsResponse);
                    }
                    else
                    {
                        dsResponse = null!;
                    }
                }
                else
                {
                    dsResponse = null!;
                }
            }
            catch(Exception ex) 
            {
                dsResponse = null!;
                throw;
            }
            return dsResponse;
        }
        public DataSet UpdateLoginUserProfile(List<PersonalInfo> personalInfos, List<ContactInfo> contactInfos, string parameter, App_Code.DBConnectMode.DBConnect_Mode mode, string DBConnect)
        {
            DataSet dsResponse = new();
            string UserID = string.Empty;
            string profilePhoto = string.Empty;
            string firstName = string.Empty;
            string middleName = string.Empty;
            string lastName = string.Empty;
            string Gender = string.Empty;
            string login_emailID = string.Empty;
            string DOB = string.Empty;
            string LoggedInType = string.Empty;
            string LoggedDesignation = string.Empty;
            SqlConnection conn = new();
            string Country = string.Empty;
            string State = string.Empty;
            string City = string.Empty;
            string PinCode = string.Empty;
            string CurrentAddr = string.Empty;
            string Phone_Number = string.Empty;
            string Phone_Code = string.Empty;
            string dbconnectionString = string.Empty;
            try
            {
                foreach (PersonalInfo person_item in personalInfos)
                {
                    if (person_item != null)
                    {
                        if (person_item.UserID != null)
                        {
                            UserID = person_item.UserID!;
                        }
                        if (person_item.DOB != null)
                        {
                            DOB = person_item.DOB;
                        }
                        if (person_item.firstName != null)
                        {
                            firstName = person_item.firstName;
                        }
                        if (person_item.middleName != null)
                        {
                            middleName = person_item.middleName;
                        }
                        if (person_item.lastName != null)
                        {
                            lastName = person_item.lastName;
                        }
                        if (person_item.LoggedDesignation != null)
                        {
                            LoggedDesignation = person_item.LoggedDesignation;
                        }
                        if (person_item.LoggedInType != null)
                        {
                            LoggedInType = person_item.LoggedInType;
                        }
                        if (person_item.Gender != null)
                        {
                            Gender = person_item.Gender;
                        }
                        if (person_item.login_emailID != null)
                        {
                            login_emailID = person_item.login_emailID;
                        }
                        if (person_item.profilePhoto != null)
                        {
                            profilePhoto = person_item.profilePhoto;
                        }

                    }

                }

                foreach (ContactInfo contactInfo_item in contactInfos)
                {
                    if (contactInfo_item != null)
                    {
                        if (contactInfo_item.Country != null)
                        {
                            Country = contactInfo_item.Country;
                        }
                        if (contactInfo_item.State != null)
                        {
                            State = contactInfo_item.State;
                        }
                        if (contactInfo_item.City != null)
                        {
                            City = contactInfo_item.City;
                        }
                        if (contactInfo_item.PinCode != null)
                        {
                            PinCode = contactInfo_item.PinCode;
                        }
                        if (contactInfo_item.CurrentAddr != null)
                        {
                            CurrentAddr = contactInfo_item.CurrentAddr;
                        }
                        if (contactInfo_item.Phone_Code != null)
                        {
                            Phone_Code = contactInfo_item.Phone_Code;
                        }
                        if (contactInfo_item.Phone_Number != null)
                        {
                            Phone_Number = contactInfo_item.Phone_Number;
                        }

                    }
                }

                XETutGlobalX_DB.DBConnect dBConnect = new();
                switch (mode)
                {
                    case DBConnectMode.DBConnect_Mode.Simple:
                        dbconnectionString = dBConnect.GetConnectionString(DBConnect);
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

                    SqlCommand cmd = new("usp_UpdateProfileDetails", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@profilePhoto", profilePhoto);
                    cmd.Parameters.AddWithValue("@firstName", firstName);
                    cmd.Parameters.AddWithValue("@middleName", middleName);
                    cmd.Parameters.AddWithValue("@lastName", lastName);
                    cmd.Parameters.AddWithValue("@Gender", Gender);
                    cmd.Parameters.AddWithValue("@login_emailID", login_emailID);
                    cmd.Parameters.AddWithValue("@DOB", DOB);
                    cmd.Parameters.AddWithValue("@LoggedInType", LoggedInType);
                    cmd.Parameters.AddWithValue("@LoggedDesignation", LoggedDesignation);
                    cmd.Parameters.AddWithValue("@Country", Country);
                    cmd.Parameters.AddWithValue("@State", State);
                    cmd.Parameters.AddWithValue("@City", City);
                    cmd.Parameters.AddWithValue("@PinCode", PinCode);
                    cmd.Parameters.AddWithValue("@CurrentAddr", CurrentAddr);
                    cmd.Parameters.AddWithValue("@Phone_Number", Phone_Number);
                    cmd.Parameters.AddWithValue("@Phone_Code", Phone_Code);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dsResponse);
                }
                else
                {
                    dsResponse = null!;
                }

            }
            catch (Exception ex)
            {
                dsResponse = null!;
            }
            
            return dsResponse;
        }
    }
}
