using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace Database
{
    public static class Interface
    {
        private static string mConnectionString = ConfigurationManager.ConnectionStrings["nrpa_lorakon"].ConnectionString;
        private static SqlConnection mConnection = new SqlConnection(mConnectionString);
        private static SqlCommand mCommand = new SqlCommand("", mConnection);
        private static bool mIsOpen = false;

        public static string connectionString
        {
            get { return mConnectionString; }
        }

        public static SqlConnection connection
        {
            get { return mConnection; }
        }        

        public static void open()
        {
            if(!mIsOpen)
                mConnection.Open();
            mIsOpen = true;
        }

        public static void close()
        {
            if(mIsOpen)
                mConnection.Close();
            mIsOpen = false;
        }

        public static bool isOpen
        {
            get { return mIsOpen; }
        }

        public static SqlCommand command
        {
            get { return mCommand; }
            set { mCommand = value; }
        }

        public static int getNextSerialnumber()
        {
            mCommand.CommandText = "DECLARE @i AS INT EXEC csp_GetSerialnumber @i OUTPUT SELECT @i";
            mCommand.CommandType = CommandType.Text;
            return (int)mCommand.ExecuteScalar();
        }                                                                                                                                                
    }    

    public class Base
    {
        protected Guid mID;

        protected Base() { mID = Guid.Empty; }

        public Guid ID { get { return mID; } set { mID = value; } }
    }

    public class Identifiers : Base
    {        
        protected string mName;

        public Identifiers()
        {
            mName = mID.ToString();
        }

        public Identifiers(Guid ID, string name)
        {
            mID = ID;
            mName = name;
        }

        public string Name { get { return mName; } set { mName = value; } }

        public void setValues(Guid ID, string name)
        {
            mID = ID;
            mName = name;
        }
    }

    public class Device : Base
    {        
        protected Guid mAccountID;
        protected Guid mDeviceCategoryID;
        protected Guid mDeviceTypeID;
        protected string mSerialNumber;
        protected string mStatus;
        protected string mOwnership;
        protected string mComment;
        protected DateTime mReceivedNew;

        public Device() {}        

        public Device(Guid accountID, Guid categoryID, Guid typeID, string serialnumber, string status, string ownership, string comment, DateTime receivedNew) 
        {            
            mAccountID = accountID;
            mDeviceCategoryID = categoryID;
            mDeviceTypeID = typeID;
            mSerialNumber = serialnumber;
            mStatus = status;
            mOwnership = ownership;
            mComment = comment;
            mReceivedNew = receivedNew;
        }        
    
        public Guid AccountID { get { return mAccountID; } set { mAccountID = value; } }
        public Guid DeviceCategoryID { get { return mDeviceCategoryID; } set { mDeviceCategoryID = value; } }
        public Guid DeviceTypeID { get { return mDeviceTypeID; } set { mDeviceTypeID = value; } }
        public string SerialNumber { get { return mSerialNumber; } set { mSerialNumber = value; } }
        public string Status { get { return mStatus; } set { mStatus = value; } }
        public string Ownership { get { return mOwnership; } set { mOwnership = value; } }
        public string Comment { get { return mComment; } set { mComment = value; } }
        public DateTime ReceivedNew { get { return mReceivedNew; } set { mReceivedNew = value; } }        

        public bool insert_with_ID(Guid ID)
        {
            if (ID == Guid.Empty)
                throw new Exception("Device insert_with_ID: Invalid identifier");

            mID = ID;
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_insert_on_device_with_ID";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@ID", mID);
            Interface.command.Parameters.AddWithValue("@accountID", mAccountID);
            Interface.command.Parameters.AddWithValue("@categoryID", mDeviceCategoryID);
            Interface.command.Parameters.AddWithValue("@typeID", mDeviceTypeID);
            Interface.command.Parameters.AddWithValue("@serialnumber", mSerialNumber);
            Interface.command.Parameters.AddWithValue("@status", mStatus);
            Interface.command.Parameters.AddWithValue("@ownership", mOwnership);
            Interface.command.Parameters.AddWithValue("@comment", mComment);
            Interface.command.Parameters.AddWithValue("@receivedNew", mReceivedNew);

            return Interface.command.ExecuteNonQuery() > 0;
        }

        public bool update_by_ID()
        {
            if (mID == Guid.Empty)
                throw new Exception("Device update_by_ID: Invalid identifier");                

            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_update_all_on_device_where_ID";
            Interface.command.CommandType = CommandType.StoredProcedure;            
            Interface.command.Parameters.AddWithValue("@accountID", mAccountID);
            Interface.command.Parameters.AddWithValue("@categoryID", mDeviceCategoryID);
            Interface.command.Parameters.AddWithValue("@typeID", mDeviceTypeID);
            Interface.command.Parameters.AddWithValue("@serialnumber", mSerialNumber);
            Interface.command.Parameters.AddWithValue("@status", mStatus);
            Interface.command.Parameters.AddWithValue("@ownership", mOwnership);
            Interface.command.Parameters.AddWithValue("@comment", mComment);
            Interface.command.Parameters.AddWithValue("@receivedNew", mReceivedNew);
            Interface.command.Parameters.AddWithValue("@ID", mID);

            return Interface.command.ExecuteNonQuery() > 0;
        }

        public bool delete_by_ID()
        {
            if (mID == Guid.Empty)
                throw new Exception("Device delete_by_ID: Invalid identifier");                

            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_delete_on_device_where_ID";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@ID", mID);

            return Interface.command.ExecuteNonQuery() > 0;
        }

        public bool select_all_where_ID(Guid ID)
        {
            if (ID == Guid.Empty)
                throw new Exception("Device select_all_where_ID: Invalid identifier");

            mID = ID;
            bool returnValue = false;            
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_select_all_on_device_where_ID";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@ID", mID);

            SqlDataReader reader = Interface.command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                mID = reader.GetGuid(0);
                mAccountID = reader.GetGuid(1);
                mDeviceCategoryID = reader.GetGuid(2);
                mDeviceTypeID = reader.GetGuid(3);
                mSerialNumber = reader.GetString(4);
                mStatus = reader.GetString(5);
                mOwnership = reader.GetString(6);
                mComment = reader.GetString(7);
                mReceivedNew = reader.GetDateTime(8);
                returnValue = true;
            }
            reader.Close();
            return returnValue;
        }

        public static bool select_all_where_categoryID(Guid categoryID, string orderKey, ref List<Device> deviceList)
        {
            if(categoryID == Guid.Empty)
                throw new Exception("Device select_all_where_categoryID_orderkey: Invalid category identifier");                

            bool returnValue = false;
            SqlDataReader reader = null;

            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_select_all_on_device_where_categoryID_orderkey";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@categoryID", categoryID);
            Interface.command.Parameters.AddWithValue("@orderKey", orderKey);

            deviceList.Clear();
            reader = Interface.command.ExecuteReader();
            while (reader.Read())
            {
                Device device = new Device(                    
                    reader.GetGuid(1),
                    reader.GetGuid(2),
                    reader.GetGuid(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetString(6),
                    reader.GetString(7),
                    reader.GetDateTime(8));
                device.ID = reader.GetGuid(0);

                deviceList.Add(device);
                returnValue = true;
            }
            reader.Close();
            return returnValue;
        }

        public static bool select_all_where_typeID(Guid typeID, string orderKey, ref List<Device> deviceList)
        {
            if (typeID == Guid.Empty)
                throw new Exception("Device select_all_where_typeID_orderkey: Invalid type identifier");                

            bool returnValue = false;
            SqlDataReader reader = null;

            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_select_all_on_device_where_typeID_orderkey";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@typeID", typeID);
            Interface.command.Parameters.AddWithValue("@orderKey", orderKey);

            deviceList.Clear();
            reader = Interface.command.ExecuteReader();
            while (reader.Read())
            {
                Device device = new Device(                    
                    reader.GetGuid(1),
                    reader.GetGuid(2),
                    reader.GetGuid(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetString(6),
                    reader.GetString(7),
                    reader.GetDateTime(8));
                device.ID = reader.GetGuid(0);

                deviceList.Add(device);
                returnValue = true;
            }
            reader.Close();
            return returnValue;
        }

        public static bool select_identifiers_where_accountID_categoryID_status(Guid accountID, Guid categoryID, string status, ref List<Identifiers> idList)
        {
            if (accountID == Guid.Empty || categoryID == Guid.Empty)
                throw new Exception("Device select_identifiers_where_accountID_categoryID_status: Invalid identifier parameter");

            if (String.IsNullOrEmpty(status))
                throw new Exception("Device select_identifiers_where_accountID_categoryID_status: Invalid status parameter");

            bool returnValue = false;
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_select_identifiers_on_device_where_accountID_categoryID_status";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@accountID", accountID);
            Interface.command.Parameters.AddWithValue("@categoryID", categoryID);
            Interface.command.Parameters.AddWithValue("@status", status);

            idList.Clear();            
            SqlDataReader reader = Interface.command.ExecuteReader();
            while (reader.Read())
            {                
                idList.Add(new Identifiers(reader.GetGuid(0), reader.GetString(1)));
                returnValue = true;
            }
            reader.Close();
            return returnValue;
        }        

        public static bool select_all_where_accountID_categoryID_status(Guid accountID, Guid categoryID, string status, ref List<Device> deviceList)
        {
            if (accountID == Guid.Empty || categoryID == Guid.Empty)
                throw new Exception("Device select_all_where_accountID_categoryID_status: Invalid identifier parameter");                

            if(String.IsNullOrEmpty(status))
                throw new Exception("Device select_all_where_accountID_categoryID_status: Invalid status parameter");                

            bool returnValue = false;            
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_select_all_on_device_where_accountID_categoryID_status";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@accountID", accountID);
            Interface.command.Parameters.AddWithValue("@categoryID", categoryID);
            Interface.command.Parameters.AddWithValue("@status", status);
            deviceList.Clear();
            SqlDataReader reader = Interface.command.ExecuteReader();
            while (reader.Read())
            {
                Device device = new Device(                    
                    reader.GetGuid(1),
                    reader.GetGuid(2),
                    reader.GetGuid(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetString(6),
                    reader.GetString(7),
                    reader.GetDateTime(8));
                device.ID = reader.GetGuid(0);

                deviceList.Add(device);
                returnValue = true;
            }
            reader.Close();
            return returnValue;
        }                
        
        public DataSet select_all_comments()
        {
            if (ID == Guid.Empty)
                throw new Exception("Device get_all_comments: Invalid device identifier");

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT dateCreated, vchComment, ID FROM UnitComments WHERE UnitID = @unitID ORDER BY dateCreated DESC", Interface.connection);
            adapter.SelectCommand.Parameters.Add("@unitID", SqlDbType.UniqueIdentifier).Value = ID;
            adapter.Fill(ds);
            return ds;
        }

        public static bool insert_comment(Guid deviceID, string comment)
        {
            if (deviceID == Guid.Empty)
                throw new Exception("Device insert_comment: Invalid device identifier");            

            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "INSERT INTO UnitComments (UnitID, dateCreated, vchComment, ID) VALUES(@unitID, @created, @comment, @id)";
            Interface.command.CommandType = CommandType.Text;
            Interface.command.Parameters.AddWithValue("@unitID", deviceID);
            Interface.command.Parameters.AddWithValue("@created", DateTime.Now);
            Interface.command.Parameters.AddWithValue("@comment", comment);
            Interface.command.Parameters.AddWithValue("@id", Guid.NewGuid());

            return Interface.command.ExecuteNonQuery() > 0;            
        }

        public static bool delete_comment(Guid id)
        {
            if (id == Guid.Empty)
                throw new Exception("Device delete_comment: Invalid identifier");

            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "DELETE FROM UnitComments where ID = @id";
            Interface.command.CommandType = CommandType.Text;
            Interface.command.Parameters.AddWithValue("@id", id);            

            return Interface.command.ExecuteNonQuery() > 0;
        }
    }

    public class DeviceCategory : Base
    {        
        protected string mName;
        protected bool mSticky;

        public DeviceCategory() { }

        public DeviceCategory(string name, bool sticky)
        {
            mName = name;
            mSticky = sticky;
        }        
        
        public string Name { get { return mName; } set { mName = value; } }
        public bool Sticky { get { return mSticky; } set { mSticky = value; } }        

        public bool insert_with_ID_name(Guid ID, string name)
        {
            if (ID == Guid.Empty)
                throw new Exception("DeviceCategory insert_with_ID_name: Invalid identifier");

            if (String.IsNullOrEmpty(name))
                throw new Exception("DeviceCategory insert_with_ID_name: Invalid name");

            mID = ID;
            mName = name;
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_insert_on_devicecategory_with_ID_name";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@ID", mID);
            Interface.command.Parameters.AddWithValue("@name", mName);
            Interface.command.Parameters.AddWithValue("@sticky", mSticky);

            return Interface.command.ExecuteNonQuery() > 0;
        }

        public bool update_all_by_ID()
        {            
            if (mID == Guid.Empty)
                throw new Exception("DeviceCategory update_all_by_ID: Invalid identifier");

            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_update_all_on_devicecategory_where_ID";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@ID", mID);
            Interface.command.Parameters.AddWithValue("@name", mName);
            Interface.command.Parameters.AddWithValue("@sticky", mSticky);            

            return Interface.command.ExecuteNonQuery() > 0;
        }

        public bool delete_by_ID()
        {
            if (mID == Guid.Empty)
                throw new Exception("DeviceCategory delete_by_ID: Invalid identifier");

            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_delete_on_devicecategory_where_ID";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@ID", mID);

            return Interface.command.ExecuteNonQuery() > 0;
        }

        public bool select_all_where_ID(Guid ID)
        {
            if (ID == Guid.Empty)
                throw new Exception("DeviceCategory select_all_where_ID: Invalid identifier");

            mID = ID;
            bool returnValue = false;            
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_select_all_on_devicecategory_where_ID";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@ID", mID);

            SqlDataReader reader = Interface.command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                mID = reader.GetGuid(0);
                mName = reader.GetString(1);
                mSticky = reader.GetBoolean(2);
                returnValue = true;
            }
            reader.Close();
            return returnValue;
        }

        public bool select_all_where_name(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new Exception("DeviceCategory select_where_name: Invalid name");

            mName = name;
            bool returnValue = false;            
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_select_all_on_devicecategory_where_name";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@name", mName);

            SqlDataReader reader = Interface.command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                mID = reader.GetGuid(0);
                mName = reader.GetString(1);
                mSticky = reader.GetBoolean(2);
                returnValue = true;
            }
            reader.Close();
            return returnValue;
        }
    }

    public class DeviceCategoryType : Base
    {        
        protected Guid mDeviceCategoryID;
        protected string mName;
        protected bool mSticky;

        public DeviceCategoryType() {}

        public DeviceCategoryType(string name, bool sticky)
        {            
            mName = name;
            mSticky = sticky;
        }        
        
        public Guid DeviceCategoryID { get { return mDeviceCategoryID; } set { mDeviceCategoryID = value; } }
        public string Name { get { return mName; } set { mName = value; } }
        public bool Sticky { get { return mSticky; } set { mSticky = value; } }        

        public bool insert_with_ID_categoryID_name(Guid ID, Guid categoryID, string name)
        {
            if (ID == Guid.Empty)
                throw new Exception("DeviceCategoryType insert_with_ID_categoryID_name: Invalid identifier");

            if (categoryID == Guid.Empty)
                throw new Exception("DeviceCategoryType insert_with_ID_categoryID_name: Invalid category identifier");

            if (String.IsNullOrEmpty(name))
                throw new Exception("DeviceCategoryType insert_with_ID_categoryID_name: Invalid name");

            if (ID == Guid.Empty)
                throw new Exception("DeviceCategoryType insert_with_ID_categoryID_name: Invalid identifier");

            mID = ID;
            mDeviceCategoryID = categoryID;
            mName = name;
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_insert_on_devicetype_with_ID_categoryID_name";
            Interface.command.CommandType = CommandType.StoredProcedure;            
            Interface.command.Parameters.AddWithValue("@ID", mID);
            Interface.command.Parameters.AddWithValue("@categoryID", mDeviceCategoryID);
            Interface.command.Parameters.AddWithValue("@name", mName);
            Interface.command.Parameters.AddWithValue("@sticky", mSticky);

            return Interface.command.ExecuteNonQuery() > 0;
        }

        public bool update_all_by_ID()
        {
            if (mID == Guid.Empty)
                throw new Exception("DeviceCategoryType update_all_by_ID: Invalid identifier");

            if (String.IsNullOrEmpty(mName))
                throw new Exception("DeviceCategoryType update_all_by_ID: Invalid name");

            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_update_all_on_devicetype_where_ID";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@ID", mID);
            Interface.command.Parameters.AddWithValue("@categoryID", mDeviceCategoryID);
            Interface.command.Parameters.AddWithValue("@name", mName);
            Interface.command.Parameters.AddWithValue("@sticky", mSticky);            

            return Interface.command.ExecuteNonQuery() > 0;
        }

        public bool delete_by_ID()
        {
            if (mID == Guid.Empty)
                throw new Exception("DeviceCategoryType delete_by_ID: Invalid identifier");

            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_delete_on_devicetype_where_ID";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@ID", mID);

            return Interface.command.ExecuteNonQuery() > 0;
        }

        public bool select_all_where_ID(Guid ID)
        {
            if (ID == Guid.Empty)
                throw new Exception("DeviceCategoryType select_all_where_ID: Invalid identifier");

            mID = ID;
            bool returnValue = false;            
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_select_all_on_devicetype_where_ID";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@ID", mID);

            SqlDataReader reader = Interface.command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                mID = reader.GetGuid(0);
                mDeviceCategoryID = reader.GetGuid(1);
                mName = reader.GetString(2);
                mSticky = reader.GetBoolean(3);
                returnValue = true;
            }
            reader.Close();
            return returnValue;
        }
    }

    public class Configuration : Base
    {    
        protected string mName;
        protected string mStart;
        protected string mNews;        
        protected string mSectionManager;
        protected string mRingtestAdminEmail;        

        public Configuration() {}

        public Configuration(string name, string start, string news, string sectionManager, string ringtestAdminEmail)
        {            
            mName = name;
            mStart = start;            
            mSectionManager = sectionManager;
            mRingtestAdminEmail = ringtestAdminEmail;        
        }        

        public string Name { get { return mName; } set { mName = value; } }
        public string Start { get { return mStart; } set { mStart = value; } }
        public string News { get { return mNews; } set { mNews = value; } }
        public string SectionManager { get { return mSectionManager; } set { mSectionManager = value; } }
        public string RingtestAdminEmail { get { return mRingtestAdminEmail; } set { mRingtestAdminEmail = value; } }        

        public bool insert_with_ID_name(Guid ID, string name)
        {
            if (ID == Guid.Empty)
                throw new Exception("Configuration insert_with_ID_name: Invalid identifier");

            if (String.IsNullOrEmpty(name))
                throw new Exception("Configuration insert_with_ID_name: Invalid name");

            mID = ID;
            mName = name;
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_insert_on_configuration_with_ID_name";
            Interface.command.CommandType = CommandType.StoredProcedure;            
            Interface.command.Parameters.AddWithValue("@ID", Guid.NewGuid());
            Interface.command.Parameters.AddWithValue("@name", mName);
            Interface.command.Parameters.AddWithValue("@start", (String.IsNullOrEmpty(mStart) ? "" : mStart));
            Interface.command.Parameters.AddWithValue("@news", (String.IsNullOrEmpty(mNews) ? "" : mNews));
            Interface.command.Parameters.AddWithValue("@sectionManager", (String.IsNullOrEmpty(mSectionManager) ? "" : mSectionManager));
            Interface.command.Parameters.AddWithValue("@ringtestAdminEmail", (String.IsNullOrEmpty(mRingtestAdminEmail) ? "" : mRingtestAdminEmail));        

            return Interface.command.ExecuteNonQuery() > 0;            
        }        

        public bool update_all_by_name()
        {
            if (String.IsNullOrEmpty(mName))
                throw new Exception("Configuration update_all_by_name: Invalid name");

            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_update_all_on_configuration_where_name";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@name", mName);
            Interface.command.Parameters.AddWithValue("@start", mStart);
            Interface.command.Parameters.AddWithValue("@news", mNews);
            Interface.command.Parameters.AddWithValue("@sectionManager", mSectionManager);
            Interface.command.Parameters.AddWithValue("@ringtestAdminEmail", mRingtestAdminEmail);            

            return Interface.command.ExecuteNonQuery() > 0;
        }

        public bool select_all_where_name(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new Exception("Configuration select_all_where_name: Invalid name");

            mName = name;
            bool returnValue = false;
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_select_all_on_configuration_where_name";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@name", mName);

            SqlDataReader reader = Interface.command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                ID = reader.GetGuid(0);
                mName = reader.GetString(1).Trim();
                mStart = reader.GetString(2).Trim();
                mNews = reader.GetString(3).Trim();                
                mSectionManager = reader.GetString(4).Trim();
                mRingtestAdminEmail = reader.GetString(5).Trim();                
                returnValue = true;
            }
            reader.Close();

            return returnValue;
        }
    }

    public class RingtestReport : Base
    {    
        protected Guid mDetectorID;        
        protected Guid mRingtestID;
        protected Guid mAccountID;
        protected Guid mContactID;
        protected Guid mRingtestBoxID;
        protected string mMCAType;
        protected float mBackground;
        protected int mIntegralBackground;
        protected int mCountingBackground;
        protected float mGeometryFactor;
        protected float mActivity;
        protected float mActivityRef;
        protected float mUncertainty;
        protected float mAvgIntegralSample;
        protected float mAvgLivetimeSample;
        protected DateTime mRefDate;
        protected DateTime mMeasureDate;
        protected float mError;
        protected bool mWantEvaluation;
        protected bool mEvaluated;
        protected bool mApproved;
        protected bool mAnswerByEmail;
        protected bool mAnswerSent;
        protected bool mIsInspector1000;
        protected string mComment;
        protected int mAcceptableLimit;
        protected float mCalculatedUncertainty;

        public RingtestReport() 
        {
            mDetectorID = Guid.Empty;
            mRingtestID = Guid.Empty;
            mAccountID = Guid.Empty;
            mContactID = Guid.Empty;
            mRingtestBoxID = Guid.Empty;
            mMCAType = "Serie10";

            mBackground = float.MinValue;
            mIntegralBackground = int.MinValue;
            mCountingBackground = int.MinValue;
            mGeometryFactor = float.MinValue;
            mActivity = float.MinValue;
            mActivityRef = float.MinValue;
            mUncertainty = float.MinValue;
            mAvgIntegralSample = float.MinValue;
            mAvgLivetimeSample = float.MinValue;   

            mRefDate = DateTime.Now;
            mMeasureDate = DateTime.Now;
            mError = float.MinValue;
            mWantEvaluation = false;
            mEvaluated = false;
            mApproved = false;
            mAnswerByEmail = false;
            mAnswerSent = false;
            mIsInspector1000 = false;
            mComment = "";
            mAcceptableLimit = 10;
            mCalculatedUncertainty = float.MinValue;
        }

        public RingtestReport(Guid detectorID, Guid ringtestID, Guid accountID, Guid contactID, Guid ringtestBoxID, string mcaType,
            float background, int integralBackground, int countingBackground, float geometryFactor, float activity, float activityRef, float uncertainty,
            float avgIntegralSample, float avgLivetimeSample, DateTime refDate, DateTime measureDate, float error, bool wantEvaluation,
            bool evaluated, bool approved, bool answerByEmail, bool answerSent, bool isInspector1000, string comment, int acceptableLimit, float calculatedUncertainty)
        {            
            mDetectorID = detectorID;
            mRingtestID = ringtestID;
            mAccountID = accountID;
            mContactID = contactID;
            mRingtestBoxID = ringtestBoxID;
            mMCAType = mcaType;
            mBackground = background;
            mIntegralBackground = integralBackground;
            mCountingBackground = countingBackground;
            mGeometryFactor = geometryFactor;
            mActivity = activity;
            mActivityRef = activityRef;
            mUncertainty = uncertainty;
            mAvgIntegralSample = avgIntegralSample;
            mAvgLivetimeSample = avgLivetimeSample;
            mRefDate = refDate;
            mMeasureDate = measureDate;
            mError = error;
            mWantEvaluation = wantEvaluation;
            mEvaluated = evaluated;
            mApproved = approved;
            mAnswerByEmail = answerByEmail;
            mAnswerSent = answerSent;
            mIsInspector1000 = isInspector1000;
            mComment = comment;
            mAcceptableLimit = acceptableLimit;
            mCalculatedUncertainty = calculatedUncertainty;
        }        
        
        public Guid DetectorID { get { return mDetectorID; } set { mDetectorID = value; } }        
        public Guid RingtestID { get { return mRingtestID; } set { mRingtestID = value; } }
        public Guid AccountID { get { return mAccountID; } set { mAccountID = value; } }
        public Guid ContactID { get { return mContactID; } set { mContactID = value; } }
        public Guid RingtestBoxID { get { return mRingtestBoxID; } set { mRingtestBoxID = value; } }
        public string MCAType { get { return mMCAType; } set { mMCAType = value; } }
        public float Background { get { return mBackground; } set { mBackground = value; } }
        public int IntegralBackground { get { return mIntegralBackground; } set { mIntegralBackground = value; } }
        public int CountingBackground { get { return mCountingBackground; } set { mCountingBackground = value; } }
        public float GeometryFactor { get { return mGeometryFactor; } set { mGeometryFactor = value; } }
        public float Activity { get { return mActivity; } set { mActivity = value; } }
        public float ActivityRef { get { return mActivityRef; } set { mActivityRef = value; } }
        public float Uncertainty { get { return mUncertainty; } set { mUncertainty = value; } }
        public float AvgIntegralSample { get { return mAvgIntegralSample; } set { mAvgIntegralSample = value; } }
        public float AvgLivetimeSample { get { return mAvgLivetimeSample; } set { mAvgLivetimeSample = value; } }
        public DateTime RefDate { get { return mRefDate; } set { mRefDate = value; } }
        public DateTime MeasureDate { get { return mMeasureDate; } set { mMeasureDate = value; } }
        public float Error { get { return mError; } set { mError = value; } }
        public bool WantEvaluation { get { return mWantEvaluation; } set { mWantEvaluation = value; } }
        public bool Evaluated { get { return mEvaluated; } set { mEvaluated = value; } }
        public bool Approved { get { return mApproved; } set { mApproved = value; } }
        public bool AnswerByEmail { get { return mAnswerByEmail; } set { mAnswerByEmail = value; } }
        public bool AnswerSent { get { return mAnswerSent; } set { mAnswerSent = value; } }
        public bool IsInspector1000 { get { return mIsInspector1000; } set { mIsInspector1000 = value; } }
        public string Comment { get { return mComment; } set { mComment = value; } }
        public int AcceptableLimit { get { return mAcceptableLimit; } set { mAcceptableLimit = value; } }
        public float CalculatedUncertainty { get { return mCalculatedUncertainty; } set { mCalculatedUncertainty = value; } }

        public bool insert_with_ID(Guid ID)
        {
            if (ID == Guid.Empty)
                throw new Exception("RingtestReport insert_with_ID: Invalid identifier");

            mID = ID;
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_insert_on_ringtestreport_with_ID";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@ID", mID);
            Interface.command.Parameters.AddWithValue("@detectorID", mDetectorID);
            Interface.command.Parameters.AddWithValue("@ringtestID", mRingtestID);
            Interface.command.Parameters.AddWithValue("@accountID", mAccountID);
            Interface.command.Parameters.AddWithValue("@contactID", mContactID);
            Interface.command.Parameters.AddWithValue("@ringtestBoxID", mRingtestBoxID);
            Interface.command.Parameters.AddWithValue("@mcaType", mMCAType);
            Interface.command.Parameters.AddWithValue("@background", mBackground);
            Interface.command.Parameters.AddWithValue("@integralBackground", mIntegralBackground);
            Interface.command.Parameters.AddWithValue("@countingBackground", mCountingBackground);
            Interface.command.Parameters.AddWithValue("@geometryFactor", mGeometryFactor);
            Interface.command.Parameters.AddWithValue("@activity", mActivity);
            Interface.command.Parameters.AddWithValue("@activityRef", mActivityRef);
            Interface.command.Parameters.AddWithValue("@uncertainty", mUncertainty);
            Interface.command.Parameters.AddWithValue("@avgIntegralSample", mAvgIntegralSample);
            Interface.command.Parameters.AddWithValue("@avgLivetimeSample", mAvgLivetimeSample);
            Interface.command.Parameters.AddWithValue("@refDate", mRefDate);
            Interface.command.Parameters.AddWithValue("@measureDate", mMeasureDate);
            Interface.command.Parameters.AddWithValue("@error", mError);
            Interface.command.Parameters.AddWithValue("@wantEvaluation", mWantEvaluation);
            Interface.command.Parameters.AddWithValue("@evaluated", mEvaluated);
            Interface.command.Parameters.AddWithValue("@approved", mApproved);
            Interface.command.Parameters.AddWithValue("@answerByEmail", mAnswerByEmail);
            Interface.command.Parameters.AddWithValue("@answerSent", mAnswerSent);
            Interface.command.Parameters.AddWithValue("@isInspector1000", mIsInspector1000);
            Interface.command.Parameters.AddWithValue("@comment", mComment);
            Interface.command.Parameters.AddWithValue("@acceptableLimit", mAcceptableLimit);
            Interface.command.Parameters.AddWithValue("@calculatedUncertainty", mCalculatedUncertainty);

            return (Interface.command.ExecuteNonQuery() > 0);            
        }

        public bool update_all_by_ID()
        {
            if (mID == Guid.Empty)
                throw new Exception("RingtestReport update_all_by_ID: Invalid identifier");
            
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_update_all_on_ringtestreport_where_ID";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@ID", mID);
            Interface.command.Parameters.AddWithValue("@detectorID", mDetectorID);
            Interface.command.Parameters.AddWithValue("@ringtestID", mRingtestID);
            Interface.command.Parameters.AddWithValue("@accountID", mAccountID);
            Interface.command.Parameters.AddWithValue("@contactID", mContactID);
            Interface.command.Parameters.AddWithValue("@ringtestBoxID", mRingtestBoxID);
            Interface.command.Parameters.AddWithValue("@mcaType", mMCAType);
            Interface.command.Parameters.AddWithValue("@background", mBackground);
            Interface.command.Parameters.AddWithValue("@integralBackground", mIntegralBackground);
            Interface.command.Parameters.AddWithValue("@countingBackground", mCountingBackground);
            Interface.command.Parameters.AddWithValue("@geometryFactor", mGeometryFactor);
            Interface.command.Parameters.AddWithValue("@activity", mActivity);
            Interface.command.Parameters.AddWithValue("@activityRef", mActivityRef);
            Interface.command.Parameters.AddWithValue("@uncertainty", mUncertainty);
            Interface.command.Parameters.AddWithValue("@avgIntegralSample", mAvgIntegralSample);
            Interface.command.Parameters.AddWithValue("@avgLivetimeSample", mAvgLivetimeSample);
            Interface.command.Parameters.AddWithValue("@refDate", mRefDate);
            Interface.command.Parameters.AddWithValue("@measureDate", mMeasureDate);
            Interface.command.Parameters.AddWithValue("@error", mError);
            Interface.command.Parameters.AddWithValue("@wantEvaluation", mWantEvaluation);
            Interface.command.Parameters.AddWithValue("@evaluated", mEvaluated);
            Interface.command.Parameters.AddWithValue("@approved", mApproved);
            Interface.command.Parameters.AddWithValue("@answerByEmail", mAnswerByEmail);
            Interface.command.Parameters.AddWithValue("@answerSent", mAnswerSent);
            Interface.command.Parameters.AddWithValue("@isInspector1000", mIsInspector1000);
            Interface.command.Parameters.AddWithValue("@comment", mComment);
            Interface.command.Parameters.AddWithValue("@acceptableLimit", mAcceptableLimit);
            Interface.command.Parameters.AddWithValue("@calculatedUncertainty", mCalculatedUncertainty);

            return (Interface.command.ExecuteNonQuery() > 0);
        }

        public bool update_Bool_by_ID(string columnName, bool state)
        {
            if (mID == Guid.Empty)
                throw new Exception("RingtestReport update_Bool_by_ID: Invalid identifier");

            Interface.command.Parameters.Clear();            
            Interface.command.CommandText = "IF EXISTS(SELECT ID FROM RingtestReport WHERE ID = @ID) UPDATE RingtestReport SET " + columnName + " = @state WHERE ID = @ID";
            Interface.command.CommandType = CommandType.Text;
            Interface.command.Parameters.AddWithValue("@ID", mID);            
            Interface.command.Parameters.AddWithValue("@state", state);            

            return Interface.command.ExecuteNonQuery() > 0;
        }

        public bool select_all_where_ID(Guid ID)
        {
            if (ID == Guid.Empty)
                throw new Exception("RingtestReport select_all_where_ID: Invalid identifier");

            mID = ID;
            bool returnValue = false;            
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_select_all_on_ringtestreport_where_ID";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@ID", mID);

            SqlDataReader reader = Interface.command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                mID = reader.GetGuid(0);
                mDetectorID = reader.GetGuid(1);
                mRingtestID = reader.GetGuid(2);
                mAccountID = reader.GetGuid(3);
                mContactID = reader.GetGuid(4);
                mRingtestBoxID = reader.GetGuid(5);
                mMCAType = reader.GetString(6);                
                mBackground = Convert.ToSingle(reader.GetValue(7));
                mIntegralBackground = reader.GetInt32(8);
                mCountingBackground = reader.GetInt32(9);
                mGeometryFactor = Convert.ToSingle(reader.GetValue(10));
                mActivity = Convert.ToSingle(reader.GetValue(11));
                mActivityRef = Convert.ToSingle(reader.GetValue(12));
                mUncertainty = Convert.ToSingle(reader.GetValue(13));
                mAvgIntegralSample = Convert.ToSingle(reader.GetValue(14));
                mAvgLivetimeSample = Convert.ToSingle(reader.GetValue(15));
                mRefDate = reader.GetDateTime(16);
                mMeasureDate = reader.GetDateTime(17);
                mError = Convert.ToSingle(reader.GetValue(18));
                mWantEvaluation = reader.GetBoolean(19);
                mEvaluated = reader.GetBoolean(20);
                mApproved = reader.GetBoolean(21);
                mAnswerByEmail = reader.GetBoolean(22);
                mAnswerSent = reader.GetBoolean(23);
                mIsInspector1000 = reader.GetBoolean(24);
                mComment = reader.GetString(25);
                mAcceptableLimit = reader.GetInt32(26);
                mCalculatedUncertainty = Convert.ToSingle(reader.GetValue(27));
                returnValue = true;
            }
            reader.Close();
            return returnValue;
        }

        public bool select_all_where_accountID_detectorID_ringtestID(Guid accountID, Guid detectorID, Guid ringtestID)
        {
            if (accountID == Guid.Empty || detectorID == Guid.Empty || ringtestID == Guid.Empty)
                throw new Exception("RingtestReport select_all_where_accountID_detectorID_ringtestID: Invalid identifier");

            mAccountID = accountID;
            mDetectorID = detectorID;
            mRingtestID = ringtestID;
            bool returnValue = false;
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_select_all_on_ringtestreport_where_accountID_detectorID_ringtestID";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@accountID", mAccountID);
            Interface.command.Parameters.AddWithValue("@detectorID", mDetectorID);
            Interface.command.Parameters.AddWithValue("@ringtestID", mRingtestID);

            SqlDataReader reader = Interface.command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                mID = reader.GetGuid(0);
                mDetectorID = reader.GetGuid(1);
                mRingtestID = reader.GetGuid(2);
                mAccountID = reader.GetGuid(3);
                mContactID = reader.GetGuid(4);
                mRingtestBoxID = reader.GetGuid(5);
                mMCAType = reader.GetString(6);
                mBackground = Convert.ToSingle(reader.GetValue(7));
                mIntegralBackground = reader.GetInt32(8);
                mCountingBackground = reader.GetInt32(9);
                mGeometryFactor = Convert.ToSingle(reader.GetValue(10));
                mActivity = Convert.ToSingle(reader.GetValue(11));
                mActivityRef = Convert.ToSingle(reader.GetValue(12));
                mUncertainty = Convert.ToSingle(reader.GetValue(13));
                mAvgIntegralSample = Convert.ToSingle(reader.GetValue(14));
                mAvgLivetimeSample = Convert.ToSingle(reader.GetValue(15));
                mRefDate = reader.GetDateTime(16);
                mMeasureDate = reader.GetDateTime(17);
                mError = Convert.ToSingle(reader.GetValue(18));
                mWantEvaluation = reader.GetBoolean(19);
                mEvaluated = reader.GetBoolean(20);
                mApproved = reader.GetBoolean(21);
                mAnswerByEmail = reader.GetBoolean(22);
                mAnswerSent = reader.GetBoolean(23);
                mIsInspector1000 = reader.GetBoolean(24);
                mComment = reader.GetString(25);
                mAcceptableLimit = reader.GetInt32(26);
                mCalculatedUncertainty = Convert.ToSingle(reader.GetValue(27));
                returnValue = true;
            }
            reader.Close();

            return returnValue;
        }

        public bool select_all_where_ringtestID_AccountID_approved(Guid ringtestID, Guid accountID, bool approved)
        {
            if (ringtestID == Guid.Empty || accountID == Guid.Empty)
                throw new Exception("RingtestReport select_all_where_ringtestID_AccountID_approved: Invalid identifier");
            
            mRingtestID = ringtestID;
            bool returnValue = false;
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "SELECT * FROM RingtestReport WHERE RingtestID = @ringtestID AND AccountID = @accountID AND bitApproved = @approved";
            Interface.command.CommandType = CommandType.Text;            
            Interface.command.Parameters.AddWithValue("@ringtestID", mRingtestID);
            Interface.command.Parameters.AddWithValue("@accountID", accountID);
            Interface.command.Parameters.AddWithValue("@approved", approved);

            SqlDataReader reader = Interface.command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                mID = reader.GetGuid(0);
                mDetectorID = reader.GetGuid(1);
                mRingtestID = reader.GetGuid(2);
                mAccountID = reader.GetGuid(3);
                mContactID = reader.GetGuid(4);
                mRingtestBoxID = reader.GetGuid(5);
                mMCAType = reader.GetString(6);
                mBackground = Convert.ToSingle(reader.GetValue(7));
                mIntegralBackground = reader.GetInt32(8);
                mCountingBackground = reader.GetInt32(9);
                mGeometryFactor = Convert.ToSingle(reader.GetValue(10));
                mActivity = Convert.ToSingle(reader.GetValue(11));
                mActivityRef = Convert.ToSingle(reader.GetValue(12));
                mUncertainty = Convert.ToSingle(reader.GetValue(13));
                mAvgIntegralSample = Convert.ToSingle(reader.GetValue(14));
                mAvgLivetimeSample = Convert.ToSingle(reader.GetValue(15));
                mRefDate = reader.GetDateTime(16);
                mMeasureDate = reader.GetDateTime(17);
                mError = Convert.ToSingle(reader.GetValue(18));
                mWantEvaluation = reader.GetBoolean(19);
                mEvaluated = reader.GetBoolean(20);
                mApproved = reader.GetBoolean(21);
                mAnswerByEmail = reader.GetBoolean(22);
                mAnswerSent = reader.GetBoolean(23);
                mIsInspector1000 = reader.GetBoolean(24);
                mComment = reader.GetString(25);
                mAcceptableLimit = reader.GetInt32(26);
                mCalculatedUncertainty = Convert.ToSingle(reader.GetValue(27));
                returnValue = true;
            }
            reader.Close();

            return returnValue;
        }

        public bool select_all_where_ringtestID_AccountID_DetectorID_approved(Guid ringtestID, Guid accountID, Guid detectorID, bool approved)
        {
            if (ringtestID == Guid.Empty || accountID == Guid.Empty || detectorID == Guid.Empty)
                throw new Exception("RingtestReport select_all_where_ringtestID_AccountID_DetectorID_approved: Invalid identifier");

            mRingtestID = ringtestID;
            bool returnValue = false;
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "SELECT * FROM RingtestReport WHERE RingtestID = @ringtestID AND AccountID = @accountID AND DetectorID = @detectorID AND bitApproved = @approved";
            Interface.command.CommandType = CommandType.Text;
            Interface.command.Parameters.AddWithValue("@ringtestID", mRingtestID);
            Interface.command.Parameters.AddWithValue("@accountID", accountID);
            Interface.command.Parameters.AddWithValue("@detectorID", detectorID);
            Interface.command.Parameters.AddWithValue("@approved", approved);

            SqlDataReader reader = Interface.command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                mID = reader.GetGuid(0);
                mDetectorID = reader.GetGuid(1);
                mRingtestID = reader.GetGuid(2);
                mAccountID = reader.GetGuid(3);
                mContactID = reader.GetGuid(4);
                mRingtestBoxID = reader.GetGuid(5);
                mMCAType = reader.GetString(6);
                mBackground = Convert.ToSingle(reader.GetValue(7));
                mIntegralBackground = reader.GetInt32(8);
                mCountingBackground = reader.GetInt32(9);
                mGeometryFactor = Convert.ToSingle(reader.GetValue(10));
                mActivity = Convert.ToSingle(reader.GetValue(11));
                mActivityRef = Convert.ToSingle(reader.GetValue(12));
                mUncertainty = Convert.ToSingle(reader.GetValue(13));
                mAvgIntegralSample = Convert.ToSingle(reader.GetValue(14));
                mAvgLivetimeSample = Convert.ToSingle(reader.GetValue(15));
                mRefDate = reader.GetDateTime(16);
                mMeasureDate = reader.GetDateTime(17);
                mError = Convert.ToSingle(reader.GetValue(18));
                mWantEvaluation = reader.GetBoolean(19);
                mEvaluated = reader.GetBoolean(20);
                mApproved = reader.GetBoolean(21);
                mAnswerByEmail = reader.GetBoolean(22);
                mAnswerSent = reader.GetBoolean(23);
                mIsInspector1000 = reader.GetBoolean(24);
                mComment = reader.GetString(25);
                mAcceptableLimit = reader.GetInt32(26);
                mCalculatedUncertainty = Convert.ToSingle(reader.GetValue(27));
                returnValue = true;
            }
            reader.Close();

            return returnValue;
        }

        public bool delete_by_ID()
        {
            if (mID == Guid.Empty)
                throw new Exception("RingtestReport delete_by_ID: Invalid identifier");

            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "DELETE FROM RingtestReport WHERE ID = @ID";
            Interface.command.CommandType = CommandType.Text;
            Interface.command.Parameters.AddWithValue("@ID", mID);

            return Interface.command.ExecuteNonQuery() > 0;
        }

        public bool add_comment(string contactName, string comment)
        {
            if (mID == Guid.Empty)
                throw new Exception("RingtestReport add_comment: Invalid identifier");

            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "INSERT INTO RingtestReportComments (RingtestReportID, dateCreated, vchContactName, textComment, ID) VALUES(@reportId, @created, @contactName, @comment, @id)";
            Interface.command.CommandType = CommandType.Text;
            Interface.command.Parameters.AddWithValue("@reportId", mID);
            Interface.command.Parameters.AddWithValue("@created", DateTime.Now);
            Interface.command.Parameters.AddWithValue("@contactName", contactName);
            Interface.command.Parameters.AddWithValue("@comment", comment);
            Interface.command.Parameters.AddWithValue("@id", Guid.NewGuid());

            return Interface.command.ExecuteNonQuery() > 0;
        }

        public static DataSet get_comments(Guid id)
        {
            if (id == Guid.Empty)
                throw new Exception("RingtestReport get_comments: Invalid identifier");            

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT dateCreated, vchContactName, textComment, ID FROM RingtestReportComments WHERE RingtestReportID = @reportId ORDER BY dateCreated DESC", Database.Interface.connection);
            adapter.SelectCommand.Parameters.AddWithValue("@reportId", id);
            adapter.Fill(ds);
            return ds;
        }

        public static bool delete_where_AccountID_and_Year(Guid accountID, Guid ringtestID)
        {
            if (accountID == Guid.Empty)
                throw new Exception("RingtestReport delete_where_AccountID_and_Year: Invalid identifier");

            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "DELETE FROM RingtestReport WHERE AccountID = @accountID AND RingtestID = @ringtestID";
            Interface.command.CommandType = CommandType.Text;
            Interface.command.Parameters.AddWithValue("@accountID", accountID);
            Interface.command.Parameters.AddWithValue("@ringtestID", ringtestID);

            return Interface.command.ExecuteNonQuery() > 0;
        }

        public static void select_RingtestBoxID_where_AccountID(Guid accountID, ref List<Guid> idList)
        {
            if (accountID == Guid.Empty)
                throw new Exception("RingtestReport select_RingtestBoxID_where_AccountID: Invalid identifier");

            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "SELECT RingtestID, RingtestBoxID FROM RingtestReport WHERE AccountID = @accountID ORDER BY dateMeasureDate DESC";
            Interface.command.CommandType = CommandType.Text;
            Interface.command.Parameters.AddWithValue("@accountID", accountID);

            idList.Clear();
            SqlDataReader reader = Interface.command.ExecuteReader();
            Guid lastID = Guid.Empty;
            while (reader.Read())
            {
                if(reader.GetGuid(0) != lastID)
                    idList.Add(reader.GetGuid(1));
                lastID = reader.GetGuid(0);
            }
            reader.Close();            
        }

        public static DataSet select_Year_Error_CalculatedUncertainty_RingtestBoxID_MCAType_ActivityRef_where_AccountID(Guid accountID)
        {
            if (accountID == Guid.Empty)
                throw new Exception("RingtestReport select_Year_Error_where_AccountID: Invalid identifier");            

            DataSet dataSet = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT Ringtest.intYear, RingtestReport.realError, RingtestReport.realCalculatedUncertainty, RingtestReport.RingtestBoxID, RingtestReport.vchMCAType, RingtestReport.realActivityRef FROM Ringtest, RingtestReport WHERE RingtestReport.AccountID = @accountID AND Ringtest.ID = RingtestReport.RingtestID AND RingtestReport.bitEvaluated = 1 AND RingtestReport.bitApproved = 1 ORDER BY Ringtest.intYear ASC", Interface.connection);
            adapter.SelectCommand.Parameters.AddWithValue("@accountID", accountID);
            adapter.Fill(dataSet);
            return dataSet;
        }

        public static DataSet select_Error_CalculatedUncertainty_RingtestBoxID_MCAType_ActivityRef_where_AccountID_Year(Guid accountID, int year)
        {
            if (accountID == Guid.Empty)
                throw new Exception("RingtestReport select_Error_Uncertainty_where_AccountID_Year: Invalid identifier");

            DataSet dataSet = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT RingtestReport.realError, RingtestReport.realCalculatedUncertainty, RingtestReport.RingtestBoxID, RingtestReport.vchMCAType, RingtestReport.realActivityRef FROM Ringtest, RingtestReport WHERE RingtestReport.AccountID = @accountID AND Ringtest.ID = RingtestReport.RingtestID AND RingtestReport.bitEvaluated = 1 AND RingtestReport.bitApproved = 1 AND Ringtest.intYear = @year", Interface.connection);
            adapter.SelectCommand.Parameters.AddWithValue("@accountID", accountID);
            adapter.SelectCommand.Parameters.AddWithValue("@year", year);
            adapter.Fill(dataSet);
            return dataSet;
        }        
    }

    public class PendingAccount : Base
    {        
        protected string mName;
        protected string mContact;
        protected string mAddress;
        protected string mPostbox;
        protected string mPostal;
        protected string mEmail;
        protected string mPhone;
        protected string mMobile;
        protected string mFax;
        protected string mWebsite;

        public PendingAccount() {}

        public PendingAccount(string name, string contact, string address, string postbox, string postal, string email, string phone, string mobile, string fax, string website)
        {            
            mName = name;
            mContact = contact;
            mAddress = address;
            mPostbox = postbox;
            mPostal = postal;
            mEmail = email;
            mPhone = phone;
            mMobile = mobile;
            mFax = fax;
            mWebsite = website;            
        }        
        
        public string Name { get { return mName; } set { mName = value; } }
        public string Contact { get { return mContact; } set { mContact = value; } }
        public string Address { get { return mAddress; } set { mAddress = value; } }
        public string Postbox { get { return mPostbox; } set { mPostbox = value; } }
        public string Postal { get { return mPostal; } set { mPostal = value; } }
        public string Email { get { return mEmail; } set { mEmail = value; } }
        public string Phone { get { return mPhone; } set { mPhone = value; } }
        public string Mobile { get { return mMobile; } set { mMobile = value; } }
        public string Fax { get { return mFax; } set { mFax = value; } }
        public string Website { get { return mWebsite; } set { mWebsite = value; } }        

        public bool insert_with_ID(Guid ID)
        {
            if (ID == Guid.Empty)
                throw new Exception("PendingAccount insert_with_ID: Invalid identifier");

            mID = ID;
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_insert_on_pendingaccount_with_ID";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@ID", mID);            
            Interface.command.Parameters.AddWithValue("@name", mName);
            Interface.command.Parameters.AddWithValue("@contact", mContact);
            Interface.command.Parameters.AddWithValue("@address", mAddress);
            Interface.command.Parameters.AddWithValue("@postbox", mPostbox);
            Interface.command.Parameters.AddWithValue("@postal", mPostal);
            Interface.command.Parameters.AddWithValue("@email", mEmail);
            Interface.command.Parameters.AddWithValue("@phone", mPhone);
            Interface.command.Parameters.AddWithValue("@mobile", mMobile);
            Interface.command.Parameters.AddWithValue("@fax", mFax);
            Interface.command.Parameters.AddWithValue("@website", mWebsite);            

            return Interface.command.ExecuteNonQuery() > 0;
        }        

        public bool select_all_where_ID(Guid ID)
        {
            if (ID == Guid.Empty)
                throw new Exception("PendingAccount select_all_where_ID: Invalid identifier");

            mID = ID;
            bool returnValue = false;
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_select_all_on_pendingaccount_where_ID";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@ID", mID);

            SqlDataReader reader = Interface.command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                mID = reader.GetGuid(0);                
                mName = reader.GetString(1).Trim();
                mContact = reader.GetString(2).Trim();
                mAddress = reader.GetString(3).Trim();
                mPostbox = reader.GetString(4).Trim();
                mPostal = reader.GetString(5).Trim();
                mEmail = reader.GetString(6).Trim();
                mPhone = reader.GetString(7).Trim();
                mMobile = reader.GetString(8).Trim();
                mFax = reader.GetString(9).Trim();
                mWebsite = reader.GetString(10).Trim();                
                returnValue = true;
            }
            reader.Close();
            return returnValue;
        }

        public bool delete_by_ID()
        {
            if (mID == Guid.Empty)
                throw new Exception("PendingAccount delete_by_ID: Invalid identifier");

            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_delete_on_pendingaccount_by_ID";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@ID", mID);

            return (Interface.command.ExecuteNonQuery() > 0);
        }
    }

    public class Account : Base
    {    
        protected Guid mRingtestBoxID;
        protected string mName;
        protected string mContact;
        protected string mAddress;                
        protected string mPostbox;
        protected string mPostal;        
        protected string mEmail;
        protected string mPhone;
        protected string mMobile;
        protected string mFax;
        protected string mWebsite;        
        protected bool mActive;
        protected string mComment;
        protected int mLastRegistrationYear;
        protected int mRingtestCount;        
        protected string mRingtestContact;

        public Account() { }        

        public Account(Guid ringtestBoxID, string name, string contact, string address, string postbox, string postal, string email, 
            string phone, string mobile, string fax, string website, bool active, string comment, int lastRegistrationYear, 
            int ringtestCount, string ringtestContact) 
        {            
            mRingtestBoxID = ringtestBoxID;
            mName = name;
            mContact = contact;
            mAddress = address;
            mPostbox = postbox;
            mPostal = postal;
            mEmail = email;
            mPhone = phone;
            mMobile = mobile;
            mFax = fax;
            mWebsite = website;
            mActive = active;
            mComment = comment;
            mLastRegistrationYear = lastRegistrationYear;
            mRingtestCount = ringtestCount;
            mRingtestContact = ringtestContact;            
        }        

        public Guid RingtestBoxID { get { return mRingtestBoxID; } set { mRingtestBoxID = value; } }
        public string Name { get { return mName; } set { mName = value; } }
        public string Contact { get { return mContact; } set { mContact = value; } }
        public string Address { get { return mAddress; } set { mAddress = value; } }                
        public string Postbox { get { return mPostbox; } set { mPostbox = value; } }
        public string Postal { get { return mPostal; } set { mPostal = value; } }        
        public string Email { get { return mEmail; } set { mEmail = value; } }
        public string Phone { get { return mPhone; } set { mPhone = value; } }
        public string Mobile { get { return mMobile; } set { mMobile = value; } }
        public string Fax { get { return mFax; } set { mFax = value; } }
        public string Website { get { return mWebsite; } set { mWebsite = value; } }        
        public bool Active { get { return mActive; } set { mActive = value; } }
        public string Comment { get { return mComment; } set { mComment = value; } }
        public int LastRegistrationYear { get { return mLastRegistrationYear; } set { mLastRegistrationYear = value; } }
        public int RingtestCount { get { return mRingtestCount; } set { mRingtestCount = value; } }
        public string RingtestContact { get { return mRingtestContact; } set { mRingtestContact = value; } }        

        public bool insert_with_ID(Guid ID)
        {
            if (ID == Guid.Empty)
                throw new Exception("Account insert_with_ID: Invalid identifier");

            mID = ID;
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_insert_on_account_with_ID";
            Interface.command.CommandType = CommandType.StoredProcedure;            
            Interface.command.Parameters.AddWithValue("@ID", mID);
            Interface.command.Parameters.AddWithValue("@ringtestBoxID", mRingtestBoxID);
            Interface.command.Parameters.AddWithValue("@name", mName);
            Interface.command.Parameters.AddWithValue("@contact", mContact);
            Interface.command.Parameters.AddWithValue("@address", mAddress);
            Interface.command.Parameters.AddWithValue("@postbox", mPostbox);
            Interface.command.Parameters.AddWithValue("@postal", mPostal);
            Interface.command.Parameters.AddWithValue("@email", mEmail);
            Interface.command.Parameters.AddWithValue("@phone", mPhone);
            Interface.command.Parameters.AddWithValue("@mobile", mMobile);
            Interface.command.Parameters.AddWithValue("@fax", mFax);
            Interface.command.Parameters.AddWithValue("@website", mWebsite);            
            Interface.command.Parameters.AddWithValue("@active", mActive);
            Interface.command.Parameters.AddWithValue("@comment", mComment);
            Interface.command.Parameters.AddWithValue("@lastRegistrationYear", mLastRegistrationYear);
            Interface.command.Parameters.AddWithValue("@ringtestCount", mRingtestCount);
            Interface.command.Parameters.AddWithValue("@ringtestContact", mRingtestContact);            

            return Interface.command.ExecuteNonQuery() > 0;            
        }

        public bool update_Int_by_ID(string columnName, int value)
        {
            if (mID == Guid.Empty)
                throw new Exception("Account update_Int_where_ID: Invalid identifier");
            
            Interface.command.Parameters.Clear();            
            Interface.command.CommandText = "IF EXISTS(SELECT ID FROM Account WHERE ID = @ID) UPDATE Account SET " + columnName + " = @val WHERE ID = @ID";
            Interface.command.CommandType = CommandType.Text;
            Interface.command.Parameters.AddWithValue("@ID", mID);
            Interface.command.Parameters.AddWithValue("@val", value);            

            return (Interface.command.ExecuteNonQuery() > 0);
        }

        public bool update_String_by_ID(string columnName, string value)
        {
            if (mID == Guid.Empty)
                throw new Exception("Account update_String_where_ID: Invalid identifier");

            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "IF EXISTS(SELECT ID FROM Account WHERE ID = @ID) UPDATE Account SET " + columnName + " = @val WHERE ID = @ID";
            Interface.command.CommandType = CommandType.Text;
            Interface.command.Parameters.AddWithValue("@ID", mID);
            Interface.command.Parameters.AddWithValue("@val", value);

            return (Interface.command.ExecuteNonQuery() > 0);
        }

        public bool update_all_by_ID()
        {
            if (mID == Guid.Empty)
                throw new Exception("Account update_all_by_ID: Invalid identifier");
            
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_update_all_on_account_where_ID";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@ID", mID);
            Interface.command.Parameters.AddWithValue("@RingtestBoxID", mRingtestBoxID);
            Interface.command.Parameters.AddWithValue("@name", mName);
            Interface.command.Parameters.AddWithValue("@contact", mContact);
            Interface.command.Parameters.AddWithValue("@address", mAddress);
            Interface.command.Parameters.AddWithValue("@postbox", mPostbox);
            Interface.command.Parameters.AddWithValue("@postal", mPostal);
            Interface.command.Parameters.AddWithValue("@email", mEmail);
            Interface.command.Parameters.AddWithValue("@phone", mPhone);
            Interface.command.Parameters.AddWithValue("@mobile", mMobile);
            Interface.command.Parameters.AddWithValue("@fax", mFax);
            Interface.command.Parameters.AddWithValue("@website", mWebsite);
            Interface.command.Parameters.AddWithValue("@active", mActive);
            Interface.command.Parameters.AddWithValue("@comment", mComment);
            Interface.command.Parameters.AddWithValue("@lastRegistrationYear", mLastRegistrationYear);
            Interface.command.Parameters.AddWithValue("@ringtestCount", mRingtestCount);
            Interface.command.Parameters.AddWithValue("@ringtestContact", mRingtestContact);            

            return Interface.command.ExecuteNonQuery() > 0;            
        }

        public bool delete_by_ID()
        {
            if (mID == Guid.Empty)
                throw new Exception("Account delete_by_ID: Invalid identifier");

            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "DELETE FROM Account WHERE ID = @id";
            Interface.command.CommandType = CommandType.Text;
            Interface.command.Parameters.AddWithValue("@id", mID);            

            return Interface.command.ExecuteNonQuery() > 0;
        }

        public bool select_all_where_ID(Guid ID)
        {
            if (ID == Guid.Empty)
                throw new Exception("Account select_all_where_ID: Invalid identifier");

            mID = ID;
            bool returnValue = false;            
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_select_all_on_account_where_ID";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@ID", mID);

            SqlDataReader reader = Interface.command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                mID = reader.GetGuid(0);
                mRingtestBoxID = reader.GetGuid(1);
                mName = reader.GetString(2).Trim();
                mContact = reader.GetString(3).Trim();
                mAddress = reader.GetString(4).Trim();
                mPostbox = reader.GetString(5).Trim();
                mPostal = reader.GetString(6).Trim();
                mEmail = reader.GetString(7).Trim();
                mPhone = reader.GetString(8).Trim();
                mMobile = reader.GetString(9).Trim();
                mFax = reader.GetString(10).Trim();
                mWebsite = reader.GetString(11).Trim();
                mActive = reader.GetBoolean(12);
                mComment = reader.IsDBNull(13) ? "" : reader.GetString(13).Trim();
                mLastRegistrationYear = reader.IsDBNull(14) ? 0 : reader.GetInt32(14);
                mRingtestCount = reader.IsDBNull(15) ? 0 : reader.GetInt32(15);
                mRingtestContact = reader.GetString(16).Trim();                
                returnValue = true;
            }
            reader.Close();
            return returnValue;
        }

        public bool select_String_where_ID(Guid ID, string column, ref string result)
        {
            if (ID == Guid.Empty)
                throw new Exception("Account select_String_where_ID: Invalid identifier");

            mID = ID;
            bool returnValue = false;
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "SELECT " + column + " FROM Account WHERE ID = @ID";
            Interface.command.CommandType = CommandType.Text;
            Interface.command.Parameters.AddWithValue("@ID", mID);

            SqlDataReader reader = Interface.command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                result = reader.GetString(0).Trim();
                returnValue = true;
            }
            reader.Close();
            return returnValue;
        }

        public bool select_Bool_where_ID(Guid ID, string column, ref bool result)
        {
            if (ID == Guid.Empty)
                throw new Exception("Account select_String_where_ID: Invalid identifier");

            mID = ID;
            bool returnValue = false;
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "SELECT " + column + " FROM Account WHERE ID = @ID";
            Interface.command.CommandType = CommandType.Text;
            Interface.command.Parameters.AddWithValue("@ID", mID);

            SqlDataReader reader = Interface.command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                result = reader.GetBoolean(0);
                returnValue = true;
            }
            reader.Close();
            return returnValue;
        }

        public void select_LastYear_from_RingtestReport()
        {
            if (mID == Guid.Empty)
                throw new Exception("Account select_LastYear_from_RingtestReport: Invalid identifier");
                        
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = @"SELECT MAX(rt.intYear)
FROM Ringtest rt INNER JOIN
RingtestReport rr ON rr.RingtestID = rt.ID INNER JOIN
Account a ON a.ID = rr.AccountID AND a.ID = @id";
            Interface.command.CommandType = CommandType.Text;
            Interface.command.Parameters.AddWithValue("@id", mID);

            object year = Interface.command.ExecuteScalar();
            if (year == DBNull.Value)
                LastRegistrationYear = 0;                                            
            else LastRegistrationYear = Convert.ToInt32(year);            
        }

        public static void select_ID(ref List<Guid> idList, int year)
        {
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "SELECT DISTINCT Account.ID FROM Account inner join RingtestReport rtr on Account.ID = rtr.AccountID inner join Ringtest rt on rtr.RingtestID = rt.ID AND rt.intYear = @year";
            Interface.command.CommandType = CommandType.Text;
            Interface.command.Parameters.AddWithValue("@year", year);
            SqlDataReader reader = Interface.command.ExecuteReader();
            idList.Clear();
            while (reader.Read())            
                idList.Add(reader.GetGuid(0));
            reader.Close();
        }

        public static void select_Name_where_LastRegistrationYear(ref List<string> nameList, int year)
        {
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "SELECT vchName FROM Account WHERE intLastRegistrationYear = @year";
            Interface.command.CommandType = CommandType.Text;
            Interface.command.Parameters.AddWithValue("@year", year);
            SqlDataReader reader = Interface.command.ExecuteReader();
            nameList.Clear();
            while (reader.Read())            
                nameList.Add(reader.GetString(0));
            reader.Close();
        }

        public static void select_Email_where_LastRegistrationYear(ref List<string> emailList, int year)
        {
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "SELECT vchEmail FROM Account WHERE intLastRegistrationYear = @year";
            Interface.command.CommandType = CommandType.Text;
            Interface.command.Parameters.AddWithValue("@year", year);
            SqlDataReader reader = Interface.command.ExecuteReader();
            emailList.Clear();
            while (reader.Read())
                emailList.Add(reader.GetString(0));
            reader.Close();
        }

        public static bool accountNameExists(string name)
        {
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "SELECT ID FROM Account WHERE vchName = @name";
            Interface.command.CommandType = CommandType.Text;
            Interface.command.Parameters.AddWithValue("@name", name);

            SqlDataReader reader = Interface.command.ExecuteReader();
            bool returnValue = reader.HasRows ? true : false;
            reader.Close();
            return returnValue;
        }

        public static void update_ringtestBoxID(Guid ringtestBoxID)
        {                                    
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "UPDATE Account SET RingtestBoxID = @ringtestBoxID";
            Interface.command.CommandType = CommandType.Text;
            Interface.command.Parameters.AddWithValue("@ringtestBoxID", ringtestBoxID);
            Interface.command.ExecuteNonQuery();                    
        }
    }

    public class AccountAbility : Base
    {    
        protected string mName;

        public AccountAbility() { }
    
        public string Name { get { return mName; } set { mName = value; } }
    }

    public class RingtestBox : Base
    {    
        protected string mKNumber;
        protected string mExternID;
        protected DateTime mRefDate;
        protected float mRefValue;
        protected float mUncertainty;        
        protected float mWeight;
        protected string mStatus;
        protected string mComment;

        public RingtestBox() { }        

        public RingtestBox(string kNumber, string externID, DateTime refDate, float refValue, float uncertainty, float weight, string status, string comment) 
        {            
            mKNumber = kNumber;
            mExternID = externID;
            mRefDate = refDate;
            mRefValue = refValue;
            mUncertainty = uncertainty;            
            mWeight = weight;
            mStatus = status;
            mComment = comment;
        }        

        public string KNumber { get { return mKNumber; } set { mKNumber = value; } }
        public string ExternID { get { return mExternID; } set { mExternID = value; } }
        public DateTime RefDate { get { return mRefDate; } set { mRefDate = value; } }
        public float RefValue { get { return mRefValue; } set { mRefValue = value; } }
        public float Uncertainty { get { return mUncertainty; } set { mUncertainty = value; } }        
        public float Weight { get { return mWeight; } set { mWeight = value; } }
        public string Status { get { return mStatus; } set { mStatus = value; } }
        public string Comment { get { return mComment; } set { mComment = value; } }

        public bool insert_with_ID_KNumber(Guid ID, string kNumber)
        {
            if (ID == Guid.Empty)
                throw new Exception("RingtestBox insert_with_ID_KNumber: Invalid identifier");

            if(String.IsNullOrEmpty(kNumber))
                throw new Exception("RingtestBox insert_with_ID_KNumber: Invalid kNumber");

            mID = ID;
            mKNumber = kNumber;
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_insert_on_ringtestbox_with_ID_KNumber";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@ID", mID);
            Interface.command.Parameters.AddWithValue("@kNumber", mKNumber);
            Interface.command.Parameters.AddWithValue("@externID", mExternID);
            Interface.command.Parameters.AddWithValue("@refDate", mRefDate);
            Interface.command.Parameters.AddWithValue("@refValue", mRefValue);
            Interface.command.Parameters.AddWithValue("@uncertainty", mUncertainty);            
            Interface.command.Parameters.AddWithValue("@weight", mWeight);
            Interface.command.Parameters.AddWithValue("@status", mStatus);
            Interface.command.Parameters.AddWithValue("@comment", mComment);

            return Interface.command.ExecuteNonQuery() > 0;
        }

        public bool update_all_by_ID()
        {
            if (mID == Guid.Empty)
                throw new Exception("RingtestBox update_all_by_ID: Invalid identifier");

            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_update_all_on_ringtestbox_where_ID";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@ID", mID);
            Interface.command.Parameters.AddWithValue("@kNumber", mKNumber);
            Interface.command.Parameters.AddWithValue("@externID", mExternID);
            Interface.command.Parameters.AddWithValue("@refDate", mRefDate);
            Interface.command.Parameters.AddWithValue("@refValue", mRefValue);
            Interface.command.Parameters.AddWithValue("@uncertainty", mUncertainty);            
            Interface.command.Parameters.AddWithValue("@weight", mWeight);
            Interface.command.Parameters.AddWithValue("@status", mStatus);
            Interface.command.Parameters.AddWithValue("@comment", mComment);            

            return Interface.command.ExecuteNonQuery() > 0;
        }

        public bool delete_by_ID()
        {
            if (mID == Guid.Empty)
                throw new Exception("RingtestBox delete_by_ID: Invalid identifier");

            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_delete_on_ringtestbox_where_ID";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@ID", mID);

            return Interface.command.ExecuteNonQuery() > 0;
        }

        public bool select_all_where_ID(Guid ID)
        {
            if (ID == Guid.Empty)
                throw new Exception("RingtestBox select_all_where_ID: Invalid identifier");

            mID = ID;
            bool returnValue = false;            
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_select_all_on_ringtestbox_where_ID";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@ID", mID);

            SqlDataReader reader = Interface.command.ExecuteReader();
            if (reader.HasRows)
            {                
                reader.Read();
                mID = reader.GetGuid(0);
                mKNumber = reader.GetString(1);
                mExternID = reader.GetString(2);
                mRefDate = reader.GetDateTime(3);                
                mRefValue = Convert.ToSingle(reader.GetValue(4));
                mUncertainty = Convert.ToSingle(reader.GetValue(5));                
                mWeight = Convert.ToSingle(reader.GetValue(6));                
                mStatus = reader.GetString(7);
                mComment = reader.GetString(8);
                returnValue = true;
            }
            reader.Close();
            return returnValue;
        }        
    }

    public class Ringtest : Base
    {    
        protected int mYear;
        protected DateTime mStartDate;
        protected string mArchiveRef;
        protected string mComment;
        protected bool mFinished;

        public Ringtest() { }        

        public Ringtest(int year, DateTime startdate, string archiveRef, string comment) 
        {            
            mYear = year;
            mStartDate = startdate;
            mArchiveRef = archiveRef;
            mComment = comment;
            mFinished = false;
        }                

        public int Year { get { return mYear; } set { mYear = value; } }
        public DateTime StartDate { get { return mStartDate; } set { mStartDate = value; } }
        public string ArchiveRef { get { return mArchiveRef; } set { mArchiveRef = value; } }
        public string Comment { get { return mComment; } set { mComment = value; } }
        public bool Finished { get { return mFinished; } set { mFinished = value; } }

        public bool insert_with_ID_year(Guid ID, int year)
        {            
            if (ID == Guid.Empty)
                throw new Exception("Ringtest insert_with_ID_year: Invalid identifier");

            if (year <= 0)
                throw new Exception("Ringtest insert_with_ID_year: Invalid year");

            mID = ID;
            mYear = year;
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_insert_on_ringtest_with_ID_year";
            Interface.command.CommandType = CommandType.StoredProcedure;            
            Interface.command.Parameters.AddWithValue("@ID", mID);
            Interface.command.Parameters.AddWithValue("@year", mYear);
            Interface.command.Parameters.AddWithValue("@startDate", mStartDate);
            Interface.command.Parameters.AddWithValue("@archiveRef", mArchiveRef);
            Interface.command.Parameters.AddWithValue("@comment", mComment);
            Interface.command.Parameters.AddWithValue("@finished", false);            

            return Interface.command.ExecuteNonQuery() > 0;
        }        

        public bool update_all_by_ID_year()
        {
            if (mID == Guid.Empty)
                throw new Exception("Ringtest update_all_by_ID_year: Invalid identifier");

            if (mYear <= 0)
                throw new Exception("Ringtest update_all_by_ID_year: Invalid year");            

            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_update_all_on_ringtest_where_ID_year";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@year", mYear);
            Interface.command.Parameters.AddWithValue("@startDate", mStartDate);
            Interface.command.Parameters.AddWithValue("@archiveRef", mArchiveRef);
            Interface.command.Parameters.AddWithValue("@comment", mComment);
            Interface.command.Parameters.AddWithValue("@finished", mFinished);            

            return Interface.command.ExecuteNonQuery() > 0;
        }

        public bool select_all_where_year(int year)
        {
            if (year <= 0)
                throw new Exception("Ringtest select_all_where_year: Invalid year");            

            mYear = year;
            bool returnValue = false;
            SqlDataReader reader = null;

            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_select_all_on_ringtest_where_year";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@year", mYear);

            reader = Interface.command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                mID = reader.GetGuid(0);
                mYear = reader.GetInt32(1);
                mStartDate = reader.GetDateTime(2);
                mArchiveRef = reader.GetString(3);
                mComment = reader.GetString(4);
                mFinished = reader.GetBoolean(5);
                returnValue = true;
            }
            reader.Close();
            return returnValue;
        }

        public bool select_all_where_ID(Guid id)
        {            
            if (id == Guid.Empty)
                throw new Exception("Ringtest select_all_where_ID: Invalid identifier");

            mID = id;            
            bool returnValue = false;
            SqlDataReader reader = null;

            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "SELECT * FROM Ringtest WHERE ID = @id";
            Interface.command.CommandType = CommandType.Text;
            Interface.command.Parameters.AddWithValue("@id", mID);

            reader = Interface.command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                mID = reader.GetGuid(0);
                mYear = reader.GetInt32(1);
                mStartDate = reader.GetDateTime(2);
                mArchiveRef = reader.GetString(3);
                mComment = reader.GetString(4);
                mFinished = reader.GetBoolean(5);
                returnValue = true;
            }
            reader.Close();
            return returnValue;
        }

        public static void disableOldRingtests()
        {            
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "UPDATE Ringtest SET bitFinished = 1 WHERE intYear < @year";
            Interface.command.CommandType = CommandType.Text;
            Interface.command.Parameters.AddWithValue("@year", DateTime.Now.Year);

            Interface.command.ExecuteNonQuery();                         
        }
    }

    public class Course : Base
    {    
        protected string mTitle;
        protected string mDescription;
        protected string mComment;
        protected bool mCompleted;

        public Course() { }

        public Course(string title, string description, string comment, bool completed)
        {            
            mTitle = title;
            mDescription = description;
            mComment = comment;
            mCompleted = completed;
        }        
        
        public string Title { get { return mTitle; } set { mTitle = value; } }
        public string Description { get { return mDescription; } set { mDescription = value; } }
        public string Comment { get { return mComment; } set { mComment = value; } }
        public bool Completed { get { return mCompleted; } set { mCompleted = value; } }

        public bool insert_with_ID(Guid ID)
        {
            if (ID == Guid.Empty)
                throw new Exception("Course insert_with_ID: Invalid identifier");

            mID = ID;
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_insert_on_course_with_ID";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@ID", mID);
            Interface.command.Parameters.AddWithValue("@title", mTitle);
            Interface.command.Parameters.AddWithValue("@description", mDescription);
            Interface.command.Parameters.AddWithValue("@comment", mComment);
            Interface.command.Parameters.AddWithValue("@completed", mCompleted);

            return Interface.command.ExecuteNonQuery() > 0;
        }

        public bool update_all_by_ID()
        {
            if (mID == Guid.Empty)
                throw new Exception("Course update_all_by_ID: Invalid identifier");

            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_update_all_on_course_where_ID";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@ID", mID);
            Interface.command.Parameters.AddWithValue("@title", mTitle);
            Interface.command.Parameters.AddWithValue("@description", mDescription);
            Interface.command.Parameters.AddWithValue("@comment", mComment);
            Interface.command.Parameters.AddWithValue("@completed", mCompleted);            

            return Interface.command.ExecuteNonQuery() > 0;
        }

        public bool delete_by_ID()
        {
            if (mID == Guid.Empty)
                throw new Exception("Course delete_by_ID: Invalid identifier");

            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_delete_on_course_where_ID";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@ID", mID);

            return Interface.command.ExecuteNonQuery() > 0;
        }

        public bool delete_links_by_ID()
        {
            if (mID == Guid.Empty)
                throw new Exception("Course delete_links_where_ID: Invalid identifier");

            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_delete_links_on_contactcourse_where_courseID";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@courseID", mID);

            return Interface.command.ExecuteNonQuery() > 0;
        }

        public bool select_all_where_ID(Guid ID)
        {
            if (ID == Guid.Empty)
                throw new Exception("Course select_all_where_ID: Invalid identifier");

            mID = ID;
            bool returnValue = false;            
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_select_all_on_course_where_ID";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@ID", mID);

            SqlDataReader reader = Interface.command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                mID = reader.GetGuid(0);
                mTitle = reader.GetString(1);
                mDescription = reader.GetString(2);
                mComment = reader.GetString(3);
                mCompleted = reader.GetBoolean(4);
                returnValue = true;
            }
            reader.Close();
            return returnValue;
        }
    }

    public class Contact : Base
    {    
        protected Guid mAccountID;
        protected string mName;
        protected string mEmail;
        protected string mPhone;
        protected string mMobile;
        protected string mStatus;

        public Contact() { }        

        public Contact(Guid accountID, string name, string email, string phone, string mobile, string status) 
        {            
            mAccountID = accountID;
            mName = name;
            mEmail = email;
            mPhone = phone;
            mMobile = mobile;
            mStatus = status;
        }        

        public Guid AccountID { get { return mAccountID; } set { mAccountID = value; } }
        public string Name { get { return mName; } set { mName = value; } }
        public string Email { get { return mEmail; } set { mEmail = value; } }
        public string Phone { get { return mPhone; } set { mPhone = value; } }
        public string Mobile { get { return mMobile; } set { mMobile = value; } }
        public string Status { get { return mStatus; } set { mStatus = value; } }

        public bool insert_with_ID(Guid ID)
        {
            if (ID == Guid.Empty)
                throw new Exception("Contact insert_with_ID: Invalid identifier");

            mID = ID;
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_insert_on_contact_with_ID";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@ID", mID);
            Interface.command.Parameters.AddWithValue("@accountID", mAccountID);
            Interface.command.Parameters.AddWithValue("@name", mName);
            Interface.command.Parameters.AddWithValue("@email", mEmail);
            Interface.command.Parameters.AddWithValue("@phone", mPhone);
            Interface.command.Parameters.AddWithValue("@mobile", mMobile);
            Interface.command.Parameters.AddWithValue("@status", mStatus);

            return (Interface.command.ExecuteNonQuery() > 0);                                    
        }

        public bool link_with_courseID(Guid courseID)
        {
            if (mID == Guid.Empty)
                throw new Exception("Contact link_with_courseID: Invalid identifier");
            
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_link_on_contactcourse_with_courseID";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@contactID", mID);
            Interface.command.Parameters.AddWithValue("@courseID", courseID);            

            return (Interface.command.ExecuteNonQuery() > 0);
        }

        public bool unlink_with_courseID(Guid courseID)
        {
            if (mID == Guid.Empty)
                throw new Exception("Contact unlink_with_courseID: Invalid identifier");

            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_unlink_on_contactcourse_with_courseID";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@contactID", mID);
            Interface.command.Parameters.AddWithValue("@courseID", courseID);

            return (Interface.command.ExecuteNonQuery() > 0);
        }

        public bool update_all_by_ID()
        {
            if (mID == Guid.Empty)
                throw new Exception("Contact update_all_by_ID: Invalid identifier");

            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_update_all_on_contact_where_ID";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@ID", mID);
            Interface.command.Parameters.AddWithValue("@accountID", mAccountID);
            Interface.command.Parameters.AddWithValue("@name", mName);
            Interface.command.Parameters.AddWithValue("@email", mEmail);
            Interface.command.Parameters.AddWithValue("@phone", mPhone);
            Interface.command.Parameters.AddWithValue("@mobile", mMobile);
            Interface.command.Parameters.AddWithValue("@status", mStatus);

            return (Interface.command.ExecuteNonQuery() > 0);
        }        

        public bool unlink_from_courses_by_ID()
        {
            if (mID == Guid.Empty)
                throw new Exception("Contact unlink_from_courses_by_ID: Invalid identifier");

            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_unlink_on_contactcourse_where_ID";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@ID", mID);

            return Interface.command.ExecuteNonQuery() > 0;
        }        

        public bool select_all_by_ID(Guid ID)
        {
            if (ID == Guid.Empty)
                throw new Exception("Contact select_all_by_ID: Invalid identifier");

            mID = ID;
            bool returnValue = false;            
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_select_all_on_contact_where_ID";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@ID", mID);

            SqlDataReader reader = Interface.command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                mID = reader.GetGuid(0);
                mAccountID = reader.GetGuid(1);
                mName = reader.GetString(2).Trim();
                mEmail = reader.GetString(3).Trim();
                mPhone = reader.GetString(4).Trim();
                mMobile = reader.GetString(5).Trim();
                mStatus = reader.GetString(6).Trim();
                returnValue = true;
            }
            reader.Close();
            return returnValue;
        }

        public static bool select_ID_from_courseID(Guid courseID, ref List<Guid> idList)
        {            
            bool returnValue = true;
            Interface.command.Parameters.Clear();
            Interface.command.CommandText = "csp_select_ID_on_contactcourse_where_courseID";
            Interface.command.CommandType = CommandType.StoredProcedure;
            Interface.command.Parameters.AddWithValue("@courseID", courseID);
            idList.Clear();
            SqlDataReader reader = Interface.command.ExecuteReader();
            while (reader.Read())
            {
                idList.Add(reader.GetGuid(0));            
            }
            reader.Close();
            return returnValue;
        }

        public static string select_EmailAddresses(List<Guid> idList)
        {
            string emails = "", newEmail;
            foreach (Guid id in idList)
            {
                Interface.command.Parameters.Clear();
                Interface.command.CommandText = "SELECT c.vchEmail FROM Contact c, Account a WHERE c.ID = @id AND a.bitActive = 1 AND c.AccountID = a.ID";
                Interface.command.CommandType = CommandType.Text;
                Interface.command.Parameters.AddWithValue("@id", id);
                newEmail = Convert.ToString(Interface.command.ExecuteScalar());
                if(!String.IsNullOrEmpty(newEmail))
                    emails += newEmail + ";";
            }
            return emails;
        }
    }   
}