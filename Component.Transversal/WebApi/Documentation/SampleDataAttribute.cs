using System;

namespace Component.Transversal.WebApi.Documentation
{
    /// <summary>
    /// Provides a way to provide sample data for the documentation pages
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class SampleDataAttribute : Attribute
    {
        /// <summary>
        /// The provided sample data for this property
        /// </summary>
        public object SampleData { get; private set; }

        /// <summary>
        /// Sets the sample data for a property
        /// </summary>
        /// <param name="sampleData">The provided sample data for this property</param>
        public SampleDataAttribute(object sampleData)
        {
            this.SampleData = sampleData;
        }
    }
}