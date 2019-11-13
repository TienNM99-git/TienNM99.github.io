using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyManagementSoftware.DAO
{
    class DAOParentage
    {
        private static DAOParentage instance;
        public static DAOParentage Instance
        {
            get
            {
                if (instance == null)
                    instance = new DAOParentage();
                return DAOParentage.instance;
            }

            private set
            {
                DAOParentage.instance = value;
            }
        }

        private DAOParentage() { }

        public Parentage GetParentageByID(int pID)      //Get a parentage from a specific ID
        {
            FamilyManagementDataContext db = new FamilyManagementDataContext();
            var query = (from Parentage in db.Parentages
                         where Parentage.pID == pID
                         select Parentage).Single();
            return query;
        }
        public List<Parentage> GetListParentage()      //Get the list of parentage
        {
            FamilyManagementDataContext db = new FamilyManagementDataContext();
            var query = from Parentage in db.Parentages
                        select Parentage;
            List<Parentage> lstParentage = new List<Parentage>();
            foreach(Parentage parentage in query)
            {
                lstParentage.Add(parentage);
            }
            return lstParentage;
        }
        public bool InsertParentage(int pID, string pName)
        {
            FamilyManagementDataContext db = new FamilyManagementDataContext();
            Parentage newParentage = new Parentage();
            newParentage.pID = pID;
            newParentage.pName = pName;
            db.Parentages.InsertOnSubmit(newParentage);
            db.SubmitChanges();
            return true;
        }
        public bool ModifyParentage(int pID, string pName)
        {
            FamilyManagementDataContext db = new FamilyManagementDataContext();
            var modQuery = (from Parentage in db.Parentages
                            where Parentage.pID == pID
                            select Parentage).Single();
            if (modQuery != null)
            {
                modQuery.pID = pID;
                modQuery.pName = pName;               
                db.SubmitChanges();
            }
            return true;
        }
        public bool DeleteParentage(int pID)
        {
            FamilyManagementDataContext db = new FamilyManagementDataContext();
            var delQuery = from Parentage in db.Parentages
                           where Parentage.pID == pID
                           select Parentage;
            db.Parentages.DeleteAllOnSubmit(delQuery);
            db.SubmitChanges();
            return true;
        }
        public string convertToUnSign(string s)
        {
            string stFormD = s.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');
            return (sb.ToString().Normalize(NormalizationForm.FormD));
        }
        public List<Parentage> searchParentage(string pName)
        {
            FamilyManagementDataContext db = new FamilyManagementDataContext();
            var searchList = GetListParentage();
            foreach (Parentage parentage in searchList)
            {
                convertToUnSign(parentage.pName);
            }
            var searchQuery = from Parentage in searchList
                              where Parentage.pName.Contains(pName)
                              select Parentage;
            return searchQuery.ToList();
        }
    }
}
