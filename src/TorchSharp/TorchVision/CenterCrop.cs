// Copyright (c) Microsoft Corporation and contributors.  All Rights Reserved.  See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static TorchSharp.torch;


namespace TorchSharp.torchvision
{
    internal class CenterCrop : ITransform
    {
        internal CenterCrop(int height, int width)
        {
            this.height = height;
            this.width = width;
        }

        public Tensor forward(Tensor input)
        {
            var hoffset = input.Dimensions - 2;
            var iHeight = input.shape[hoffset];
            var iWidth = input.shape[hoffset + 1];

            var top = (int)(iHeight - height) / 2;
            var left = (int)(iWidth - width) / 2;

            return input.crop(top, left, height, width);
        }

        protected int height, width;
    }

    public static partial class transforms
    {
        /// <summary>
        /// Crop the center of the image.
        /// </summary>
        static public ITransform CenterCrop(int height, int width)
        {
            return new CenterCrop(height, width);
        }

        /// <summary>
        /// Crop the center of the image.
        /// </summary>
        static public ITransform CenterCrop(int size)
        {
            return new CenterCrop(size, size);
        }
    }
}
