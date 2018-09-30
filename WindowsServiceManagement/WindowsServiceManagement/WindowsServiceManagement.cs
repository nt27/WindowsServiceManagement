using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Management.Automation;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Configuration;
using System.Xml;
using System.Drawing;

namespace WindowsServiceManagement
{
    public partial class WindowsServiceManagement : Form
    {
        public WindowsServiceManagement()
        {
            InitializeComponent();
        }

        private void WindowsServiceManagement_Load(object sender, EventArgs e)
        {
            try
            {
                lblTitle.Text = ConfigurationManager.AppSettings["ApplicationTitle"].ToString();
                LoadServiceDetailsInGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

                grdServiceDetails.DataSource = null;
                grdServiceDetails.Columns.Clear();
                grdServiceDetails.Rows.Clear();


                grdServiceDetails.DataSource = dtServiceDetails;

                for (int i = 0; i <= grdServiceDetails.Rows.Count - 1; i++)
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
                }

                if (grdServiceDetails.Columns.Count == 4)
                {
                    DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
                    grdServiceDetails.Columns.Add(btn);
                    btn.HeaderText = "Action";
                    btn.Text = "Start / Stop";
                    btn.Name = "btnAction";
                    btn.UseColumnTextForButtonValue = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void grdServiceDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 4)
                {
                    string strServerName = grdServiceDetails.Rows[e.RowIndex].Cells[1].Value.ToString();
                    string strServiceName = grdServiceDetails.Rows[e.RowIndex].Cells[2].Value.ToString();
                    string strCurrentStatus = grdServiceDetails.Rows[e.RowIndex].Cells[3].Value.ToString();
                    string strAction = "";
                    strCurrentStatus = Regex.Replace(strCurrentStatus, @"\t|\n|\r", "");
                    if (strCurrentStatus == "Running")
                        strAction = "Stop";
                    else if (strCurrentStatus == "Stopped")
                        strAction = "Start";

                    ExecutePowerShellScript(strServerName, strServiceName, strAction);
                    LoadServiceDetailsInGrid();
                    grdServiceDetails.CurrentCell = grdServiceDetails.Rows[e.RowIndex].Cells[0];
                    grdServiceDetails.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                MessageBox.Show(ex.Message);
                return null;
            }
        }
    }
}
