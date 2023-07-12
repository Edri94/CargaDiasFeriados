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
            SeleccionarCalendario();
        }

        private void cmbAño_SelectedIndexChanged(object sender, EventArgs e)
        {
            SeleccionarCalendario();
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
                    dtGrdVwFeriados.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
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
                            SqlConnection cnn = (SqlConnection)context.Database.Connection;

                            cnn.Open();
                            SqlCommand cmd = cnn.CreateCommand();
                            cmd.CommandText = "INSERT INTO DIAS_FERIADOS(fecha, tipo_dia_feriado) VALUES(@param1, @param2)";
                            cmd.Parameters.AddWithValue("@param1", dia.fecha);
                            cmd.Parameters.AddWithValue("@param2", dia.tipo_dia_feriado);
                            int afectados = cmd.ExecuteNonQuery();

                            if (afectados > 0)
                            {
                                SeleccionarCalendario();
                            }
                        }
                    }
                }
                else if(btnGuardar.Text == "Editar")
                {
                    using (CATALOGOSEntities context = new CATALOGOSEntities())
                    {

                        DIAS_FERIADOS dia = context.DIAS_FERIADOS.Where(w => w.fecha == fecha_selected && w.tipo_dia_feriado == pais_seleted).FirstOrDefault();

                        dia.tipo_dia_feriado = ObtenerPais();

                      

                        SqlConnection cnn = (SqlConnection)context.Database.Connection;

                        cnn.Open();
                        SqlCommand cmd = cnn.CreateCommand();
                        cmd.CommandText = "UPDATE DIAS_FERIADOS SET tipo_dia_feriado=@param1 WHERE fecha=@param2";
                        cmd.Parameters.AddWithValue("@param1", dia.tipo_dia_feriado);
                        cmd.Parameters.AddWithValue("@param2", dia.fecha);

                        int afectados = cmd.ExecuteNonQuery();

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
                fecha_selected = DateTime.Parse(dtGrdVwFeriados.Rows[e.RowIndex].Cells[0].Value.ToString());

                btnCancelar.Text = "Eliminar";
                btnGuardar.Text = "Editar";

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
        }
    } 
}
