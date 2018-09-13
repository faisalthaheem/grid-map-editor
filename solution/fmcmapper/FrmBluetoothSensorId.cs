using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fmcmapper
{
    public partial class FrmBluetoothSensorId : Form
    {
        public string BluetoothAddress { get; set; }

        public FrmBluetoothSensorId()
        {
            InitializeComponent();
            this.BluetoothAddress = "00:00:00:00:00:00";
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            this.BluetoothAddress = string.Format("{0}:{1}:{2}:{3}:{4}:{5}",
                a0.Text,
                a1.Text,
                a2.Text,
                a3.Text,
                a4.Text,
                a5.Text);

            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void FrmBluetoothSensorId_Load(object sender, EventArgs e)
        {
            
        }

        private void FrmBluetoothSensorId_Shown(object sender, EventArgs e)
        {
            a0.Focus();
        }
    }
}
