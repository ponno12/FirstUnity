using System;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Capabilities of a face subsystem implementation.
    /// </summary>
    [Flags]
    public enum FaceSubsystemCapabilities
    {
        /// <summary>
        /// The subsystem has no capabilities
        /// </summary>
        None = 0,

        /// <summary>
        /// The subsystem can produce a <c>Pose</c> for a face.
        /// </summary>
        Pose = 1 << 0,

        /// <summary>
        /// The subsystem can generate vertices and triangle indices for a mesh representing a face.
        /// </summary>
        MeshVerticesAndIndices = 1 << 1,

        /// <summary>
        /// The subsystem can supply texture coordinates for a face mesh.
        /// </summary>
        MeshUVs = 1 << 2,

        /// <summary>
        /// The subsystem can supply normals for a face mesh.
        /// </summary>
        MeshNormals = 1 << 3,


        /// <summary>
        /// The subsystem can supply eye tracking data for a face.
        /// </summary>
        EyeTracking = 1 << 4
    }

    /// <summary>
    /// This struct is an initializer for the creation of a <see cref="XRFaceSubsystemDescriptor"/>.
    /// </summary>
    /// <remarks>
    /// Face Tracking data provider should create during <c>InitializeOnLoad</c> a descriptor using
    /// the params here to specify which of the XRFaceSubsystem features it supports.
    /// </remarks>
    public struct FaceSubsystemParams : IEquatable<FaceSubsystemParams>
    {
        /// <summary>
        /// The string identifier for a specific implementation.
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// The concrete <c>Type</c> which will be instantiated if <c>Create</c> is called on this subsystem descriptor.
        /// </summary>
        public Type subsystemImplementationType { get; set; }

        /// <summary>
        /// Whether the subsystem supports getting a pose for the face.
        /// </summary>
        public bool supportsFacePose
        {
            get => (m_Capabilities & FaceSubsystemCapabilities.Pose) != 0;
            set
            {
                if (value)
                {
                    m_Capabilities |= FaceSubsystemCapabilities.Pose;
                }
                else
                {
                    m_Capabilities &= ~FaceSubsystemCapabilities.Pose;
                }
            }
        }

        /// <summary>
        /// Whether the subsystem supports getting vertices and triangle indices describing a face mesh.
        /// </summary>
        public bool supportsFaceMeshVerticesAndIndices
        {
            get => (m_Capabilities & FaceSubsystemCapabilities.MeshVerticesAndIndices) != 0;
            set
            {
                if (value)
                {
                    m_Capabilities |= FaceSubsystemCapabilities.MeshVerticesAndIndices;
                }
                else
                {
                    m_Capabilities &= ~FaceSubsystemCapabilities.MeshVerticesAndIndices;
                }
            }
        }

        /// <summary>
        /// Whether the subsystem supports texture coordinates for the face mesh.
        /// </summary>
        public bool supportsFaceMeshUVs
        {
            get => (m_Capabilities & FaceSubsystemCapabilities.MeshUVs) != 0;
            set
            {
                if (value)
                {
                    m_Capabilities |= FaceSubsystemCapabilities.MeshUVs;
                }
                else
                {
                    m_Capabilities &= ~FaceSubsystemCapabilities.MeshUVs;
                }
            }
        }

        /// <summary>
        /// Whether the subsystem supports normals for the face mesh.
        /// </summary>
        public bool supportsFaceMeshNormals
        {
            get => (m_Capabilities & FaceSubsystemCapabilities.MeshNormals) != 0;
            set
            {
                if (value)
                {
                    m_Capabilities |= FaceSubsystemCapabilities.MeshNormals;
                }
                else
                {
                    m_Capabilities &= ~FaceSubsystemCapabilities.MeshNormals;
                }
            }
        }

        /// <summary>
        /// Whether the subsystem supports eye tracking for each detected face.
        /// </summary>
        public bool supportsEyeTracking
        {
            get => (m_Capabilities & FaceSubsystemCapabilities.EyeTracking) != 0;
            set
            {
                if (value)
                {
                    m_Capabilities |= FaceSubsystemCapabilities.EyeTracking;
                }
                else
                {
                    m_Capabilities &= FaceSubsystemCapabilities.EyeTracking;
                }
            }
        }

        FaceSubsystemCapabilities m_Capabilities;

        /// <summary>
        /// Tests for equality.
        /// </summary>
        /// <param name="other">The other <see cref="FaceSubsystemParams"/> to compare against.</param>
        /// <returns>`True` if every field in <paramref name="other"/> is equal to this <see cref="FaceSubsystemParams"/>, otherwise false.</returns>
        public bool Equals(FaceSubsystemParams other)
        {
            return
                (m_Capabilities == other.m_Capabilities) &&
                (id == other.id) &&
                (subsystemImplementationType == other.subsystemImplementationType);
        }

        /// <summary>
        /// Tests for equality.
        /// </summary>
        /// <param name="obj">The `object` to compare against.</param>
        /// <returns>`True` if <paramref name="obj"/> is of type <see cref="FaceSubsystemParams"/> and
        /// <see cref="Equals(FaceSubsystemParams)"/> also returns `true`; otherwise `false`.</returns>
        public override bool Equals(object obj) => (obj is FaceSubsystemParams) && Equals((FaceSubsystemParams)obj);

        /// <summary>
        /// Generates a hash suitable for use with containers like `HashSet` and `Dictionary`.
        /// </summary>
        /// <returns>A hash code generated from this object's fields.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = HashCode.ReferenceHash(id);
                hashCode = (hashCode * 486187739) + HashCode.ReferenceHash(subsystemImplementationType);
                hashCode = (hashCode * 486187739) + ((int)m_Capabilities).GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// Tests for equality. Same as <see cref="Equals(FaceSubsystemParams)"/>.
        /// </summary>
        /// <param name="lhs">The left-hand side of the comparison.</param>
        /// <param name="rhs">The right-hand side of the comparison.</param>
        /// <returns>`True` if <paramref name="lhs"/> is equal to <paramref name="rhs"/>, otherwise `false`.</returns>
        public static bool operator==(FaceSubsystemParams lhs, FaceSubsystemParams rhs) => lhs.Equals(rhs);

        /// <summary>
        /// Tests for inequality. Same as `!`<see cref="Equals(FaceSubsystemParams)"/>.
        /// </summary>
        /// <param name="lhs">The left-hand side of the comparison.</param>
        /// <param name="rhs">The right-hand side of the comparison.</param>
        /// <returns>`True` if <paramref name="lhs"/> is not equal to <paramref name="rhs"/>, otherwise `false`.</returns>
        public static bool operator!=(FaceSubsystemParams lhs, FaceSubsystemParams rhs) => lhs.Equals(rhs);
    }

    /// <summary>
    /// The descriptor of the <see cref="XRFaceSubsystem"/> that shows which face tracking features are available on that XRSubsystem.
    /// </summary>
    /// <remarks>
    /// You use the <c>Create</c> factory method along with <see cref="FaceSubsystemParams"/> struct to construct and
    /// register one of these from each face tracking data provider.
    /// </remarks>
    public class XRFaceSubsystemDescriptor : SubsystemDescriptor<XRFaceSubsystem>
    {
        XRFaceSubsystemDescriptor(FaceSubsystemParams descriptorParams)
        {
            id = descriptorParams.id;
            subsystemImplementationType = descriptorParams.subsystemImplementationType;
            supportsFacePose = descriptorParams.supportsFacePose;
            supportsFaceMeshVerticesAndIndices = descriptorParams.supportsFaceMeshVerticesAndIndices;
            supportsFaceMeshUVs = descriptorParams.supportsFaceMeshUVs;
            supportsFaceMeshNormals = descriptorParams.supportsFaceMeshNormals;
            supportsEyeTracking = descriptorParams.supportsEyeTracking;
        }

        /// <summary>
        /// Whether the subsystem can produce a <c>Pose</c> for each detected face.
        /// </summary>
        public bool supportsFacePose { get; }

        /// <summary>
        /// Whether the subsystem supports face meshes, and can produce vertices and triangle indices representing a face mesh.
        /// </summary>
        public bool supportsFaceMeshVerticesAndIndices { get; }

        /// <summary>
        /// Whether the subsystem supports texture coordinates for each face mesh.
        /// </summary>
        public bool supportsFaceMeshUVs { get; }

        /// <summary>
        /// Whether the subsystem supports normals for each face mesh.
        /// </summary>
        public bool supportsFaceMeshNormals { get; }

        /// <summary>
        /// Whether the subsystem supports eye tracking for each detected face.
        /// </summary>
        public bool supportsEyeTracking { get; }

        /// <summary>
        /// Creates a subsystem descriptor. Used to register an implementation of the <see cref="XRFaceSubsystem"/>.
        /// </summary>
        /// <param name="descriptorParams">Parameters describing the <see cref="XRFaceSubsystem"/>.</param>
        public static void Create(FaceSubsystemParams descriptorParams)
        {
            var descriptor = new XRFaceSubsystemDescriptor(descriptorParams);
            SubsystemRegistration.CreateDescriptor(descriptor);
        }
    }
}
