using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APE3
{
    public partial class FormCaratula: Form
    {
        public FormCaratula()
        {
            InitializeComponent();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Validaciones m = new Validaciones();
            this.Hide();  // Oculta la ventana actual (Caratula)
            m.ShowDialog();  // Abre la nueva ventana de forma modal (espera a que se cierre para continuar)
            this.Show();
        }
    }
}
