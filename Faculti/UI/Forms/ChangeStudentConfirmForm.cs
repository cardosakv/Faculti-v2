using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Faculti.UI.Forms
{
    public partial class ChangeStudentConfirmForm : Form
    {
        public ChangeStudentConfirmForm()
        {
            InitializeComponent();
            ControlInteractives.SetButtonHoverEvent(ConfirmButton);
            CancelButton.DialogResult = DialogResult.Cancel;
            ConfirmButton.DialogResult = DialogResult.OK;
        }

        private void ChangeStudentConfirmForm_Load(object sender, EventArgs e)
        {
            FormAnimation.FadeIn(this);
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
