// Copyright (c) Microsoft Corporation and contributors.  All Rights Reserved.  See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TorchSharp.Tensor;
using TorchSharp.NN;

namespace TorchSharp.TorchVision
{
    internal class VerticalFlip : ITransform
    {
        internal VerticalFlip()
        {
        }

        public TorchTensor forward(TorchTensor input)
        {
            return input.flipud();
        }
    }

    public static partial class Transforms
    {
        static public ITransform VerticalFlip()
        {
            return new VerticalFlip();
        }
    }
}