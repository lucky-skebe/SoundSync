using SharPipes.Pipes.Base;
using SharPipes.Pipes.Base.InteractionInfos;
using SharPipes.Pipes.Base.PipeLineDefinitions;
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

        public TimedAVGElement(string? name = null) : base(name)
        {
            Src = new PipeSrcPad<double>(this, "src");
            background_thread = new Thread(new ThreadStart(BackgroundWorker))
            {
                IsBackground = true
            };
            Sink = new PipeSinkPad<float>(this, "sink", (f) =>
            {
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

        public override IEnumerable<IInteraction> Interactions
        {
            get
            {
                yield return new IntParameterInteraction("Time (ms)", () => this.AVGMs, (value) => { this.AVGMs = value; });
            }
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

        public override string TypeName => "Timed Average";

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
            if (!Sink.IsLinked)
            {
                return GraphState.INCOMPLETE;
            }
            else if (!Src.IsLinked)
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
            if (Sink.Peer != null)
            {
                yield return Sink.Peer.Parent;
            }
        }

        public override Task TransitionReadyPlaying()
        {
            this.running = true;
            background_thread.Start();
            return Task.CompletedTask;
        }

        public override Task TransitionPlayingReady()
        {
            this.running = false;
            return Task.CompletedTask;
        }

        public override IEnumerable<IPipeSinkPad> GetSinkPads()
        {
            yield return Sink;
        }

        public override IEnumerable<IPipeSrcPad> GetSrcPads()
        {
            yield return Src;
        }

        public override IEnumerable<PropertyValue> GetPropertyValues()
        {
            yield return new PropertyValue(nameof(AVGMs), AVGMs);
        }

        public override IPipeSrcPad? GetSrcPad(string fromPad)
            => fromPad.ToLower() switch
            {
                "src" => this.Src,
                _ => null
            };

        public override IPipeSinkPad? GetSinkPad(string toPad)
            => toPad.ToLower() switch
            {
                "sink" => this.Sink,
                _ => null
            };

        protected override IEnumerable<IPropertySetter> GetPropertySetters()
        {
            yield return new PropertySetter<int>(() => AVGMs);
        }
    }
}
