using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseDeDatosTarea
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            // Validar que los datos de entrada sean correctos
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtDescription.Text) ||
                !int.TryParse(txtQuantity.Text, out int quantity))
            {
                MessageBox.Show("Ingrese valores válidos para Nombre, Descripción y Cantidad.");
                return;
            }

            try
            {
                using (var context = new ToolBorrowingEntities())
                {
                    // Crear un nuevo objeto Tool
                    var newTool = new Tool
                    {
                        Name = txtName.Text,
                        Description = txtDescription.Text,
                        Quantity = quantity,
                        Status = true  // Se asume que la herramienta está disponible (true)
                    };

                    context.Tools.Add(newTool);
                    context.SaveChanges();
                }

                MessageBox.Show("Herramienta insertada exitosamente.");
                // Limpiar campos de entrada
                txtName.Clear();
                txtDescription.Clear();
                txtQuantity.Clear();
                // Recargar la lista para reflejar los cambios
                btnLoad_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar herramienta: " + ex.Message);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new ToolBorrowingEntities())
                {
                    // Obtener todas las herramientas de la tabla Tools
                    var tools = context.Tools.ToList();
                    dataGridView1.DataSource = tools;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar herramientas: " + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'toolBorrowingDataSet.Tools' Puede moverla o quitarla según sea necesario.
            this.toolsTableAdapter.Fill(this.toolBorrowingDataSet.Tools);

        }

        private void button1_Click(object sender, EventArgs e)
        {
           // Usar el nombre real de tu servidor. Nota: en C# se debe escapar la barra invertida.
            string servidor = "PCVONSANTIAGO\\MSSQLSERVER02";
            string baseDatos = "ToolBorrowing";
            string username = textBox1.Text;
            string password = textBox2.Text;

            UsuarioValidator validator = new UsuarioValidator();
            bool valido = validator.ValidarUsuario(servidor, baseDatos, username, password);

            if (valido)
            {
                MessageBox.Show("Usuario validado correctamente.");
                Form2 form2 = new Form2(username);
                form2.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Credenciales incorrectas, por favor verifique sus datos.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Supongamos que tienes un TextBox: txtDeleteUserName
            string userNameToDelete = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(userNameToDelete))
            {
                MessageBox.Show("Debe ingresar el nombre del usuario a eliminar.");
                return;
            }

            try
            {
                using (var context = new ToolBorrowingEntities())
                {
                    // Ejecutar el procedimiento almacenado DeleteUser
                    context.Database.ExecuteSqlCommand(
                        "EXEC DeleteUser @UserName",
                        new SqlParameter("@UserName", userNameToDelete)
                    );
                }
                MessageBox.Show("Usuario eliminado correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el usuario: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Supongamos que tienes los siguientes TextBox en el formulario:
            // txtNewUserName, txtNewLoginName y txtRoleName
            string newUserName = textBox1.Text.Trim();
            string newLoginName = textBox2.Text.Trim();
            string roleName = textBox3.Text.Trim();

            if (string.IsNullOrEmpty(newUserName) || string.IsNullOrEmpty(newLoginName) || string.IsNullOrEmpty(roleName))
            {
                MessageBox.Show("Debe completar todos los campos para crear un usuario.");
                return;
            }

            try
            {
                using (var context = new ToolBorrowingEntities())
                {
                    // Ejecutar el procedimiento almacenado CreateUserWithRole
                    context.Database.ExecuteSqlCommand(
                        "EXEC CreateUserWithRole @UserName, @LoginName, @RoleName",
                        new SqlParameter("@UserName", newUserName),
                        new SqlParameter("@LoginName", newLoginName),
                        new SqlParameter("@RoleName", roleName)
                    );
                }
                MessageBox.Show("Usuario creado y rol asignado correctamente (si el rol existe).");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el usuario: " + ex.Message);
            }
        }
    }
}
