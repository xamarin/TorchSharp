// Copyright (c) Microsoft Corporation and contributors.  All Rights Reserved.  See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static TorchSharp.torch;

using static TorchSharp.TensorExtensionMethods;

namespace TorchSharp.torchvision
{
    internal class AdjustSharpness : ITransform
    {
        internal AdjustSharpness(double sharpness)
        {
            if (sharpness < 0.0)
                throw new ArgumentException($"The sharpness factor ({sharpness}) must be non-negative.");
            this.sharpness = sharpness;
        }

        public Tensor forward(Tensor input)
        {
            if (input.shape[input.shape.Length - 1] <= 2 || input.shape[input.shape.Length - 2] <= 2)
                return input;

            return Blend(input, BlurredDegenerateImage(input), sharpness);
        }

        private Tensor BlurredDegenerateImage(Tensor input)
        {
            var device = input.device;
            var dtype = input.IsIntegral() ? ScalarType.Float32 : input.dtype;
            var kernel = Float32Tensor.ones(3, 3, device: device);
            kernel[1, 1] = Float32Tensor.from(5.0f);
            kernel /= kernel.sum();
            kernel = kernel.expand(input.shape[input.shape.Length - 3], 1, kernel.shape[0], kernel.shape[1]);

            var result_tmp = SqueezeIn(input, out var needCast, out var needSqueeze, out var out_dtype);
            result_tmp = torch.nn.functional.conv2d(result_tmp,kernel, groups: result_tmp.shape[result_tmp.shape.Length - 3]);
            result_tmp = SqueezeOut(result_tmp, needCast, needSqueeze, out_dtype);

            var result = input.clone();
            result.index_put_(result_tmp, TensorIndex.Ellipsis, TensorIndex.Slice(1,-1), TensorIndex.Slice(1, -1));
            return result;
        }

        protected double sharpness;


        private Tensor Blend(Tensor img1, Tensor img2, double ratio)
        {
            var bound = img1.IsIntegral() ? 255.0 : 1.0;
            return (img1 * ratio + img2 * (1.0 - ratio)).clamp(0, bound).to(img2.dtype);
        }

        private Tensor SqueezeIn(Tensor img, out bool needCast, out bool needSqueeze, out ScalarType dtype)
        {
            needSqueeze = false;

            if (img.Dimensions < 4) {
                img = img.unsqueeze(0);
                needSqueeze = true;
            }

            dtype = img.dtype;
            needCast = false;

            if (dtype != ScalarType.Float32 && dtype != ScalarType.Float64) {
                needCast = true;
                img = img.to_type(ScalarType.Float32);
            }

            return img;
        }

        private Tensor SqueezeOut(Tensor img, bool needCast, bool needSqueeze, ScalarType dtype)
        {
            if (needSqueeze) {
                img = img.squeeze(0);
            }

            if (needCast) {
                if (TensorExtensionMethods.IsIntegral(dtype))
                    img = img.round();

                img = img.to_type(dtype);
            }

            return img;
        }
    }

    public static partial class transforms
    {
        /// <summary>
        /// Adjust the sharpness of the image. 
        /// </summary>
        /// <param name="sharpness">
        /// How much to adjust the sharpness. Can be any non negative number.
        /// 0 gives a blurred image, 1 gives the original image while 2 increases the sharpness by a factor of 2.
        /// </param>
        /// <returns></returns>
        static public ITransform AdjustSharpness(double sharpness)
        {
            return new AdjustSharpness(sharpness);
        }
    }
}
