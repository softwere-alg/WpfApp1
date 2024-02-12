using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.Columns.Add(new ColumnHeader() { Name = "Test" });
            listView1.Columns.Add(new ColumnHeader() { Name = "Test2" });

            var item = new ListViewItem();
            item.SubItems.Add("a");
            item.SubItems.Add("b");
            listView1.Items.Add(item);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items[0].SubItems[0].Text = "C";
        }
    }
}
