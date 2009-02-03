namespace AJAXASMXHandler
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;

    public class ResponseFilter : Stream
    {
        #region Fields

        long length;
        Stream responseStream;

        #endregion Fields

        #region Constructors

        public ResponseFilter(HttpResponse response)
        {
            this.responseStream = response.Filter;
        }

        #endregion Constructors

        #region Properties

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override long Length
        {
            get { return length; }
        }

        public override long Position
        {
            get { return 0; }
            set { }
        }

        #endregion Properties

        #region Methods

        public override void Close()
        {
            responseStream.Close();
        }

        public override void Flush()
        {
            responseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return responseStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return responseStream.Seek(offset, origin);
        }

        public override void SetLength(long length)
        {
            responseStream.SetLength(length);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this.responseStream.Write(buffer, offset, count);
            length += count;
        }

        #endregion Methods
    }
}