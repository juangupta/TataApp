using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TataApp.ViewModels;
using Xamarin.Forms;

namespace TataApp.Views
{
    public partial class EmployeesPage : ContentPage
    {
        public EmployeesPage()
        {
            var employeesViewModel = EmployeesViewModel.GetInstance();
            base.Appearing += (object sender, EventArgs e) =>
            {
                employeesViewModel.RefreshCommand.Execute(this);
            };
            InitializeComponent();
        }
    }
}
