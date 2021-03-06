﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BagOUtils.Guards.Messages;

namespace BagOUtils.Guards
{
    /// <summary>
    /// Defend against invalid states/parameters/arguments when the
    /// state/parameter/argument is a numeric value. Each of the methods
    /// defines a "guard". The guard validates that the
    /// state/parameter/argument does not have a particular type of
    /// potential error. If the guard finds the state/parameter/argument
    /// to be in error, an exception is thrown. If the value is
    /// validated, the value is returned, allowing for a fluent guard API.
    /// </summary>
    public static class NumericGuards
    {
        /// <summary>
        /// Guard that a numeric value is equal to or above the minimum value.
        /// </summary>
        /// <param name="value">
        /// Value to test.
        /// </param>
        /// <param name="argumentName">
        /// Name of argument/parameter/state being tested.
        /// </param>
        /// <param name="minimumValue">
        /// Minimum acceptable value.
        /// </param>
        /// <returns>
        /// The value is returned if it is valid.
        /// </returns>
        public static T GuardMinimum<T>(this T value, string argumentName, T minimumValue)
            where T : IComparable<T>
        {
            Func<string> messageBuilder = () =>
            {
                var message = CustomTemplate
                .BelowMinimum
                .UsingItem(argumentName)
                .UsingValue(value)
                .ComparedTo(minimumValue)
                .Prepare();
                return message;
            };

            return value
                .GuardMinimumWithMessage(argumentName, minimumValue, messageBuilder);
        }

        /// <summary>
        /// Guard that a numeric value is equal to or above the minimum value.
        /// </summary>
        /// <param name="value">
        /// Value to test.
        /// </param>
        /// <param name="argumentName">
        /// Name of argument/parameter/state being tested.
        /// </param>
        /// <param name="minimumValue">
        /// Minimum acceptable value.
        /// </param>
        /// <returns>
        /// The value is returned if it is valid.
        /// </returns>
        public static T GuardMinimumWithMessage<T>(this T value, string argumentName, T minimumValue, Func<string> messageBuilder)
            where T : IComparable<T>
        {
            if (value.LessThan(minimumValue))
            {
                throw new ArgumentOutOfRangeException(argumentName, messageBuilder());
            }

            return value;
        }

        /// <summary>
        /// Guard that a numeric value is equal to or less than the
        /// maximum value.
        /// </summary>
        /// <param name="value">
        /// Value to test.
        /// </param>
        /// <param name="argumentName">
        /// Name of argument/parameter/state being tested.
        /// </param>
        /// <param name="minimumValue">
        /// Maximum acceptable value.
        /// </param>
        /// <returns>
        /// The value is returned if it is valid.
        /// </returns>
        public static T GuardMaximum<T>(this T value, string argumentName, T maximumValue)
            where T : IComparable<T>
        {
            Func<string> messageBuilder = () =>
            {
                var message = CustomTemplate
                .AboveMaximum
                .UsingItem(argumentName)
                .UsingValue(value)
                .ComparedTo(maximumValue)
                .Prepare();
                return message;
            };

            return value
                .GuardMaximumWithMessage(argumentName, maximumValue, messageBuilder);
        }

        /// <summary>
        /// Guard that a numeric value is equal to or less than the
        /// maximum value and use the provided message if it is not.
        /// </summary>
        /// <param name="value">
        /// Value to test.
        /// </param>
        /// <param name="argumentName">
        /// Name of argument/parameter/state being tested.
        /// </param>
        /// <param name="minimumValue">
        /// Maximum acceptable value.
        /// </param>
        /// <returns>
        /// The value is returned if it is valid.
        /// </returns>
        public static T GuardMaximumWithMessage<T>(this T value, string argumentName, T maximumValue, Func<string> messageBuilder)
            where T : IComparable<T>
        {
            if (value.GreaterThan(maximumValue))
            {
                throw new ArgumentOutOfRangeException(argumentName, maximumValue, messageBuilder());
            }

            return value;
        }

        /// <summary>
        /// Validate that the value is within the provided limits, inclusive.
        /// </summary>
        /// <param name="value">
        /// Value to test.
        /// </param>
        /// <param name="argumentName">
        /// Name of argument/parameter.
        /// </param>
        /// <param name="lowerLimit">
        /// Lower limit of valid values.
        /// </param>
        /// <param name="upperLimit">
        /// Upper limit of valid values.
        /// </param>
        public static T GuardInRange<T>(this T value, string argumentName, T lowerLimit, T upperLimit)
            where T : IComparable<T>
        {
            Func<string> messageBuilder = () =>
            {
                var message = CustomTemplate
                .OutOfRange
                .UsingItem(argumentName)
                .WithMinimum(lowerLimit)
                .WithMaximum(upperLimit)
                .Prepare();
                return message;
            };

            return value
                .GuardInRangeWithMessage(argumentName, lowerLimit, upperLimit, messageBuilder);
        }

        /// <summary>
        /// Validate that the value is within the provided limits, inclusive.
        /// </summary>
        /// <param name="value">
        /// Value to test.
        /// </param>
        /// <param name="argumentName">
        /// Name of argument/parameter.
        /// </param>
        /// <param name="lowerLimit">
        /// Lower limit of valid values.
        /// </param>
        /// <param name="upperLimit">
        /// Upper limit of valid values.
        /// </param>
        public static T GuardInRangeWithMessage<T>(this T value, string argumentName, T lowerLimit, T upperLimit, Func<string> messageBuilder)
            where T : IComparable<T>
        {
            if (value.LessThan(lowerLimit) || value.GreaterThan(upperLimit))
            {
                throw new ArgumentOutOfRangeException(argumentName, messageBuilder());
            }
            return value;
        }
    }
}
