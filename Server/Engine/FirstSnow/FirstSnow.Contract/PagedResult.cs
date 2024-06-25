﻿using System.Collections.Generic;

namespace FirstSnow.Contract.Model
{
    public class PagedResult<T>
    {
        public PagedResult(IEnumerable<T> data, int allCount)
        {
            Data = data;
            PageCount = allCount;
        }
        public IEnumerable<T> Data { get; }
        public int PageCount { get; }
    }

}