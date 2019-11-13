using FamilyManagementSoftware.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FamilyManagementSoftware
{
    public partial class AddingForm : Form
    {
        BindingSource parentageSource = new BindingSource();
        BindingSource memberSource= new BindingSource();
        BindingSource relationSource = new BindingSource(); 
        //List<TreeNode<Member>> lstParentageNode = new List<TreeNode<Member>>();
        TreeNode<Member> root;
        int rootID;
        int selectedID;
        public AddingForm()
        {
            InitializeComponent();
            //FormBorderStyle = FormBorderStyle.None;
            //WindowState = FormWindowState.Maximized;
            Load();
        }     
        #region Methods
        void Load()
        {
            dgvMember.DataSource = memberSource;
            dgvParentage.DataSource = parentageSource;
            dgvRelation.DataSource = relationSource;
            LoadListMember();
            LoadListParentage();
            LoadRelationships();           
            LoadFamilyListIntoCombobox(cmbFamilyList);
            AddMemberBinding();
            AddParentageBinding();
            AddRelationBinding();
        }
        void LoadListMember()
        {
            memberSource.DataSource = DAOMember.Instance.GetListMember();
        }
        void LoadListParentage()
        {
            parentageSource.DataSource = DAOParentage.Instance.GetListParentage();
        }
        void LoadRelationships()
        {
            relationSource.DataSource = DAORelation.Instance.GetListRelationship();
        }

        void AddMemberBinding()
        {
            txtmemID.DataBindings.Add(new Binding("Text", dgvMember.DataSource, "memID", true, DataSourceUpdateMode.Never));
            txtName.DataBindings.Add(new Binding("Text", dgvMember.DataSource, "memName", true, DataSourceUpdateMode.Never));            
            txtRustic.DataBindings.Add(new Binding("Text", dgvMember.DataSource, "rustic", true, DataSourceUpdateMode.Never));
            txtAddress.DataBindings.Add(new Binding("Text", dgvMember.DataSource, "address", true, DataSourceUpdateMode.Never));
            dtpBirth.DataBindings.Add(new Binding("Value", dgvMember.DataSource, "dob", true, DataSourceUpdateMode.Never));
            dtpDeath.DataBindings.Add(new Binding("Value", dgvMember.DataSource, "dod", true, DataSourceUpdateMode.Never));         
        }
        void AddParentageBinding()
        {
            txtpID.DataBindings.Add(new Binding("Text", dgvParentage.DataSource, "pID", true, DataSourceUpdateMode.Never));
            txtpName.DataBindings.Add(new Binding("Text", dgvParentage.DataSource, "pName", true, DataSourceUpdateMode.Never));
        }
        void AddRelationBinding()
        {
            txtP1ID.DataBindings.Add(new Binding("Text", dgvRelation.DataSource, "person1ID", true, DataSourceUpdateMode.Never));
            txtP2ID.DataBindings.Add(new Binding("Text", dgvRelation.DataSource, "person2ID", true, DataSourceUpdateMode.Never));
            txtRelation.DataBindings.Add(new Binding("Text", dgvRelation.DataSource, "relation", true, DataSourceUpdateMode.Never));
            txtFamilyID.DataBindings.Add(new Binding("Text", dgvRelation.DataSource, "parentageID", true, DataSourceUpdateMode.Never));
        }
        void LoadFamilyListIntoCombobox(ComboBox cmb)
        {
            cmb.DataSource = parentageSource;
            cmb.DisplayMember = "pName";
            cmb.ValueMember = "pID";
        }
        private void cmbFamilyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            memberSource.DataSource = DAOMember.Instance.GetAllMemOfAParentage(cmbFamilyList.SelectedIndex + 1);
        }
        #endregion
        #region Event Handler
        private event EventHandler insertMember;
        public event EventHandler InsertMember
        {
            add { insertMember += value; }
            remove { insertMember -= value; }
        }
        private event EventHandler modifyMember;
        public event EventHandler ModifyMember
        {
            add { modifyMember += value; }
            remove { modifyMember -= value; }
        }
        private event EventHandler deleteMember;
        public event EventHandler DeleteMember
        {
            add { deleteMember += value; }
            remove { deleteMember -= value; }
        }
        private event EventHandler insertParentage;
        public event EventHandler InsertParentage
        {
            add { insertParentage += value; }
            remove { insertParentage -= value; }
        }
        private event EventHandler modifyParentage;
        public event EventHandler ModifyParentage
        {
            add { modifyParentage += value; }
            remove { modifyParentage -= value; }
        }
        private event EventHandler deleteParentage;
        public event EventHandler DeleteParentage
        {
            add { deleteParentage += value; }
            remove { deleteParentage -= value; }
        }
        private event EventHandler insertRelation;
        public event EventHandler InsertRelation
        {
            add { insertRelation += value; }
            remove { insertRelation -= value; }
        }
        private event EventHandler modifyRelation;
        public event EventHandler ModifyRelation
        {
            add { modifyRelation += value; }
            remove { modifyRelation -= value; }
        }
        private event EventHandler deleteRelation;
        public event EventHandler DeleteRelation
        {
            add { deleteRelation += value; }
            remove { deleteRelation -= value; }
        }

        #endregion
        #region Click Events
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string memName = txtName.Text;
                int memID = int.Parse(txtmemID.Text);
                string rustic = txtRustic.Text;
                string gender;
                if (rdbMale.Checked)
                {
                    gender = rdbMale.Text;
                }
                else
                {
                    gender = rdbFemale.Text;
                }
                string curAdd = txtAddress.Text;
                DateTime dob = dtpBirth.Value;
                DateTime dod = dtpDeath.Value;
                if (DAOMember.Instance.InsertMember(memID, memName, rustic, gender, dob, dod, curAdd))
                {
                    MessageBox.Show("Add member successful");
                    LoadListMember();
                    if (insertMember != null)
                    {
                        insertMember(this, new EventArgs());

                    }
                }
                else
                {
                    MessageBox.Show("Adding Unsuccessful");
                }
            }
            catch { }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }               
        private void btnMod_Click_1(object sender, EventArgs e)
        {
            try
            {
                string memName = txtName.Text;
                int memID = int.Parse(txtmemID.Text);
                string rustic = txtRustic.Text;
                string gender;
                if (rdbMale.Checked)
                {
                    gender = rdbMale.Text;
                }
                else
                {
                    gender = rdbFemale.Text;
                }
                string curAdd = txtAddress.Text;
                DateTime dob = dtpBirth.Value;
                DateTime dod = dtpDeath.Value;
                if (DAOMember.Instance.ModifyMember(memID, memName, rustic, gender, dob, dod, curAdd))
                {
                    MessageBox.Show("Modify member successful");
                    LoadListMember();
                    if (modifyMember != null)
                    {
                        modifyMember(this, new EventArgs());

                    }
                }
                else
                {
                    MessageBox.Show("Modify member Unsuccessful");
                }
            }
            catch { }
        }

        private void btnDel_Click_1(object sender, EventArgs e)
        {
            int memID = int.Parse(txtmemID.Text);
            if (DAOMember.Instance.DeleteMember(memID))
            {
                DAORelation.Instance.DeleteMemberRelation(memID);
                MessageBox.Show("Delete member successful");
                LoadListMember();
                if (deleteMember != null)
                {
                    deleteMember(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Delete member Unsuccessful");
            }
        }

        private void btnAddParentage_Click_1(object sender, EventArgs e)
        {
            try
            {
                int pID = int.Parse(txtpID.Text);
                string pName = txtpName.Text;
                if (DAOParentage.Instance.InsertParentage(pID, pName))
                {
                    MessageBox.Show("Insert parentage successful");
                    LoadListParentage();
                    if (insertParentage != null)
                    {
                        insertParentage(this, new EventArgs());
                    }
                }
                else
                {
                    MessageBox.Show("Insert parentage Unsuccessful");
                }
            }
            catch { }
        }

        private void btnModifyParentage_Click_1(object sender, EventArgs e)
        {
            try
            {
                int pID = int.Parse(txtpID.Text);
                string pName = txtpName.Text;
                if (DAOParentage.Instance.ModifyParentage(pID, pName))
                {
                    MessageBox.Show("Modify parentage successful");
                    LoadListParentage();
                    if (modifyParentage != null)
                    {
                        modifyParentage(this, new EventArgs());
                    }
                }
                else
                {
                    MessageBox.Show("Modify parentage Unsuccessful");
                }
            }
            catch { }
        }

        private void btnDeleteParentage_Click_1(object sender, EventArgs e)
        {
            int pID = int.Parse(txtpID.Text);
            if (DAOParentage.Instance.DeleteParentage(pID))
            {
                MessageBox.Show("Delete parentage successful");
                LoadListParentage();
                if (deleteParentage != null)
                {
                    deleteParentage(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Delete parentage Unsuccessful");
            }
        }
        private void btnAddRelation_Click(object sender, EventArgs e)
        {
            try
            {
                int p1ID = int.Parse(txtP1ID.Text);
                int p2ID = int.Parse(txtP2ID.Text);
                string relation = txtRelation.Text;
                int familyID = int.Parse(txtFamilyID.Text);
                if (DAORelation.Instance.InsertRelation(p1ID, p2ID, relation, familyID))
                {
                    MessageBox.Show("Insert relationship successful");
                    LoadRelationships();
                    if (insertRelation != null)
                    {
                        insertRelation(this, new EventArgs());
                    }
                }
                else
                {
                    MessageBox.Show("Modify relationship Unsuccessful");
                }
            }
            catch { }
        }

        private void btnModRelation_Click(object sender, EventArgs e)
        {
            try
            {
                int p1ID = int.Parse(txtP1ID.Text);
                int p2ID = int.Parse(txtP2ID.Text);
                string relation = txtRelation.Text;
                int familyID = int.Parse(txtFamilyID.Text);
                if (DAORelation.Instance.ModifyRelation(p1ID, p2ID, relation, familyID))
                {
                    MessageBox.Show("Modify relationship successful");
                    LoadRelationships();
                    if (modifyRelation != null)
                    {
                        modifyRelation(this, new EventArgs());
                    }
                }
                else
                {
                    MessageBox.Show("Modify relationship Unsuccessful");
                }
            }
            catch { }
        }

        private void btnDelRelation_Click(object sender, EventArgs e)
        {
            int p1ID = int.Parse(txtP1ID.Text);
            int p2ID = int.Parse(txtP2ID.Text);
            if (DAORelation.Instance.DeleteRelation(p1ID, p2ID))
            {
                MessageBox.Show("Delete relationship successful");
                LoadRelationships();
                if (deleteRelation != null)
                {
                    deleteRelation(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Delete relationship Unsuccessful");
            }

        }
        #endregion

        #region Search Operations
        List<Member> Search(string memName)
        {
            List<Member> searchResult = DAOMember.Instance.searchMember(memName);
            return searchResult;
        }
        List<Parentage> ParentageSearch(string pName)
        {
            List<Parentage> searchResult = DAOParentage.Instance.searchParentage(pName);
            return searchResult;
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            memberSource.DataSource = Search(txtName.Text);
        }

        private void btnPSearch_Click(object sender, EventArgs e)
        {
            parentageSource.DataSource = ParentageSearch(txtpName.Text);
        }
        #endregion


        private void ArrangeTree()
        {
            using (Graphics gr = pnlTree.CreateGraphics())
            {
                // Arrange the tree once to see how big it is.
                float xmin = 0, ymin = 0;
                root.Arrange(gr, ref xmin, ref ymin);

                // Arrange the tree again to center it horizontally.
                xmin = (this.pnlTree.Width - xmin) / 2;
                ymin = 50;
                root.Arrange(gr, ref xmin, ref ymin);
            }

            //pnlTree.Refresh();
        }
       
        TreeNode<Member> SelectedNode;
        private void FindNodeUnderMouse(PointF pt)
        {
            // Deselect the previously selected node.
            if (SelectedNode != null)
            {
                SelectedNode.Data.Selected = false;
                lblNode.Text = "";
            }
            // Find the node at this position (if any).
            using (Graphics gr = pnlTree.CreateGraphics())
            {
                SelectedNode = root.NodeAtPoint(gr, pt);
            }
            // Select the node.
            if (SelectedNode != null)
            {
                SelectedNode.Data.Selected = true;
                lblNode.Text = SelectedNode.Data.Description;
            }

            // Redraw.
            pnlTree.Refresh();
        }

        private void pnlTree_MouseClick(object sender, MouseEventArgs e)
        {
            FindNodeUnderMouse(e.Location);
        }
        private TreeNode<Member> CreateNode(Member member) //Create TreeNode from a specified Member
        {
            Button btnMember = new Button();
            TreeNode<Member> treeNode = new TreeNode<Member>(new Member(member.memID,member.memName +" ("+ (member.gender.Trim()=="Male"? "♂" : "♀")  + ")", btnMember));
            return treeNode;
        }
        private void AddChildNode(TreeNode<Member> node) 
        {
            List<Member> child = DAORelation.Instance.GetChildList(node.Data.memID);  //Retrieve the child list of the node
            foreach(Member mem in child)
            {
                node.Children.Add(CreateNode(mem));          //Add child node into the node
                if(node.Data.memID==selectedID)              //Set selected properties = true if this is the node which selected in datagridview
                {
                    SelectedNode = root;
                    node.Data.Selected = true;
                }
            }
            foreach(TreeNode<Member> treeNode in node.Children)  //Use recursion to add child node for the children 
            {
                AddChildNode(treeNode);          
            }
        }      
        private void pnlTree_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            if (root != null)
            {
                root.DrawTree(e.Graphics); 
                ArrangeTree();

            }
        }
        private void btnViewTree_Click(object sender, EventArgs e)
        {          
           
            selectedID = int.Parse(dgvMember.CurrentRow.Cells["memID"].Value.ToString());  //Get the selected id
            rootID = DAORelation.Instance.GetIDAbove(selectedID);                          //Get the root id for drawing
            root = CreateNode(DAOMember.Instance.GetMemberByID(rootID));
            AddChildNode(root);          
            pnlTree.Refresh();
            pnlTree.Refresh();           
        }

        private void pnlTree_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            FindNodeUnderMouse2(e.Location);
        }
        private void FindNodeUnderMouse2(PointF pt)
        {
            using (Graphics gr = pnlTree.CreateGraphics())
            {
                selectedID = root.NodeAtPoint(gr,pt).Data.memID;
                rootID = DAORelation.Instance.GetIDAbove(selectedID);
                root = CreateNode(DAOMember.Instance.GetMemberByID(rootID));
                AddChildNode(root);           
                pnlTree.Refresh();
                pnlTree.Refresh();
            }
        }

        private void AddingForm_Load(object sender, EventArgs e)
        {

        }
    }
}
