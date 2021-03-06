﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyManagementSoftware.DAO
{
    class DAORelation
    {
        private static DAORelation instance;
        public static DAORelation Instance
        {
            get
            {
                if (instance == null)
                    instance = new DAORelation();
                return DAORelation.instance;
            }

            private set
            {
                DAORelation.instance = value;
            }
        }

        private DAORelation() { }
        public System.Data.Linq.Table<Relationship> GetListRelationship()
        {
            FamilyManagementDataContext db = new FamilyManagementDataContext();
            return db.Relationships;
        }
        public DataTable GetAllRelationOfAFamily(int pID)           //Get all relations of a parentage
        {
            DataTable allRelation = new DataTable();
            FamilyManagementDataContext db = new FamilyManagementDataContext();
            var selectQuery = from Relationship in db.Relationships
                                 where Relationship.parentageID == pID
                                 select Relationship;
            foreach(var r in selectQuery)
            {
                allRelation.Rows.Add(r);
            }
            return allRelation;
        }
     
        public List<Member> GetChildList(int mem1Id)
        {
            FamilyManagementDataContext db = new FamilyManagementDataContext();
            var query = from Relationship in db.Relationships
                        where Relationship.person1ID == mem1Id
                        select Relationship.person2ID;
            List<Member> childList = new List<Member>();
            foreach(int id in query)
            {
                childList.Add(DAOMember.Instance.GetMemberByID(id));

            }
            return childList;
        }
        public int GetIDAbove(int memID)         //Get the id of person who above a specific member
        {
            FamilyManagementDataContext db = new FamilyManagementDataContext();
            var query = (from Relationship in db.Relationships
                         where Relationship.person2ID == memID
                         select Relationship.person1ID).SingleOrDefault();   //Get the person1ID which indicates a father of a member
            if(query==null)
            {
                return memID;                                                //If there's no id above, return that member's ID
            }
            else                                                             //Else query to have the grandfather
            {
                var query2 = (from Relationship in db.Relationships
                              where Relationship.person2ID == int.Parse(query.ToString())  
                              select Relationship.person1ID).SingleOrDefault();
                if(query2==null)
                {
                    return int.Parse(query.ToString());
                }
                else
                {
                    return int.Parse(query2.ToString());
                }
            }
        }
        public bool InsertRelation(int mem1ID, int mem2ID, string relation, int pID)
        {
            FamilyManagementDataContext db = new FamilyManagementDataContext();
            Relationship newRelation = new Relationship();
            newRelation.person1ID = mem1ID;
            newRelation.person2ID = mem2ID;
            newRelation.relation = relation;
            newRelation.parentageID=pID;
            db.Relationships.InsertOnSubmit(newRelation);
            db.SubmitChanges();
            return true;
        }
        public bool ModifyRelation(int p1ID, int p2ID, string relation, int pID)
        {
            FamilyManagementDataContext db = new FamilyManagementDataContext();
            var modQuery = (from Relationship in db.Relationships
                            where Relationship.person1ID==p1ID && Relationship.person2ID==p2ID 
                            select Relationship).Single();
            if (modQuery != null)
            {
                modQuery.relation = relation;
                modQuery.parentageID = pID;
                db.SubmitChanges();
            }
            return true;
        }
        public bool DeleteRelation(int p1ID, int p2ID)                 //Delete a specific relation satisfy p1ID and p2ID
        {
            FamilyManagementDataContext db = new FamilyManagementDataContext();
            var delQuery = from Relationship in db.Relationships
                           where Relationship.person1ID == p1ID && Relationship.person2ID==p2ID || Relationship.person1ID==p2ID && Relationship.person2ID==p1ID
                           select Relationship;
            db.Relationships.DeleteAllOnSubmit(delQuery);
            db.SubmitChanges();
            return true;
        }
        public bool DeleteMemberRelation(int memID)                     //Delete all relation of a member
        {
            FamilyManagementDataContext db = new FamilyManagementDataContext();
            var delQuery = from Relationship in db.Relationships
                           where Relationship.person1ID == memID || Relationship.person2ID == memID
                           select Relationship;
            db.Relationships.DeleteAllOnSubmit(delQuery);
            db.SubmitChanges();
            return true;
        }

    }
}
