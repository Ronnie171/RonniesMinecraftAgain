using UnityEngine;

namespace TestInterfaces {
    public interface IMeshCreator {
        /// <summary>
        /// Builds a new mesh inside of <paramref name="mesh"/>. Clears all previously existing mesh data.
        /// </summary>
        /// <param name="mesh">The mesh to recreate.</param>
        void RecreateMesh(Mesh mesh);
    }
}
