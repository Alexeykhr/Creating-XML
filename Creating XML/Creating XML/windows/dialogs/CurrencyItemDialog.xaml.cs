using Creating_XML.src.db.tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Creating_XML.windows.dialogs
{
    public partial class CurrencyItemDialog : Window
    {
        private CurrencyTable item;

        public CurrencyItemDialog(CurrencyTable item)
        {
            InitializeComponent();
            fName.Text = item.Name;
            fRate.Text = item.Rate;
            this.item = item;
        }
    }
}
