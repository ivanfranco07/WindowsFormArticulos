using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class frmAgregarArticulo : Form
    {
        Articulo articulo= null;
        public frmAgregarArticulo()
        {
            InitializeComponent();
        }
        public frmAgregarArticulo(Articulo articulo)
        {
            InitializeComponent( );
            this.articulo = articulo;
            Text = "Modificar artículo";
            btnAgregarArticulo.Text = "Modificar";
        }

        private void frmAgregarArticulo_Load(object sender, EventArgs e)
        {
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            try
            {
                cbCategoria.DataSource = categoriaNegocio.listarCategorias();
                cbCategoria.ValueMember = "Id";
                cbCategoria.DisplayMember= "Descripcion";

                cbMarca.DataSource = marcaNegocio.listarMarcas();
                cbMarca.ValueMember = "Id";
                cbMarca.DisplayMember = "Descripcion";

                if (articulo != null)
                {
                    txtCodigo.Text= articulo.Codigo;
                    txtNombre.Text= articulo.Nombre;
                    txtDescripcion.Text= articulo.Descripcion;
                    cbMarca.SelectedValue = articulo.IdMarca.Id; 
                    cbCategoria.SelectedValue = articulo.IdCategoria.Id;
                    txtUrl.Text = articulo.ImagenUrl;
                    txtPrecio.Text = articulo.Precio.ToString();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void btnCancelarArticulo_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAgregarArticulo_Click(object sender, EventArgs e)
        {
            if(articulo == null)
                articulo = new Articulo();
            try
            {
                if (txtCodigo.Text == "" || txtNombre.Text == "" || txtDescripcion.Text == "" || txtPrecio.Text == "")
                {
                    MessageBox.Show("Por favor, completar todos los campos antes de agregar el artículo.");
                }
                else
                {
                    articulo.Codigo = txtCodigo.Text;
                    articulo.Nombre = txtNombre.Text;
                    articulo.Descripcion = txtDescripcion.Text;
                    articulo.IdMarca = (Marca)cbMarca.SelectedItem;
                    articulo.IdCategoria = (Categoria)cbCategoria.SelectedItem;
                    articulo.ImagenUrl = txtUrl.Text;
                    articulo.Precio = decimal.Parse(txtPrecio.Text);
                    ArticuloNegocio articuloNegocio = new ArticuloNegocio();
                    if (articulo.Id != 0)
                    {
                        articuloNegocio.modificarArticulo(articulo);
                        MessageBox.Show("Artículo modificado exitosamente");
                    }
                    else
                    {
                        articuloNegocio.agregarArticulo(articulo);
                        MessageBox.Show("Artículo agregado exitosamente");
                    }
                    Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
    }
}
