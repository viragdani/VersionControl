using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using Week06_MNB_.Entities;
using Week06_MNB_.MnbServiceReference;

namespace Week06_MNB_
{
    public partial class Form1 : Form
    {
        BindingList<RateData> Rates = new BindingList<RateData>();
        string result;
        public Form1()
        {
            InitializeComponent();
            RefreshData();
        }
        private void RefreshData()
        {
            Rates.Clear();
            CallWebservice();
            dataGridView1.DataSource = Rates;
            XmlProcessing();
            chartRateData.DataSource = Rates;

            var series = chartRateData.Series[0];
            series.ChartType = SeriesChartType.Line;
            series.XValueMember = "Date";
            series.YValueMembers = "Value";
            series.BorderWidth = 2;

            var legend = chartRateData.Legends[0];
            legend.Enabled = false;

            var chartArea = chartRateData.ChartAreas[0];
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.IsStartedFromZero = false;
        }
        private void CallWebservice() 
        {
            MNBArfolyamServiceSoapClient mnbService = new MNBArfolyamServiceSoapClient();
            GetExchangeRatesRequestBody request = new GetExchangeRatesRequestBody();
            request.currencyNames = comboBox1.SelectedItem.ToString();
            request.startDate = dateTimePicker1.Value.ToString();
            request.endDate = dateTimePicker2.Value.ToString();
            var response=mnbService.GetExchangeRates(request);
            result=response.GetExchangeRatesResult;
        }
        private void XmlProcessing()
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(result);

            // Végigmegünk a dokumentum fő elemének gyermekein
            foreach (XmlElement element in xml.DocumentElement)
            {
                // Létrehozzuk az adatsort és rögtön hozzáadjuk a listához
                // Mivel ez egy referencia típusú változó, megtehetjük, hogy előbb adjuk a listához és csak később töltjük fel a tulajdonságait
                var rate = new RateData();
                Rates.Add(rate);

                // Dátum
                rate.Date = DateTime.Parse(element.GetAttribute("date"));

                // Valuta
                var childElement = (XmlElement)element.ChildNodes[0];
                rate.Currency = childElement.GetAttribute("curr");

                // Érték
                var unit = decimal.Parse(childElement.GetAttribute("unit"));
                var value = decimal.Parse(childElement.InnerText);
                if (unit != 0)
                    rate.Value = value / unit;
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}
