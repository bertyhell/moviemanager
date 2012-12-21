using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class DatabaseDetails
    {

        public DatabaseDetails()
        {

        }

        private int _databaseVersion;
        public int DatabaseVersion
        {
            get { return _databaseVersion; }
            set { _databaseVersion = value; }
        }

        private int _requiredVersion;
        public int RequiredVersion
        {
            get { return _requiredVersion; }
            set { _requiredVersion = value; }
        }
        
        private List<DatabaseVersionRecord> _versionRecords = new List<DatabaseVersionRecord>();
        public List<DatabaseVersionRecord> VersionRecords
        {
            get { return _versionRecords; }
            set { _versionRecords = value; }
        }
    }

    public class DatabaseVersionRecord
    {
        public DatabaseVersionRecord()
        {

        }

        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private int _version;
        public int Version
        {
            get { return _version; }
            set { _version = value; }
        }

        private DateTime _timestamp;
        public DateTime Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
    }
}
