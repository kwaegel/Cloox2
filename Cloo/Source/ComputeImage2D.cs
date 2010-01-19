﻿#region License

/*

Copyright (c) 2009 Fatjon Sakiqi

Permission is hereby granted, free of charge, to any person
obtaining a copy of this software and associated documentation
files (the "Software"), to deal in the Software without
restriction, including without limitation the rights to use,
copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the
Software is furnished to do so, subject to the following
conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.

*/

#endregion

namespace Cloo
{
    using System;
    using System.Collections.Generic;
    using Cloo.Bindings;

    public class ComputeImage2D: ComputeImage
    {
        #region Fields

        private int height;
        private int rowPitch;
        private int width;        

        #endregion
        
        #region Properties

        /// <summary>
        /// The height of the image in pixels.
        /// </summary>
        public int Height
        {
            get { return height; }
        }

        /// <summary>
        /// The size of the image row in bytes.
        /// </summary>
        public int RowPitch
        {
            get { return rowPitch; }
        }

        /// <summary>
        /// The width of the image in pixels.
        /// </summary>
        public int Width
        {
            get { return width; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new 2D image.
        /// </summary>
        /// <param name="context">A valid OpenCL context on which the image object is to be created.</param>
        /// <param name="flags">A bit-field that is used to specify allocation and usage information about the image.</param>
        /// <param name="format">A structure that describes the format properties of the image.</param>
        /// <param name="width">Width of the image in pixels.</param>
        /// <param name="height">Height of the image in pixels.</param>
        /// <param name="rowPitch">The scan-line pitch in bytes.</param>
        /// <param name="data">The image data that may be already allocated by the application.</param>
        public ComputeImage2D( ComputeContext context, ComputeMemoryFlags flags, ComputeImageFormat format, int width, int height, int rowPitch, IntPtr data )
            : base( context, flags )
        {
            unsafe
            {
                ComputeErrorCode error = ComputeErrorCode.Success;
                Handle = CL10.CreateImage2D(
                    context.Handle,
                    flags,
                    &format,
                    new IntPtr( width ),
                    new IntPtr( height ),
                    new IntPtr( rowPitch ),
                    data,
                    out error );
                ComputeException.ThrowOnError( error );
            }
            Init();
        }

        private ComputeImage2D( IntPtr handle, ComputeContext context, ComputeMemoryFlags flags )
            : base( context, flags )
        {
            Handle = handle;
            
            Init();
        }

        #endregion

        #region Public methods

        public static ComputeImage2D CreateFromGLRenderbuffer( ComputeContext context, ComputeMemoryFlags flags, int renderbufferId )
        {
            IntPtr image = IntPtr.Zero;
            unsafe
            {
                ComputeErrorCode error = ComputeErrorCode.Success;
                image = CL10.CreateFromGLRenderbuffer(
                    context.Handle,
                    flags,
                    renderbufferId,
                    out error );
                ComputeException.ThrowOnError( error );
            }
            return new ComputeImage2D( image, context, flags );
        }

        public static ComputeImage2D CreateFromGLTexture2D( ComputeContext context, ComputeMemoryFlags flags, int textureTarget, int mipLevel, int textureId )
        {
            IntPtr image = IntPtr.Zero;
            unsafe
            {
                ComputeErrorCode error = ComputeErrorCode.Success;
                image = CL10.CreateFromGLTexture2D(
                    context.Handle,
                    flags,
                    textureTarget,
                    mipLevel,
                    textureId,
                    out error );
                ComputeException.ThrowOnError( error );
            }
            return new ComputeImage2D( image, context, flags );
        }

        /// <summary>
        /// Gets a collection of supported 2D image formats with the given context.
        /// </summary>
        /// <param name="context">A valid OpenCL context on which the image object(s) will be created.</param>
        /// <param name="flags">A bit-field that is used to specify allocation and usage information about the image object(s) that will be created.</param>
        public static ICollection<ComputeImageFormat> GetSupportedFormats( ComputeContext context, ComputeMemoryFlags flags )
        {
            return GetSupportedFormats( context, flags, ComputeMemoryType.Image2D );
        }

        #endregion

        #region Private methods

        private void Init()
        {
            Size = ( long )GetInfo<ComputeMemoryInfo, IntPtr>( ComputeMemoryInfo.Size, CL10.GetMemObjectInfo );
            width = ( int )GetInfo<ComputeImageInfo, IntPtr>( ComputeImageInfo.Width, CL10.GetImageInfo );
            height = ( int )GetInfo<ComputeImageInfo, IntPtr>( ComputeImageInfo.Height, CL10.GetImageInfo );
            rowPitch = ( int )GetInfo<ComputeImageInfo, IntPtr>( ComputeImageInfo.RowPitch, CL10.GetImageInfo );
        }

        #endregion
    }
}