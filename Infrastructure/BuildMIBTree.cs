using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SnmpSharpNet;
using AxNetwork;
using System.IO;
using System.Web.UI.WebControls;
using AdminPanelDevice.Models;

namespace AdminPanelDevice.Infrastructure
{
    public class BuildMIBTree
    {
        AxNetwork.SnmpMibBrowser objSnmpMIB = new AxNetwork.SnmpMibBrowser();
        AxNetwork.NwConstants objConstants = new AxNetwork.NwConstants();
        SnmpObject objSnmp;
        int count = 0;
        int IDid = 0;
        int Expand = 0;
        TreeView treeView =new TreeView();
        string strMibFile;
        public string access, status;
        public TreeNode nodes = new TreeNode();
        public List<MibTreeInformation> MibSave = new List<MibTreeInformation>();
        public List<TreeStructure> ParentChild = new List<TreeStructure>();
        int ID = 1;
        public int DeviceMibID = 0;
        public int DeviceTreeID = 0;
        DeviceContext db = new DeviceContext();
        public struct son
        {
            public string name { get; set; }
            public int numb { get; set; }
        }
        List<son> Par = new List<son>();

        public BuildMIBTree(string MibName, int DeviceID)
        {
           
            TreeNode t1, troot;
            TreeNode tr;
            
            string strParent;
            char[] strTrim1 = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            char[] strTrim2 = { '.' };
            //objSnmpMIB.AddMibFile(@"C:\Users\tyupi\Desktop\MIB\Tredess\SELENIO\SEL-ENC1-MIB-7.1-01.mib");
            //objSnmpMIB.LoadMibFile(@"C:\Users\tyupi\Desktop\MIB\Tredess\SELENIO\SEL-ENC1-MIB-7.1-01.mib");

            objSnmpMIB.AddMibFile(@"C:\Users\tyupi\Documents\visual studio 2017\Projects\AdminPanelDevice\AdminPanelDevice\" + MibName);
            objSnmpMIB.LoadMibFile(@"C:\Users\tyupi\Documents\visual studio 2017\Projects\AdminPanelDevice\AdminPanelDevice\" + MibName);

            objSnmp = (SnmpObject)objSnmpMIB.Get("iso.org.dod.internet.private.enterprises");
            tr = new TreeNode("");
            treeView.Nodes.Add(tr);
            DeviceMibID = db.MibTreeInformations.Select(s => s.DeviceID).ToList().LastOrDefault();
            while (objSnmpMIB.LastError == 0)
            {
                
                t1 = new TreeNode(objSnmp.OIDNameShort);
                t1.Target = objSnmp.OID;
                strParent = objSnmp.OID.TrimEnd(strTrim1).TrimEnd(strTrim2);
                MibTreeInformation mibstr = new MibTreeInformation();

                if (objSnmp.Access == objConstants.nwMIB_ACCESS_NOACCESS)
                    access = "NOACCESS";
                else if (objSnmp.Access == objConstants.nwMIB_ACCESS_NOTIFY)
                    access = "NOTIFY";
                else if (objSnmp.Access == objConstants.nwMIB_ACCESS_READONLY)
                    access = "READONLY";
                else if (objSnmp.Access == objConstants.nwMIB_ACCESS_WRITEONLY)
                    access = "WRITEONLY";
                else if (objSnmp.Access == objConstants.nwMIB_ACCESS_READWRITE)
                    access = "READWRITE";
                else if (objSnmp.Access == objConstants.nwMIB_ACCESS_READCREATE)
                    access = "READCREATE";
                else
                    access = "n/a";

                if (objSnmp.Status == objConstants.nwMIB_STATUS_CURRENT)
                    status = "CURRENT";
                else if (objSnmp.Status == objConstants.nwMIB_STATUS_DEPRECATED)
                    status = "DEPRECATED";
                else if (objSnmp.Status == objConstants.nwMIB_STATUS_OBSOLETE)
                    status = "OBSOLETE";
                else if (objSnmp.Status == objConstants.nwMIB_STATUS_MANDATORY)
                    status = "MANDATORY";
                else
                    status = "n/a";
                DeviceMibID++;

                mibstr.Name = objSnmp.OIDNameShort;
                //mibstr.ID = ID;
                mibstr.MibID = DeviceMibID;
                mibstr.OID = objSnmp.OID;
                mibstr.Mib = "RFC1213MIB";
                mibstr.Syntax = objSnmp.Syntax;
                mibstr.Access = access;
                mibstr.Status = status;
                mibstr.DefVal = "";
                mibstr.Indexes = "";
                mibstr.Description = objSnmp.Description;
                mibstr.DeviceID = DeviceID;
                MibSave.Add(mibstr);
                db.MibTreeInformations.Add(mibstr);
                //troot =FindNodeByValue(treeView1.Nodes, strParent);
                //t1.ForeColor = objSnmp.IsUserMib ? Color.DarkBlue : Color.Black;

                //ID++;

                if ((troot = FindNodeByValue(treeView.Nodes, strParent)) != null)
                {
                    
                    troot.ChildNodes.Add(t1);

                }
                else
                {
                    treeView.Nodes.Add(t1);
                    //OidDescription.Add(t1);
                }
                objSnmp = (SnmpObject)objSnmpMIB.GetNext();

            }
            treeView.ExpandAll();
            SearchTreeView(MibSave[MibSave.Count - 1].OID, treeView.Nodes, DeviceID);
            treeView.ExpandAll();
            db.SaveChanges();
        }
        /// <summary>
        private TreeNode SearchTreeView(string p_sSearchTerm, TreeNodeCollection p_Nodes , int DeviceID)
        {
            int ss = 0;
            MibTreeInformation treeid = new MibTreeInformation();
            son SonParent = new son();
            foreach (TreeNode node in p_Nodes)
            {
                if (node.Text == p_sSearchTerm || (string)node.Target == p_sSearchTerm)
                {
                    Expand = 1;
                    SonParent.name = node.Text;
                    SonParent.numb = IDid - 1;
                    Par.Add(SonParent);
                    //TreeStructure parent = new TreeStructure();
                    //parent.ID = 1;
                    //parent.Child = 1;
                    //parent.Parrent = 1;
                    //parent.OIDName = MibSave[0].Name;
                    //ParentChild.Add(parent);
                    DeviceTreeID = db.TreeStructure.Select(s => s.DeviceID).ToList().LastOrDefault();
                    for (int j = 0; j < MibSave.Count; j++)
                    {
                        TreeStructure parent = new TreeStructure();
                        if (j == 0)
                        {
                            //parent.ID = 1;
                            parent.Child = 1;
                            parent.Parrent = 1;
                            DeviceTreeID++;
                            parent.TreeID = DeviceTreeID;
                            parent.DeviceID = DeviceID;
                            parent.OIDName = MibSave[0].Name;
                            ParentChild.Add(parent);
                        }
                        else
                        {
                            var response = Par.Where(r => r.name == MibSave[j].Name).FirstOrDefault();
                            //parent.ID = j + 1;
                            parent.Child = j + 1;
                            parent.OIDName = response.name;
                            parent.Parrent = response.numb;
                            DeviceTreeID++;
                            parent.TreeID = DeviceTreeID;
                            parent.DeviceID = DeviceID;
                            ParentChild.Add(parent);
                        }
                       
                        db.TreeStructure.Add(parent);
                        //db.SaveChanges();   // shenaxva bazasHi
                    }


                    return node;
                }
                if (node.ChildNodes.Count > 0)
                {
                    ss++;
                    var expandTree = node;
                    if (ID >= 1)
                    {

                        for (int i = 0; i < node.ChildNodes.Count; i++)
                        {
                            var nodeID = node.ChildNodes[i].Parent;
                            if (nodeID != null)
                            {
                                treeid = MibSave.Where(t => t.Name == nodeID.Text).FirstOrDefault();
                                SonParent.name = node.ChildNodes[i].Text;
                                SonParent.numb = treeid.ID;
                                Par.Add(SonParent);
                            }
                        }
                    }
                    IDid++;
                    TreeNode child = SearchTreeView(p_sSearchTerm, node.ChildNodes, DeviceID);
                    if (child != null)
                        return child;
                }
                else
                {
                    if (IDid == 0)
                        IDid = 1;

                }
            }
            return null;
        }
        /// </summary>
        private TreeNode FindNodeByValue(TreeNodeCollection tRoot, string strOid)
        {
            int i;
            string strTag;
            TreeNode t;

            if (tRoot == null)
                return null;

            for (i = 0; i < tRoot.Count; i++)
            {
                strTag = (string)tRoot[i].Target;
                if (strTag == strOid)
                {
                    return tRoot[i];
                }
                else
                if ((t = FindNodeByValue(tRoot[i].ChildNodes, strOid)) != null)
                    return t;
            }

            return null;
        }
    }
}