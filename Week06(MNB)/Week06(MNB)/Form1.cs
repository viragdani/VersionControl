using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Week06_MNB_.MnbServiceReference;

namespace Week06_MNB_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CallWebservice();
        }

        private void CallWebservice() 
        {
            MNBArfolyamServiceSoapClient mnbService = new MNBArfolyamServiceSoapClient();
            GetExchangeRatesRequestBody request = new GetExchangeRatesRequestBody();
            request.currencyNames = "EUR";
            request.startDate = "2020-01-01";
            request.endDate = "2020-06-30";
            var response=mnbService.GetExchangeRates(request);
            var result=response.GetExchangeRatesResult;
        }
        
    }
}
