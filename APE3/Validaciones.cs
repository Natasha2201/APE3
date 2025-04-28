using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace APE3
{
    public partial class Validaciones: Form
    {
        private ErrorProvider errorProvider;

        public Validaciones()
        {
            InitializeComponent();
            errorProvider = new ErrorProvider();
            textBoxTelefono.Text = "09";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            errorProvider.Clear(); 
            StringBuilder errores = new StringBuilder();

            if (string.IsNullOrWhiteSpace(textBoxCedula.Text))
            {
                errores.AppendLine("Cédula: campo vacío (null).");
                errorProvider.SetError(textBoxCedula, "Debe ingresar una cédula.");
                textBoxCedula.Focus();
            }
            else if (!EsCedulaValida(textBoxCedula.Text))
            {
                errores.AppendLine($"Cédula inválida: {textBoxCedula.Text}");
                errorProvider.SetError(textBoxCedula, "Cédula incorrecta.");
                textBoxCedula.Focus();
            }

            if (string.IsNullOrWhiteSpace(textBoxCorreo.Text))
            {
                errores.AppendLine("Correo: campo vacío (null).");
                errorProvider.SetError(textBoxCorreo, "Debe ingresar un correo.");
                textBoxCorreo.Focus();
            }
            else if (!EsCorreoValido(textBoxCorreo.Text))
            {
                errores.AppendLine($"Correo inválido: {textBoxCorreo.Text}");
                errorProvider.SetError(textBoxCorreo, "Correo incorrecto.");
                textBoxCorreo.Focus();
            }

            if (string.IsNullOrWhiteSpace(textBoxTelefono.Text))
            {
                errores.AppendLine("Teléfono: campo vacío (null).");
                errorProvider.SetError(textBoxTelefono, "Debe ingresar un teléfono.");
                textBoxTelefono.Focus();
            }
            else if (!EsTelefonoValido(textBoxTelefono.Text))
            {
                errores.AppendLine($"Teléfono inválido: {textBoxTelefono.Text}. Debe tener 10 dígitos.");
                errorProvider.SetError(textBoxTelefono, "Formato incorrecto.");
                textBoxTelefono.Focus();
            }

            if (string.IsNullOrWhiteSpace(textBoxFecha.Text))
            {
                errores.AppendLine("Fecha: campo vacío (null).");
                errorProvider.SetError(textBoxFecha, "Debe ingresar una fecha.");
                textBoxFecha.Focus();
            }
            else if (!EsFechaValida(textBoxFecha.Text))
            {
                errores.AppendLine($"Fecha inválida: {textBoxFecha.Text}. Formato esperado: dd/MM/yyyy");
                errorProvider.SetError(textBoxFecha, "Formato inválido.");
                textBoxFecha.Focus();
            }

            // Validaciones opcionales para nombre/apellido
            if (string.IsNullOrWhiteSpace(textBoxNombre.Text))
            {
                errores.AppendLine("Nombre: campo vacío (null).");
                errorProvider.SetError(textBoxNombre, "Debe ingresar un nombre.");
                textBoxNombre.Focus();
            }

            if (string.IsNullOrWhiteSpace(textBoxApellido.Text))
            {
                errores.AppendLine("Apellido: campo vacío (null).");
                errorProvider.SetError(textBoxApellido, "Debe ingresar un apellido.");
                textBoxApellido.Focus();
            }

            // Mostrar errores si existen
            if (errores.Length > 0)
            {
                MessageBox.Show("Fallas encontradas:\n\n" + errores.ToString(), 
                    "Errores de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("¡Todos los datos son válidos!", "Validación Exitosa",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool EsTelefonoValido(string numero)
        {
            numero = numero.Trim();
            if (numero.Length != 10)
                return false;

            if (!numero.All(char.IsDigit))
                return false;

            if (!numero.StartsWith("09"))
                return false;

            return true;
        }


        public bool EsFechaValida(string fecha)
        {
            DateTime fechaConvertida;

            return DateTime.TryParseExact(
                fecha,
                "dd/MM/yyyy", 
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out fechaConvertida
            );
        }

        public bool EsCorreoValido(string correo)
        {
            string patron = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(correo, patron);
        }


        public static bool EsCedulaValida(string cedula)
        {
            if (cedula.Length != 10 || !cedula.All(char.IsDigit))
                return false;

            int digitoRegion = int.Parse(cedula.Substring(0, 2));
            if (digitoRegion < 1 || digitoRegion > 24)
                return false;

            int ultimoDigito = int.Parse(cedula[9].ToString());

            int sumaPares = int.Parse(cedula[1].ToString()) +
                            int.Parse(cedula[3].ToString()) +
                            int.Parse(cedula[5].ToString()) +
                            int.Parse(cedula[7].ToString());

            int sumaImpares = 0;
            for (int i = 0; i < 9; i += 2)
            {
                int num = int.Parse(cedula[i].ToString()) * 2;
                if (num > 9) num -= 9;
                sumaImpares += num;
            }

            int sumaTotal = sumaPares + sumaImpares;
            int digitoValidador = 10 - (sumaTotal % 10);
            if (digitoValidador == 10) digitoValidador = 0;

            return digitoValidador == ultimoDigito;
        }


        private void buttonLimpiar_Click(object sender, EventArgs e)
        {
            textBoxNombre.Clear();
            textBoxApellido.Clear();
            textBoxTelefono.Clear();
            textBoxFecha.Clear();
            textBoxCorreo.Clear();
            textBoxCedula.Clear();
            errorProvider.Clear();
            textBoxNombre.Focus();
            textBoxTelefono.Text = "09"; 
        }

       

        private void textBoxNombre_TextChanged(object sender, EventArgs e)
        {
            if (textBoxNombre != null && textBoxNombre.Text.Length > 0)
            {
                string original = textBoxNombre.Text;
                string capitalizado = CapitalizarPrimeraLetra(original);

                if (original != capitalizado)
                {
                    int cursorPos = textBoxNombre.SelectionStart;
                    textBoxNombre.Text = capitalizado;
                    textBoxNombre.SelectionStart = cursorPos;
                }
            }
        }

        private string CapitalizarPrimeraLetra(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return texto;

            texto = texto.TrimStart(); 
            texto = texto.ToLower(); 
            return char.ToUpper(texto[0]) + texto.Substring(1);
        }


        private void textBoxNombre_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && e.KeyChar != ' ')
            {
                e.Handled = true; 
            }
        }

        private void textBoxApellido_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && e.KeyChar != ' ')
            {
                e.Handled = true; 
            }
        }

        private void textBoxCedula_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
                return;
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }
            if (textBoxCedula.Text.Length >= 10 && textBoxCedula.SelectionLength == 0)
            {
                e.Handled = true;
            }
        }

        private void textBoxTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
                return;
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }
            if (textBoxTelefono.Text.Length >= 10 && textBoxTelefono.SelectionLength == 0)
            {
                e.Handled = true;
            }
        }

        private void textBoxApellido_TextChanged(object sender, EventArgs e)
        {
            if (textBoxApellido != null && textBoxApellido.Text.Length > 0)
            {
                string original = textBoxApellido.Text;
                string capitalizado = CapitalizarPrimeraLetra(original);

                if (original != capitalizado)
                {
                    int cursorPos = textBoxApellido.SelectionStart;
                    textBoxApellido.Text = capitalizado;
                    textBoxApellido.SelectionStart = cursorPos;
                }
            }
        }

        private void textBoxFecha_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
                return;

            if (char.IsDigit(e.KeyChar))
                return;

            if (e.KeyChar == '/')
                return;

            e.Handled = true;
        }
    }
}


