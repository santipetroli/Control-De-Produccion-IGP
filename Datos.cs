using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using OfficeOpenXml;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Drawing.Printing;
using System.Xml.Linq;


namespace Operarios
{
    public partial class Datos : Form
    {
        Consultas consultas = new Consultas();
        public Datos()
        {
            InitializeComponent();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AjustarGrid()
        {
            if (rbMaquina.Checked || rbOperario.Checked || rbOperYMaq.Checked || rbProducto.Checked)
            {
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            else
            {
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void rbOperario_CheckedChanged(object sender, EventArgs e)
        {
            string fechainicio = dateTimePicker1.Value.ToShortDateString();
            string fechafin = dateTimePicker2.Value.ToShortDateString();
            if (rbOperario.Checked)
            {
                lblGeneral.Text = "Operario:";
                consultas.LlenarComboBoxOperarios(cbGeneral);
                cbGeneral.SelectedIndex = 0;
                consultas.LlenarDataGridViewOperarios(dataGridView1, cbGeneral.Text, DateTime.Parse(fechainicio), DateTime.Parse(fechafin));
                AjustarGrid();
            }
        }

        private void rbMaquina_CheckedChanged(object sender, EventArgs e)
        {
            string fechainicio = dateTimePicker1.Value.ToShortDateString();
            string fechafin = dateTimePicker2.Value.ToShortDateString();
            if (rbMaquina.Checked)
            {
                lblGeneral.Text = "Máquina:";
                consultas.LlenarComboBoxMaquinas(cbGeneral);
                cbGeneral.SelectedIndex = 0;
                consultas.LlenarDataGridViewMaquinas(dataGridView1, cbGeneral.Text, DateTime.Parse(fechainicio), DateTime.Parse(fechafin));
                AjustarGrid();
            }
        }

        private void rbProducto_CheckedChanged(object sender, EventArgs e)
        {
            string fechainicio = dateTimePicker1.Value.ToShortDateString();
            string fechafin = dateTimePicker2.Value.ToShortDateString();
            if (rbProducto.Checked)
            {
                cbProducto.Visible = true;
                lblGeneral.Text = "Rubro:";
                label2.Visible = true;
                consultas.LlenarComboBox(cbGeneral);
                cbGeneral.SelectedIndex = 0;
                cbProducto.SelectedIndex = 0;
                consultas.LlenarDataGridViewProducto(dataGridView1, cbProducto.Text, DateTime.Parse(fechainicio), DateTime.Parse(fechafin));
                AjustarGrid();
            }
            else
            {
                cbProducto.Visible = false;
                label2.Visible = false;
            }
        }

        private void cbGeneral_SelectedIndexChanged(object sender, EventArgs e)
        {
            string fechainicio = dateTimePicker1.Value.ToShortDateString();
            string fechafin = dateTimePicker2.Value.ToShortDateString();
            if (rbProducto.Checked)
            {
                string nombre = cbGeneral.Text;
                consultas.LlenarComboBoxProductos(cbProducto, nombre);
            }
            else if (rbOperario.Checked)
            {
                consultas.LlenarDataGridViewOperarios(dataGridView1, cbGeneral.Text, DateTime.Parse(fechainicio), DateTime.Parse(fechafin));
                AjustarGrid();
            }
            else if (rbMaquina.Checked)
            {
                consultas.LlenarDataGridViewMaquinas(dataGridView1, cbGeneral.Text, DateTime.Parse(fechainicio), DateTime.Parse(fechafin));
                AjustarGrid();
            }
            else if (rbOperYMaq.Checked)
            {
                consultas.LlenarGridOperYMaq(dataGridView1, cbGeneral.Text, cbProducto.Text, DateTime.Parse(fechainicio), DateTime.Parse(fechafin));
                AjustarGrid();
            }
            else if (btnProductividad.Checked)
            {
                consultas.LlenarDataGridProductividad(dataGridView1, cbGeneral.Text, DateTime.Parse(fechainicio), DateTime.Parse(fechafin));
                AjustarGrid();
            }
        }

        private void cbProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            string fechainicio = dateTimePicker1.Value.ToShortDateString();
            string fechafin = dateTimePicker2.Value.ToShortDateString();
            if (rbProducto.Checked)
            {
                consultas.LlenarDataGridViewProducto(dataGridView1, cbProducto.Text, DateTime.Parse(fechainicio), DateTime.Parse(fechafin));
                AjustarGrid();
            }
            else if (rbOperYMaq.Checked)
            {
                consultas.LlenarGridOperYMaq(dataGridView1, cbGeneral.Text, cbProducto.Text, DateTime.Parse(fechainicio), DateTime.Parse(fechafin));
                AjustarGrid();
            }
        }

        private void ExportToExcel(DataGridView dataGridView)
        {
            OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Archivo Excel|*.xlsx";
            saveFileDialog.Title = "Guardar archivo Excel";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileInfo excelFile = new FileInfo(saveFileDialog.FileName);

                using (ExcelPackage package = new ExcelPackage(excelFile))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.SingleOrDefault(x => x.Name == "Hoja1");

                    if (worksheet == null)
                    {
                        worksheet = package.Workbook.Worksheets.Add("Hoja1");
                    }
                    else
                    {
                        worksheet.Cells.Clear();
                    }

                    using (var headerRange = worksheet.Cells[1, 1, 1, dataGridView.Columns.Count])
                    {
                        headerRange.Style.Font.Bold = true;
                        headerRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    }

                    for (int col = 1; col <= dataGridView.Columns.Count; col++)
                    {
                        worksheet.Cells[1, col].Value = dataGridView.Columns[col - 1].HeaderText;
                        worksheet.Column(col).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    }

                    for (int row = 0; row < dataGridView.Rows.Count; row++)
                    {
                        for (int col = 0; col < dataGridView.Columns.Count; col++)
                        {
                            var cellValue = dataGridView.Rows[row].Cells[col].Value;

                            if (col == 9 && cellValue != null)
                            {
                                DateTime dateValue;
                                if (DateTime.TryParse(cellValue.ToString(), out dateValue))
                                {
                                    worksheet.Cells[row + 2, col + 1].Value = dateValue;
                                    worksheet.Cells[row + 2, col + 1].Style.Numberformat.Format = "dd/mm/yyyy";
                                }
                                else
                                {
                                    worksheet.Cells[row + 2, col + 1].Value = cellValue;
                                }
                            }
                            else
                            {
                                worksheet.Cells[row + 2, col + 1].Value = cellValue;
                            }
                        }
                    }

                    worksheet.Cells.AutoFitColumns();

                    package.Save();
                }

                MessageBox.Show("Datos exportados correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ExportToPdf(DataGridView dataGridView)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Archivo PDF|*.pdf";
            saveFileDialog.Title = "Guardar archivo PDF";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);

                    pdfDoc.Open();

                    PdfPTable pdfTable = new PdfPTable(dataGridView.Columns.Count);
                    pdfTable.DefaultCell.Padding = 3;
                    pdfTable.WidthPercentage = 100;
                    pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

                    float[] columnWidths = new float[dataGridView.Columns.Count];

                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        int colIndex = column.Index;
                        float headerWidth = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10).GetCalculatedBaseFont(true).GetWidthPoint(column.HeaderText, 10);
                        columnWidths[colIndex] = headerWidth;

                        foreach (DataGridViewRow row in dataGridView.Rows)
                        {
                            DataGridViewCell cell = row.Cells[colIndex];
                            if (cell.Value != null)
                            {
                                float cellWidth = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10).GetCalculatedBaseFont(true).GetWidthPoint(cell.Value.ToString(), 10);
                                if (cellWidth > columnWidths[colIndex])
                                {
                                    columnWidths[colIndex] = cellWidth;
                                }
                            }
                        }
                    }

                    pdfTable.SetWidths(columnWidths);

                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        PdfPCell headerCell = new PdfPCell(new Phrase(column.HeaderText, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10)));
                        pdfTable.AddCell(headerCell);
                    }

                    foreach (DataGridViewRow row in dataGridView.Rows)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            PdfPCell pdfCell;
                            if (cell.Value is DateTime dateTimeValue)
                            {
                                string formattedDate = dateTimeValue.ToString("dd/MM/yyyy");
                                pdfCell = new PdfPCell(new Phrase(formattedDate, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10)));
                            }
                            else
                            {
                                pdfCell = new PdfPCell(new Phrase(cell.Value?.ToString(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10)));
                            }
                            pdfTable.AddCell(pdfCell);
                        }
                    }

                    pdfDoc.Add(pdfTable);
                    pdfDoc.Close();
                }

                MessageBox.Show("Datos exportados correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            string fechainicio = dateTimePicker1.Value.ToShortDateString();
            string fechafin = dateTimePicker2.Value.ToShortDateString();
            if (rbProducto.Checked)
            {
                consultas.LlenarDataGridViewProducto(dataGridView1, cbProducto.Text, DateTime.Parse(fechainicio), DateTime.Parse(fechafin));
                AjustarGrid();
            }
            else if (rbOperario.Checked)
            {
                consultas.LlenarDataGridViewOperarios(dataGridView1, cbGeneral.Text, DateTime.Parse(fechainicio), DateTime.Parse(fechafin));
                AjustarGrid();
            }
            else if (rbMaquina.Checked)
            {
                consultas.LlenarDataGridViewMaquinas(dataGridView1, cbGeneral.Text, DateTime.Parse(fechainicio), DateTime.Parse(fechafin));
                AjustarGrid();
            }
            else if (btnProductividad.Checked)
            {
                consultas.LlenarDataGridProductividad(dataGridView1, cbGeneral.Text, DateTime.Parse(fechainicio), DateTime.Parse(fechafin));
                AjustarGrid();
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            string fechainicio = dateTimePicker1.Value.ToShortDateString();
            string fechafin = dateTimePicker2.Value.ToShortDateString();
            if (rbProducto.Checked)
            {
                consultas.LlenarDataGridViewProducto(dataGridView1, cbProducto.Text, DateTime.Parse(fechainicio), DateTime.Parse(fechafin));
                AjustarGrid();
            }
            else if (rbOperario.Checked)
            {
                consultas.LlenarDataGridViewOperarios(dataGridView1, cbGeneral.Text, DateTime.Parse(fechainicio), DateTime.Parse(fechafin));
                AjustarGrid();
            }
            else if (rbMaquina.Checked)
            {
                consultas.LlenarDataGridViewMaquinas(dataGridView1, cbGeneral.Text, DateTime.Parse(fechainicio), DateTime.Parse(fechafin));
                AjustarGrid();
            }
            else if (btnProductividad.Checked)
            {
                consultas.LlenarDataGridProductividad(dataGridView1, cbGeneral.Text, DateTime.Parse(fechainicio), DateTime.Parse(fechafin));
                AjustarGrid();
            }
        }

        private void rbOperYMaq_CheckedChanged(object sender, EventArgs e)
        {
            string fechainicio = dateTimePicker1.Value.ToShortDateString();
            string fechafin = dateTimePicker2.Value.ToShortDateString();
            if (rbOperYMaq.Checked)
            {
                cbProducto.Visible = true;
                lblGeneral.Text = "Operario:";
                label2.Text = "Máquina:";
                label2.Visible = true;
                consultas.LlenarComboBoxOperarios(cbGeneral);
                consultas.LlenarComboBoxMaquinas(cbProducto);
                cbGeneral.SelectedIndex = 0;
                cbProducto.SelectedIndex = 0;
                consultas.LlenarGridOperYMaq(dataGridView1, cbGeneral.Text, cbProducto.Text, DateTime.Parse(fechainicio), DateTime.Parse(fechafin));
                AjustarGrid();
            }
            else
            {
                cbProducto.Visible = false;
                label2.Visible = false;
                label2.Text = "Producto:";
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnExportarPDF_Click(object sender, EventArgs e)
        {
            ExportToPdf(dataGridView1);
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            ExportToExcel(dataGridView1);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            double suma = 0;

            foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
            {
                if (cell.ColumnIndex >= 3 && cell.ColumnIndex <= 6)
                {
                    if (double.TryParse(cell.Value.ToString(), out double valor))
                    {
                        suma += valor;
                        lblSuma.Text = "SUMA: " + suma.ToString();
                    }
                }
                else
                {
                    lblSuma.Text = "";
                }
            }

        }

        private void btnProductividad_CheckedChanged(object sender, EventArgs e)
        {
            string fechainicio = dateTimePicker1.Value.ToShortDateString();
            string fechafin = dateTimePicker2.Value.ToShortDateString();
            if (btnProductividad.Checked)
            {
                lblGeneral.Text = "Máquina:";
                consultas.LlenarComboBoxMaquinas(cbGeneral);
                cbGeneral.SelectedIndex = 0;
                consultas.LlenarDataGridProductividad(dataGridView1, cbGeneral.Text, DateTime.Parse(fechainicio), DateTime.Parse(fechafin));
                AjustarGrid();
            }
        }
    }
}
