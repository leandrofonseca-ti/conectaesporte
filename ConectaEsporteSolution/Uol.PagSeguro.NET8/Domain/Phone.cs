﻿// Copyright [2011] [PagSeguro Internet Ltda.]
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System;
using Uol.PagSeguro.NET8.Util;

namespace Uol.PagSeguro.NET8.Domain
{
    /// <summary>
    /// Represents a phone number
    /// </summary>
    public class Phone
    {
        /// <summary>
        /// Initializes a new instance of the Phone class
        /// </summary>
        public Phone()
        {
        }

        /// <summary>
        /// Initializes a new instance of the Phone class
        /// </summary>
        /// <param name="areaCode"></param>
        /// <param name="number"></param>
        public Phone(string areaCode, string number)
        {
            this.AreaCode = PagSeguroUtil.GetOnlyNumbers(areaCode);
            this.Number = PagSeguroUtil.GetOnlyNumbers(number);
        }

        /// <summary>
        /// Area code
        /// </summary>
        public string AreaCode
        {
            get;
            set;
        }

        /// <summary>
        /// Phone number
        /// </summary>
        public string Number
        {
            get;
            set;
        }
    }
}