using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;


namespace TorchSharp
{
    using Tensor;

    // This file contains the mathematical operators on TorchTensor

    public enum FFTNormType
    {
        Backward = 0,
        Forward = 1,
        Ortho = 2
    }

    public static partial class torch
    {
        public static partial class fft
        {
            [DllImport("LibTorchSharp")]
            static extern IntPtr THSTensor_fft(IntPtr tensor, long n, long dim, sbyte norm);

            /// <summary>
            /// Computes the one dimensional discrete Fourier transform of input.
            /// </summary>
            /// <param name="input">The input tensor</param>
            /// <param name="n">Signal length. If given, the input will either be zero-padded or trimmed to this length before computing the FFT.</param>
            /// <param name="dim">The dimension along which to take the one dimensional FFT.</param>
            /// <param name="norm">Normalization mode.</param>
            /// <returns></returns>
            /// <remarks>The name was changed because it would conflict with its surrounding scope. That's not legal in .NET.</remarks>
            public static TorchTensor fft_(TorchTensor input, long n = -1, long dim = -1, FFTNormType norm = FFTNormType.Backward)
            {
                var res = THSTensor_fft(input.Handle, n, dim, (sbyte)norm);
                if (res == IntPtr.Zero) { torch.CheckForErrors(); }
                return new TorchTensor(res);
            }

            [DllImport("LibTorchSharp")]
            static extern IntPtr THSTensor_ifft(IntPtr tensor, long n, long dim, sbyte norm);

            public static TorchTensor ifft(TorchTensor input, long n = -1, long dim = -1, FFTNormType norm = FFTNormType.Backward)
            {
                var res = THSTensor_ifft(input.Handle, n, dim, (sbyte)norm);
                if (res == IntPtr.Zero) { torch.CheckForErrors(); }
                return new TorchTensor(res);
            }

            [DllImport("LibTorchSharp")]
            static extern IntPtr THSTensor_fft2(IntPtr tensor, IntPtr s, IntPtr dim, sbyte norm);

            public static TorchTensor fft2(TorchTensor input, long[] s = null, long[] dim = null, FFTNormType norm = FFTNormType.Backward)
            {
                if (input.Dimensions < 2) throw new ArgumentException("fft2() input should be at least 2D");
                if (dim == null) dim = new long[] { -2, -1 };
                unsafe {
                    fixed (long* ps = s, pDim = dim) {
                        var res = THSTensor_fft2(input.Handle, (IntPtr)ps, (IntPtr)pDim, (sbyte)norm);
                        if (res == IntPtr.Zero) { torch.CheckForErrors(); }
                        return new TorchTensor(res);
                    }
                }
            }

            [DllImport("LibTorchSharp")]
            static extern IntPtr THSTensor_ifft2(IntPtr tensor, IntPtr s, IntPtr dim, sbyte norm);

            public static TorchTensor ifft2(TorchTensor input, long[] s = null, long[] dim = null, FFTNormType norm = FFTNormType.Backward)
            {
                if (input.Dimensions < 2) throw new ArgumentException("ifft2() input should be at least 2D");
                if (dim == null) dim = new long[] { -2, -1 };
                unsafe {
                    fixed (long* ps = s, pDim = dim) {
                        var res = THSTensor_ifft2(input.Handle, (IntPtr)ps, (IntPtr)pDim, (sbyte)norm);
                        if (res == IntPtr.Zero) { torch.CheckForErrors(); }
                        return new TorchTensor(res);
                    }
                }
            }

            [DllImport("LibTorchSharp")]
            static extern IntPtr THSTensor_fftn(IntPtr tensor, IntPtr s, int s_length, IntPtr dim, int dim_length, sbyte norm);

            public static TorchTensor fftn(TorchTensor input, long[] s = null, long[] dim = null, FFTNormType norm = FFTNormType.Backward)
            {
                var slen = (s == null) ? 0 : s.Length;
                var dlen = (dim == null) ? 0 : dim.Length;
                unsafe {
                    fixed (long* ps = s, pDim = dim) {
                        var res = THSTensor_fftn(input.Handle, (IntPtr)ps, slen, (IntPtr)pDim, dlen, (sbyte)norm);
                        if (res == IntPtr.Zero) { torch.CheckForErrors(); }
                        return new TorchTensor(res);
                    }
                }
            }

            [DllImport("LibTorchSharp")]
            static extern IntPtr THSTensor_ifftn(IntPtr tensor, IntPtr s, int s_length, IntPtr dim, int dim_length, sbyte norm);

            public static TorchTensor ifftn(TorchTensor input, long[] s = null, long[] dim = null, FFTNormType norm = FFTNormType.Backward)
            {
                var slen = (s == null) ? 0 : s.Length;
                var dlen = (dim == null) ? 0 : dim.Length;
                unsafe {
                    fixed (long* ps = s, pDim = dim) {
                        var res = THSTensor_ifftn(input.Handle, (IntPtr)ps, slen, (IntPtr)pDim, dlen, (sbyte)norm);
                        if (res == IntPtr.Zero) { torch.CheckForErrors(); }
                        return new TorchTensor(res);
                    }
                }
            }

            [DllImport("LibTorchSharp")]
            static extern IntPtr THSTensor_irfft(IntPtr tensor, long n, long dim, sbyte norm);

            public static TorchTensor irfft(TorchTensor input, long n = -1, long dim = -1, FFTNormType norm = FFTNormType.Backward)
            {
                var res = THSTensor_irfft(input.Handle, n, dim, (sbyte)norm);
                if (res == IntPtr.Zero) { torch.CheckForErrors(); }
                return new TorchTensor(res);
            }

            [DllImport("LibTorchSharp")]
            static extern IntPtr THSTensor_rfft(IntPtr tensor, long n, long dim, sbyte norm);

            public static TorchTensor rfft(TorchTensor input, long n = -1, long dim = -1, FFTNormType norm = FFTNormType.Backward)
            {
                var res = THSTensor_rfft(input.Handle, n, dim, (sbyte)norm);
                if (res == IntPtr.Zero) { torch.CheckForErrors(); }
                return new TorchTensor(res);
            }

            [DllImport("LibTorchSharp")]
            static extern IntPtr THSTensor_rfft2(IntPtr tensor, IntPtr s, IntPtr dim, sbyte norm);

            public static TorchTensor rfft2(TorchTensor input, long[] s = null, long[] dim = null, FFTNormType norm = FFTNormType.Backward)
            {
                if (input.Dimensions < 2) throw new ArgumentException("rfft2() input should be at least 2D");
                if (dim == null) dim = new long[] { -2, -1 };
                unsafe {
                    fixed (long* ps = s, pDim = dim) {
                        var res = THSTensor_rfft2(input.Handle, (IntPtr)ps, (IntPtr)pDim, (sbyte)norm);
                        if (res == IntPtr.Zero) { torch.CheckForErrors(); }
                        return new TorchTensor(res);
                    }
                }
            }

            [DllImport("LibTorchSharp")]
            static extern IntPtr THSTensor_irfft2(IntPtr tensor, IntPtr s, IntPtr dim, sbyte norm);

            public static TorchTensor irfft2(TorchTensor input, long[] s = null, long[] dim = null, FFTNormType norm = FFTNormType.Backward)
            {
                if (input.Dimensions < 2) throw new ArgumentException("irfft2() input should be at least 2D");
                if (dim == null) dim = new long[] { -2, -1 };
                unsafe {
                    fixed (long* ps = s, pDim = dim) {
                        var res = THSTensor_irfft2(input.Handle, (IntPtr)ps, (IntPtr)pDim, (sbyte)norm);
                        if (res == IntPtr.Zero) { torch.CheckForErrors(); }
                        return new TorchTensor(res);
                    }
                }
            }

            [DllImport("LibTorchSharp")]
            static extern IntPtr THSTensor_rfftn(IntPtr tensor, IntPtr s, int s_length, IntPtr dim, int dim_length, sbyte norm);

            public static TorchTensor rfftn(TorchTensor input, long[] s = null, long[] dim = null, FFTNormType norm = FFTNormType.Backward)
            {
                var slen = (s == null) ? 0 : s.Length;
                var dlen = (dim == null) ? 0 : dim.Length;
                unsafe {
                    fixed (long* ps = s, pDim = dim) {
                        var res = THSTensor_rfftn(input.Handle, (IntPtr)ps, slen, (IntPtr)pDim, dlen, (sbyte)norm);
                        if (res == IntPtr.Zero) { torch.CheckForErrors(); }
                        return new TorchTensor(res);
                    }
                }
            }

            [DllImport("LibTorchSharp")]
            static extern IntPtr THSTensor_irfftn(IntPtr tensor, IntPtr s, int s_length, IntPtr dim, int dim_length, sbyte norm);

            public static TorchTensor irfftn(TorchTensor input, long[] s = null, long[] dim = null, FFTNormType norm = FFTNormType.Backward)
            {
                var slen = (s == null) ? 0 : s.Length;
                var dlen = (dim == null) ? 0 : dim.Length;
                unsafe {
                    fixed (long* ps = s, pDim = dim) {
                        var res = THSTensor_irfftn(input.Handle, (IntPtr)ps, slen, (IntPtr)pDim, dlen, (sbyte)norm);
                        if (res == IntPtr.Zero) { torch.CheckForErrors(); }
                        return new TorchTensor(res);
                    }
                }
            }

            [DllImport("LibTorchSharp")]
            static extern IntPtr THSTensor_hfft(IntPtr tensor, long n, long dim, sbyte norm);

            public static TorchTensor hfft(TorchTensor input, long n = -1, long dim = -1, FFTNormType norm = FFTNormType.Backward)
            {
                var res = THSTensor_hfft(input.Handle, n, dim, (sbyte)norm);
                if (res == IntPtr.Zero) { torch.CheckForErrors(); }
                return new TorchTensor(res);
            }

            [DllImport("LibTorchSharp")]
            static extern IntPtr THSTensor_ihfft(IntPtr tensor, long n, long dim, sbyte norm);

            public static TorchTensor ihfft(TorchTensor input, long n = -1, long dim = -1, FFTNormType norm = FFTNormType.Backward)
            {
                var res = THSTensor_ihfft(input.Handle, n, dim, (sbyte)norm);
                if (res == IntPtr.Zero) { torch.CheckForErrors(); }
                return new TorchTensor(res);
            }

            [DllImport("LibTorchSharp")]
            static extern IntPtr THSTensor_fftshift(IntPtr tensor, IntPtr dim, int dim_length);

            public static TorchTensor fftshift(TorchTensor input, long[] dim = null)
            {
                var dlen = (dim == null) ? 0 : dim.Length;
                unsafe {
                    fixed (long* pDim = dim) {
                        var res = THSTensor_fftshift(input.Handle, (IntPtr)pDim, dlen);
                        if (res == IntPtr.Zero) { torch.CheckForErrors(); }
                        return new TorchTensor(res);
                    }
                }
            }

            [DllImport("LibTorchSharp")]
            static extern IntPtr THSTensor_ifftshift(IntPtr tensor, IntPtr dim, int dim_length);

            public static TorchTensor ifftshift(TorchTensor input, long[] dim = null)
            {
                var dlen = (dim == null) ? 0 : dim.Length;
                unsafe {
                    fixed (long* pDim = dim) {
                        var res = THSTensor_ifftshift(input.Handle, (IntPtr)pDim, dlen);
                        if (res == IntPtr.Zero) { torch.CheckForErrors(); }
                        return new TorchTensor(res);
                    }
                }
            }
        }
    }
}