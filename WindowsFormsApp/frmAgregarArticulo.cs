﻿using Dominio;
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
using System.Configuration;
using System.IO;

namespace WindowsFormsApp
{
    public partial class frmAgregarArticulo : Form
    {
        Articulo articulo= null;
        OpenFileDialog openFileDialog = null;
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
                    MessageBox.Show("Por favor, completar todos los campos del artículo.");
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
                    //Forma de guardar la imagen si fue cargada localmente
                    if (openFileDialog != null && !(txtUrl.Text.ToUpper().Contains("HTTP"))) 
                    {
                        File.Copy(openFileDialog.FileName, ConfigurationManager.AppSettings["catalogoArticuloImg"] + openFileDialog.SafeFileName);
                    }
                    Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void agregarImagen(string direccion)
        {
            try
            {
                pbAgregarArticulo.Load(direccion);
            }
            catch (Exception)
            {
                pbAgregarArticulo.Load("https://xentra.glomastore.mx/img/sin_imagen.png"); ;
            }
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "jpg|* .jpg|png|*.png";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtUrl.Text = openFileDialog.FileName;
                agregarImagen(openFileDialog.FileName);
            }
        }
    }
}
