using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FamilyManagementSoftware.DAO
{
    class DAOMember 
    {
        private static DAOMember instance;
        public static DAOMember Instance
        {
            get
            {
                if (instance == null)
                    instance = new DAOMember();
                return DAOMember.instance;
            }

            private set
            {
                DAOMember.instance = value;
            }
        }
        
        private DAOMember() { }
        public Member GetMemberByID(int memID)
        {
            FamilyManagementDataContext db = new FamilyManagementDataContext();

            var query = from Member in db.Members
                        where Member.memID == memID
                        select Member;

            return query.Single(); ;
        }
        public List<Member> GetListMember()
        {
            FamilyManagementDataContext db = new FamilyManagementDataContext();
            var query = from Member in db.Members
                        orderby Member.memID
                        select Member;
            List<Member> lstMember = new List<Member>();
            foreach(var member in query)
            {
                lstMember.Add(member);
            }
            return lstMember;
        }
        public List<Member> GetAllMemOfAParentage(int pID)
        {
            FamilyManagementDataContext db = new FamilyManagementDataContext();
            List<int> listID1 = new List<int>();
            var query = from Relationship in db.Relationships
                        where Relationship.parentageID == pID
                        select Relationship.person1ID;
            foreach (int id1 in query)
            {
                listID1.Add(id1);
            }
            List<int> listID2 = new List<int>();
            var query2 = from Relationship in db.Relationships
                         where Relationship.parentageID == pID
                         select Relationship.person2ID;
            foreach (int id2 in query2)
            {
                listID2.Add(id2);
            }
            List<int> ID = listID1.Union(listID2).ToList();
            List<Member> memList = new List<Member>();
            foreach (int id in ID)
            {
                memList.Add(DAOMember.Instance.GetMemberByID(id));
            }
            return memList;
        }
        public bool InsertMember(int memID, string name, string rustic, string gender, DateTime dob, DateTime dod, string curAdd)
        {

                FamilyManagementDataContext db = new FamilyManagementDataContext();
                Member newMem = new Member();
                newMem.memID = memID;
                newMem.memName = name;
                newMem.rustic = rustic;
                newMem.gender = gender;
                newMem.dob = dob;
                newMem.dod = dod;
                newMem.address = curAdd;
                db.Members.InsertOnSubmit(newMem);
                db.Members.Context.SubmitChanges();
                return true;
   
        }
        public bool ModifyMember(int memID, string name, string rustic, string gender, DateTime dob, DateTime dod, string curAdd)
        {
            FamilyManagementDataContext db = new FamilyManagementDataContext();
            var modQuery = (from Member in db.Members
                            where Member.memID==memID
                            select Member).Single();
            if (modQuery != null)
            {
                modQuery.memName = name;
                modQuery.memID = memID;
                modQuery.rustic = rustic;
                modQuery.gender = gender;
                modQuery.dob = dob;
                modQuery.dod = dod;
                modQuery.address = curAdd;
                db.SubmitChanges();
            }
            return true;

        }
        public bool DeleteMember(int memID)
        {
            FamilyManagementDataContext db = new FamilyManagementDataContext();
            var delQuery = (from Member in db.Members
                            where Member.memID == memID
                            select Member).Single();
            db.Members.DeleteOnSubmit(delQuery);
            db.SubmitChanges();
            return true;
        }
       
       
        public List<Member> searchMember(string memName)
        {
            FamilyManagementDataContext db = new FamilyManagementDataContext();            
            var searchList = GetListMember();
            var searchQuery = from Member in searchList
                              where Member.memName.Contains(memName)
                              select Member;
            return searchQuery.ToList();
        }
                             
        }
       
    }
