using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Operarios
{
    public static class Conexiones
    {
        private static readonly string cadenaConexion = @"server = DESKTOP-NSRMI7J; User ID=sa;Password=santi; database = Operarios";

        public static SqlConnection ObtenerConexion()
        {
            SqlConnection conexion = new SqlConnection(cadenaConexion);
            conexion.Open();

            return conexion;
        }
    }

    public class Consultas
    {
        public static bool EstadoConexion()
        {
            try
            {
                using (SqlConnection connection = Conexiones.ObtenerConexion())
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void LlenarComboBox(ComboBox comboBox)
        {
            using (SqlConnection connection = Conexiones.ObtenerConexion())
            {
                try
                {
                    string query = "SELECT Nombre FROM Rubro";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    SqlDataReader reader = cmd.ExecuteReader();

                    comboBox.Items.Clear();

                    while (reader.Read())
                    {
                        string item = reader["Nombre"].ToString();
                        comboBox.Items.Add(item);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al obtener los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void LlenarComboBoxProductos(ComboBox comboBox, string nombre)
        {
            using (SqlConnection connection = Conexiones.ObtenerConexion())
            {
                try
                {
                    string query = "SELECT p.Nombre as 'Nombre' FROM Productos P INNER JOIN Rubro R ON R.ID = P.idRubro WHERE r.Nombre = @rubro";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.Add("@rubro", SqlDbType.NVarChar).Value = nombre;

                    SqlDataReader reader = cmd.ExecuteReader();

                    comboBox.Items.Clear();

                    while (reader.Read())
                    {
                        string item = reader["Nombre"].ToString();
                        comboBox.Items.Add(item);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al obtener los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void LlenarComboBoxOperarios(ComboBox comboBox)
        {
            using (SqlConnection connection = Conexiones.ObtenerConexion())
            {
                try
                {
                    string query = "SELECT Nombre FROM Operario";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    SqlDataReader reader = cmd.ExecuteReader();

                    comboBox.Items.Clear();

                    while (reader.Read())
                    {
                        string item = reader["Nombre"].ToString();
                        comboBox.Items.Add(item);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al obtener los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void LlenarComboBoxMaquinas(ComboBox comboBox)
        {
            using (SqlConnection connection = Conexiones.ObtenerConexion())
            {
                try
                {
                    string query = "SELECT Nombre FROM Maquinas";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    SqlDataReader reader = cmd.ExecuteReader();

                    comboBox.Items.Clear();

                    while (reader.Read())
                    {
                        string item = reader["Nombre"].ToString();
                        comboBox.Items.Add(item);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al obtener los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void LlenarDataGridViewOperarios(DataGridView dataGridView, string nombre, DateTime fechaInicio, DateTime fechaFin)
        {
            string query = "SELECT OPERARIO, PRODUCTO, UNIDAD, CANTIDAD, KG, HORAS, KG/HORAS AS 'KG/HORA', SCRAP, MAQUINA, FECHA FROM Confeccionado WHERE Operario = @nombre AND FECHA BETWEEN @fechaInicio AND @fechaFin ORDER BY Fecha";
            SqlConnection conexion = Conexiones.ObtenerConexion();
            try
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@nombre", nombre);
                    comando.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    comando.Parameters.AddWithValue("@fechaFin", fechaFin);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(comando))
                    {
                        DataTable tabla = new DataTable();
                        adapter.Fill(tabla);
                        foreach (DataRow row in tabla.Rows)
                        {
                            if (!row.IsNull("KG/HORA"))
                            {
                                double kgHora = Convert.ToDouble(row["KG/HORA"]);
                                row["KG/HORA"] = kgHora.ToString("0.00");
                            }
                        }
                        dataGridView.DataSource = tabla;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void LlenarDataGridViewMaquinas(DataGridView dataGridView, string nombre, DateTime fechaInicio, DateTime fechaFin)
        {
            string query = "SELECT OPERARIO, PRODUCTO, UNIDAD, CANTIDAD, KG, HORAS, KG/HORAS AS 'KG/HORA', SCRAP, MAQUINA, FECHA from Confeccionado where Maquina = @maquina AND FECHA BETWEEN @fechaInicio AND @fechaFin ORDER BY Fecha";
            SqlConnection conexion = Conexiones.ObtenerConexion();
            try
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@maquina", nombre);
                    comando.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    comando.Parameters.AddWithValue("@fechaFin", fechaFin);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(comando))
                    {
                        DataTable tabla = new DataTable();
                        adapter.Fill(tabla);
                        foreach (DataRow row in tabla.Rows)
                        {
                            if (!row.IsNull("KG/HORA"))
                            {
                                double kgHora = Convert.ToDouble(row["KG/HORA"]);
                                row["KG/HORA"] = kgHora.ToString("0.00");
                            }
                        }
                        dataGridView.DataSource = tabla;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void LlenarDataGridViewProducto(DataGridView dataGridView, string nombre, DateTime fechaInicio, DateTime fechaFin)
        {
            string query = "SELECT OPERARIO, PRODUCTO, UNIDAD, CANTIDAD, KG, HORAS, KG/HORAS AS 'KG/HORA', SCRAP, MAQUINA, FECHA from Confeccionado where Producto = @producto AND FECHA BETWEEN @fechaInicio AND @fechaFin ORDER BY Fecha";
            SqlConnection conexion = Conexiones.ObtenerConexion();
            try
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@producto", nombre);
                    comando.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    comando.Parameters.AddWithValue("@fechaFin", fechaFin);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(comando))
                    {
                        DataTable tabla = new DataTable();
                        adapter.Fill(tabla);
                        foreach (DataRow row in tabla.Rows)
                        {
                            if (!row.IsNull("KG/HORA"))
                            {
                                double kgHora = Convert.ToDouble(row["KG/HORA"]);
                                row["KG/HORA"] = kgHora.ToString("0.00");
                            }
                        }
                        dataGridView.DataSource = tabla;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void LlenarGridOperYMaq(DataGridView dataGridView, string operario, string maquina, DateTime fechaInicio, DateTime fechaFin)
        {
            string query = "SELECT OPERARIO, PRODUCTO, UNIDAD, CANTIDAD, KG, HORAS, KG/HORAS AS 'KG/HORA', SCRAP, MAQUINA, FECHA from Confeccionado where Operario = @operario AND Maquina = @maquina AND FECHA BETWEEN @fechaInicio AND @fechaFin ORDER BY Fecha";
            SqlConnection conexion = Conexiones.ObtenerConexion();
            try
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@operario", operario);
                    comando.Parameters.AddWithValue("@maquina", maquina);
                    comando.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    comando.Parameters.AddWithValue("@fechaFin", fechaFin);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(comando))
                    {
                        DataTable tabla = new DataTable();
                        adapter.Fill(tabla);
                        foreach (DataRow row in tabla.Rows)
                        {
                            if (!row.IsNull("KG/HORA"))
                            {
                                double kgHora = Convert.ToDouble(row["KG/HORA"]);
                                row["KG/HORA"] = kgHora.ToString("0.00");
                            }
                        }
                        dataGridView.DataSource = tabla;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void LlenarDataGridProductividad(DataGridView dataGridView, string nombre, DateTime fechaInicio, DateTime fechaFin)
        {
            string query = "Select Operario, ROUND(SUM(Kg) / SUM(HORAS), 2) as Productividad From Confeccionado Where Maquina = @maquina AND FECHA BETWEEN @fechaInicio AND @fechaFin Group by Operario order by Productividad";
            SqlConnection conexion = Conexiones.ObtenerConexion();
            try
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@maquina", nombre);
                    comando.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    comando.Parameters.AddWithValue("@fechaFin", fechaFin);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(comando))
                    {
                        DataTable tabla = new DataTable();
                        adapter.Fill(tabla);
                        dataGridView.DataSource = tabla;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public double ObtenerPeso(string nombre)
        {
            double peso = 0;

            string query = "SELECT Peso FROM Productos WHERE Nombre = @nombre";

            using (SqlConnection connection = Conexiones.ObtenerConexion())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nombre", nombre);
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        peso = Convert.ToDouble(result);
                    }
                }
            }

            return peso;
        }

        public void EnviarProducto(string operario, string maquina, string producto, double cantidad, double kg, DateTime fecha, string unidad, double horas, double scrap, string observacion)
        {
            try
            {
                using (SqlConnection conexion = Conexiones.ObtenerConexion())
                {
                    string query = "INSERT INTO Confeccionado (Operario, Maquina, Producto, Cantidad, Unidad, Kg, Fecha, Horas, Scrap, Observaciones) VALUES (@operario, @maquina, @producto, @cantidad, @unidad, @kg, @fecha, @horas, @scrap, @observaciones)";
                    using (SqlCommand comando = new SqlCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@operario", operario);
                        comando.Parameters.AddWithValue("@maquina", maquina);
                        comando.Parameters.AddWithValue("@producto", producto);
                        comando.Parameters.AddWithValue("@cantidad", cantidad);
                        comando.Parameters.AddWithValue("@unidad", unidad);
                        comando.Parameters.AddWithValue("@kg", kg);
                        comando.Parameters.AddWithValue("@fecha", fecha);
                        comando.Parameters.AddWithValue("@horas", horas);
                        comando.Parameters.AddWithValue("@scrap", scrap);
                        comando.Parameters.AddWithValue("@observaciones", observacion);
                        comando.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo envíar a la base de datos, consulte con un administrador. Código de error: " + ex);
            }            
        }
    }
}
