using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        List<Articulo> articulos;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cargarListado();
            ocultarColumna();
            mostrarImagen(articulos[0].ImagenUrl);
            cbCampo.Items.Add("Categoría");
            cbCampo.Items.Add("Marca");
            cbCampo.Items.Add("Precio");
        }
        private void cargarListado()
        {
            ArticuloNegocio articuloNegocio = new ArticuloNegocio();
            try
            {
                articulos = articuloNegocio.ListarArticulos();
                dgvArticulos.DataSource = articulos;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ocultarColumna()
        {
            dgvArticulos.Columns["ImagenUrl"].Visible = false;
            dgvArticulos.Columns["Id"].Visible = false;
        }
        private void mostrarImagen(string imagen)
        {
            try
            {
                pbArticulos.Load(imagen);
            }
            catch (Exception)
            {
                pbArticulos.Load("https://previews.123rf.com/images/vectorknight/vectorknight1711/vectorknight171100101/90815439-page-not-found-concept-404-error-web-page-with-cute-cartoon-face-flat-line-illustration-concept.jpg");
            }
        }

        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            Articulo auxiliar = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
            mostrarImagen(auxiliar.ImagenUrl);
            ocultarColumna();
        }

        private void btnAgregarArticulo_Click(object sender, EventArgs e)
        {
            frmAgregarArticulo frmAgregarArticulo = new frmAgregarArticulo();
            frmAgregarArticulo.ShowDialog();
            cargarListado();
        }

        private void btnModificarArticulo_Click(object sender, EventArgs e)
        {
            frmAgregarArticulo frmAgregarArticulo=new frmAgregarArticulo((Articulo)dgvArticulos.CurrentRow.DataBoundItem);
            frmAgregarArticulo.ShowDialog();
            cargarListado();
        }
        private void txtFiltroRapido_TextChanged_1(object sender, EventArgs e)
        {
            string filtro = txtFiltroRapido.Text;
            List<Articulo> auxiliarArticulos;
            if (filtro == "")
                auxiliarArticulos = articulos;
            else
                auxiliarArticulos = articulos.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Descripcion.ToUpper().Contains(filtro.ToUpper()) || x.Codigo.ToUpper().Contains(filtro.ToUpper()));

            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = auxiliarArticulos;
        }

        private void btnEliminarFisico_Click(object sender, EventArgs e)
        {
            ArticuloNegocio articuloNegocio = new ArticuloNegocio();
            DialogResult result = MessageBox.Show(
                "¿Estás seguro de que quieres realizar esta acción?",
                "Confirmación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
                );

            if (result == DialogResult.Yes)
            {
                MessageBox.Show("Acción realizada correctamente.");
                articuloNegocio.eliminarArticulo((Articulo)dgvArticulos.CurrentRow.DataBoundItem);
            }
            else
            {
                MessageBox.Show("Acción cancelada.");
            }
            cargarListado();
        }
        private void cbCampo_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbCampo.Text == "Precio")
            {
                cbCriterio.Items.Clear();
                cbCriterio.Items.Add("Mayor a");
                cbCriterio.Items.Add("Menor a");
                cbCriterio.Items.Add("Igual a");
            }
            else
            {
                cbCriterio.Items.Clear();
                cbCriterio.Items.Add("Contiene");
                cbCriterio.Items.Add("Empieza con");
                cbCriterio.Items.Add("Termina con");
            }
        }
        private bool validarNumeros(string cadena)
        {
            foreach (char c in cadena)
            {
                if (!(char.IsNumber(c)))
                    return false;
            }
            return true;
        }

        private void btnAplicarFiltro_Click(object sender, EventArgs e)
        {
            if(cbCampo.SelectedIndex >= 0)
            {
                if (cbCriterio.SelectedIndex >= 0)
                {
                    if (txtFiltroEspecifico.Text=="") 
                    {
                        cargarListado();
                    }
                    else
                    {
                        if (cbCampo.Text == "Marca" || cbCampo.Text == "Categoría")
                        {
                            ArticuloNegocio articuloNegocio = new ArticuloNegocio();
                            string campo = cbCampo.Text;
                            string criterio = cbCriterio.Text;
                            string parametro = txtFiltroEspecifico.Text;
                            articulos = articuloNegocio.filtrarArticulos(campo, criterio, parametro);
                            dgvArticulos.DataSource = null;
                            dgvArticulos.DataSource = articulos;
                            return;
                        }
                        if (cbCampo.Text == "Precio" && validarNumeros(txtFiltroEspecifico.Text)) 
                        {
                            ArticuloNegocio articuloNegocio = new ArticuloNegocio();
                            string campo = cbCampo.Text;
                            string criterio = cbCriterio.Text;
                            string parametro = txtFiltroEspecifico.Text;
                            articulos = articuloNegocio.filtrarArticulos(campo, criterio, parametro);
                            dgvArticulos.DataSource = null;
                            dgvArticulos.DataSource = articulos;
                        }
                        else
                            MessageBox.Show("Solo se admiten números para filtrar por el campo Precio");
                    }
                }
                else
                    MessageBox.Show("Por favor, especificar un criterio.");

            }
            else
                MessageBox.Show("Por favor, especificar un campo.");

        }
    }
}
