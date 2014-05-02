using FISCA.Presentation.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace K12.General.Feedback
{
    public partial class IsViewForm : BaseForm
    {
        public IsViewForm(Dictionary<string, name> TimeDic)
        {
            InitializeComponent();

            foreach (string each in TimeDic.Keys)
            {
                name n = TimeDic[each];

                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridViewX1);
                row.Cells[0].Value = n._key;
                row.Cells[1].Value = n._value;
                row.Cells[2].Value = n._link;
                row.Cells[2].Tag = n._link;
                row.Tag = n;
                dataGridViewX1.Rows.Add(row);
            }

        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridViewX1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGridViewX1.Rows[e.RowIndex];
            if (row.Tag != null)
            {
                name n = (name)row.Tag;
                IsViewForm_Open open = new IsViewForm_Open(n);
                //open.TopMost = true;
                open.ShowDialog();
            }
        }

        private void dataGridViewX1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (dataGridViewX1.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag != null)
            {
                if (dataGridViewX1.Columns[e.ColumnIndex] == Column3)
                {
                    string url = dataGridViewX1.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag.ToString();
                    try
                    {
                        System.Diagnostics.Process.Start(url);
                    }
                    catch (Exception ex)
                    {
                        MsgBox.Show(ex.Message);
                    }
                    //UrlBrowser browser = new UrlBrowser(url);
                    //browser.ShowDialog();
                }
            }
        }

        private void 開啟最新消息內容ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewX1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridViewX1.CurrentRow;
                if (row.Tag != null)
                {
                    name n = (name)row.Tag;
                    IsViewForm_Open open = new IsViewForm_Open(n);
                    //open.TopMost = true;
                    open.ShowDialog();
                }
            }




        }
    }

    public class name
    {
        public string _key { get; set; }
        public string _value { get; set; }
        public string _link { get; set; }
    }
}
