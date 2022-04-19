using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormGeography.Models;

namespace WinFormGeography
{
    public partial class Form1 : Form
    {
        private GeographyContext context;
        public Form1()
        {
            context = new GeographyContext();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var peaks = context.Peaks
                .Where(x => x.Elevation < 3500)
                .Select(x => new
                {
                    x.PeakName,
                    x.Elevation,
                    x.Mountain.MountainRange
                })
                .OrderBy(x => x.PeakName)
                .ThenBy(x => x.Elevation)
                .ThenByDescending(x => x.MountainRange)
                .ToList();

            dataGridView1.DataSource = peaks;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var peaks = context.Peaks
                .Join(context.Mountains,
                (p => p.MountainId),
                (m => m.Id),
                (p, m) => new
                {
                    p.PeakName,
                    p.Elevation,
                    m.MountainRange,
                    m.Id
                })
                .Join(context.MountainsCountries,
                (m => m.Id),
                (mc => mc.MountainId),
                (m,mc) => new 
                {
                    m.PeakName,
                    m.Elevation,
                    m.MountainRange,
                    mc.CountryCode
                })
                .Join(context.Countries,
                (mc => mc.CountryCode),
                (c => c.CountryCode),
                (mc,c) => new
                {
                    mc.PeakName,
                    mc.Elevation,
                    mc.MountainRange,
                    c.ContinentCode
                })
                .Where(c => c.ContinentCode == "NA")
                .Select(x => new
                {
                    x.PeakName,
                    x.Elevation,
                    x.MountainRange
                })
                .OrderBy(p => p.PeakName)
                .ThenBy(p => p.Elevation)
                .ThenByDescending(p => p.MountainRange)
                .ToList();

            dataGridView1.DataSource = peaks;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var peaks = context.Peaks
                .Where(x => x.Mountain.MountainRange == textBox1.Text)
                .Select(x => new
                {
                    x.PeakName,
                    x.Elevation,
                    x.Mountain.MountainRange
                })
                .OrderBy(x => x.PeakName)
                .ThenBy(x => x.Elevation)
                .ThenByDescending(x => x.MountainRange)
                .ToList();

            dataGridView1.DataSource = peaks;
        }
    }
}
