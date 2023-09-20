using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Operarios
{
    public partial class Inicio : Form
    {
        Consultas consultas = new Consultas();
        public Inicio()
        {
            InitializeComponent();
            consultas.LlenarComboBoxOperarios(cbOperario);
            consultas.LlenarComboBoxMaquinas(cbMaquina);
            InitializeComboBoxes();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void InitializeComboBoxes()
        {
            for (int i = 1; i <= 6; i++)
            {
                ComboBox currentComboBox = (ComboBox)Controls.Find($"cbRubro{i}", true).FirstOrDefault();
                if (currentComboBox != null)
                {
                    consultas.LlenarComboBox(currentComboBox);
                }
            }
        }

       /* private void UpdateControlVisibility()
        {
            // Obtén el valor de numericUpDown1
            int visibleCount = (int)numericUpDown1.Value;

            // Obtén la máquina seleccionada
            string maquinaSeleccionada = cbMaquina.SelectedItem?.ToString();

            // Lista de los ComboBox de rubro
            ComboBox[] cbRubros = { cbRubro1, cbRubro2, cbRubro3, cbRubro4, cbRubro5, cbRubro6 };

            // Lista de los conjuntos de controles relacionados
            List<Control[]> controlSets = new List<Control[]>
            {
                new Control[] { cbRubro1, cbProducto1, txtCantidad1, numHoras1, txtScrap1 },
                new Control[] { cbRubro2, cbProducto2, txtCantidad2, numHoras2, txtScrap2 },
                new Control[] { cbRubro3, cbProducto3, txtCantidad3, numHoras3, txtScrap3 },
                new Control[] { cbRubro4, cbProducto4, txtCantidad4, numHoras4, txtScrap4 },
                new Control[] { cbRubro5, cbProducto5, txtCantidad5, numHoras5, txtScrap5 },
                new Control[] { cbRubro6, cbProducto6, txtCantidad6, numHoras6, txtScrap6 }
            };

            foreach (var cb in cbRubros)
            {
                cb.Items.Clear();
            }

            List<string> opcionesRubro = new List<string>();

            if (maquinaSeleccionada == "Bobinera")
            {
                opcionesRubro.Add("Bobina de Arranque");
            }
            else if (maquinaSeleccionada == "Camisetera")
            {
                opcionesRubro.Add("Camiseta");
            }
            else if (maquinaSeleccionada == "Coreana")
            {
                opcionesRubro.Add("Block");
            }
            else if (maquinaSeleccionada == "Hde")
            {
                opcionesRubro.Add("Residuo");
                opcionesRubro.Add("Consorcio");
            }
            else if (maquinaSeleccionada == "Taiwanesa")
            {
                opcionesRubro.Add("Super Consorcio");
                opcionesRubro.Add("Camiseta");
                opcionesRubro.Add("Laminas");
                opcionesRubro.Add("Bolsas sueltas");
            }

            foreach (var cb in cbRubros)
            {
                foreach (var opcion in opcionesRubro)
                {
                    cb.Items.Add(opcion);
                }
            }

            for (int i = 0; i < controlSets.Count; i++)
            {
                bool isVisible = i < visibleCount;
                foreach (var control in controlSets[i])
                {
                    control.Visible = isVisible;
                }
            }
        }*/

        private void ConfigureControls(int visibleCount)
        {
            ComboBox[] cbRubros = { cbRubro1, cbRubro2, cbRubro3, cbRubro4, cbRubro5, cbRubro6 };
            ComboBox[] cbProductos = { cbProducto1, cbProducto2, cbProducto3, cbProducto4, cbProducto5, cbProducto6 };
            Label[] lblProductos = { lblProducto1, lblProducto2, lblProducto3, lblProducto4, lblProducto5, lblProducto6 };
            Label[] lblCantidades = { lblCantidad1, lblCantidad2, lblCantidad3, lblCantidad4, lblCantidad5, lblCantidad6 };
            Label[] lblHoras = { lblHoras1, lblHoras2, lblHoras3, lblHoras4, lblHoras5, lblHoras6 };
            Label[] lblScrap = { lblScrap1, lblScrap2, lblScrap3, lblScrap4, lblScrap5, lblScrap6 };
            TextBox[] txtCantidades = { txtCantidad1, txtCantidad2, txtCantidad3, txtCantidad4, txtCantidad5, txtCantidad6 };
            TextBox[] txtScrap = { txtScrap1, txtScrap2, txtScrap3, txtScrap4, txtScrap5, txtScrap6 };
            NumericUpDown[] numHoras = { numHoras1, numHoras2, numHoras3, numHoras4, numHoras5, numHoras6 };

            for (int i = 0; i < 6; i++)
            {
                bool isVisible = i < visibleCount;

                cbRubros[i].Visible = isVisible;
                cbProductos[i].Visible = isVisible;
                lblProductos[i].Visible = isVisible;
                lblCantidades[i].Visible = isVisible;
                lblScrap[i].Visible = isVisible;
                lblHoras[i].Visible = isVisible;
                txtScrap[i].Visible = isVisible;
                txtCantidades[i].Visible = isVisible;
                numHoras[i].Visible = isVisible;

                if (!isVisible)
                {
                    cbRubros[i].SelectedIndex = -1;
                    cbProductos[i].SelectedIndex = -1;
                    txtScrap[i].Text = "";
                    txtCantidades[i].Text = "";
                    numHoras[i].Value = 0;
                    lblCantidades[i].Text = "Cantidad:";
                }
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int visibleCount = (int)numericUpDown1.Value;

            ConfigureControls(visibleCount);

            ComboBox selectedCbRubro = (ComboBox)Controls.Find($"cbRubro{visibleCount + 1}", true).FirstOrDefault();
            if (selectedCbRubro != null)
                consultas.LlenarComboBox(selectedCbRubro);
        }

        private void RubroComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            ComboBox currentComboBox = sender as ComboBox;

            if (currentComboBox == null)
                return;

            Label lblCantidad = null;
            TextBox txtCantidad = null;
            ComboBox cbProducto = null;

            switch (currentComboBox.Name)
            {
                case "cbRubro1":
                    lblCantidad = lblCantidad1;
                    txtCantidad = txtCantidad1;
                    cbProducto = cbProducto1;
                    break;
                case "cbRubro2":
                    lblCantidad = lblCantidad2;
                    txtCantidad = txtCantidad2;
                    cbProducto = cbProducto2;
                    break;
                case "cbRubro3":
                    lblCantidad = lblCantidad3;
                    txtCantidad = txtCantidad3;
                    cbProducto = cbProducto3;
                    break;
                case "cbRubro4":
                    lblCantidad = lblCantidad4;
                    txtCantidad = txtCantidad4;
                    cbProducto = cbProducto4;
                    break;
                case "cbRubro5":
                    lblCantidad = lblCantidad5;
                    txtCantidad = txtCantidad5;
                    cbProducto = cbProducto5;
                    break;
                case "cbRubro6":
                    lblCantidad = lblCantidad6;
                    txtCantidad = txtCantidad6;
                    cbProducto = cbProducto6;
                    break;
            }

            if (currentComboBox.Text == "")
            {
                lblCantidad.Text = "Cantidad";
                txtCantidad.Enabled = false;
            }
            else
            {
                string nombre = currentComboBox.Text;
                consultas.LlenarComboBoxProductos(cbProducto, nombre);
                txtCantidad.Enabled = true;

                switch (currentComboBox.Text)
                {
                    case "Bobina de Arranque":
                        lblCantidad.Text = "Unidades:";
                        break;
                    case "Camiseta":
                        lblCantidad.Text = "Paquetes:";
                        break;
                    case "Block":
                        lblCantidad.Text = "Millares:";
                        break;
                    case "Consorcio":
                    case "Residuo":
                        lblCantidad.Text = "Paquetes:";
                        break;
                    case "Super consorcio":
                        lblCantidad.Text = "Bolsas:";
                        break;
                    case "Lamina":
                        lblCantidad.Text = "Unidades:";
                        break;
                    case "Bolsas sueltas":
                        lblCantidad.Text = "Bolsas:";
                        break;
                }
                switch (currentComboBox.Name) // desplegar cbproductos
                {
                    case "cbRubro1":
                        cbProducto1.DroppedDown = true;
                        break;
                    case "cbRubro2":
                        cbProducto2.DroppedDown = true;
                        break;
                    case "cbRubro3":
                        cbProducto3.DroppedDown = true;
                        break;
                    case "cbRubro4":
                        cbProducto4.DroppedDown = true;
                        break;
                    case "cbRubro5":
                        cbProducto5.DroppedDown = true;
                        break;
                    case "cbRubro6":
                        cbProducto6.DroppedDown = true;
                        break;
                }
            }
        }
   
        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            TextBox targetTextBox = null;
            Label lblCantidad = null;

            switch (comboBox.Name)
            {
                case "cbProducto1":
                    targetTextBox = txtCantidad1;
                    lblCantidad = lblCantidad1;
                    break;
                case "cbProducto2":
                    targetTextBox = txtCantidad2;
                    lblCantidad = lblCantidad2;
                    break;
                case "cbProducto3":
                    targetTextBox = txtCantidad3;
                    lblCantidad = lblCantidad3;
                    break;
                case "cbProducto4":
                    targetTextBox = txtCantidad4;
                    lblCantidad = lblCantidad4;
                    break;
                case "cbProducto5":
                    targetTextBox = txtCantidad5;
                    lblCantidad = lblCantidad5;
                    break;
                case "cbProducto6":
                    targetTextBox = txtCantidad6;
                    lblCantidad = lblCantidad6;
                    break;
                default:
                        break;
            }
            if (comboBox.Text == "KMZ 60X80")
            {
                lblCantidad.Text = "Bolsas:";
            }
                targetTextBox?.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cbOperario.SelectedIndex != -1 && cbMaquina.SelectedIndex != -1)
            {
                int cantidadProductos = (int)numericUpDown1.Value;

                if (cantidadProductos >= 1 && cantidadProductos <= 6)
                {
                    List<ComboBox> comboBoxes = new List<ComboBox> { cbProducto1, cbProducto2, cbProducto3, cbProducto4, cbProducto5, cbProducto6 };
                    List<TextBox> textBoxes = new List<TextBox> { txtCantidad1, txtCantidad2, txtCantidad3, txtCantidad4, txtCantidad5, txtCantidad6 };
                    List<Label> labels = new List<Label> { lblCantidad1, lblCantidad2, lblCantidad3, lblCantidad4, lblCantidad5, lblCantidad6 };
                    List<NumericUpDown> numerics = new List<NumericUpDown> { numHoras1, numHoras2, numHoras3, numHoras4, numHoras5, numHoras6 };
                    List<TextBox> textBoxScrap = new List<TextBox> { txtScrap1, txtScrap2, txtScrap3, txtScrap4, txtScrap5, txtScrap6 };

                    bool camposValidos = true;

                    for (int i = 0; i < cantidadProductos; i++)
                    {
                        if (comboBoxes[i].SelectedIndex == -1 || string.IsNullOrEmpty(textBoxes[i].Text))
                        {
                            camposValidos = false;
                            MessageBox.Show($"Asegúrate de ingresar la cantidad del PRODUCTO {i + 1}.");
                            break;
                        }

                        if (string.IsNullOrEmpty(textBoxScrap[i].Text))
                        {
                            textBoxScrap[i].Text = "0";
                        }

                        if (numerics[i].Value == 0)
                        {
                            camposValidos = false;
                            MessageBox.Show($"Asegúrate de ingresar las horas trabajadas en el PRODUCTO {i + 1}.");
                            break;
                        }
                    }

                    if (camposValidos)
                    {
                        for (int i = 0; i < cantidadProductos; i++)
                        {
                            string producto = comboBoxes[i].Text;
                            double cantidad = double.Parse(textBoxes[i].Text);
                            string unidad = labels[i].Text;
                            double horas = double.Parse(numerics[i].Text);
                            double scrap = double.Parse(textBoxScrap[i].Text);

                            double pesoProducto = consultas.ObtenerPeso(producto);
                            double kg = pesoProducto * cantidad;

                            string observaciones = "";

                            if (!string.IsNullOrWhiteSpace(txtObservacion.Text) && txtObservacion.Text != "Añade una observación...")
                            {
                                observaciones = txtObservacion.Text;
                            }

                            consultas.EnviarProducto(cbOperario.Text, cbMaquina.Text, producto, cantidad, kg, dateTimePicker1.Value, unidad, horas, scrap, observaciones);
                        }

                        MessageBox.Show("Enviado.");
                        foreach (ComboBox comboBox in comboBoxes)
                        {
                            comboBox.SelectedIndex = -1;
                        }

                        cbMaquina.SelectedIndex = -1;
                        numericUpDown1.Value = 0;
                        cbOperario.SelectedIndex = -1;
                        cbOperario.DroppedDown = true;
                        dateTimePicker1.Value = DateTime.Today;
                        txtObservacion.Text = "Añade una observación...";
                    }
                }
                else
                {
                    MessageBox.Show("Debe seleccionar al menos 1 producto.");
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar operario y máquina.");
            }
        }

        private void btnDatos_Click(object sender, EventArgs e)
        {
            Datos datos = new Datos();
            this.Hide();
            datos.ShowDialog();
            this.Show();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void txtObservacion_Click(object sender, EventArgs e)
        {
            if (txtObservacion.Text == "Añade una observación...")
            {
                txtObservacion.Text = "";
            }
        }

        private void txtObservacion_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtObservacion.Text))
            {
                txtObservacion.Text = "Añade una observación...";
            }
        }

        private void cbOperario_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbMaquina.DroppedDown = true;
        }
    }
}
