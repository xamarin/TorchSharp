// Copyright (c) Microsoft Corporation and contributors.  All Rights Reserved.  See License.txt in the project root for license information.
using System;
using System.Runtime.InteropServices;
using TorchSharp.Tensor;

namespace TorchSharp.NN
{
    /// <summary>
    /// This class is used to represent a Mish module.
    /// </summary>
    public class Mish : Module
    {
        internal Mish (IntPtr handle, IntPtr boxedHandle) : base (handle, boxedHandle) { }

        [DllImport ("LibTorchSharp")]
        private static extern IntPtr THSNN_Mish_forward (Module.HType module, IntPtr tensor);

        public override TorchTensor forward (TorchTensor tensor)
        {
            var res = THSNN_Mish_forward (handle, tensor.Handle);
            if (res == IntPtr.Zero) { Torch.CheckForErrors(); }
            return new TorchTensor (res);
        }

        public override string GetName ()
        {
            return typeof (Mish).Name;
        }
    }

    public static partial class Modules
    {
        [DllImport ("LibTorchSharp")]
        extern static IntPtr THSNN_Mish_ctor(out IntPtr pBoxedModule);

        /// <summary>
        /// A Self Regularized Non-Monotonic Neural Activation Function.
        /// </summary>
        /// <returns></returns>
        static public Mish Mish ()
        {
            var handle = THSNN_Mish_ctor (out var boxedHandle);
            if (handle == IntPtr.Zero) { Torch.CheckForErrors(); }
            return new Mish (handle, boxedHandle);
        }
    }
    public static partial class Functions
    {
        /// <summary>
        /// A Self Regularized Non-Monotonic Neural Activation Function.
        /// </summary>
        /// <param name="x">The input tensor</param>
        /// <returns></returns>
        static public TorchTensor Mish (TorchTensor x)
        {
            using (var m = Modules.Mish()) {
                return m.forward (x);
            }
        }
    }

}