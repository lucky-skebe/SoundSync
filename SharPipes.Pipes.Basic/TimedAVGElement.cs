using SharPipes.Pipes.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharPipes.Pipes.Basic
{
    public class TimedAVGElement : PipeTransform
    {
        float accumulator = 0;
        int count = 0;

        private readonly Thread background_thread;

        public TimedAVGElement()
        {
            Src = new PipeSrcPad<double>(this);
            background_thread = new Thread(new ThreadStart(BackgroundWorker))
            {
                IsBackground = true
            };
            Sink = new PipeSinkPad<float>(this, (f) => {
                lock (this)
                {
                    accumulator += Math.Abs(f);
                    count += 1;
                }
            });
        }

        private bool running = false;

        private void BackgroundWorker()
        {
            while (running)
            {
                Thread.Sleep(AVGMs);
                double avg = GetAverageSample();

                Src.Push(avg);
            }
        }

        public double GetAverageSample()
        {
            lock (this)
            {
                double avg = accumulator / count;
                count = 0;
                accumulator = 0.0f;
                return avg;
            }
        }

        public override IEnumerable<ParameterInfo> DescribeParameters()
        {
            yield return new ParameterInfo("Time (ms)", ParameterType.Float, "AVGMs");
        }

        private int _AVGMs = 50;

        public int AVGMs
        {
            get { return _AVGMs; }
            set { _AVGMs = value; }
        }


        public PipeSinkPad<float> Sink
        {
            get;
            set;
        }

        public PipeSrcPad<double> Src
        {
            get;
            set;
        }

        public override PipeSinkPad<TValue>? GetSink<TValue>(string name)
        {
            return null;
        }

        public override PipeSrcPad<TValue>? GetSrc<TValue>(string name)
        {
            return null;
        }

        public override GraphState Check()
        {
            if (Sink.Edge == null)
            {
                return GraphState.INCOMPLETE;
            }
            else if(Src.Edge == null)
            {
                return GraphState.INCOMPLETE;
            }
            else
            {
                return GraphState.OK;
            }
        }

        public override IEnumerable<IPipeElement> GetPrevNodes()
        {
            if (Sink.Edge != null)
            {
                yield return Sink.Edge.From.Parent;
            }
        }

        public override Task Start()
        {
            this.running = true;
            background_thread.Start();
            return Task.CompletedTask;
        }

        public override Task Stop()
        {
            this.running = false;
            return Task.CompletedTask;
        }
    }
}
