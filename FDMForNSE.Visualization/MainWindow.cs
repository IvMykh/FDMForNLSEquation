using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

using FDMForNSE.AlgorithmImplementation;
using ZedGraph;

namespace FDMForNSE.Visualization
{
    using SolutionEnumerator = IEnumerator<IEnumerable<ApproximationPoint>>;
    using Timer = System.Threading.Timer;

    public partial class MainWindow : Form
    {
        private int                 CALLBACK_FREQUENCY = 1;

        private NlseSolver          _eqSolver;
        private SolutionEnumerator  _enumerator;
        private Timer               _timer;
        private ZedGraphControl     _zedGraph;
        private double              _currTimeMoment;


        public MainWindow()
        {
            InitializeComponent();

            setDefaultConfiguration();

            _timer                  = null;
            _zedGraph               = createZedGraphControl();
            _currTimeMoment         = _eqSolver.TInterval.Start;

            setupSolutionTypeComboBox();

            graphPanel.Controls.Add(_zedGraph);
        }

        private ZedGraphControl createZedGraphControl()
        {
            var zedGraph = new ZedGraphControl();

            zedGraph.Location   = new Point(0, 0);
            zedGraph.Name       = "zedGraph";
            zedGraph.Size       = new Size(graphPanel.Width, graphPanel.Height);
            zedGraph.Anchor     =   AnchorStyles.Bottom | 
                                    AnchorStyles.Top | 
                                    AnchorStyles.Left | 
                                    AnchorStyles.Right;

            GraphPane myPane = zedGraph.GraphPane;
            
            // Turn off the axis frame and all the opposite side tics
            myPane.Title.Text                   = "NLSE Solitons Collision";
            
            myPane.XAxis.MajorTic.IsOpposite    = false;
            myPane.XAxis.MinorTic.IsOpposite    = false;
            myPane.YAxis.MajorTic.IsOpposite    = false;
            myPane.YAxis.MinorTic.IsOpposite    = false;

            zedGraph.AxisChange();

            return zedGraph;
        }
        private void setupSolutionTypeComboBox()
        {
            solutionTypeComboBox.Items.AddRange(ConfigVariantName.GetConfigVariantsRange());
            solutionTypeComboBox.SelectedIndex = 0;
        }

        private void startStopButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;

            if (_timer == null)
            {
                _enumerator     = _eqSolver.SequenceOfApproximations().GetEnumerator();
                _timer          = new Timer(this.Animate, null, CALLBACK_FREQUENCY, CALLBACK_FREQUENCY);
                _currTimeMoment = _eqSolver.TInterval.Start;
                
                button.Text = "Stop";
            }
            else
            {
                lock (this)
                {
                    _timer.Dispose();
                    _timer = null;

                    _enumerator.Dispose();
                    _enumerator = null;
                }

                button.Text = "Start";
            }

            configGroupBox.Enabled = !configGroupBox.Enabled;
        }
        private void resetConfigButton_Click(object sender, EventArgs e)
        {
            setDefaultConfiguration();
        }

        private void setDefaultConfiguration()
        {
            var eqSolver = new NlseSolver(ConfigurationsStore.Store[ConfigVariant.OneSoliton]);
            setConfiguration(eqSolver);
        }
        private void setConfiguration(NlseSolver eqSolver)
        {
            _eqSolver = eqSolver;
            
            _enumerator = null;
            durationNumericUpDown.Value = (int)_eqSolver.TInterval.End;
            xStepNumericUpDown.Value = (decimal)_eqSolver.Net.XStep;
            tStepNumericUpDown.Value = (decimal)_eqSolver.Net.TStep;
        }

        private void setUpTimeProgressBar()
        {
            timeProgressBar.Minimum = (int)_eqSolver.TInterval.Start;
            timeProgressBar.Maximum = (int)_eqSolver.TInterval.End;
        }

        private PointPairList getNextGraph()
        {
            if (_enumerator != null && _enumerator.MoveNext())
            {
                PointPairList list = new PointPairList();
                    
                foreach (var approxPoint in _enumerator.Current)
                {
                    list.Add(approxPoint.X, approxPoint.U.Magnitude);
                }
                    
                return list;
            }
            else
            {
                return null;
            }
        }

        private void Animate(object state)
        {
            _zedGraph.Invoke(new Action(() => 
                {
                    var nextGraph = getNextGraph();
                    Interlocked.Exchange(ref _currTimeMoment, _currTimeMoment + _eqSolver.Net.TStep);

                    if (nextGraph != null)
                    {
                        _zedGraph.GraphPane.CurveList.Clear();
                        var myCurve = _zedGraph.GraphPane.AddCurve("Solitons", nextGraph, Color.Blue, SymbolType.None);
                        myCurve.Line.Width = 2.0f;
                        _zedGraph.Refresh();

                        timeProgressBar.Value = (int)((Math.Round(_currTimeMoment))* timeProgressBar.Step);
                    }
                    else
                    {
                        if (_timer != null)
                        {
                            _timer.Change(Timeout.Infinite, Timeout.Infinite);
                        }

                        timeProgressBar.Value = timeProgressBar.Maximum;
                        startStopButton.Invoke(new Action(startStopButton.PerformClick));
                    }

                }));
        }

        private void durationNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            var numericUpDown   = sender as NumericUpDown;
            _eqSolver.TInterval = new Interval 
                { 
                    Start   = 0.0, 
                    End     = (double)numericUpDown.Value 
                };

            timeProgressBar.Step = (int)((timeProgressBar.Maximum - timeProgressBar.Minimum) / _eqSolver.TInterval.End);
        }
        private void xStepNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            var numericUpDown   = sender as NumericUpDown;
            _eqSolver.Net       = new Net 
                { 
                    XStep = (double)numericUpDown.Value, 
                    TStep = _eqSolver.Net.TStep 
                };
        }
        private void tStepNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            var numericUpDown   = sender as NumericUpDown;
            _eqSolver.Net       = new Net 
                { 
                    XStep = _eqSolver.Net.XStep,
                    TStep = (double)numericUpDown.Value 
                };
        }

        private void solutionTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cvn         = (ConfigVariantName)solutionTypeComboBox.SelectedItem;
            var eqSolver    = new NlseSolver(ConfigurationsStore.Store[cvn.ConfigVariant]);

            setConfiguration(eqSolver);
        }
    }
}
