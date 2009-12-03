namespace Dropthings.Web.Util
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using System.Xml.Linq;

    public class StaticContentFilter : Stream
    {
        #region Fields

        private static readonly char[] HREF_ATTRIBUTE = "href".ToCharArray();
        private static readonly char[] HTTP_PREFIX = "http://".ToCharArray();
        private static readonly char[] IMG_TAG = "img".ToCharArray();
        private static readonly char[] LINK_TAG = "link".ToCharArray();
        private static readonly char[] SCRIPT_TAG = "script".ToCharArray();
        private static readonly char[] SRC_ATTRIBUTE = "src".ToCharArray();

        private byte[] _CssPrefix;
        Encoding _Encoding;
        private byte[] _ImagePrefix;
        private byte[] _JavascriptPrefix;
        private char[] _ApplicationPath;

        /// <summary>
        /// Holds characters from last Write(...) call where the start tag did not
        /// end and thus the remaining characters need to be preserved in a buffer so 
        /// that a complete tag can be parsed
        /// </summary>
        char[] _PendingBuffer = null;
        long _Position;
        Stream _ResponseStream;
        StringBuilder debug = new StringBuilder();

        #endregion Fields

        #region Constructors

        public StaticContentFilter(HttpResponse response, string imagePrefix, string javascriptPrefix, string cssPrefix)
        {
            this._Encoding = response.Output.Encoding;
            this._ResponseStream = response.Filter;

            this._ImagePrefix = _Encoding.GetBytes(imagePrefix);
            this._JavascriptPrefix = _Encoding.GetBytes(javascriptPrefix);
            this._CssPrefix = _Encoding.GetBytes(cssPrefix);

            this._ApplicationPath = HttpContext.Current.Request.ApplicationPath.ToCharArray();
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
            get { return 0; }
        }

        public override long Position
        {
            get { return _Position; }
            set { _Position = value; }
        }

        #endregion Properties

        #region Methods

        public override void Close()
        {
            this.FlushPendingBuffer();
            _ResponseStream.Close();
        }

        public override void Flush()
        {
            this.FlushPendingBuffer();
            _ResponseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _ResponseStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _ResponseStream.Seek(offset, origin);
        }

        public override void SetLength(long length)
        {
            _ResponseStream.SetLength(length);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            char[] content;
            char[] charBuffer = this._Encoding.GetChars(buffer, offset, count);

            /// If some bytes were left for processing during last Write call
            /// then consider those into the current buffer
            if (null != this._PendingBuffer)
            {
                content = new char[charBuffer.Length + this._PendingBuffer.Length];
                Array.Copy(this._PendingBuffer, 0, content, 0, this._PendingBuffer.Length);
                Array.Copy(charBuffer, 0, content, this._PendingBuffer.Length, charBuffer.Length);
                this._PendingBuffer = null;
            }
            else
            {
                content = charBuffer;
            }

            int lastPosWritten = 0;
            for (int pos = 0; pos < content.Length; pos++)
            {
                // See if tag start
                char c = content[pos];
                if ('<' == c)
                {
                    pos++;
                    /* Make sure there are enough characters available in the buffer to finish
                     * tag start. This will happen when a tag partially starts but does not end
                     * For example, a partial img tag like <img
                     * We need a complete tag upto the > character.
                    */
                    if (HasTagEnd(content, pos))
                    {
                        if ('/' == content[pos])
                        {

                        }
                        else
                        {
                            if (HasMatch(content, pos, IMG_TAG))
                            {
                                lastPosWritten = this.WritePrefixIf(SRC_ATTRIBUTE,
                                    content, pos, lastPosWritten, this._ImagePrefix);
                            }
                            else if (HasMatch(content, pos, SCRIPT_TAG))
                            {
                                lastPosWritten = this.WritePrefixIf(SRC_ATTRIBUTE,
                                    content, pos, lastPosWritten, this._JavascriptPrefix);
                            }
                            else if (HasMatch(content, pos, LINK_TAG))
                            {
                                lastPosWritten = this.WritePrefixIf(HREF_ATTRIBUTE,
                                    content, pos, lastPosWritten, this._CssPrefix);
                            }

                            // If buffer was written beyond current position, skip
                            // upto the position that was written
                            if (lastPosWritten > pos)
                                pos = lastPosWritten;
                        }
                    }
                    else
                    {
                        // a tag started but it did not end in this buffer. Preserve the content
                        // in a buffer. On next write call, we will take an attempt to check it again
                        this._PendingBuffer = new char[content.Length - pos];
                        Array.Copy(content, pos, this._PendingBuffer, 0, content.Length - pos);

                        // Write from last write position upto pos. the rest is now in pending buffer
                        // will be processed later
                        this.WriteOutput(content, lastPosWritten, pos - lastPosWritten);

                        return;
                    }
                }
            }

            // Write whatever is left in the buffer from last write pos to the end of the buffer
            this.WriteOutput(content, lastPosWritten, content.Length - lastPosWritten);
        }

        private int FindAttributeValuePos(char[] attributeName, char[] content, int pos)
        {
            for (int i = pos; i < content.Length - attributeName.Length; i++)
            {
                // Tag closing reached but the attribute was not found
                if ('>' == content[i]) return -1;

                if (HasMatch(content, i, attributeName))
                {
                    pos = i + attributeName.Length;

                    // find the position of the double quote from where value is started
                    // We won't allow value without double quote, not even single quote.
                    // The content must be XHTML valid for now.
                    while ('"' != content[pos++]);

                    return pos;
                }
            }

            return -1;
        }

        private void FlushPendingBuffer()
        {
            /// Some characters were left in the buffer
            if (null != this._PendingBuffer)
            {
                this.WriteOutput(this._PendingBuffer, 0, this._PendingBuffer.Length);
                this._PendingBuffer = null;
            }
        }

        private bool HasMatch(char[] content, int pos, char[] match)
        {
            for (int i = 0; i < match.Length; i++)
                if (content[pos + i] != match[i]
                    && content[pos + i] != char.ToUpper(match[i]))
                    return false;

            return true;
        }

        private bool HasTagEnd(char[] content, int pos)
        {
            for (; pos < content.Length; pos++)
                if ('>' == content[pos])
                    return true;

            return false;
        }

        private void WriteBytes(byte[] bytes, int pos, int length)
        {
            this._ResponseStream.Write(bytes, 0, bytes.Length);
        }

        private void WriteOutput(char[] content, int pos, int length)
        {
            if (length == 0) return;

            debug.Append(content, pos, length);
            byte[] buffer = this._Encoding.GetBytes(content, pos, length);
            this.WriteBytes(buffer, 0, buffer.Length);
        }

        private void WriteOutput(string content)
        {
            debug.Append(content);
            byte[] buffer = this._Encoding.GetBytes(content);
            this.WriteBytes(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Write the prefix if the specified attribute was found and the attribute has a value
        /// that does not start with http:// prefix.
        /// If atttribute is not found, it just returns the lastWritePos as it is
        /// If attribute was found but the attribute already has a fully qualified URL, then return lastWritePos as it is
        /// If attribute has relative URL, then lastWritePos is the starting position of the attribute value. However,
        /// content from lastWritePos to position of the attribute value will already be written to output
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="content"></param>
        /// <param name="pos"></param>
        /// <param name="lastWritePos"></param>
        /// <param name="prefix"></param>
        /// <returns>The last position upto which content was written.</returns>
        private int WritePrefixIf(char[] attributeName, char[] content, int pos, int lastWritePos, byte[] prefix)
        {
            // write upto the position where image source tag comes in
            int attributeValuePos = this.FindAttributeValuePos(attributeName, content, pos);

            // ensure attribute was found
            if (attributeValuePos > 0)
            {
                if (HasMatch(content, attributeValuePos, HTTP_PREFIX))
                {
                    // We already have an absolute URL. So, nothing to do
                    return lastWritePos;
                }
                else
                {
                    // It's a relative URL. So, let's prefix the URL with the
                    // static domain name

                    // First, write content upto this position
                    this.WriteOutput(content, lastWritePos, attributeValuePos - lastWritePos);

                    // Now write the prefix
                    this.WriteBytes(prefix, 0, prefix.Length);

                    // If the attribute value starts with the application path it needs to be skipped as 
                    // that value should be in the prefix. Doubling it will cause problems. This occurs
                    // with some of the scripts.
                    if (HasMatch(content, attributeValuePos, _ApplicationPath))
                        attributeValuePos = attributeValuePos + _ApplicationPath.Length;

                    // Ensure the attribute value does not start with a leading slash because the prefix
                    // is supposed to have a trailing slash. If value does start with a leading slash,
                    // skip it
                    if ('/' == content[attributeValuePos]) attributeValuePos++;

                    return attributeValuePos;
                }
            }
            else
            {
                return lastWritePos;
            }
        }

        #endregion Methods
    }
}