﻿//-----------------------------------------------------------------------
// <copyright file="jet_retrievecolumn.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.Isam.Esent.Interop
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// The native version of the <see cref="JET_RETRIEVECOLUMN"/> structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct NATIVE_RETRIEVECOLUMN
    {
        /// <summary>
        /// The column identifier for the column to retrieve.
        /// </summary>
        public uint columnid;

        /// <summary>
        /// A pointer to begin storing data that is retrieved from the
        /// column value.
        /// </summary>
        public IntPtr pvData;

        /// <summary>
        /// The size of allocation beginning at pvData, in bytes. The
        /// retrieve column operation will not store more data at pvData
        /// than cbData.
        /// </summary>
        public uint cbData;

        /// <summary>
        /// The size, in bytes, of data that is retrieved by a retrieve
        /// column operation.
        /// </summary>
        public uint cbActual;

        /// <summary>
        /// A group of bits that contain the options for column retrieval.
        /// </summary>
        public uint grbit;

        /// <summary>
        /// The offset to the first byte to be retrieved from a column of
        /// type <see cref="JET_coltyp.LongBinary"/> or
        /// <see cref="JET_coltyp.LongText"/>.
        /// </summary>
        public uint ibLongValue;

        /// <summary>
        /// The sequence number of the values that are contained in a
        /// multi-valued column. If the itagSequence is 0 then the number
        /// of instances of a multi-valued column are returned instead of
        /// any column data. 
        /// </summary>
        public uint itagSequence;

        /// <summary>
        /// The columnid of the tagged, multi-valued, or sparse column
        /// when all tagged columns are retrieved by passing 0 as the
        /// columnid.
        /// </summary>
        public uint columnidNextTagged;

        /// <summary>
        /// Error codes and warnings returned from the retrieval of the column.
        /// </summary>
        public int err;
    }

    /// <summary>
    /// Contains input and output parameters for <see cref="Api.JetRetrieveColumns"/>.
    /// Fields in the structure describe what column value to retrieve, how to
    /// retrieve it, and where to save results.
    /// </summary>
    public class JET_RETRIEVECOLUMN
    {
        /// <summary>
        /// Gets or sets the column identifier for the column to retrieve.
        /// </summary>
        public JET_COLUMNID columnid { get; set; }

        /// <summary>
        /// Gets or sets the buffer that will store data that is retrieved from the
        /// column.
        /// </summary>
        public byte[] pvData { get; set; }

        /// <summary>
        /// Gets or sets the size of the <see cref="pvData"/> buffer, in bytes. The
        /// retrieve column operation will not store more data in pvData
        /// than cbData.
        /// </summary>
        public int cbData { get; set; }

        /// <summary>
        /// Gets the size, in bytes, of data that is retrieved by a retrieve
        /// column operation.
        /// </summary>
        public int cbActual { get; private set; }

        /// <summary>
        /// Gets or sets the options for column retrieval.
        /// </summary>
        public RetrieveColumnGrbit grbit { get; set; }

        /// <summary>
        /// Gets or sets the offset to the first byte to be retrieved from a column of
        /// type <see cref="JET_coltyp.LongBinary"/> or
        /// <see cref="JET_coltyp.LongText"/>.
        /// </summary>
        public int ibLongValue { get; set; }

        /// <summary>
        /// Gets or sets the sequence number of the values that are contained in a
        /// multi-valued column. If the itagSequence is 0 then the number
        /// of instances of a multi-valued column are returned instead of
        /// any column data. 
        /// </summary>
        public int itagSequence { get; set; }

        /// <summary>
        /// Gets the columnid of the tagged, multi-valued, or sparse column
        /// when all tagged columns are retrieved by passing 0 as the
        /// columnid.
        /// </summary>
        public JET_COLUMNID columnidNextTagged { get; private set; }

        /// <summary>
        /// Gets the warnings or error code returned from the retrieval of the column.
        /// </summary>
        public JET_wrn err { get; private set; }

        /// <summary>
        /// Check to see if cbData is negative or greater than cbData.
        /// </summary>
        internal void CheckDataSize()
        {
            if (this.cbData < 0)
            {
                throw new ArgumentOutOfRangeException("cbData", "data length cannot be negative");
            }

            if ((null == this.pvData && 0 != this.cbData) || (null != this.pvData && this.cbData > this.pvData.Length))
            {
                throw new ArgumentOutOfRangeException(
                    "cbData",
                    this.cbData,
                    "cannot be greater than the length of the pvData buffer");
            }
        }

        /// <summary>
        /// Gets the NATIVE_RETRIEVECOLUMN structure that represents the object.
        /// </summary>
        /// <returns>A NATIVE_RETRIEVECOLUMN structure whose fields match the class.</returns>
        internal NATIVE_RETRIEVECOLUMN GetNativeRetrievecolumn()
        {
            var retinfo = new NATIVE_RETRIEVECOLUMN
            {
                columnid = this.columnid.Value,
                cbData = checked((uint)this.cbData),
                grbit = (uint)this.grbit,
                ibLongValue = checked((uint)this.ibLongValue),
                itagSequence = checked((uint)this.itagSequence),
            };
            return retinfo;
        }

        /// <summary>
        /// Update the output members of the class from a NATIVE_RETRIEVECOLUMN
        /// structure. This should be done after the columns are retrieved.
        /// </summary>
        /// <param name="native">
        /// The structure containing the updated output fields.
        /// </param>
        internal void UpdateFromNativeRetrievecolumn(NATIVE_RETRIEVECOLUMN native)
        {
            this.cbActual = checked((int)native.cbActual);
            this.columnidNextTagged = new JET_COLUMNID { Value = native.columnidNextTagged };
            this.err = (JET_wrn) native.err;
        }
    }
}