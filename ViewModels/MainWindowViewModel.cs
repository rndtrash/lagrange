using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive;
using Avalonia.Controls;
using DynamicData.Binding;
using HarfBuzzSharp;
using OxyPlot;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Algo.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public class Vector2
        {
            public double X { get; set; }
            public double Y { get; set; }

            public Vector2()
            {
            }

            public DataPoint ToDataPoint() => new DataPoint(X, Y);
        }

        [Reactive] public double From { get; set; } = -1;
        [Reactive] public double To { get; set; } = 1;
        [Reactive] public int Steps { get; set; } = 1000;

        public ObservableCollection<Vector2> InitialPoints { get; } = new();
        public ObservableCollection<DataPoint> InitialPointSeries { get; } = new();
        public ObservableCollection<DataPoint> InterpolatedPointSeries { get; } = new();

        private Utils.PointPlacementMethod CurrentPointPlacementMethod = Utils.PointPlacementMethods.First().Value;

        ReactiveCommand<string, Unit> AddNewPointCommand { get; }
        ReactiveCommand<string, Unit> RenderPlotCommand { get; }
        ReactiveCommand<Unit, Unit> PointsChangedEvent { get; }

        public MainWindowViewModel()
        {
            AddNewPointCommand = ReactiveCommand.Create((string s) => InitialPoints.Add(new() { X = 0, Y = 0 }));
            RenderPlotCommand = ReactiveCommand.Create((string s) => RenderPlot());
            PointsChangedEvent = ReactiveCommand.Create(InitialPointsChanged);

            InitialPoints.CollectionChanged += (sender, args) => InitialPointsChanged();
        }

        public void InitialPointsChanged()
        {
            InitialPointSeries.Clear();

            foreach (var p in InitialPoints)
            {
                InitialPointSeries.Add(p.ToDataPoint());
            }
        }

        public void PointPlacementMethodChanged(object? sender)
        {
            if (sender is not ComboBox comboBox)
                return;

            CurrentPointPlacementMethod = Utils.PointPlacementMethods[comboBox.SelectedItem as string];
        }

        private void RenderPlot()
        {
            InterpolatedPointSeries.Clear();

            for (int step = 0; step <= Steps; step++)
            {
                double x = CurrentPointPlacementMethod(From, To, step, Steps);
                InterpolatedPointSeries.Add(new(x, Lagrange(InitialPointSeries.ToArray(), x)));
            }
        }

        private double Lagrange(DataPoint[] v, double a)
        {
            double f = 0;

            for (var i = 0; i < v.Length; i++)
            {
                double l = 1;

                for (int j = 0; j < v.Length; j++)
                {
                    if (i == j)
                        continue;

                    l *= (a - v[j].X) / (v[i].X - v[j].X);
                }

                l *= v[i].Y;
                f += l;
            }

            return f;
        }
    }
}