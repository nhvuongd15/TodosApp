using Microsoft.Data.SqlClient;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TodosApp.Db;
using TodosApp.Models;
using TodosApp.Utils;
using Task = TodosApp.Models.Task;

namespace TodosApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            User u = User.getUserByUsername("admin")!;

            var t = new Task(u.Id!.Value, "Task 1", "Description of Task 1");

            bool re = t.Save();

            if (re)
            {
                MessageBox.Show(t.ToString());
            }
            else
            {
                MessageBox.Show("Failed to save task");
            }

            DBConnection.Close();
        }
    }
}