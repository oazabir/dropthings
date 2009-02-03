#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

namespace AJAXASMXHandler
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Transactions;

    public class TransactionalMethodAttribute : Attribute
    {
        #region Fields

        private IsolationLevel _IsolationLevel = IsolationLevel.ReadCommitted;
        private int _Timeout = 30;
        private TransactionScopeOption _TransactionOption = TransactionScopeOption.Required;

        #endregion Fields

        #region Constructors

        public TransactionalMethodAttribute(TransactionScopeOption option, IsolationLevel isolationLevel, int timeout)
        {
            this.Timeout = timeout;
            this.IsolationLevel = isolationLevel;
            this.TransactionOption = option;
        }

        public TransactionalMethodAttribute(int timeout)
        {
            this.Timeout = timeout;
        }

        public TransactionalMethodAttribute(TransactionScopeOption option)
        {
            this.TransactionOption = option;
        }

        public TransactionalMethodAttribute(TransactionScopeOption option, IsolationLevel isolationLevel)
        {
            this.TransactionOption = option;
            this.IsolationLevel = isolationLevel;
        }

        public TransactionalMethodAttribute(IsolationLevel isolationLevel)
        {
            this.IsolationLevel = isolationLevel;
        }

        public TransactionalMethodAttribute()
        {
        }

        #endregion Constructors

        #region Properties

        public IsolationLevel IsolationLevel
        {
            get { return _IsolationLevel; }
            set { _IsolationLevel = value; }
        }

        public int Timeout
        {
            get { return _Timeout; }
            set { _Timeout = value; }
        }

        public TransactionScopeOption TransactionOption
        {
            get { return _TransactionOption; }
            set { _TransactionOption = value; }
        }

        #endregion Properties
    }
}