using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using AdminPanelDevice.Models;
namespace AdminPanelDevice.Infrastructure
{

    public class mibinformation
    {
        public string NodeLabel { get; set; }
        public string NodeOid { get; set; }
        public int NodeType { get; set; }
        public string NodeTypeString { get; set; }
        public int NodeSyntax { get; set; }
        public string NodeSyntaxString { get; set; }
        public int NodeAccess { get; set; }
        public string NodeIndex { get; set; }
        public string NodeParentName { get; set; }
        public string NodeDescription { get; set; }
        public string NodeModuleName { get; set; }
        public string NodeFileName { get; set; }
        public int ListID { get; set; }
    }

    public class mibinf
    {
        public string OIDName { get; set; }
        public int Child { get; set; }
        public int Parrent { get; set; }
        public int TreeID { get; set; }
    }



    public class MibTreeCreate
    {
        public  int id = 0; public static int DeviceID;
        private TreeView tvwMibNodes = new TreeView();
        private System.ComponentModel.IContainer component = new System.ComponentModel.Container();
        private nsoftware.IPWorksSNMP.Mibbrowser mibbrowser = new nsoftware.IPWorksSNMP.Mibbrowser();
        public static List<mibinformation> miblist = new List<mibinformation>();
        public static List<mibinf> mibchild = new List<mibinf>();
        public List<MibTreeInformation> TreeMibList = new List<MibTreeInformation>();
        public List<TreeStructure> TreeChildParentList = new List<TreeStructure>();
        public DeviceContext db = new DeviceContext();

        public MibTreeCreate(string fileName,int deviceID)
        {
            mibbrowser.OnMibNode += new nsoftware.IPWorksSNMP.Mibbrowser.OnMibNodeHandler(mibbrowser1_OnMibNode);

            string filenem = "C:\\Users\\tyupi\\Documents\\Visual Studio 2017\\Projects\\AdminPanelDevice\\AdminPanelDevice\\" + fileName;
            mibbrowser.LoadMib(filenem);
            id = db.MibTreeInformations.Select(m => m.MibID).ToList().FirstOrDefault() + 1;
            DeviceID = deviceID;
            mibbrowser.ListSuccessors();

            id = 1;
            foreach (var item in TreeMibList)
            {
                TreeStructure childparent = new TreeStructure();
                var parrent = TreeMibList.Where(m => m.Name == item.ParentName).FirstOrDefault();
                childparent.OIDName = item.Name;
                if (parrent == null)
                {
                    childparent.Parrent = 0;
                }
                else
                {
                    childparent.Parrent = parrent.MibID + 1;
                }
                childparent.TreeID = id;
                childparent.Child = id;
                childparent.DeviceID = deviceID;
                TreeChildParentList.Add(childparent);
                id++;
            }
            db.MibTreeInformations.AddRange(TreeMibList);
            db.TreeStructure.AddRange(TreeChildParentList);
            db.SaveChanges();

        }



        public void mibbrowser1_OnMibNode(object sender, nsoftware.IPWorksSNMP.MibbrowserMibNodeEventArgs e)
        {
         
                string[] subids = e.NodeOid.Split(".".ToCharArray());
                string oid = "";
                TreeNodeCollection treeNodeCollection = tvwMibNodes.Nodes;

                foreach (string subid in subids)
                {
                    MyTreeNode matchingTreeNode = null;
                    foreach (MyTreeNode treeNode in treeNodeCollection)
                    {
                        if (treeNode.subid == subid)
                        {

                            matchingTreeNode = treeNode;

                            break;
                        }
                    }

                    oid += "." + subid;
                    if (matchingTreeNode == null)
                    {
                    MibTreeInformation tree = new MibTreeInformation();

                    tree.Name = e.NodeLabel;
                    tree.OID = e.NodeOid;
                    tree.Mib = e.NodeModuleName;
                    tree.Syntax = e.NodeSyntaxString;
                    tree.Description = e.NodeDescription;
                    tree.Access =e.NodeAccess.ToString();
                    tree.Status = e.NodeTypeString;
                    tree.Indexes = e.NodeIndex;
                    tree.MibID = id;
                    tree.DefVal = "";
                    tree.DeviceID = DeviceID;
                    //db.MibTreeInformations.Add(tree);
                    //db.SaveChanges();

                    tree.ParentName = e.NodeParentName;
                    TreeMibList.Add(tree);
                    //db.MibTreeInformations.Add(tree);
                        //mibinformation mib = new mibinformation();
                        //mib.NodeAccess = e.NodeAccess;
                        //mib.NodeDescription = e.NodeDescription;
                        //mib.NodeFileName = e.NodeFileName;
                        //mib.NodeIndex = e.NodeIndex;
                        //mib.NodeLabel = e.NodeLabel;
                        //mib.NodeModuleName = e.NodeModuleName;
                        //mib.NodeOid = e.NodeOid;
                        //mib.NodeParentName = e.NodeParentName;
                        //mib.NodeSyntax = e.NodeSyntax;
                        //mib.NodeSyntaxString = e.NodeSyntaxString;
                        //mib.NodeType = e.NodeType;
                        //mib.NodeTypeString = e.NodeTypeString;
                        //mib.ListID = id;
                        //miblist.Add(mib);
                        id++;
                        matchingTreeNode = new MyTreeNode(oid, e.NodeLabel, subid, e.NodeTypeString);
                        treeNodeCollection.Add(matchingTreeNode);
                    }

                    treeNodeCollection = matchingTreeNode.ChildNodes;
                }

        }

        class MyTreeNode : TreeNode
        {
            internal string oid, subid;

            internal MyTreeNode(string oid, string label, string subid, string nodeTypeString) : base(label)
            {
                this.Text = label;
                this.oid = oid;
                this.subid = subid;
            }
        }
    }
}
