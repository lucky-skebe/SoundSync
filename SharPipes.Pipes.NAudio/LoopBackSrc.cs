using NAudio.Wave;
using SharPipes.Pipes.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SharPipes.Pipes.NAudio
{
    public class LoopBackSrc : PipeSrc
    {
        private readonly WasapiLoopbackCapture loopback;
        public LoopBackSrc()
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

        public override Task Start()
        {
            loopback.StartRecording();
            return Task.CompletedTask;
        }

        public override Task Stop()
        {
            loopback.StopRecording();
            return Task.CompletedTask;
        }

        public override IEnumerable<IPipeSrcPad> GetSrcPads()
        {
            yield return Src;
        }
    }
}
