﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TravelingAssemblyMen.Model;

namespace TravelingAssemblyMen
{
    public partial class Form1 : Form
    {
        private TAP _problem;
        private Int32 _customerCountInput = 1;
        private Int32 _assemblerCountInput = 1;
        private Double _overtimePenaltyWeight = 1;
        private Double _overallWorkdloadWeight = 1;

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonGenerateTAP_Click(object sender, EventArgs e)
        {
            _problem = new TAP(_assemblerCountInput, _customerCountInput);
            labelFitnessValue.Text = "0";
            buttonSaveTAP.Enabled = true;
            buttonSolveGreedy.Enabled = true;
            buttonSolveRandomly.Enabled = true;
            panelSolvedGraph.Invalidate();
        }

        private void numericCustomercount_ValueChanged(object sender, EventArgs e)
        {
            _customerCountInput = Decimal.ToInt32(numericCustomercount.Value);
        }

        private void numericAssemblercount_ValueChanged(object sender, EventArgs e)
        {
            _assemblerCountInput = Decimal.ToInt32(numericAssemblercount.Value);
        }

        private void panelSolvedGraph_Paint(object sender, PaintEventArgs e)
        {
            Double width = e.Graphics.VisibleClipBounds.Width;
            Double height = e.Graphics.VisibleClipBounds.Height;

            Position origin = new Position();
            origin.x = width / 2;
            origin.y = width / 2;

            Double pixelsPerKilometer = (width - 40) / 100;

            // headquarters
            new Location(0, 0).Draw(e.Graphics, origin, pixelsPerKilometer, Color.Red);

            if (_problem != null)
            {
                _problem.DrawSolution(e.Graphics, origin, pixelsPerKilometer);
            }
        }

        private void buttonSolveRandomly_Click(object sender, EventArgs e)
        {
            _problem.SolveRandomly();
            panelSolvedGraph.Invalidate();
            labelFitnessValue.Text = _problem.FitnessValue(_overtimePenaltyWeight, _overallWorkdloadWeight);
        }

        private void textBoxOvertimePenalty_TextChanged(object sender, EventArgs e)
        {
            double newValue;

            if (Double.TryParse(textBoxOvertimePenalty.Text, out newValue))
            {
                _overtimePenaltyWeight = newValue;
                return;
            }

            MessageBox.Show("The input Value for the overtime penalty weight is not a correct floating point value.");
        }

        private void textBoxOverallWorkload_TextChanged(object sender, EventArgs e)
        {
            double newValue;

            if (Double.TryParse(textBoxOverallWorkload.Text, out newValue))
            {
                _overallWorkdloadWeight = newValue;
                return;
            }

            MessageBox.Show("The input Value for the overall workload weight is not a correct floating point value.");
        }

        private void buttonSaveTAP_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                _problem.Save(saveFileDialog.FileName);
            }
        }

        private void buttonOpenTAP_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _problem = TAP.Open(openFileDialog.FileName);
                    labelFitnessValue.Text = "0";
                    buttonSaveTAP.Enabled = true;
                    buttonSolveGreedy.Enabled = true;
                    buttonSolveRandomly.Enabled = true;
                    numericAssemblercount.Value = _problem.NumberOfAssembler;
                    numericCustomercount.Value = _problem.NumberOfCustomers;
                    panelSolvedGraph.Invalidate();
                }
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show("Oops! There was an error parsing the location " + exception.Message + " in the file " + openFileDialog.FileName + ".");
            }
        }

        private void buttonSolveGreedy_Click(object sender, EventArgs e)
        {
            _problem.SolveGreedy();
            panelSolvedGraph.Invalidate();
            labelFitnessValue.Text = _problem.FitnessValue(_overtimePenaltyWeight, _overallWorkdloadWeight);
        }
    }
}
