using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Xml;
using System.Data;
using System.Management.Automation;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

namespace ServiceManagement_WebApp
{
    public partial class WebPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadServiceDetailsInGrid();
        }
        private string ExecutePowerShellScript(string strServerName, string strServiceName, string strAction)
        {
            try
            {
                string strScriptFilePath = ConfigurationManager.AppSettings["PowerShellFilePath"].ToString();
                using (PowerShell PowerShellInstance = PowerShell.Create())
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(strScriptFilePath);
                    string strFileContent = sr.ReadToEnd();
                    PowerShellInstance.AddScript(strFileContent);
                    PowerShellInstance.AddParameter("ServerName", strServerName);
                    PowerShellInstance.AddParameter("WindowsServiceName", strServiceName);
                    PowerShellInstance.AddParameter("Action", strAction);
                    Collection<PSObject> PSOutput = PowerShellInstance.Invoke();
                    string strOutput = "";
                    foreach (PSObject outputItem in PSOutput)
                    {
                        if (outputItem != null)
                        {
                            strOutput += outputItem.BaseObject.ToString() + "\n";
                        }
                    }
                    return strOutput;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                return null;
            }
        }
        private void LoadServiceDetailsInGrid()
        {
            try
            {
                //Data Table Columns
                DataTable dtServiceDetails = new DataTable();
                dtServiceDetails.Columns.Add("Environment", typeof(string));
                dtServiceDetails.Columns.Add("ServerName", typeof(string));
                dtServiceDetails.Columns.Add("ServiceName", typeof(string));
                dtServiceDetails.Columns.Add("CurrentStatus", typeof(string));

                string strServiceConfigFilePath = ConfigurationManager.AppSettings["ServiceConfigFilePath"].ToString();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(strServiceConfigFilePath);

                foreach (XmlNode xmlNodeEnv in xmlDoc.DocumentElement.SelectNodes("/WindowsServiceDetails/Environment"))
                {
                    string strEnvironment = xmlNodeEnv.Attributes["Name"].InnerText;
                    XmlDocument xmlDocService = new XmlDocument();
                    for (int i = 0; i <= xmlNodeEnv.ChildNodes.Count - 1; i++)
                    {
                        string strServerName = xmlNodeEnv.ChildNodes[i].Attributes["ServerName"].InnerText;
                        string strServiceName = xmlNodeEnv.ChildNodes[i].Attributes["ServiceName"].InnerText;
                        dtServiceDetails.Rows.Add(strEnvironment, strServerName, strServiceName, "Status");
                    }
                }
                foreach (DataRow dtRow in dtServiceDetails.Rows)
                {
                    string strServerName = dtRow[1].ToString();
                    string strServiceName = dtRow[2].ToString();
                    string strAction = "Status";

                    dtRow[3] = ExecutePowerShellScript(strServerName, strServiceName, strAction);
                }

                //grdServiceDetails.DataSource = null;
                //grdServiceDetails.Columns.Clear();
                //grdServiceDetails.Rows.cle


                grdPreProdDetails.DataSource = dtServiceDetails;
                grdPreProdDetails.DataBind();

                grdPrdDetails.DataSource = dtServiceDetails;
                grdPrdDetails.DataBind();

                /* for (int i = 0; i <= grdServiceDetails.Rows.Count - 1; i++)
                 {
                     string strCurrentStatus = grdServiceDetails.Rows[i].Cells[3].Value.ToString();
                     strCurrentStatus = Regex.Replace(strCurrentStatus, @"\t|\n|\r", "");
                     if (strCurrentStatus == "Running")
                     {
                         grdServiceDetails.Rows[i].DefaultCellStyle.ForeColor = Color.Green;
                     }
                     else if (strCurrentStatus == "Stopped")
                     {
                         grdServiceDetails.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                     }
                 }*/

                /*if (grdServiceDetails.Columns.Count == 4)
               {
                  DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
                   grdServiceDetails.Columns.Add(btn);
                   btn.HeaderText = "Action";
                   btn.Text = "Start / Stop";
                   btn.Name = "btnAction";
                   btn.UseColumnTextForButtonValue = true;
            }*/

            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }

        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadServiceDetailsInGrid();
        }
    }
}