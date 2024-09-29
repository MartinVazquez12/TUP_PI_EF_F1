using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TUP_PI_EF_F1.Datos;
using TUP_PI_EF_F1.Negocio;

//CURSO – LEGAJO – APELLIDO – NOMBRE
//1W1   - 405708 - Vazquez Flexes - Martin

namespace TUP_PI_EF_F1
{
    public partial class frmCompetidores : Form
    {
        private AccesoDatos oBD;
        private List<Competidor> lCompeti;

        public frmCompetidores()
        {
            InitializeComponent();
            oBD = new AccesoDatos();
        }

        public void habilitar(bool x)
        {
            txtNumero.Enabled= x;   
            txtNombre.Enabled= x;
            cboEscuderia.Enabled= x;
            cboPais.Enabled= x;
            dtpFecNac.Enabled = x;
            btnGrabar.Enabled= x;
            btnNuevo.Enabled= !x;
            btnSalir.Enabled= !x;
            lstCompetidores.Enabled= x;
        }

        private void frmCompetidores_Load(object sender, EventArgs e)
        {
            habilitar(false);

            CargarCombo(cboEscuderia, "escuderias");
            CargarCombo(cboPais, "paises");
            CargarLista(lstCompetidores, "competidores");
        }

        private void CargarCombo(ComboBox combo, string nombreTabla)
        {
            DataTable tabla = oBD.ConsultarBD(nombreTabla);
            combo.DataSource= tabla;
            combo.ValueMember = tabla.Columns[0].ToString();
            combo.DisplayMember = tabla.Columns[1].ToString();
            combo.DropDownStyle = ComboBoxStyle.DropDownList;

        }

        private void CargarLista(ListBox lista, string nombreTabla)
        {
            lista.Items.Clear();
            DataTable tabla = oBD.ConsultarBD(nombreTabla);
            List<Competidor> competidor = new List<Competidor>();
            Competidor oComp = null;

            foreach (DataRow fila in tabla.Rows)
            {
                oComp = new Competidor();
                oComp.Numero = Convert.ToInt32(fila[0].ToString());
                oComp.Nombre = fila[1].ToString();
                oComp.Pais = Convert.ToInt32(fila[2].ToString());
                oComp.Escuderia = Convert.ToInt32(fila[3].ToString());
                oComp.FechaNacimiento = Convert.ToDateTime(fila[4].ToString());
                
                lista.Items.Add(oComp);
            }


        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            habilitar(true);
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            habilitar(false);
            if (ValidarDatos())
            {
                Competidor oComp = new Competidor();
                oComp.Numero = Convert.ToInt32(txtNumero.Text);
                oComp.Nombre = txtNombre.Text;
                oComp.Pais = Convert.ToInt32(cboPais.SelectedValue);
                oComp.Escuderia = Convert.ToInt32(cboEscuderia.SelectedValue);
                oComp.FechaNacimiento = Convert.ToDateTime(dtpFecNac.Value);
                
                List<Parametro> parametros = new List<Parametro>();
                parametros.Add(new Parametro("@numero", oComp.Numero));
                parametros.Add(new Parametro("@nombre", oComp.Nombre));
                parametros.Add(new Parametro("@pais", oComp.Pais));
                parametros.Add(new Parametro("@fechanac", oComp.FechaNacimiento));
                parametros.Add(new Parametro("@escuderia", oComp.Escuderia));

                oBD.InsertarBD("competidores", parametros);
                CargarLista(lstCompetidores, "competidores");

            }

        }

        private bool ValidarDatos()
        {
            if(txtNumero.Text == string.Empty)
            {
                MessageBox.Show("Ingrese un numero");
                txtNumero.Focus();
                return false;        
            }
            if (txtNombre.Text == string.Empty)
            {
                MessageBox.Show("Ingrese un nombre");
                txtNombre.Focus();
                return false;
            }
            if (cboPais.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una opcion");
                cboPais.Focus();
                return false;
            }
            if (cboEscuderia.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una opcion");
                cboEscuderia.Focus();
                return false;
            }
            if (dtpFecNac.Value == DateTime.Today)
            {
                MessageBox.Show("Seleccione una fecha valida");
                dtpFecNac.Focus();
                return false;
            }
            if ((DateTime.Today.Year - dtpFecNac.Value.Year) < 18)
            {
                MessageBox.Show("No se pueden ingresar menores de 18");
                dtpFecNac.Focus();
                return false;
            }

            return true;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("DESEA SALIR DEL PROGRAMA?","SALIR", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                this.Close();
            
        }

        private void grpDatos_Enter(object sender, EventArgs e)
        {

        }
    }
}
