using System;
using System.Collections.Generic;
using System.Data;

namespace SchedulTask.Test.Web
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Schedul.Instance.AttchTask(new Dictionary<string, UnitTask>()
                {
                    {"A",new UnitTask(1000,new SyncTask(),"")},
                    {"B",new UnitTask(500, new SyncTaskA(),"")},
                });
            }

            //new UnitTask(1, 6000, new SyncTask()),
            //new UnitTask(2, 6000, new SyncTaskA()),
            var status = Schedul.Instance.Tasks;
            var dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Status");
            dt.Columns.Add("Title");
            foreach (var statu in status)
            {
                var row = dt.NewRow();
                row["Id"] = statu.Key;
                row["Title"] = statu.Key;
                row["Status"] = statu.Value.Status;
                dt.Rows.Add(row);
            }
            repOrder.DataSource = dt;
            repOrder.DataBind();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
        }
    }
}