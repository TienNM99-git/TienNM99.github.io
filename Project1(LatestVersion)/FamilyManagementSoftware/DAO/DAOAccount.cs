using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyManagementSoftware.DAO
{
    public class DAOAccount
    {
        private static DAOAccount instance;

        public static DAOAccount Instance
        {
            get
            {
                if (instance == null)
                    instance = new DAOAccount();
                return DAOAccount.instance;
            }

            private set
            {
                DAOAccount.instance = value;
            }
        }
        private DAOAccount() { }

        public bool Login(string userName, string passWord)
        {
            FamilyManagementDataContext db = new FamilyManagementDataContext();
            var result = db.Authentications.ToList();
            foreach (var r in result)
            {
                if (r.username.Trim() == userName && r.password.Trim() == passWord)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
