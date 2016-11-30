using System;
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
        private Double _overtimePenaltyWeight = 1;
        private Double _overallDistanceWeight = 1;

        public Form1()
        {
            InitializeComponent();
        }

        private void UpdateFitness()
        {
            labelFitnessValue.Text = _problem.FitnessValue();
        }

        private void buttonGenerateTAP_Click(object sender, EventArgs e)
        {
            _problem = new TAP((Int32)numericAssemblercount.Value, (Int32)numericCustomercount.Value, _overtimePenaltyWeight, _overallDistanceWeight);
            labelFitnessValue.Text = "0";
            buttonSaveTAP.Enabled = true;
            buttonSolveGreedy.Enabled = true;
            buttonSolveRandomly.Enabled = true;
            buttonSwap.Enabled = false;
            button2Opt.Enabled = false;
            panelSolvedGraph.Invalidate();
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
            TravelingAssemblyMen.Model.Location.HQ.Draw(e.Graphics, origin, pixelsPerKilometer, Color.Red);

            if (_problem != null)
            {
                _problem.DrawSolution(e.Graphics, origin, pixelsPerKilometer);
            }
        }

        private void buttonSolveRandomly_Click(object sender, EventArgs e)
        {
            _problem.SolveRandomly();
            buttonSwap.Enabled = true;
            button2Opt.Enabled = true;
            panelSolvedGraph.Invalidate();
            UpdateFitness();
        }

        private void textBoxOvertimePenalty_TextChanged(object sender, EventArgs e)
        {
            double newValue;

            if (Double.TryParse(textBoxOvertimePenalty.Text, out newValue))
            {
                _overtimePenaltyWeight = newValue;

                if (_problem != null)
                {
                    _problem.OvertimePenaltyWeight = newValue;
                }

                return;
            }

            MessageBox.Show("The input Value for the overtime penalty weight is not a correct floating point value.");
            textBoxOvertimePenalty.Text = _overtimePenaltyWeight.ToString();
        }

        private void textBoxOverallWorkload_TextChanged(object sender, EventArgs e)
        {
            double newValue;

            if (Double.TryParse(textBoxOverallDistance.Text, out newValue))
            {
                _overallDistanceWeight = newValue;

                if (_problem != null)
                {
                    _problem.OverallWorkloadWeight = newValue;
                }

                return;
            }

            MessageBox.Show("The input Value for the overall workload weight is not a correct floating point value.");
            textBoxOverallDistance.Text = _overallDistanceWeight.ToString();
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
                    _problem = TAP.Open(openFileDialog.FileName, _overtimePenaltyWeight, _overallDistanceWeight);
                    labelFitnessValue.Text = "0";
                    buttonSaveTAP.Enabled = true;
                    buttonSolveGreedy.Enabled = true;
                    buttonSolveRandomly.Enabled = true;
                    buttonSwap.Enabled = false;
                    button2Opt.Enabled = false;
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
            buttonSwap.Enabled = true;
            button2Opt.Enabled = true;
            panelSolvedGraph.Invalidate();
            UpdateFitness();
        }

        private void button2Opt_Click(object sender, EventArgs e)
        {
            _problem.LocalOptimisation(LocalOptimisationStyle.TwoOpt, (int)numericUpDownNeighborhoodRange.Value);
            panelSolvedGraph.Invalidate();
            UpdateFitness();
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            if (panelSolvedGraph.Width > panelSolvedGraph.Height)
            {
                this.Width -= panelSolvedGraph.Width - panelSolvedGraph.Height;
                panelSolvedGraph.Invalidate();
                return;
            }

            this.Height -= panelSolvedGraph.Height - panelSolvedGraph.Width;
            panelSolvedGraph.Invalidate();
        }

        private void buttonSwap_Click(object sender, EventArgs e)
        {
            _problem.LocalOptimisation(LocalOptimisationStyle.Swap, (int)numericUpDownNeighborhoodRange.Value);
            panelSolvedGraph.Invalidate();
            UpdateFitness();
        }

        private void buttonInsert_Click(object sender, EventArgs e)
        {
            _problem.LocalOptimisation(LocalOptimisationStyle.Insert1, (int)numericUpDownNeighborhoodRange.Value);
            panelSolvedGraph.Invalidate();
            UpdateFitness();
        }

        private void buttonInsert2_Click(object sender, EventArgs e)
        {
            _problem.LocalOptimisation(LocalOptimisationStyle.Insert3, (int)numericUpDownNeighborhoodRange.Value);
            panelSolvedGraph.Invalidate();
            UpdateFitness();
        }

        private void buttonInsert3_Click(object sender, EventArgs e)
        {
            _problem.LocalOptimisation(LocalOptimisationStyle.Insert3, (int)numericUpDownNeighborhoodRange.Value);
            panelSolvedGraph.Invalidate();
            UpdateFitness();
        }
    }
}
