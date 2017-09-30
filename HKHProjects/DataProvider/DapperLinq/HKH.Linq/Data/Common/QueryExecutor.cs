﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// This source code is made available under the terms of the Microsoft Public License (MS-PL)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HKH.Linq.Data.Common
{
    public interface ICreateExecutor
    {
        QueryExecutor CreateExecutor();
    }

    public abstract class QueryExecutor
    {
        // called from compiled execution plan
        public abstract int RowsAffected { get; }
        public abstract IEnumerable<T> Execute<T>(QueryCommand command, object[] paramValues);
        public abstract IEnumerable<int> ExecuteBatch(QueryCommand query, IEnumerable<object[]> paramSets, int batchSize, bool stream);
        public abstract IEnumerable<T> ExecuteBatch<T>(QueryCommand query, IEnumerable<object[]> paramSets, int batchSize, bool stream);
        public abstract IEnumerable<T> ExecuteDeferred<T>(QueryCommand query, object[] paramValues);
        public abstract int ExecuteCommand(QueryCommand query, object[] paramValues);
    }
}