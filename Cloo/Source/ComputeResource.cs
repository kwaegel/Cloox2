#region License

/*

Copyright (c) 2009 - 2011 Fatjon Sakiqi

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

    /// <summary>
    /// Represents an OpenCL resource.
    /// </summary>
    /// <remarks> An OpenCL resource is an OpenCL object that can be created and deleted by the application. </remarks>
    /// <seealso cref="ComputeObject"/>
    public abstract class ComputeResource : ComputeObject, IDisposable
    {
        #region Fields

        protected static readonly System.Collections.Hashtable resourceTable = new System.Collections.Hashtable();

        #endregion

        #region Public methods

        /// <summary>
        /// Deletes the <see cref="ComputeResource"/> and frees its accompanying OpenCL resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Dispose of all ComputeResources that have not already been removed.
        /// 
        /// This is a workaround for a bug with OpenGL/OpenCL automatic garbage collection.
        /// </summary>
        public static void DisposeAll()
        {
            System.Collections.ArrayList values = new System.Collections.ArrayList(resourceTable.Values);
            for (int i = 0; i < values.Count; i++)
            {
                ComputeResource resource = (ComputeResource)values[i];
                resource.Dispose();
            }
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Releases the associated OpenCL object.
        /// </summary>
        /// <param name="manual"></param>
        protected abstract void Dispose(bool manual);

        #endregion

        #region Private methods

        /// <summary>
        /// 
        /// </summary>
        ~ComputeResource()
        {
            Dispose(false);
        }

        #endregion
    }
}