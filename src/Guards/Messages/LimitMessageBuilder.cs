﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagOUtils.Guards.Messages
{
    public class LimitMessageBuilder<T>
        where T : IComparable<T>
    {
        private T value;
        private T min;
        private T max;
        private bool isMinComparison;
        private bool isMaxComparison;

        internal LimitMessageBuilder(T value)
        {
            this.value = value;
        }

        //-------------------------------------------------------------------------
        //
        // Fluent API
        //
        //-------------------------------------------------------------------------

        public LimitMessageBuilder<T> WithMin(T min)
        {
            this.min = min;
            this.isMinComparison = true;
            return this;
        }

        public LimitMessageBuilder<T> MaybeMin(bool isUsed, T min)
        {
            if (isUsed)
            {
                this.min = min;
                this.isMinComparison = true;
            }
            return this;
        }

        public LimitMessageBuilder<T> WithMax(T max)
        {
            this.max = max;
            this.isMaxComparison = true;
            return this;
        }

        public LimitMessageBuilder<T> MaybeMax(bool isUsed, T max)
        {
            if(isUsed)
            {
                this.max = max;
                this.isMaxComparison = true;
            }
            return this;
        }

        public string BuildMessage()
        {
            var template = LimitMessageTemplate.GetTemplate(this.isMinComparison, this.isMaxComparison);
            return string.Format(template, value, this.min, this.max);
        }
    }
}