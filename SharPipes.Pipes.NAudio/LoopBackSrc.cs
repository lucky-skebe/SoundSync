using NAudio.Wave;
using SharPipes.Pipes.Base;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace SharPipes.Pipes.NAudio
{
    [Export(typeof(IPipeElement))]
    public class LoopBackSrc : PipeSrc
    {
        private readonly WasapiLoopbackCapture loopback;
        public LoopBackSrc(string? name = null) : base(name)
        {
            loopback = new WasapiLoopbackCapture();
            loopback.DataAvailable += Loopback_DataAvailable;
            Src = new PipeSrcPad<float>(this, "src");
        }



        private void Loopback_DataAvailable(object sender, WaveInEventArgs args)
        {
            var buffer = new WaveBuffer(args.Buffer);

            for (int index = 0; index < args.BytesRecorded / 4; index++)
            {
                var sample = buffer.FloatBuffer[index];

                Src.Push(sample);
            }

        }

        public PipeSrcPad<float> Src;

        public override PipeSrcPad<TValue>? GetSrc<TValue>(string name)
        {
            return null;
        }

        public override GraphState Check()
        {
            if (Src.IsLinked)
            {
                return GraphState.OK;
            }
            else
            {
                return GraphState.INCOMPLETE;
            }
        }


        public override string TypeName => "Audio Loopback";
        public override IEnumerable<IPipeElement> GetPrevNodes()
        {
            return Enumerable.Empty<IPipeElement>();
        }

        protected override Task TransitionReadyPlaying()
        {
            loopback.StartRecording();
            return Task.CompletedTask;
        }

        protected override Task TransitionPlayingReady()
        {
            loopback.StopRecording();
            return Task.CompletedTask;
        }

        public override IEnumerable<IPipeSrcPad> GetSrcPads()
        {
            yield return Src;
        }

        public override IPipeSrcPad? GetSrcPad(string fromPad)
            => fromPad.ToLower() switch
            {
                "src" => this.Src,
                _ => null
            };

        public override IPipeSinkPad? GetSinkPad(string toPad)
            => null;

    }
}
