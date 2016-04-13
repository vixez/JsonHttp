using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestAppWinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            JsonHttp.Options options = new JsonHttp.Options()
            {
                AllowAutoRedirect = true
            };
            WordOfTheDay wotd = await JsonHttp.Get<WordOfTheDay>(new Uri("http://urban-word-of-the-day.herokuapp.com/today"), options);
            wotd = wotd;
        }
    }
}
