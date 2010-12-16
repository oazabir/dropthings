using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Dropthings.RestApi
{
    class StreamWrapper : Stream, IDisposable
    {
        private Stream WrappedStream;
        private Action<byte[]> OnCompleteRead;
        private MemoryStream InternalBuffer;

        public StreamWrapper(Stream stream, int internalBufferCapacity, Action<byte[]> onCompleteRead)
        {
            this.WrappedStream = stream;
            this.OnCompleteRead = onCompleteRead;
            this.InternalBuffer = new MemoryStream(internalBufferCapacity);
        }
        public override bool CanRead
        {
            get { return this.WrappedStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return this.WrappedStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return this.WrappedStream.CanWrite; }
        }

        public override void Flush()
        {
            this.WrappedStream.Flush();
        }

        public override long Length
        {
            get { return this.WrappedStream.Length; }
        }

        public override long Position
        {
            get
            {
                return this.WrappedStream.Position;
            }
            set
            {
                this.WrappedStream.Position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int bytesRead = this.WrappedStream.Read(buffer, offset, count);

            if (bytesRead > 0)
                this.InternalBuffer.Write(buffer, offset, bytesRead);
            else
                this.OnCompleteRead(this.InternalBuffer.ToArray());

            return bytesRead;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return this.WrappedStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            this.WrappedStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this.WrappedStream.Write(buffer, offset, count);
        }

        public new void Dispose()
        {
            this.WrappedStream.Dispose();
        }
    }
}
