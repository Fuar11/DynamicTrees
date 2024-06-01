// https://github.com/Willster419/RelhaxModpack/blob/master/RelhaxModpack/RelhaxModpack/Common/BadMemeException.cs
// ---------------------------------------------
// BadMemeException - by The Willster419
// Additional Credits:
//      The Illusion: Expanded to ensure this matches best practices
// ---------------------------------------------
// Reusage Rights ------------------------------
// You are free to use this script or portions of it in your own mods, provided you give me credit in your description and maintain this section of comments in any released source code
//
// Warning !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
// Ensure you change the namespace to whatever namespace your mod uses, so it doesnt conflict with other mods
// ---------------------------------------------

namespace DynamicTrees.Utilities.Exceptions
{
    /// <summary>
    /// An exception used mostly for mistakes that could happen during development, 'sanity check' type verification. And also for bad memes.
    /// </summary>
    [System.Serializable]
    public class BadMemeException : System.Exception
    {

        public BadMemeException() : base() { }

        /// <summary>
        /// Throw a bad meme exception.
        /// </summary>
        /// <param name="message">The message to tell the developer why his meme is bad.</param>
        public BadMemeException(string message) : base(message) { }

        
        public BadMemeException(string message, System.Exception inner) : base(message, inner) { }
    }
}
