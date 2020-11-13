using Futoszalag_8.het.Abstractions;
using Futoszalag_8.het.Entitites;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Futoszalag_8.het
{
    public partial class Form1 : Form
    {
        Toy _nextToy;
        List<Toy> _toys = new List<Toy>();
        private IToyFactory _factory;
        public IToyFactory Factory
        {
            get { return _factory; }
            set { _factory = value;
                DisplayNext();
            }
        }
        public Form1()
        {
            InitializeComponent();
            
        }

        private void createTimer_Tick(object sender, EventArgs e)
        {
            Toy t = Factory.CreateNew();
            _toys.Add(t);
            mainPanel.Controls.Add(t);
            t.Left = -t.Width;
            t.Top = mainPanel.Height - t.Width;
        }

        private void conveyorTimer_Tick(object sender, EventArgs e)
        {
            var maxPosition = 0;
            foreach (var toy in _toys)
            {
                toy.MoveToy();
                if (toy.Left > maxPosition)
                    maxPosition = toy.Left;
            }

            if (maxPosition > 1000)
            {
                var oldestToy = _toys[0];
                mainPanel.Controls.Remove(oldestToy);
                _toys.Remove(oldestToy);
            }
        }

        private void btnCar_Click(object sender, EventArgs e)
        {
            Factory = new CarFactory();
        }

        private void btnBall_Click(object sender, EventArgs e)
        {
            Factory = new BallFactory();
        }
        private void DisplayNext()
        {
            if (_nextToy!=null)
            {
                Controls.Remove(_nextToy);
            }
            _nextToy = Factory.CreateNew();
            _nextToy.Top = label1.Top + 40;
            Controls.Add(_nextToy);
        }

        private void btnColor1_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var ColorPicker = new ColorDialog();
            ColorPicker.Color = btnColor1.BackColor;
            if (ColorPicker.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            button.BackColor = ColorPicker.Color;
        }
    }
}
