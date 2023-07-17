using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CargaDiasFeriados
{
    public partial class Form1 : Form
    {

        List<Fecha> meses;
        List<Fecha> años;
        List<DIAS_FERIADOS> dias_feriados;

        DateTime fecha_selected;
        byte pais_seleted;
        bool click_cmbAño = false, click_cmbMes = false;

        int cmbYearSelect, cmbMesSelect;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                meses = new List<Fecha>();
                años = new List<Fecha>();
                dias_feriados = new List<DIAS_FERIADOS>();

                LlenarMeses();
                LlenarAños();

                SeleccionarFecha(DateTime.Now);
                cmbAño.SelectedValue = DateTime.Now.Year;
                cmbMes.SelectedValue = DateTime.Now.Month;
                SeleccionarCalendario();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error en Load", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }          
        }

        private void SeleccionarFecha(DateTime fecha)
        {
            dtpMesFestivo.SelectionStart = fecha;
            dtpMesFestivo.SelectionEnd = fecha;
        }

        private void LlenarAños()
        {
            DateTime primer_fecha = new DateTime(DateTime.Now.Year, 1, 1);

            for (int i = 1; i <= 100; i++)
            {
                años.Add(new Fecha { Nombre = primer_fecha.ToString("yyyy"), Numero = primer_fecha.Year });
                primer_fecha = primer_fecha.AddYears(1);
            }

            cmbAño.DataSource = años;
            cmbAño.ValueMember = "Numero";
            cmbAño.DisplayMember = "Nombre";
        }

        private void LlenarMeses()
        {
            DateTime primer_fecha = new DateTime(DateTime.Now.Year, 1, 1);

            for (int i = 1; i <= 12; i++)
            {
                meses.Add(new Fecha { Nombre = primer_fecha.ToString("MMMM"), Numero = primer_fecha.Month });
                primer_fecha = primer_fecha.AddMonths(1);
            }

            cmbMes.DataSource = meses;
            cmbMes.ValueMember = "Numero";
            cmbMes.DisplayMember = "Nombre";
        }

        private class Fecha
        {
            public int Numero { get; set; }
            public string Nombre { get; set; }
        }

        private void cmbMes_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarDiasMes();
            SeleccionarCalendario();
        }

        private void CargarDiasMes()
        {
            if (click_cmbMes)
            {
                if (cmbMes.SelectedValue != null)
                {
                    DateTime fecha_inicio = new DateTime((int)cmbAño.SelectedValue, (int)cmbMes.SelectedValue, 1);
                    DateTime fecha_fin = new DateTime((int)cmbAño.SelectedValue, (int)cmbMes.SelectedValue, UltimoDiaMes(fecha_inicio));

                    using (CATALOGOSEntities context = new CATALOGOSEntities())
                    {
                        List<DIAS_FERIADOS> lista_dias = context.DIAS_FERIADOS.Where(w => w.fecha >= fecha_inicio && w.fecha <= fecha_fin).ToList();

                        if (lista_dias.Count() == 0)
                        {
                            DialogResult dialogResult = MessageBox.Show("No hay dias cargados para este Mes, ¿Desea cargar todos los fines de semana de este mes?", "Sin Dias Feriados", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                CargarFinesFecha(fecha_inicio.Year, fecha_inicio.Month);
                            }
                        }
                    }
                }
            }
        }

        private void cmbAño_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CargarDiasAño();
                SeleccionarCalendario();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error en Combo Box", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void CargarDiasAño()
        {
            if (click_cmbAño)
            {
                if (cmbAño.SelectedValue != null)
                {
                    DateTime fecha_inicio = new DateTime((int)cmbAño.SelectedValue, 1, 1);
                    DateTime fecha_fin = new DateTime((int)cmbAño.SelectedValue, 12, 31);

                    using (CATALOGOSEntities context = new CATALOGOSEntities())
                    {
                        List<DIAS_FERIADOS> lista_dias = context.DIAS_FERIADOS.Where(w => w.fecha >= fecha_inicio && w.fecha <= fecha_fin).ToList();

                        if(lista_dias.Count() == 0)
                        {
                            DialogResult dialogResult = MessageBox.Show("No hay dias cargados para este año, ¿Desea cargar todos los fines de semana de este año?", "Sin Dias Feriados", MessageBoxButtons.YesNo);
                            if(dialogResult == DialogResult.Yes)
                            {
                                CargarFinesFecha(fecha_inicio.Year);
                            }
                        }
                    }
                }
            }
        }

        private void CargarFinesFecha(int year, int mes = 0)
        {

            this.cmbYearSelect = year;
            this.cmbMesSelect = mes;

            if (!bgwCargaFines.IsBusy)
            {
                bgwCargaFines.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("Ya hay una tarea ejecutandose. Favor de Esperar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }           
        }

        private void SeleccionarCalendario()
        {
            try
            {
                if (cmbAño.SelectedValue != null && cmbMes.SelectedValue != null)
                {
                    int año = ((Fecha)cmbAño.SelectedItem).Numero;
                    int mes = ((Fecha)cmbMes.SelectedItem).Numero;


                    SeleccionarFecha(new DateTime(año, mes, 1));

                    dias_feriados = ConsultaFeriadosPorMes(new DateTime(año, mes, 1));

                    DataTable dt = LlenarListFeriados(dias_feriados);
                    dtGrdVwFeriados.DataSource = dt;
                    dtGrdVwFeriados.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dtGrdVwFeriados.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dtGrdVwFeriados.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error en Calendario", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private DataTable LlenarListFeriados(List<DIAS_FERIADOS> dias_feriados)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Fecha");
            dt.Columns.Add("Tipo");

            foreach (DIAS_FERIADOS dia in dias_feriados)
            {
                DataRow dr = dt.NewRow();
                dr["Fecha"] = dia.fecha.ToString("dd-MM-yyyy");

                if(dia.tipo_dia_feriado == 1)
                {
                    dr["Tipo"] = "Mexico";
                }
                else if (dia.tipo_dia_feriado == 2)
                {
                    dr["Tipo"] = "Estados Unidos";
                }
                else if (dia.tipo_dia_feriado == 3)
                {
                    dr["Tipo"] = "Ambos";
                }

                dt.Rows.Add(dr);

            }

            return dt;
        }

        private List<DIAS_FERIADOS> ConsultaFeriadosPorMes(DateTime dateTime)
        {
            using (CATALOGOSEntities context = new CATALOGOSEntities())
            {
                DateTime fecha1 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
                DateTime fecha2 = new DateTime(dateTime.Year, dateTime.Month, UltimoDiaMes(dateTime));

                List < DIAS_FERIADOS > resultado =
                    context.DIAS_FERIADOS
                    .Where(w =>
                        w.fecha >= fecha1
                        &&
                        w.fecha <= fecha2
                    )
                    .OrderBy(o => o.fecha)
                    .ToList();

                return resultado;
                
            }
        }

        private int UltimoDiaMes(DateTime dateTime)
        {
            int dia = 0;
            DateTime fecha_sum = dateTime;

            do
            {
                fecha_sum = fecha_sum.AddDays(1);
                dia++;

            } while (fecha_sum.Month == dateTime.Month);

            return dia;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            SqlConnection cnn;
            try
            {
                if(btnGuardar.Text == "Guardar")
                {
                    DIAS_FERIADOS dia = new DIAS_FERIADOS();

                    dia.tipo_dia_feriado = ObtenerPais();                
                    dia.fecha = dtpMesFestivo.SelectionStart;

                    if (ConsultarFecha(dia))
                    {
                        MessageBox.Show("Esta fecha ya esta dada de alta", "Fecha Duplicada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {

                        using (CATALOGOSEntities context = new CATALOGOSEntities())
                        {
                           cnn = (SqlConnection)context.Database.Connection;

                            cnn.Open();
                            SqlCommand cmd = cnn.CreateCommand();
                            cmd.CommandText = "INSERT INTO DIAS_FERIADOS(fecha, tipo_dia_feriado) VALUES(@param1, @param2)";
                            cmd.Parameters.AddWithValue("@param1", dia.fecha);
                            cmd.Parameters.AddWithValue("@param2", dia.tipo_dia_feriado);
                            int afectados = cmd.ExecuteNonQuery();
                            cnn.Close();
                            if (afectados > 0)
                            {
                                SeleccionarCalendario();
                            }

                        }
                    }
                }
                else if(btnGuardar.Text == "Actualizar")
                {
                    using (CATALOGOSEntities context = new CATALOGOSEntities())
                    {

                        DIAS_FERIADOS dia = context.DIAS_FERIADOS.Where(w => w.fecha == fecha_selected && w.tipo_dia_feriado == pais_seleted).FirstOrDefault();

                        dia.tipo_dia_feriado = ObtenerPais();

                        cnn = (SqlConnection)context.Database.Connection;

                        cnn.Open();
                        SqlCommand cmd = cnn.CreateCommand();
                        cmd.CommandText = "UPDATE DIAS_FERIADOS SET tipo_dia_feriado=@param1 WHERE fecha=@param2";
                        cmd.Parameters.AddWithValue("@param1", dia.tipo_dia_feriado);
                        cmd.Parameters.AddWithValue("@param2", dia.fecha);
                        int afectados = cmd.ExecuteNonQuery();
                        cnn.Close();

                        if (afectados > 0)
                        {
                            SeleccionarCalendario();
                        }
                    }
                }
                btnCancelar.Text = "Cancelar";
                btnGuardar.Text = "Guardar";


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al Guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnGuardar.Enabled = false;
            }
            
        }

        private byte ObtenerPais()
        {
            byte respuesta = 0;

            if (rbMexico.Checked)
            {
                respuesta = 1;
            }
            else if (rbEUA.Checked)
            {
                respuesta = 2;
            }
            else if (rbAmbos.Checked)
            {
                respuesta = 3;
            }

            return respuesta;
        }

        private void dtGrdVwFeriados_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                btnGuardar.Enabled = true;

                fecha_selected = DateTime.Parse(dtGrdVwFeriados.Rows[e.RowIndex].Cells[0].Value.ToString());

                btnCancelar.Text = "Eliminar";
                btnGuardar.Text = "Actualizar";

                SeleccionarFecha(fecha_selected);

                string pais = dtGrdVwFeriados.Rows[e.RowIndex].Cells[1].Value.ToString();
                pais_seleted = 0;

                switch (pais)
                {
                    case "Mexico":
                        pais_seleted = 1;
                        rbMexico.Checked = true;
                        break;
                    case "Estados Unidos":
                        pais_seleted = 2;
                        rbEUA.Checked = true;
                        break;
                    case "Ambos":
                        pais_seleted = 3;
                        rbAmbos.Checked = true;
                        break;
                }

               
                bool existe = ConsultarFecha(new DIAS_FERIADOS { tipo_dia_feriado = pais_seleted, fecha = fecha_selected });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al Seleccionar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private bool ConsultarFecha(DIAS_FERIADOS dia)
        {
            using(CATALOGOSEntities context = new CATALOGOSEntities())
            {
                 DIAS_FERIADOS resultado = context.DIAS_FERIADOS.Where(w => w.fecha == dia.fecha).FirstOrDefault();

                if(resultado == null)
                {
                    return false;
                } 
                else
                {
                    return true;
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if(btnCancelar.Text == "Cancelar")
            {

            }
            else if(btnCancelar.Text == "Eliminar")
            {
                DialogResult dialogResult = MessageBox.Show("Esta seguro de borrar el dia feriado?", "Borrando Dia Feriado", MessageBoxButtons.YesNo);

                if(dialogResult == DialogResult.Yes)
                {
                    using (CATALOGOSEntities context = new CATALOGOSEntities())
                    {

                        DIAS_FERIADOS dia = context.DIAS_FERIADOS.Where(w => w.fecha == fecha_selected && w.tipo_dia_feriado == pais_seleted).FirstOrDefault();

                        SqlConnection cnn = (SqlConnection)context.Database.Connection;

                        cnn.Open();
                        SqlCommand cmd = cnn.CreateCommand();
                        cmd.CommandText = "DELETE FROM DIAS_FERIADOS WHERE fecha=@param1 AND tipo_dia_feriado=@param2";
                        cmd.Parameters.AddWithValue("@param1", dia.fecha);
                        cmd.Parameters.AddWithValue("@param2", dia.tipo_dia_feriado);
                        int afectados = cmd.ExecuteNonQuery();

                        if (afectados > 0)
                        {
                            SeleccionarCalendario();
                        }
                    }
                }
                else if(dialogResult == DialogResult.No)
                {
                    return;
                }                
            }
            btnCancelar.Text = "Cancelar";
            btnGuardar.Text = "Guardar";
            btnGuardar.Enabled = false;
        }

        private void cmbAño_Click(object sender, EventArgs e)
        {
            click_cmbAño = true;
        }

        private void dtpMesFestivo_Enter(object sender, EventArgs e)
        {
            btnGuardar.Enabled = true;
        }

        private void cmbMes_Click(object sender, EventArgs e)
        {
            click_cmbMes = true;
        }

        private void bgwCargaFines_DoWork(object sender, DoWorkEventArgs e)
        {

            Cargando(true);

            SqlConnection cnn;
            SqlTransaction transaction;
            SqlCommand cmd;

            using (CATALOGOSEntities context = new CATALOGOSEntities())
            {
                cnn = (SqlConnection)context.Database.Connection;
                cnn.Open();
                cmd = cnn.CreateCommand();
                transaction = cnn.BeginTransaction();

                cmd.Connection = cnn;
                cmd.Transaction = transaction;

                try
                {
                    List<DIAS_FERIADOS> dias_cargar = new List<DIAS_FERIADOS>();

                    DateTime fecha_contador;
                    int afectados = 0;

                    if (this.cmbMesSelect <= 0)
                    {
                        fecha_contador = new DateTime(this.cmbYearSelect, 1, 1);

                        do
                        {
                            fecha_contador = fecha_contador.AddDays(1);
                            string nombre_dia = fecha_contador.ToString("dddd");

                            if (nombre_dia == "sábado" || nombre_dia == "domingo")
                            {
                                //dias_cargar.Add(new DIAS_FERIADOS { fecha = fecha_contador, tipo_dia_feriado = 3 });
                                cmd.Parameters.Clear();

                                cmd.CommandText = "INSERT INTO DIAS_FERIADOS(fecha, tipo_dia_feriado) VALUES(@param1, @param2)";
                                cmd.Parameters.AddWithValue("@param1", fecha_contador);
                                cmd.Parameters.AddWithValue("@param2", 3);

                                afectados += cmd.ExecuteNonQuery();                             
                            }

                        } while (fecha_contador.Year == this.cmbYearSelect);
                    }
                    else
                    {
                        fecha_contador = new DateTime(this.cmbYearSelect, this.cmbMesSelect, 1);

                        do
                        {
                            fecha_contador = fecha_contador.AddDays(1);
                            string nombre_dia = fecha_contador.ToString("dddd");

                            if (nombre_dia == "sábado" || nombre_dia == "domingo")
                            {
                                //dias_cargar.Add(new DIAS_FERIADOS { fecha = fecha_contador, tipo_dia_feriado = 3 });
                                cmd.Parameters.Clear();

                                cmd.CommandText = "INSERT INTO DIAS_FERIADOS(fecha, tipo_dia_feriado) VALUES(@param1, @param2)";
                                cmd.Parameters.AddWithValue("@param1", fecha_contador);
                                cmd.Parameters.AddWithValue("@param2", 3);

                                afectados += cmd.ExecuteNonQuery();                             
                            }                        

                        } while (fecha_contador.Month == this.cmbMesSelect);
                    }

                    transaction.Commit();

                    MessageBox.Show($"Se cargaron {afectados} dias", "Carga Fines de semana", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    MessageBox.Show(ex.Message, "Error al Cargar Fines de Semana", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                finally
                {
                    cnn.Close();
                }
            }

        }

        private void bgwCargaFines_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Cargando(false);
        }

        private void Cargando(bool cargando)
        {
            if (loading.InvokeRequired)
            {
                loading.Invoke(new MethodInvoker(delegate {
                    loading.Visible = cargando;
                    loading.BackColor = Color.FromArgb(153, 180, 209);
                    loading.Refresh();

                }));

                dtGrdVwFeriados.Invoke(new MethodInvoker(delegate {
                    dtGrdVwFeriados.Visible = !cargando;
                    dtGrdVwFeriados.Refresh();

                }));
            }
            else
            {
                loading.Visible = cargando;
                loading.BackColor = Color.FromArgb(153, 180, 209);
                loading.Refresh();

                dtGrdVwFeriados.Visible = !cargando;
                dtGrdVwFeriados.Refresh();
            }
        }

        private void dtpMesFestivo_Leave(object sender, EventArgs e)
        {
            if(btnGuardar.Focused)
            {
                btnGuardar.PerformClick();
            }
            else
            {
                btnGuardar.Enabled = false;
            }
            
        }
    } 
}
