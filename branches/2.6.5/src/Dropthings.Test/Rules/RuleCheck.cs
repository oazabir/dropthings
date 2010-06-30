namespace Dropthings.Test.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Microsoft.VisualStudio.TestTools.WebTesting;

    internal class RuleCheck : IDisposable
    {
        #region Fields

        private bool _StopOnError = true;
        private ValidationEventArgs _e;

        #endregion Fields

        #region Constructors

        public RuleCheck(ValidationEventArgs e, bool stopOnError)
        {
            this._e = e;
            this._StopOnError = stopOnError;
        }

        #endregion Constructors

        #region Properties

        public bool StopOnError
        {
            get { return _StopOnError; }
            set { _StopOnError = value; }
        }

        #endregion Properties

        #region Methods

        void IDisposable.Dispose()
        {
            // If the validation rule failed and it is requested to stop the web test on failure, then
            // stop the web test from further testing
            if (!this._e.IsValid && this._StopOnError)
                this._e.WebTest.Stop();
        }

        #endregion Methods
    }
}