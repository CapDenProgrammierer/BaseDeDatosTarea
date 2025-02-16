using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace BaseDeDatosTarea
{
    public partial class Form2 : Form
    {
        private string _loggedUser;
        public Form2(string loggedUser)
        {
            InitializeComponent();
            _loggedUser = loggedUser;
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            lblUser.Text = "Bienvenido, " + _loggedUser;
            LoadActiveLoans();
        }

        private void LoadActiveLoans()
        {
            try
            {
                using (var context = new ToolBorrowingEntities())
                {
                    // Llamar al procedimiento almacenado GetActiveLoans pasándole el parámetro @UserId
                    var loans = context.Database.SqlQuery<ActiveLoan>(
                        "EXEC GetActiveLoans @UserId",
                        new SqlParameter("@UserId", _loggedUser)
                    ).ToList();

                    dgvLoans.DataSource = loans;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los préstamos: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // textBox1 contendrá el ToolId para el nuevo préstamo.
            if (!int.TryParse(textBox1.Text.Trim(), out int toolId))
            {
                MessageBox.Show("Ingrese un ToolId válido.");
                return;
            }

            try
            {
                // Usando la cadena de conexión dinámica almacenada en GlobalConfig (o pasarla directamente)
                using (var context = new ToolBorrowingEntities(GlobalCOnfig.EFConnectionString))
                {
                    context.Database.ExecuteSqlCommand(
                        "EXEC InsertLoan @UserId, @ToolId",
                        new SqlParameter("@UserId", _loggedUser),
                        new SqlParameter("@ToolId", toolId)
                    );
                }
                MessageBox.Show("Loan insertado correctamente.");
                LoadActiveLoans(); // Refrescar la lista de Loans
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar el loan: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // En este caso, textBox1 contendrá el LoanId del préstamo a actualizar.
            if (!int.TryParse(textBox1.Text.Trim(), out int loanId))
            {
                MessageBox.Show("Ingrese un LoanId válido.");
                return;
            }

            try
            {
                using (var context = new ToolBorrowingEntities(GlobalCOnfig.EFConnectionString))
                {
                    context.Database.ExecuteSqlCommand(
                        "EXEC ReturnLoan @LoanId",
                        new SqlParameter("@LoanId", loanId)
                    );
                }
                MessageBox.Show("Loan actualizado (devuelto) correctamente.");
                LoadActiveLoans(); // Refrescar la lista de Loans
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el loan: " + ex.Message);
            }
        }
    }
    public class ActiveLoan
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ToolName { get; set; }
        public DateTime LoanDate { get; set; }
        public string Status { get; set; }
    }
}
