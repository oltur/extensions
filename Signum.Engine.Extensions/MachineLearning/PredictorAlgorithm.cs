﻿using Signum.Entities;
using Signum.Entities.DynamicQuery;
using Signum.Entities.MachineLearning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Signum.Engine.MachineLearning
{
    public class TrainingProgress
    {
        public string Message;
        public decimal? Progress;

        public PredictorState State { get; set; }
    }

    public class PredictorTrainingContext
    {
        public PredictorEntity Predictor { get; }
        public CancellationToken CancellationToken { get; }

        public string Message { get; set; }
        public decimal? Progress { get; set; }

        public List<PredictorResultColumn> Columns { get; private set; }
        public List<PredictorResultColumn> InputColumns { get; private set; }
        public List<PredictorResultColumn> OutputColumns { get; private set; }

        public MainQuery MainQuery { get; internal set; }
        public Dictionary<PredictorMultiColumnEntity, MultiColumnQuery> MultiColumnQuery { get; internal set; }

        public PredictorTrainingContext(PredictorEntity predictor, CancellationToken cancellationToken)
        {
            this.Predictor = predictor;
            this.CancellationToken = cancellationToken;
        }

        public event Action<string, decimal?> OnReportProgres;

        public void ReportProgress(string message, decimal? progress)
        {
            this.CancellationToken.ThrowIfCancellationRequested();

            this.Message = message;
            this.Progress = progress;
            this.OnReportProgres?.Invoke(message, progress);
        }


        public void SetColums(PredictorResultColumn[] columns)
        {
            this.Columns = columns.ToList();
            this.InputColumns = columns.Where(a => a.PredictorColumn.Usage == PredictorColumnUsage.Input).ToList();
            this.OutputColumns = columns.Where(a => a.PredictorColumn.Usage == PredictorColumnUsage.Output).ToList();
        }

        public (List<ResultRow> training, List<ResultRow> test) SplitTrainTest()
        {
            Random r = Predictor.Settings.Seed == null ? null : new Random(Predictor.Settings.Seed.Value);
            List<ResultRow> training = new List<ResultRow>();
            List<ResultRow> test = new List<ResultRow>();

            foreach (var item in this.MainQuery.ResultTable.Rows)
            {
                if (r.Next() < Predictor.Settings.TestPercentage)
                    test.Add(item);
                else
                    training.Add(item);
            }

            return (training, test);
        }
    }

    public class MainQuery
    {
        public QueryRequest QueryRequest { get; internal set; }
        public ResultTable ResultTable { get; internal set; }
    }

    public class MultiColumnQuery
    {
        public PredictorMultiColumnEntity MultiColumn;
        public QueryGroupRequest QueryGroupRequest;
        public ResultTable ResultTable;
        public Dictionary<Lite<Entity>, Dictionary<object[], object[]>> GroupedValues;

        public ResultColumn[] Aggregates { get; internal set; }
    }

    public abstract class PredictorAlgorithm
    {
        public virtual string ValidatePredictor(PredictorEntity predictor) => null;
        public abstract void Train(PredictorTrainingContext ctx);
        public abstract object[] Predict(PredictorEntity predictor, PredictorResultColumn[] columns, object[] input);
    }

    public abstract class ClasificationPredictorAlgorithm : PredictorAlgorithm
    {
        public override object[] Predict(PredictorEntity predictor, PredictorResultColumn[] columns, object[] input)
        {
            var singleOutput = PredictDecide(predictor, columns, input);
            return new[] { singleOutput };
        }

        public abstract object PredictDecide(PredictorEntity predictor, PredictorResultColumn[] columns, object[] input);
        public abstract Dictionary<object, double> PredictProbabilities(PredictorEntity predictor, PredictorResultColumn[] columns, object[] input);
    }

    public static class PredictorAlgorithmValidation
    {
        public static IEnumerable<PredictorColumnEmbedded> GetAllPredictorColumnEmbeddeds(this PredictorEntity predictor)
        {
            return predictor.SimpleColumns.Concat(predictor.MultiColumns.SelectMany(a => a.Aggregates));
        }
    }
}
